using System.Collections.Generic;

namespace LeagueCalculator.Models.FileInputStrategy.Interfaces
{
    public interface IFileInputStrategy
    {
        bool FileIsValid(string fileContents);
        FixturesUpload GetFixtureUploadFromFile(string fileContents);
    }
}