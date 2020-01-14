using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using SalesForceBackup.Interfaces;
using SalesForceBackup.SFDC;
using TinyIoC;

namespace SalesForceBackup
{
    /// <summary>
    /// Downloads backup files from the Salesforce website by scraping the web page.
    /// </summary>
    public class SalesForceWebDownloader : IDownloader
    {
        private readonly IAppSettings _appSettings;
        private readonly IErrorHandler _errorHandler;

        /// <summary>
        /// Initializes a new SalesForceWebDownloader.
        /// </summary>
        public SalesForceWebDownloader()
        {
            _appSettings = TinyIoCContainer.Current.Resolve<IAppSettings>();
            _errorHandler = TinyIoCContainer.Current.Resolve<IErrorHandler>();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Downloads the backups from the SalesForce.com website.
        /// </summary>
        /// <returns>An array of the paths to the files that were downloaded.</returns>
        public string[] Download()
        {
            var files = new List<string>();
            var baseAddress = new Uri(String.Format("{0}://{1}", _appSettings.Get(AppSettingKeys.Scheme), _appSettings.Get(AppSettingKeys.Host)));

            try
            {
                Console.Write("Connecting to SalesForce.com ... ");
                var sessionId = LogIn();
                Console.WriteLine("\u221A");

                Console.Write("Getting list of export files ... ");
                var exportFiles = DownloadListOfExportFiles(sessionId);
                Console.WriteLine("\u221A");

                for (int i=0; i<exportFiles.Count; i++) {                    
                    var exportFile = exportFiles[i];
                    Console.Write(String.Format("Downloading {0} of {1}: {2} ... ", i + 1, exportFiles.Count, exportFile.FileName));
                    DownloadExportFile(exportFile, baseAddress, sessionId).Wait();
                    Console.WriteLine("\u221A");

                    files.Add(String.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), new[] { Environment.CurrentDirectory, exportFile.FileName }));
                }
            }
            catch (Exception e)
            {
                _errorHandler.HandleError(e);
            }
            return files.ToArray();
        }

        private string LogIn()
        {            
            var sfClient = new SforceService();
            var username = _appSettings.Get(AppSettingKeys.Username);
            var password = _appSettings.Get(AppSettingKeys.Password) + _appSettings.Get(AppSettingKeys.SecurityToken);
            var currentLoginResult = sfClient.login(username, password);

            // Change the binding to the new endpoint
            sfClient.Url = currentLoginResult.serverUrl;

            // Create a new session header object and set the session id to that returned by the login
            var sessionId = currentLoginResult.sessionId;
            sfClient.SessionHeaderValue = new SessionHeader { sessionId = sessionId };
            return sessionId;
        }

        private List<DataExportFile> DownloadListOfExportFiles(string sessionId)
        {
            List<DataExportFile> result = new List<DataExportFile>();
           
            var page = DownloadWebpage(_appSettings.Get(AppSettingKeys.DataExportPage), sessionId);
            var regex = new Regex(_appSettings.Get(AppSettingKeys.FilenamePattern), RegexOptions.IgnoreCase);
            var matches = regex.Matches(page.Content.ReadAsStringAsync().Result);
            foreach (Match match in matches)
            {
                var fileName = match.Groups[1].ToString().Split(new[] { '&' })[0];
                var url = String.Format("{0}{1}", _appSettings.Get(AppSettingKeys.DownloadPage), match.ToString().Replace("&amp;", "&"));                
                result.Add(new DataExportFile(fileName, url.Substring(0, url.Length - 1)));
            }
            return result;
        }

        /// <summary>
        /// Retrieves a page from Salesforce.
        /// </summary>
        /// <param name="url">The relative URL for the Salesforce page.</param>
        /// <param name="sessionId">The current session ID.</param>
        /// <returns>An HttpResponseMessage for the url.</returns>
        private HttpResponseMessage DownloadWebpage(string url, string sessionId)
        {
            var baseAddress = new Uri(String.Format("{0}://{1}", _appSettings.Get(AppSettingKeys.Scheme), _appSettings.Get(AppSettingKeys.Host)));
            using (var handler = new HttpClientHandler { UseCookies = false })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var message = new HttpRequestMessage(HttpMethod.Get, url);
                message.Headers.Add("Cookie", String.Format("sid={0}", sessionId));
                return client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead).Result;
            }
        }

        /// <summary>
        /// Asynchronously downloads a data export file from Salesforce.
        /// </summary>
        /// <param name="dataExportFile">file to be downloaded</param>
        /// <param name="baseAddress">base address of the Salesforce server</param>
        /// <param name="sessionId">current session ID.</param>
        /// <returns>a Task that can be used to monitor the progress</returns>
        private static async System.Threading.Tasks.Task DownloadExportFile(DataExportFile dataExportFile, Uri baseAddress, string sessionId)
        {
            using (var handler = new HttpClientHandler { UseCookies = false })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var message = new HttpRequestMessage(HttpMethod.Get, dataExportFile.Url);
                message.Headers.Add("Cookie", String.Format("sid={0}", sessionId));

                using (HttpResponseMessage response = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead))
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    using (Stream fileStream = File.Open(dataExportFile.FileName, FileMode.Create))
                    {
                        await streamToReadFrom.CopyToAsync(fileStream);
                    }
                }
            }
        }

    }
}
