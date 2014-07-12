my.winerack.io
==============

Track your wine collection

Configuration
-------------

ASP.Net Identity is configured to send account emails with SMTP via mailgun.org. The following `appSettings` are required for authentication:

 - `smtp:user`
 - `smtp:password`

Images are stored on an Azure Storage Blob, the connection string is required as an `appSetting`:

    <appSettings>
      <add key="StorageConnectionString" value="DefaultEndpointsProtocol=http;AccountName=ACCOUNT_NAME;AccountKey=ACCOUNT_KEY;"/>
    </appSettings>

Database Seed
-------------

A default administrator is created, `admin@winerack.io` and `P@ssw0rd`. If deploying into production, ensure you reset this password immediately.
