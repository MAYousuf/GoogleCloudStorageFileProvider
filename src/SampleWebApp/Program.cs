using Google.Apis.Auth.OAuth2;
using GoogleCloudStorageFileProvider;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IFileProvider>(sp =>
{
    var cloudStorageOptions = builder.Configuration.GetSection("GoogleCloudStorageOptions").Get<CloudStorageOptions>();

    //var cloudStorageOptions = new CloudStorageOptions()
    //{
    //    BucketName = "{bucketname}",
    //    Credential = GoogleCredential.FromFile("{CredentialFile}")
    //};

    return new CloudStorageFileProvider(cloudStorageOptions);
});

var app = builder.Build();

var cloudStorageFileProvider = app.Services.GetRequiredService<IFileProvider>();
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

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = cloudStorageFileProvider,
    RequestPath = "/files"
});

app.MapGet("/", () => "Hello World!");

app.Run();
