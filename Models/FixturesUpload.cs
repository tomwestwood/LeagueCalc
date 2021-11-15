using System.Collections.Generic;
using Newtonsoft.Json;

namespace LeagueCalculator.Models
{
    public class FixturesUpload
    {
        [JsonProperty("Fixtures")]
        public List<Fixture> Fixtures { get; set; }

        [JsonProperty("UploadSettings")]
        public UploadSettings UploadSettings { get; set; }
    }
}