using System;
using System.Globalization;
using SalesForceBackup.Interfaces;
using SalesForceBackup.Uploaders;
using TinyIoC;

namespace SalesForceBackup.IoC
{
    public static class Bootstrap
    {
        public static void Register()
        {
            TinyIoCContainer.Current.Register<IErrorHandler>(new ConsoleErrorHandler());
            TinyIoCContainer.Current.Register<IAppSettings>(new AppSettings());
            TinyIoCContainer.Current.Register<IDownloader>(new SalesForceWebDownloader());
            TinyIoCContainer.Current.Register<AzureBlobUploader>(new AzureBlobUploader());
            TinyIoCContainer.Current.Register<FileSystemUploader>(new FileSystemUploader());
            TinyIoCContainer.Current.Register<S3Uploader>(new S3Uploader());
            TinyIoCContainer.Current.Register<Backup>();
            TinyIoCContainer.Current.Register<IFormatProvider>(new CultureInfo("en-US"));
        }
    }
}
