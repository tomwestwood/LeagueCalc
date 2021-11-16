using System;
using System.Globalization;
using Newtonsoft.Json;

namespace LeagueCalculator.Models
{
    public class Fixture
    {
        [JsonProperty("Div")]
        public string Div { get; set; }

        [JsonProperty("Date")]
        public string Date { get; set; }

        [JsonProperty("Time")]
        public string Time { get; set; }

        [JsonProperty("HomeTeam")]
        public string HomeTeam { get; set; }

        [JsonProperty("AwayTeam")]
        public string AwayTeam { get; set; }

        [JsonProperty("FTHG")]
        public int FTHG { get; set; }

        [JsonProperty("FTAG")]
        public int FTAG { get; set; }

        [JsonProperty("FTR")]
        public string FTR { get; set; }

        [JsonProperty("HTHG")]
        public int HTHG { get; set; }

        [JsonProperty("HTAG")]
        public int HTAG { get; set; }

        [JsonProperty("HTR")]
        public string HTR { get; set; }

        [JsonProperty("Referee")]
        public string Referee { get; set; }

        [JsonProperty("KickOff")]
        public DateTime KickOff => DateTime.ParseExact(Date + " " + Time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
    }
}