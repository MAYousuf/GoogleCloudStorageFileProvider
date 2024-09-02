# GoogleCloudStorageFileProvider

Google Cloud Storage file provider (`IFileProvider`) for ASP.NET Core.

### Installation

```
Install-Package GoogleCloudStorageFileProvider
```

### Usage

Configure access to your Google Storage bucket via json credential file. 

Below is the usage example for both flows - where access to files from Storage bucket is enabled on the `/files` route (including directory browsing in the browser).

**Progarm.cs**

```csharp 
    using GoogleCloudStorageFileProvider;
    using Microsoft.Extensions.FileProviders;

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSingleton<IFileProvider>(sp =>
    {
        var cloudStorageOptions = builder.Configuration.GetSection("GoogleCloudStorageOptions").Get<CloudStorageOptions>();
        return new CloudStorageFileProvider(cloudStorageOptions);
    });

    var app = builder.Build();

    var cloudStorageFileProvider = app.Services.GetRequiredService<IFileProvider>();
    app.UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = cloudStorageFileProvider,
        RequestPath = "/files"
    });

    app.UseDirectoryBrowser(new DirectoryBrowserOptions
    {
        FileProvider = cloudStorageFileProvider,
        RequestPath = "/files"
    });

    app.MapGet("/", () => "Hello World!");

    app.Run();
```
We can optimize performance by caching files as below

```csharp
var cacheMaxAgeOneWeek = (60 * 60 * 24 * 7).ToString();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = cloudStorageFileProvider,
    RequestPath = "/files",

    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append(
             "Cache-Control", $"public, max-age={cacheMaxAgeOneWeek}");
    }
});
```

applicationsetting.json

```json
  "GoogleCloudStorageOptions": {
    "BucketName": "{gcp bucket}",
    "CredentialFile": "{credential file}"
  }
```

### Current limitations

The watch functionality of the file provider is currently not supported.

### Credits

This project is based on the work done in [Strathweb.AspNetCore.AzureBlobFileProvider](https://github.com/filipw/Strathweb.AspNetCore.AzureBlobFileProvider). 

