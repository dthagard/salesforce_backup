# salesforce_backup
Performs automated backups of SalesForce.com data to either AWS or Azure.

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

		
License
=======

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
