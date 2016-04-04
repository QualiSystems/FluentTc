using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using FluentTc.Domain;

namespace FluentTc.Engine
{
    public interface IChangedFilesParser
    {
        IList<IChangedFile> ParseChangedFiles(string changedFilesPath);
    }

    public class ChangedFilesParser : IChangedFilesParser
    {
        private readonly IFileSystem m_FileSystem;
        private readonly char m_AltDirectorySeparatorChar;
        private readonly char m_DirectorySeparatorChar;

        public ChangedFilesParser(IFileSystem fileSystem)
        {
            m_FileSystem = fileSystem;
            m_AltDirectorySeparatorChar = m_FileSystem.Path.AltDirectorySeparatorChar;
            m_DirectorySeparatorChar = m_FileSystem.Path.DirectorySeparatorChar;
        }

        public IList<IChangedFile> ParseChangedFiles(string changedFilesPath)
        {
            return m_FileSystem.File.ReadAllLines(changedFilesPath)
                .Select(ParseChangedFile).ToList();
        }

        private IChangedFile ParseChangedFile(string fileLine)
        {
            var lineParts = fileLine.Split(':');
            if (lineParts.Length < 2)
            {
                throw new ArgumentException("Could not parse line " + fileLine);
            }

            var relativeFilePath = lineParts[0].Replace(m_AltDirectorySeparatorChar, m_DirectorySeparatorChar);
            return new ChangedFile(relativeFilePath, GetChangeStatus(lineParts[1]), lineParts[2]);
        }

        private static FileChangeStatus GetChangeStatus(string fileChangeStatus)
        {
            FileChangeStatus result;
            if (Enum.TryParse(fileChangeStatus, true, out result))
            {
                return result;
            }
            throw new ArgumentException("Could not parse FileStatusChange: " + fileChangeStatus);
        }
    }
}