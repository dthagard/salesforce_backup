﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SalesForceBackup.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SalesForceBackup.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required argument is missing.
        /// </summary>
        internal static string ConfigurationArgumentMissing {
            get {
                return ResourceManager.GetString("ConfigurationArgumentMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument missing for {0} value.
        /// </summary>
        internal static string ConfigurationArgumentValueMissing {
            get {
                return ResourceManager.GetString("ConfigurationArgumentValueMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Run &apos;{0} --help&apos; to display help.
        /// </summary>
        internal static string ConfigurationProblemCallHelp {
            get {
                return ResourceManager.GetString("ConfigurationProblemCallHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Check the provided AWS Credentials.
        /// </summary>
        internal static string ConfigurationS3Credentials {
            get {
                return ResourceManager.GetString("ConfigurationS3Credentials", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to login at Salesforce. Please check your credentials..
        /// </summary>
        internal static string ConfigurationSfdcCredentialsInvalid {
            get {
                return ResourceManager.GetString("ConfigurationSfdcCredentialsInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connecting to SalesForce.com ... .
        /// </summary>
        internal static string StatusConnectingToSalesforce {
            get {
                return ResourceManager.GetString("StatusConnectingToSalesforce", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Downloading {0} of {1}: {2} ... .
        /// </summary>
        internal static string StatusDownloading {
            get {
                return ResourceManager.GetString("StatusDownloading", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Getting list of export files ... .
        /// </summary>
        internal static string StatusFilePage {
            get {
                return ResourceManager.GetString("StatusFilePage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reading Settings....
        /// </summary>
        internal static string StatusReadSettings {
            get {
                return ResourceManager.GetString("StatusReadSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to upload file to S3: {0}.
        /// </summary>
        internal static string StatusS3UploadFailed {
            get {
                return ResourceManager.GetString("StatusS3UploadFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Starting backup....
        /// </summary>
        internal static string StatusStartBackup {
            get {
                return ResourceManager.GetString("StatusStartBackup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown error occured.
        /// </summary>
        internal static string StatusUnknownError {
            get {
                return ResourceManager.GetString("StatusUnknownError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Uploading {0} to AWS S3 ... .
        /// </summary>
        internal static string StatusUploadingS3 {
            get {
                return ResourceManager.GetString("StatusUploadingS3", resourceCulture);
            }
        }
    }
}