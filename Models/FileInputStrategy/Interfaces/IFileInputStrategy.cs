using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace LeagueCalculator.Models.FileInputStrategy.Interfaces
{
    public interface IFileInputStrategy
    {
        bool FileIsValid(string fileContents);
        List<Fixture> GetFixturesFromFileUpload(IFormFile file);
    }
}