using System;
using LeagueCalculator.Models.FileInputStrategy.Interfaces;

namespace LeagueCalculator.Models.FileInputStrategy.Concrete
{
    public class ExcelInputStrategy : IFileInputStrategy
    {
        public bool FileIsValid(string fileContents)
        {
            // stuff like does the file contain required fields?
            throw new NotImplementedException();
        }

        public FixturesUpload GetFixtureUploadFromFile(string fileContents)
        {
            throw new NotImplementedException();
        }
    }
}