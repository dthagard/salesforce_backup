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
    /// Downloads backup files from the SalesForce.com website by scraping the web page.
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
            var sfClient = new SforceService();

            try
            {
                //Login to SalesForce
                Console.WriteLine("Connecting to SalesForce.com...");
                var username = _appSettings.Get(AppSettingKeys.Username);
                var password = _appSettings.Get(AppSettingKeys.Password) + _appSettings.Get(AppSettingKeys.SecurityToken);
                var currentLoginResult = sfClient.login(username, password);

                //Change the binding to the new endpoint
                sfClient.Url = currentLoginResult.serverUrl;

                //Create a new session header object and set the session id to that returned by the login
                var sessionId = currentLoginResult.sessionId;
                sfClient.SessionHeaderValue = new SessionHeader {sessionId = sessionId};

                //Retrieve the page containing the list of SalesForce exports.
                Console.WriteLine("Getting files for download...");
                var page = GetSalesForcePage(_appSettings.Get(AppSettingKeys.DataExportPage), sessionId);

                //Find any available downloads on the page
                var regex = new Regex(_appSettings.Get(AppSettingKeys.FilenamePattern), RegexOptions.IgnoreCase);
                var matches = regex.Matches(page.Content.ReadAsStringAsync().Result);
                foreach (Match match in matches) {
                    //Get the correctly formatted download file
                    var fileName = match.Groups[1].ToString().Split(new[] { '&' })[0];
                    var url = String.Format("{0}{1}", _appSettings.Get(AppSettingKeys.DownloadPage),
                        match.ToString().Replace("&amp;", "&"));
                    url = url.Substring(0, url.Length - 1);

                    //Retrieve the file
                    Console.WriteLine("Downloading {0} from SalesForce.com...", fileName);
                    var download = GetSalesForcePage(url, sessionId);
                    var array = download.Content.ReadAsByteArrayAsync().Result;
                    using (var fileStream = File.Create(fileName))
                    {
                        fileStream.Write(array, 0, array.Length);
                    }

                    var filePath = String.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture),
                        new [] { Environment.CurrentDirectory, fileName });
                    files.Add(filePath);
                }
            }
            catch (Exception e)
            {
                _errorHandler.HandleError(e);
            }
            return files.ToArray();
        }

        /// <summary>
        /// Retrieves a page from SalesForce.
        /// </summary>
        /// <param name="url">The full URL for the SalesForce page.</param>
        /// <param name="sessionId">The current session ID.</param>
        /// <returns>An HttpResponseMessage for the url.</returns>
        /// <remarks>
        /// SalesForce requires that you populate an organizationId and sessionId for authenticated requests.
        /// </remarks>
        private HttpResponseMessage GetSalesForcePage(string url, string sessionId)
        {
            var baseAddress = new Uri(String.Format("{0}://{1}", _appSettings.Get(AppSettingKeys.Scheme), _appSettings.Get(AppSettingKeys.Host)));
            using (var handler = new HttpClientHandler { UseCookies = false })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var message = new HttpRequestMessage(HttpMethod.Get, url);
                message.Headers.Add("Cookie", String.Format("oid={0}; sid={1}", _appSettings.Get(AppSettingKeys.OrganizationId), sessionId));
                return client.SendAsync(message).Result;
            }
        }

    }
}
