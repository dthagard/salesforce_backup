# Salesforce-Backup [![Build Status](https://dev.azure.com/huddeldaddel/huddeldaddel/_apis/build/status/huddeldaddel.salesforce-backup?branchName=master)](https://dev.azure.com/huddeldaddel/huddeldaddel/_build/latest?definitionId=6&branchName=master)

Performs automated backups of SalesForce.com export data locally or to either AWS or Azure.

Usage
=====

You need to edit the App.config settings for your particular needs. The key values are as follows:

| Key | Description | Default |
| --- | --- | --- |
| **AwsAccessKey** | The access key for the account with permissions to the AWS S3 bucket. | null |
| **AWSRegion** | The region the AWS S3 bucket is located. | us-east-1 |
| **AWSSecretKey** | The secret key for the account with permissions to the AWS S3 bucket. | null |
| **AzureAccountName** | The Azure account name to connect to. | null |
| **AzureBlobEndpoint** | The endpoint for the Azure storage account. | null|
| **AzureContainer** | The Azure container to use for the backup. | monthlybackups |
| **AzureFolder** | The folder to place the backup in inside the Azure container. | salesforce |
| **AzureSharedKey** | The shared key for accessing the Azure storage container. | null |
| **host** | The SalesForce.com host. | na17.salesforce.com |
| **organizationId** | The organization id for SalesForce.com. | null |
| **password** | The password for SalesForce.com. | null |
| **S3Bucket** | The bucket to store the backup in AWS S3. | monthlybackups |
| **S3Folder** | The folder to store the backup in AWS S3. | salesforce |
| **scheme** | The schema for connecting to SalesForce.com. | https |
| **uploader** | The uploader to use for the backup. Possible values are 'AWS' or 'Azure'. | Azure |
| **username** | The username for SalesForce.com. | null |

To run, simply execute the SalesForcebackup.exe at the command prompt. Optionally, you can pass in some arguments at runtime:

Usage: SalesForceBackup.exe [-hupasyz]

Options:
        -h or --help    Displays this help text
        -u              Username for SalesForce
        -p              Password for SalesForce
        -a              AWS access key
        -s              AWS secret key
        -y              Azure account name
        -z              Azure shared key
