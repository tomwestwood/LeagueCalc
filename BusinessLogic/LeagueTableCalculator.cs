using System;
using System.Linq;
using System.Collections.Generic;
using LeagueCalculator.Models;
using LeagueCalculator.Models.FileInputStrategy.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LeagueCalculator.BusinessLogic
{
    public class LeagueTableCalculator
    {
        private readonly IFileInputStrategy _inputStrategy;
        public FixturesUpload _fixtureUpload = new FixturesUpload();

        public LeagueTableCalculator(IFileInputStrategy inputStrategy)
            :this(inputStrategy, new FixturesUpload()) { }

        public LeagueTableCalculator(IFileInputStrategy inputStrategy, FixturesUpload fixturesUpload)
        {
            _inputStrategy = inputStrategy;
            _fixtureUpload = fixturesUpload;
        }

        public LeagueTable GetFixtureUpload(IFormFile file)
        {
            //if(!_inputStrategy.FileIsValid(fileStream))
            //throw new Exception("File contents are invalid");

            _fixtureUpload.Fixtures = _inputStrategy.GetFixturesFromFileUpload(file);
            return GetLeagueTable();
        }

        private LeagueTable GetLeagueTable()
        {
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

        private List<Fixture> GetResultsForTeam(string teamName) => _fixtureUpload.Fixtures.Where(fixture => fixture.HomeTeam == teamName || fixture.AwayTeam == teamName).ToList();

        private LeagueTableEntry GetLeagueTableEntry(string teamName, List<Fixture> results)
        {
            var leagueEntry = new LeagueTableEntry()
            {
                TeamName = teamName,
                GoalsScored = GetTeamGoalsScored(teamName, results),
                GoalsConceded = GetTeamGoalsConceded(teamName, results),
                Points = GetTeamPoints(teamName, results),
                GoalDifference = GetTeamGoalDifference(teamName, results),
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
            entries 
                .OrderByDescending(lte => lte.Points)
                .ThenByDescending(lte => lte.GoalDifference)
                .ThenByDescending(lte => lte.GoalsScored)
                .ThenBy(lte => lte.TeamName)
                .ToList()
                .ForEach(lte => 
            {
                lte.TeamPosition = GetJointRanking(entries, lte)?.TeamPosition ?? position;
                position++;
            });

            return new LeagueTable(entries.OrderBy(lte => lte.TeamPosition).ToList());
        }

        private LeagueTableEntry GetJointRanking(List<LeagueTableEntry> entries, LeagueTableEntry entry)
            => entries.Where(e => e != entry && e.TeamPosition > 0 && e.Points == entry.Points && e.GoalDifference == entry.GoalDifference && e.GoalsScored == entry.GoalsScored).FirstOrDefault();
    }
}