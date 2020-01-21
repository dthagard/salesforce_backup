﻿using System;
using System.Collections.Generic;
using System.IO;
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
        private static readonly Dictionary<String, String> inputOptions = new Dictionary<String, String>()
        {
            { "-u", AppSettingKeys.Username },
            { "-p", AppSettingKeys.Password },
            { "-t", AppSettingKeys.SecurityToken },
            { "-h", AppSettingKeys.Host },
            { "-a", AppSettingKeys.AwsAccessKey },
            { "-y", AppSettingKeys.AzureAccountName },
            { "-z", AppSettingKeys.AzureSharedKey },
            { "-s", AppSettingKeys.AwsSecretKey }
        };

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
            CheckRequiredArguments();


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
                        Console.WriteLine("Argument missing for " + arg + " value");
                        Console.WriteLine("Run '" + AppDomain.CurrentDomain.FriendlyName + " --help' to display help");
                        Environment.Exit((int)Enums.ExitCode.ConfigurationError);
                    }
                }
            }           
        }

        private static void CheckRequiredArguments()
        {
            if(String.IsNullOrEmpty(_appSettings.Get(AppSettingKeys.Host)) 
                || String.IsNullOrEmpty(_appSettings.Get(AppSettingKeys.Username))
                || String.IsNullOrEmpty(_appSettings.Get(AppSettingKeys.Password))
                || String.IsNullOrEmpty(_appSettings.Get(AppSettingKeys.SecurityToken)))
            {
                Console.WriteLine("Required argument is missing");
                Console.WriteLine("Run '" + AppDomain.CurrentDomain.FriendlyName + " --help' to display help");
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
                sr.WriteLine("");
                sr.WriteLine("Usage: {0} [-hupasyz]", AppDomain.CurrentDomain.FriendlyName);
                sr.WriteLine("");
                sr.WriteLine("Options:");
                sr.WriteLine("\t--help\t\tDisplays this help text");
                sr.WriteLine("\t-h  (required)  Hostname for Salesforce");
                sr.WriteLine("\t-u  (required)  Username for Salesforce");
                sr.WriteLine("\t-p  (required)  Password for Salesforce");
                sr.WriteLine("\t-t  (required)  Security token for Salesforce");
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
