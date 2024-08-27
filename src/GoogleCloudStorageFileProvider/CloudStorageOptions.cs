using Google.Apis.Auth.OAuth2;
using System;

namespace GoogleCloudStorageFileProvider
{
    public class CloudStorageOptions
    {

        /// <summary>
        /// GCP bucket name
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// GCP CredentialFile path
        /// </summary>
        public string CredentialFile { get; set; }

        /// <summary>
        /// Instance of GoogleCredential
        /// If provided CredentialFile will be ignored
        /// </summary>
        public GoogleCredential Credential { get; set; }

    }
}