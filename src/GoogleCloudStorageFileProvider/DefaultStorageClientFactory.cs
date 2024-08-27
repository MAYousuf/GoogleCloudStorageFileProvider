using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System;

namespace GoogleCloudStorageFileProvider
{
    public class DefaultStorageClientFactory : IStorageClientFactory
    {
        private readonly StorageClient _storageClient;

        public DefaultStorageClientFactory(CloudStorageOptions cloudStorageOptions)
        {
            _storageClient = StorageClient.Create(cloudStorageOptions.Credential ?? GoogleCredential.FromFile(cloudStorageOptions.CredentialFile));
        }

        public StorageClient GetStorageClient()
        {
            return _storageClient;
        }

        public string TransformPath(string subpath)
        {
            return subpath.TrimStart('/').TrimEnd('/');
        }
    }
}
