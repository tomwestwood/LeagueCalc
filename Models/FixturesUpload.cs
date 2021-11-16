using System.Collections.Generic;
using Newtonsoft.Json;

namespace LeagueCalculator.Models
{
    public class FixturesUpload
    {
        public FixturesUpload()
        {
            Fixtures = new List<Fixture>();
            UploadSettings = new UploadSettings();
        }

        [JsonProperty("Fixtures")]
        public List<Fixture> Fixtures { get; set; }

        [JsonProperty("UploadSettings")]
        public UploadSettings UploadSettings { get; set; }
    }
}