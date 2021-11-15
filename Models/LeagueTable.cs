using System.Collections.Generic;
using Newtonsoft.Json;

namespace LeagueCalculator.Models
{
    public class LeagueTable
    {
        public LeagueTable(List<LeagueTableEntry> entries)
        {
            LeagueTableEntries = entries;
        }

        [JsonProperty("LeagueTableEntries")]
        public List<LeagueTableEntry> LeagueTableEntries { get; set; }
    }
}