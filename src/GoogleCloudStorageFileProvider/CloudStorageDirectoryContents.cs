using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.FileProviders;

namespace GoogleCloudStorageFileProvider
{
    public class CloudStorageDirectoryContents : IDirectoryContents
    {
        private readonly IEnumerable<IFileInfo> _contents;

        public CloudStorageDirectoryContents(IEnumerable<IFileInfo> contents)
        {
            _contents = contents;
        }

        public bool Exists => _contents != null && _contents.Any();

        public IEnumerator<IFileInfo> GetEnumerator() => _contents.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _contents.GetEnumerator();
    }
}

