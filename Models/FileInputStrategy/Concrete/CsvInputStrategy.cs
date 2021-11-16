using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using LeagueCalculator.Models.FileInputStrategy.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LeagueCalculator.Models.FileInputStrategy.Concrete
{
    public class CsvInputStrategy : IFileInputStrategy
    {
        protected readonly string _delimiter;

        public CsvInputStrategy(string delimeter)
        {
            _delimiter = delimeter;
        }

        public bool FileIsValid(string fileContents)
        {
            // We could use this to pre-check CSV related stuff like does the file contain required fields, is file correct format, etc.
            throw new NotImplementedException();
        }

        public FixturesUpload GetFixtureUploadFromFile(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Fixture>().ToList();
                return new FixturesUpload()
                {
                    Fixtures = records
                };
            }
        }
    }
}