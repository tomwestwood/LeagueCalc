using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace LeagueCalculator.Models.FileInputStrategy.Interfaces
{
    public interface IFileInputStrategy
    {
        bool FileIsValid(string fileContents);
        string GetFileContents(IFormFile file);
        FixturesUpload GetFixtureUploadFromFile(string fileContents);
    }
}