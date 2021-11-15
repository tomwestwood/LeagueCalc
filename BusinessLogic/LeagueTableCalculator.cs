using System;
using System.Linq;
using System.Collections.Generic;
using LeagueCalculator.Models;
using LeagueCalculator.Models.FileInputStrategy.Interfaces;

namespace LeagueCalculator.BusinessLogic
{
    public class LeagueTableCalculator
    {
        private readonly IFileInputStrategy _inputStrategy;
        private FixturesUpload _fixtureUpload;

        public LeagueTableCalculator(IFileInputStrategy inputStrategy)
        {
            _inputStrategy = inputStrategy;
        }

        public LeagueTable GetFixtureUpload(string fileStream)
        {
            if(!_inputStrategy.FileIsValid(fileStream))
                throw new Exception("File contents are invalid");

            var fixtureUpload = _inputStrategy.GetFixtureUploadFromFile(fileStream);
            return GetLeagueTable(fixtureUpload);
        }

        private LeagueTable GetLeagueTable(FixturesUpload fixtureUpload)
        {
            _fixtureUpload = fixtureUpload;
            var leagueTableEntries = new List<LeagueTableEntry>();
            var teams = GetTeams();
            teams.ForEach(team =>
            {
                var results = GetResultsForTeam(team);
                leagueTableEntries.Add(GetLeagueTableEntry(team, results));
            });

            return GetLeagueTable(leagueTableEntries);
        }

        private List<string> GetTeams()
        {
            var homeTeams = _fixtureUpload.Fixtures.Select(fixture => fixture.HomeTeam).Distinct();
            var awayTeams = _fixtureUpload.Fixtures.Select(fixture => fixture.AwayTeam).Distinct();
            return homeTeams.Union(awayTeams).Distinct().ToList();
        }

        private List<Fixture> GetResultsForTeam(string teamName)
        {
            var homeGames = _fixtureUpload.Fixtures.Where(fixture => fixture.HomeTeam == teamName);
            var awayGames = _fixtureUpload.Fixtures.Where(fixture => fixture.HomeTeam == teamName);
            return homeGames.Union(awayGames).ToList();
        }

        private LeagueTableEntry GetLeagueTableEntry(string teamName, List<Fixture> results)
        {
            var leagueEntry = new LeagueTableEntry()
            {
                TeamName = teamName,
                GoalsScored = GetTeamGoalsScored(teamName, results),
                GoalsConceded = GetTeamGoalsConceded(teamName, results),
                Points = GetTeamPoints(teamName, results),
                Results = results
            };

            return leagueEntry;
        }

        private int GetTeamGoalsScored(string teamName, List<Fixture> results)
        {
            return results.Where(result => result.HomeTeam == teamName).Sum(result => result.FTHG) 
                + results.Where(result => result.AwayTeam == teamName).Sum(result => result.FTAG);
        }

        private int GetTeamGoalsConceded(string teamName, List<Fixture> results)
        {
            return results.Where(result => result.HomeTeam == teamName).Sum(result => result.FTAG) 
                + results.Where(result => result.AwayTeam == teamName).Sum(result => result.FTHG);
        }

        private int GetTeamPoints(string teamName, List<Fixture> results)
        {
            return results.Where(result => result.HomeTeam == teamName).Sum(result => result.FTR == "H" ? _fixtureUpload.UploadSettings.PointsForAWin : result.FTR ==  "D" ? _fixtureUpload.UploadSettings.PointsForADraw : _fixtureUpload.UploadSettings.PointsForALoss ) 
                + results.Where(result => result.AwayTeam == teamName).Sum(result => result.FTR == "A" ? _fixtureUpload.UploadSettings.PointsForAWin : result.FTR ==  "D" ? _fixtureUpload.UploadSettings.PointsForADraw : _fixtureUpload.UploadSettings.PointsForALoss );
        }

        private int GetTeamGoalDifference(string teamName, List<Fixture> results)
        {
            return results.Where(result => result.HomeTeam == teamName).Sum(result => result.FTHG - result.FTAG) 
                + results.Where(result => result.AwayTeam == teamName).Sum(result => result.FTAG - result.FTHG);
        }

        private LeagueTable GetLeagueTable(List<LeagueTableEntry> entries)
        {
            var position = 1;
            entries.OrderByDescending(lte => lte.Points).ThenByDescending(lte => lte.GoalDifference).ToList().ForEach(lte => 
            {
                lte.TeamPosition = position;
                position++;
            });

            return new LeagueTable(entries.OrderByDescending(lte => lte.TeamPosition).ToList());
        }
    }
}