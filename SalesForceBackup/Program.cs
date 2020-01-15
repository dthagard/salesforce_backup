using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SalesForceBackup.Interfaces;
using SalesForceBackup.IoC;
using TinyIoC;

namespace SalesForceBackup
{
    /// <summary>
    /// The main application.
    /// </summary>
    class Program
    {

        private static IAppSettings _appSettings;

        /// <summary>
        /// Initial application method.
        /// </summary>
        /// <param name="args">Array of command line arguments to the application.</param>
        static void Main(string[] args)
        {
            // Register our IoC containers
            Bootstrap.Register();

            Console.WriteLine("Reading Settings...");
            _appSettings = TinyIoCContainer.Current.Resolve<IAppSettings>();
            AssignValuesFromArguments(args);


            Console.WriteLine("Starting backup...");
            var backup = TinyIoCContainer.Current.Resolve<Backup>();
            backup.Run();
            
            Environment.Exit((int)Enums.ExitCode.Normal);
        }

        #region Private Methods

        /// <summary>
        /// Processes the command line arguments and assigns them as necessary.
        /// </summary>
        /// <param name="args">The list of arguments passed in from the command line.</param>
        private static void AssignValuesFromArguments(IList<string> args)
        {
            for (var i = 0; i < args.Count(); i++)
            {
                switch (args[i])
                {
                    case "--help":
                        DisplayHelp();
                        Environment.Exit((int)Enums.ExitCode.Normal);
                        break;
                    case "-u":
                        _appSettings.Set(AppSettingKeys.Username, args[++i]);
                        break;
                    case "-p":
                        _appSettings.Set(AppSettingKeys.Password, args[++i]);
                        break;
                    case "-t":
                        _appSettings.Set(AppSettingKeys.SecurityToken, args[++i]);
                        break;
                    case "-h":
                        _appSettings.Set(AppSettingKeys.Host, args[++i]);
                        break;
                    case "-a":
                        _appSettings.Set(AppSettingKeys.AwsAccessKey, args[++i]);
                        break;
                    case "-y":
                        _appSettings.Set(AppSettingKeys.AzureAccountName, args[++i]);
                        break;
                    case "-z":
                        _appSettings.Set(AppSettingKeys.AzureSharedKey, args[++i]);
                        break;
                    case "-s":
                        _appSettings.Set(AppSettingKeys.AwsSecretKey, args[++i]);
                        break;
                }
            }
        }

        /// <summary>
        /// Outputs the contents of the help to the console.
        /// </summary>
        private static void DisplayHelp()
        {
            var file = AppDomain.CurrentDomain.FriendlyName;
            var name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            var sb = new StringBuilder(1024);
            using (var sr = new StringWriter(sb))
            {
                sr.WriteLine("{0} version {1}", name, version);
                sr.WriteLine("");
                sr.WriteLine("Usage: {0} [-hupasyz]", file);
                sr.WriteLine("");
                sr.WriteLine("Options:");
                sr.WriteLine("\t-h or --help\tDisplays this help text");
                sr.WriteLine("\t-u \t\tUsername for SalesForce");
                sr.WriteLine("\t-p \t\tPassword for SalesForce");
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
