using System;
using System.Collections.Generic;
using System.Configuration;
using SalesForceBackup.Interfaces;
using TinyIoC;

namespace SalesForceBackup
{
    /// <summary>
    /// Reads the App.Config application settings.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        private static IErrorHandler _errorHandler;
        private static readonly Dictionary<string, string> ValuePairs = new Dictionary<string, string>();

        /// <summary>
        /// Instantiates a new AppSettings object.
        /// </summary>
        public AppSettings()
        {
            _errorHandler = TinyIoCContainer.Current.Resolve<IErrorHandler>();
            ReadAllSettings();
        }

        /// <summary>
        /// Read the application settings from the App.config.
        /// </summary>
        private static void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0) return;
                foreach (var key in appSettings.AllKeys)
                {
                    ValuePairs.Add(key, appSettings[key]);
                }
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine("Error reading app settings");
                _errorHandler.HandleError(e, (int)Enums.ExitCode.ConfigurationError);
            }
            catch (Exception e)
            {
                _errorHandler.HandleError(e);
            }
        }

        /// <summary>
        /// Gets the value of an Application Setting based on the key.
        /// </summary>
        /// <param name="key">The application setting to retrieve.</param>
        /// <returns>The value of the key, or a null string if it doesn't exist.</returns>
        public string Get(string key)
        {
            string value;
            ValuePairs.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// Sets the value of an Application Setting.
        /// </summary>
        /// <param name="key">The key of the application setting.</param>
        /// <param name="value">The new value of the key.</param>
        /// <remarks>The key will be created if it does not already exist.</remarks>
        public void Set(string key, string value)
        {
            if (ValuePairs.ContainsKey(key))
            {
                ValuePairs.Remove(key);
            }
            ValuePairs.Add(key, value);
        }
    }
}
