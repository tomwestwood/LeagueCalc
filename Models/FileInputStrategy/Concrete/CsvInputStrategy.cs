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
            // stuff like does the file contain required fields?
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

        // to delete...
        public FixturesUpload GetFixtureUploadFromFile_Old(string fileContents)
        {
            var fixtures = new FixturesUpload();
            var stringFixtureRows = new List<string>(fileContents.Split(_delimiter));

            // get header row locations:
            var headerRow = stringFixtureRows.First();
            var headerRowColumns = new List<string>(headerRow.Split(","));
            var index_of_div = headerRowColumns.IndexOf("Div");
            var index_of_date = headerRowColumns.IndexOf("Date");
            var index_of_home_team = headerRowColumns.IndexOf("HomeTeam");
            var index_of_away_team = headerRowColumns.IndexOf("AwayTeam");
            var index_of_full_time_home_goals = headerRowColumns.IndexOf("FTHG");
            var index_of_full_time_away_goals = headerRowColumns.IndexOf("FTAG");
            var index_of_full_time_result = headerRowColumns.IndexOf("FTR");

            stringFixtureRows.Skip(1).ToList().ForEach(fixRow =>
            {
                if (!string.IsNullOrWhiteSpace(fixRow))
                {
                    try
                    {
                        var fixtureRowContent = new List<string>(fixRow.Split(","));
                        fixtures.Fixtures.Add(new Fixture()
                        {
                            Div = fixtureRowContent[index_of_div],
                            HomeTeam = fixtureRowContent[index_of_home_team],
                            AwayTeam = fixtureRowContent[index_of_away_team],
                            FTHG = int.Parse(fixtureRowContent[index_of_full_time_home_goals]),
                            FTAG = int.Parse(fixtureRowContent[index_of_full_time_away_goals]),
                            FTR = fixtureRowContent[index_of_full_time_result]
                        });
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            });

            return fixtures;
        }
    }
}