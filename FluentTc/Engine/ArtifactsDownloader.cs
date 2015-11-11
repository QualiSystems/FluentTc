using System.Collections.Generic;
using System.IO.Abstractions;

namespace FluentTc.Engine
{
    internal interface IArtifactsDownloader
    {
        IList<string> DownloadArtifacts(int buildId, string destinationPath);
        string DownloadArtifact(int buildId, string destinationPath, string fileToDownload);
    }

    internal class ArtifactsDownloader : IArtifactsDownloader
    {
        private readonly ITeamCityCaller m_TeamCityCaller;
        private readonly IZipExtractor m_ZipExtractor;
        private readonly IFileSystem m_FileSystem;

        public ArtifactsDownloader(ITeamCityCaller teamCityCaller, IZipExtractor zipExtractor, IFileSystem fileSystem)
        {
            m_TeamCityCaller = teamCityCaller;
            m_ZipExtractor = zipExtractor;
            m_FileSystem = fileSystem;
        }

        public IList<string> DownloadArtifacts(int buildId, string destinationPath)
        {
            return DownloadAllFiles(buildId, destinationPath);
        }

        public string DownloadArtifact(int buildId, string destinationPath, string fileToDownload)
        {
            return DownloadSpecificFile(buildId, destinationPath, fileToDownload);
        }

        private string DownloadSpecificFile(int buildId, string destinationPath, string fileToDownload)
        {
            var downloadedFile = m_FileSystem.Path.Combine(destinationPath, fileToDownload);
            m_TeamCityCaller.GetDownloadFormat(
                s =>
                {
                    m_FileSystem.File.Move(s, downloadedFile);
                }, "/app/rest/builds/id:{0}/artifacts/content/{1}", buildId, fileToDownload);
            return downloadedFile;
        }

        private IList<string> DownloadAllFiles(int buildId, string destinationPath)
        {
            IList<string> extractedFiles = null;
            m_TeamCityCaller.GetDownloadFormat(
                s =>
                {
                    extractedFiles = m_ZipExtractor.ExtractZipFile(s, destinationPath);
                    m_FileSystem.File.Delete(s);
                }, "/downloadArtifacts.html?buildId={0}", buildId);

            return extractedFiles ?? new List<string>();
        }
    }
}