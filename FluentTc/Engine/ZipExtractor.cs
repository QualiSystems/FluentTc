using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace FluentTc.Engine
{
    internal interface IZipExtractor
    {
        IList<string> ExtractZipFile(string archiveFilenameIn, string outFolder, string password = null);
    }

    internal class ZipExtractor : IZipExtractor
    {
        private readonly IFileSystem m_FileSystem;

        public ZipExtractor(IFileSystem fileSystem)
        {
            m_FileSystem = fileSystem;
        }

        public IList<string> ExtractZipFile(string archiveFilenameIn, string outFolder, string password = null)
        {
            ZipFile zf = null;
            try
            {
                var fs = m_FileSystem.File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                if (!string.IsNullOrEmpty(password))
                {
                    zf.Password = password; // AES encrypted entries are handled automatically
                }
                var fileList = new List<string>();
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue; // Ignore directories
                    }
                    var entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    var buffer = new byte[4096]; // 4K is optimum
                    var zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    var fullZipToPath = Path.Combine(outFolder, entryFileName);
                    fileList.Add(fullZipToPath);
                    var directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        m_FileSystem.Directory.CreateDirectory(directoryName);

                    if (m_FileSystem.File.Exists(fullZipToPath))
                        RemoveReadOnly(fullZipToPath);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (var streamWriter = m_FileSystem.File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
                return fileList;
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
        }

        private void RemoveReadOnly(string filePath)
        {
            var fileAttributes = m_FileSystem.File.GetAttributes(filePath);

            if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                m_FileSystem.File.SetAttributes(filePath, fileAttributes ^ (FileAttributes.ReadOnly));
        }
    }
}