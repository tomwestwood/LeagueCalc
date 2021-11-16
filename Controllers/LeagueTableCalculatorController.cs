using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LeagueCalculator.Tools;
using LeagueCalculator.Models;
using LeagueCalculator.Models.FileInputStrategy.Concrete;
using LeagueCalculator.BusinessLogic;

namespace LeagueCalculator.Controllers
{
    [ApiController]
    [Route("api/league-calculator")]
    public class LeagueTableCalculatorController : ControllerBase
    {
        private List<string> acceptedFileTypes = new List<string> {".csv"};

        public LeagueTableCalculatorController() { }

        [HttpPost]
        public LeagueTable CalculateLeagueFromFile()
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                if (file == null || file.Length == 0)
                    throw new Exception($"No file or blank file uploaded...");

                var extensionType = Path.GetExtension(file.FileName);
                if(!acceptedFileTypes.Contains(extensionType))
                    throw new Exception($"Unsupported file type");

                LeagueTableCalculator leagueTableCalculator;
                switch(extensionType)
                {
                    case ".csv":
                        leagueTableCalculator = new LeagueTableCalculator(new CsvInputStrategy(Environment.NewLine));
                        break;
                    default: 
                        throw new Exception("Unsupported file type...");  
                }

                //var stringFileContents = FileTools.GetFileStringContents(file);
                var leagueTable = leagueTableCalculator.GetFixtureUpload(file);
                return leagueTable;
            }
            catch(Exception ex)
            {
                throw new Exception($"Problem uploading file... {ex.Message.ToString()}");
            }
        }
    }
}
