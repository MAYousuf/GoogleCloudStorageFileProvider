using System;
using System.Linq;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace GoogleCloudStorageFileProvider
{
    public class CloudStorageFileProvider : IFileProvider
    {
        private readonly IStorageClientFactory _storageClientFactory;
        private readonly CloudStorageOptions _cloudStorageOptions;

        public CloudStorageFileProvider(CloudStorageOptions cloudStorageOptions, IStorageClientFactory storageClientFactory)
        {
            _cloudStorageOptions = cloudStorageOptions;
            _storageClientFactory = storageClientFactory;
        }

        public CloudStorageFileProvider(CloudStorageOptions cloudStorageOptions)
        {
            _storageClientFactory = new DefaultStorageClientFactory(cloudStorageOptions);
            _cloudStorageOptions = cloudStorageOptions;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {

            subpath = NormalizePath(subpath);

            try
            {
                var objects = _storageClientFactory.GetStorageClient().ListObjects(_cloudStorageOptions.BucketName, subpath);
                var contents = objects.Select(o => new CloudStorageFileInfo(_storageClientFactory, o));
                return new CloudStorageDirectoryContents(contents);
            }
            catch
            {
                return NotFoundDirectoryContents.Singleton;
            }
        }

        public IFileInfo GetFileInfo(string subpath)
        {

            subpath = NormalizePath(subpath);

            try
            {
                var obj = _storageClientFactory.GetStorageClient().GetObject(_cloudStorageOptions.BucketName, subpath);
                return new CloudStorageFileInfo(_storageClientFactory, obj);
            }
            catch
            {
                return new NotFoundFileInfo(subpath);
            }
        }

        public IChangeToken Watch(string filter) => throw new NotImplementedException();

        private string NormalizePath(string subpath)
        {
            return subpath?.TrimStart('/') ?? string.Empty;
        }
    }
}

