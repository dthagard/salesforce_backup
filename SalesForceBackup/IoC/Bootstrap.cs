using System;
using System.Configuration;
using SalesForceBackup.Interfaces;
using TinyIoC;

namespace SalesForceBackup.IoC
{
    public static class Bootstrap
    {
        public static void Register()
        {
            TinyIoCContainer.Current.Register<IErrorHandler>(new ConsoleErrorHandler());
            TinyIoCContainer.Current.Register<IAppSettings>(new AppSettings());
            TinyIoCContainer.Current.Register<IUploader>(String.Equals(ConfigurationManager.AppSettings[AppSettingKeys.Uploader], Uploaders.Aws,
                StringComparison.CurrentCultureIgnoreCase)
                ? (IUploader) new S3Uploader()
                : new AzureBlobUploader());
            TinyIoCContainer.Current.Register<IDownloader>(new SalesForceWebDownloader());

            TinyIoCContainer.Current.Register<Backup>();
        }
    }
}
