using System.Collections.Generic;
using Newtonsoft.Json;

namespace LeagueCalculator.Models
{
    public class LeagueTableEntry
    {
        [JsonProperty("TeamPosition")]
        public int TeamPosition { get; set; }

        [JsonProperty("TeamName")]
        public string TeamName { get; set; }

        [JsonProperty("GoalsScored")]
        public int GoalsScored { get; set; }

        [JsonProperty("GoalsConceded")]
        public int GoalsConceded { get; set; }

        [JsonProperty("GoalDifference")]
        public int GoalDifference { get; set; }

        [JsonProperty("Points")]
        public int Points { get; set; }

        [JsonProperty("Results")]
        public List<Fixture> Results { get; set; }
    }
}