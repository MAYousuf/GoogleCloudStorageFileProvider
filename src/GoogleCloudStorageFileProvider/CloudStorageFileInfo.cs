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

        public CloudStorageFileInfo(IStorageClientFactory storageClientFactory, Google.Apis.Storage.v1.Data.Object gcpObject)
        {
            _storageClientFactory = storageClientFactory;
            _gcpObject = gcpObject ?? throw new ArgumentNullException(nameof(gcpObject));
        }

        public bool Exists => _gcpObject != null;

        public long Length => MapUlongToLong(_gcpObject.Size);

        public string PhysicalPath => null;

        public string Name => Path.GetFileName(_gcpObject.Name);

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
    //public class AzureBlobFileInfo : IFileInfo
    //{
    //    private readonly CloudBlockBlob _blockBlob;

    //    public AzureBlobFileInfo(IListBlobItem blob)
    //    {
    //        switch (blob)
    //        {
    //            case CloudBlobDirectory d:
    //                Exists = true;
    //                IsDirectory = true;
    //                Name = ((CloudBlobDirectory)blob).Prefix.TrimEnd('/');
    //                break;

    //            case CloudBlockBlob b:
    //                _blockBlob = b;
    //                Name = !string.IsNullOrEmpty(b.Parent.Prefix) ? b.Name.Replace(b.Parent.Prefix, "") : b.Name;
    //                Exists = b.Exists();
    //                if (Exists)
    //                {
    //                    b.FetchAttributes();
    //                    Length = b.Properties.Length;
    //                    LastModified = b.Properties.LastModified ?? DateTimeOffset.MinValue;
    //                }
    //                else
    //                {
    //                    Length = -1;
    //                    // IFileInfo.PhysicalPath docs say: Return null if the file is not directly accessible.
    //                    // (PhysicalPath should maybe also be null for blobs that do exist but that would be a potentially breaking change.)
    //                    PhysicalPath = null;
    //                }
    //                break;
    //        }
    //    }

    //    public Stream CreateReadStream() => _blockBlob.OpenRead();

    //    public bool Exists { get; }
    //    public long Length { get; }
    //    public string PhysicalPath { get; }
    //    public string Name { get; }
    //    public DateTimeOffset LastModified { get; }
    //    public bool IsDirectory { get; }
    //}
}

