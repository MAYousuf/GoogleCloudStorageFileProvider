using System;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.FileProviders;

namespace GoogleCloudStorageFileProvider
{
    public class CloudStorageFileInfo : IFileInfo
    {
        private readonly Google.Apis.Storage.v1.Data.Object _gcpObject;
        private readonly IStorageClientFactory _storageClientFactory;
        private readonly string _subpath;

        public CloudStorageFileInfo(IStorageClientFactory storageClientFactory, Google.Apis.Storage.v1.Data.Object gcpObject, string subpath)
        {
            _storageClientFactory = storageClientFactory;
            _gcpObject = gcpObject ?? throw new ArgumentNullException(nameof(gcpObject));
            _subpath = subpath;
        }

        public bool Exists => _gcpObject != null;

        public long Length => MapUlongToLong(_gcpObject.Size);

        public string PhysicalPath => null;

        public string Name => _subpath.Length > 0 ? _gcpObject.Name.Remove(0, _subpath.Length).TrimEnd('/') : _gcpObject.Name.TrimEnd('/');

        public DateTimeOffset LastModified => _gcpObject.UpdatedDateTimeOffset ?? DateTimeOffset.MinValue;

        public bool IsDirectory => _gcpObject.Name.EndsWith("/");

        public Stream CreateReadStream()
        {
            var stream = new MemoryStream();
            _storageClientFactory.GetStorageClient().DownloadObject(_gcpObject, stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static long MapUlongToLong(ulong? ulongValue)
        {
            if (ulongValue.HasValue)
                return unchecked((long)ulongValue);
            else
                return unchecked((long)0);

        }

        public static ulong MapLongToUlong(long longValue)
        {
            return unchecked((ulong)(longValue - long.MinValue));
        }
    }
}

