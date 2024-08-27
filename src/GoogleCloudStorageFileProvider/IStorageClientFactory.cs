using Google.Cloud.Storage.V1;

namespace GoogleCloudStorageFileProvider
{
    //public interface IBlobContainerFactory
    //{
    //    CloudBlobContainer GetContainer(string subpath);
    //    string TransformPath(string subpath);
    //}

    public interface IStorageClientFactory
    {
        StorageClient GetStorageClient();
    }
}
