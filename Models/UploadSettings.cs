using Newtonsoft.Json;

namespace LeagueCalculator.Models
{
    public class UploadSettings
    {
        private const int _defaultPointsForAWin = 3;
        private const int _defaultPointsForADraw = 1;
        private const int _defaultPointsForALoss = 0;

        public UploadSettings()
        {
            PointsForAWin = _defaultPointsForAWin;
            PointsForADraw = _defaultPointsForADraw;
            PointsForALoss = _defaultPointsForALoss;
        }

        public UploadSettings(int pointsForAWin, int pointsForADraw, int pointsForALoss)
        {
            PointsForAWin = pointsForAWin;
            PointsForADraw = pointsForADraw;
            PointsForALoss = pointsForALoss;
        }

        [JsonProperty("PointsForAWin")]
        public int PointsForAWin { get; set; }

        [JsonProperty("PointsForADraw")]
        public int PointsForADraw { get; set; }

        [JsonProperty("PointsForALoss")]
        public int PointsForALoss { get; set; }
    }
}