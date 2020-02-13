using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SalesForceBackup.Interfaces;
using SalesForceBackup.IoC;
using SalesForceBackup.Uploaders;
using TinyIoC;

namespace SalesForceBackup
{
    /// <summary>
    /// The main application.
    /// </summary>
    class Program
    {

        private static IAppSettings _appSettings;
        private static readonly Dictionary<String, String> inputOptions = new Dictionary<String, String>()
        {
            { "-u", AppSettingKeys.Username },
            { "-p", AppSettingKeys.Password },
            { "-t", AppSettingKeys.SecurityToken },
            { "-h", AppSettingKeys.Host },
            { "-a", AppSettingKeys.AwsAccessKey },
            { "-y", AppSettingKeys.AzureAccountName },
            { "-z", AppSettingKeys.AzureSharedKey },
            { "-s", AppSettingKeys.AwsSecretKey },
            { "-d", AppSettingKeys.Uploader },
            { "-o", AppSettingKeys.OutputDirectory }
        };

        /// <summary>
        /// Initial application method.
        /// </summary>
        /// <param name="args">Array of command line arguments to the application.</param>
        static void Main(string[] args)
        {
            // Register our IoC containers
            Bootstrap.Register();
            _appSettings = TinyIoCContainer.Current.Resolve<IAppSettings>();

            Console.WriteLine(Properties.Resources.StatusReadSettings);
            InitializeSettings();
            AssignValuesFromArguments(args);
            CheckArguments();

            Console.WriteLine(Properties.Resources.StatusStartBackup);
            var backup = TinyIoCContainer.Current.Resolve<Backup>();
            switch(_appSettings.Get(AppSettingKeys.Uploader))
            {
                case UploaderValues.Aws:
                    backup.Uploader = TinyIoCContainer.Current.Resolve<S3Uploader>();
                    break;
                case UploaderValues.Azure:
                    backup.Uploader = TinyIoCContainer.Current.Resolve<AzureBlobUploader>();
                    break;
                case UploaderValues.FileSystem:
                    backup.Uploader = TinyIoCContainer.Current.Resolve<FileSystemUploader>();
                    break;
            }
            backup.Run();
            
            Environment.Exit((int)Enums.ExitCode.Normal);
        }

        #region Private Methods

        /// <summary>
        /// Initializes the application settings.
        /// </summary>
        private static void InitializeSettings()
        {
            _appSettings.Set(AppSettingKeys.Uploader, UploaderValues.FileSystem);
            _appSettings.Set(AppSettingKeys.OutputDirectory, Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Processes the command line arguments and assigns them as necessary.
        /// </summary>
        /// <param name="args">The list of arguments passed in from the command line.</param>
        private static void AssignValuesFromArguments(IList<string> args)
        {
            for (var i = 0; i < args.Count; i++)
            {
                var arg = args[i];
                if(arg == "--help")
                {
                    DisplayHelp();
                    Environment.Exit((int)Enums.ExitCode.Normal);
                }

                if(inputOptions.ContainsKey(arg))
                {
                    if (i + 1 < args.Count)
                    {
                        _appSettings.Set(inputOptions[arg], args[++i]);
                    }
                    else
                    {
                        IFormatProvider formatProvider = TinyIoCContainer.Current.Resolve<IFormatProvider>();
                        Console.WriteLine(string.Format(formatProvider, Properties.Resources.ConfigurationArgumentValueMissing, arg));
                        Console.WriteLine(string.Format(formatProvider, Properties.Resources.ConfigurationProblemCallHelp, AppDomain.CurrentDomain.FriendlyName));
                        Environment.Exit((int)Enums.ExitCode.ConfigurationError);
                    }
                }
            }           
        }

        /// <summary>
        /// Checks that all required command line arguments are given and that argument values are OK. 
        /// Terminates the application if required values are missing or invalid.
        /// </summary>
        /// <param name="args">The list of arguments passed in from the command line.</param>
        private static void CheckArguments()
        {
            if(string.IsNullOrEmpty(_appSettings.Get(AppSettingKeys.Host)) 
                || string.IsNullOrEmpty(_appSettings.Get(AppSettingKeys.Username))
                || string.IsNullOrEmpty(_appSettings.Get(AppSettingKeys.Password))
                || string.IsNullOrEmpty(_appSettings.Get(AppSettingKeys.SecurityToken)))
            {
                IFormatProvider formatProvider = TinyIoCContainer.Current.Resolve<IFormatProvider>();
                Console.WriteLine(Properties.Resources.ConfigurationArgumentMissing);
                Console.WriteLine(string.Format(formatProvider, Properties.Resources.ConfigurationProblemCallHelp, AppDomain.CurrentDomain.FriendlyName));
                Environment.Exit((int)Enums.ExitCode.ConfigurationError);
            }

            var uploaderName = _appSettings.Get(AppSettingKeys.Uploader);
            if(UploaderValues.Aws != uploaderName && UploaderValues.Azure != uploaderName && UploaderValues.FileSystem != uploaderName)
            {
                Console.WriteLine(Properties.Resources.ConfigurationInvalidUploader);
                Environment.Exit((int)Enums.ExitCode.ConfigurationError);
            }
        }

        /// <summary>
        /// Outputs the contents of the help to the console.
        /// </summary>
        private static void DisplayHelp()
        {
            var name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            var sb = new StringBuilder();
            using (var sr = new StringWriter(sb))
            {
                sr.WriteLine("{0} version {1}", name, version);
                sr.WriteLine("Options:");
                sr.WriteLine("\t--help\t\tDisplays this help text");
                sr.WriteLine("\t-h  (required)  Hostname for Salesforce");
                sr.WriteLine("\t-u  (required)  Username for Salesforce");
                sr.WriteLine("\t-p  (required)  Password for Salesforce");
                sr.WriteLine("\t-t  (required)  Security token for Salesforce");
                sr.WriteLine("\t-d \t\tDestination ('FileSystem' (default) | 'Azure' | 'AWS')");
                sr.WriteLine("\t-o \t\tOutput directory)");
                sr.WriteLine("\t-a \t\tAWS access key");
                sr.WriteLine("\t-s \t\tAWS secret key");
                sr.WriteLine("\t-y \t\tAzure account name");
                sr.WriteLine("\t-z \t\tAzure shared key");
            }
            Console.Write(sb.ToString());
        }

        #endregion // Private Methods

    }
}
