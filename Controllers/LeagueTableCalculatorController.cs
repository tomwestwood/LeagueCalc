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
        private List<string> acceptedFileTypes = new List<string> {"xls", "xslx", "csv"};

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
                    case "csv":
                    case "xls":
                    case "xslx":
                        leagueTableCalculator = new LeagueTableCalculator(new ExcelInputStrategy());
                        break;
                    default: 
                        throw new Exception("Unsupported file type...");  
                }

                var stringFileContents = FileTools.GetFileStringContents(file);
                return leagueTableCalculator.GetFixtureUpload(stringFileContents);
            }
            catch(Exception ex)
            {
                throw new Exception($"Problem uploading file... {ex.Message.ToString()}");
            }
        }
    }
}
