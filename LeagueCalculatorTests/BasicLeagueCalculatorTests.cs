using System.Collections.Generic;
using System.Linq;
using LeagueCalculator.BusinessLogic;
using LeagueCalculator.Models;
using LeagueCalculator.Models.FileInputStrategy.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace LeagueCalculatorTests
{
    public class BasicLeagueCalculatorTests
    {
        private Mock<IFileInputStrategy> _fileInputStrategyMock;
        private Mock<IFormFile> _fileMock;
        private LeagueTableCalculator _leagueTableCalculator;

        [SetUp]
        public void Setup()
        {
            _fileMock = new Mock<IFormFile>();
            _fileInputStrategyMock = new Mock<IFileInputStrategy>();
            _fileInputStrategyMock
                .Setup(x => x.GetFixturesFromFileUpload(It.IsAny<IFormFile>()))
                .Returns<IFormFile>(fu =>
                {
                    return GenerateFixtures();
                });

            _leagueTableCalculator = new LeagueTableCalculator(_fileInputStrategyMock.Object);
        }


        [TestCase("Team A", 6, 1)] // top by GD.
        [TestCase("Team B", 1, 4)]
        [TestCase("Team C", 6, 2)]
        [TestCase("Team D", 2, 3)]
        [Test]
        public void DefaultUpload_TeamHasCorrectNumberOfPointsAndPosition(string teamName, int expectedPoints, int expectedPosition)
        {
            
            var leagueTable = _leagueTableCalculator.GetFixtureUpload(_fileMock.Object);

            Assert.That(leagueTable.LeagueTableEntries.First(team => team.TeamName == teamName).Points == expectedPoints);
            Assert.That(leagueTable.LeagueTableEntries.First(team => team.TeamName == teamName).TeamPosition == expectedPosition);
        }

        [TestCase(5, 2, 0, "Team A", 10, 2)]
        [TestCase(5, 2, 0, "Team B", 2, 4)]
        [TestCase(5, 2, 0, "Team C", 11, 1)] // points for a draw would take this higher.
        [TestCase(5, 2, 0, "Team D", 4, 3)]
        [Test]
        public void CustomUpload_TeamHasCorrectNumberOfPointsAndPosition(int pointsForAWin, int pointsForADraw, int pointsForALoss, string teamName, int expectedPoints, int expectedPosition)
        {
            _leagueTableCalculator = new LeagueTableCalculator(_fileInputStrategyMock.Object, new FixturesUpload() { UploadSettings = new UploadSettings(pointsForAWin, pointsForADraw, pointsForALoss) });
            var leagueTable = _leagueTableCalculator.GetFixtureUpload(_fileMock.Object);

            Assert.That(leagueTable.LeagueTableEntries.First(team => team.TeamName == teamName).Points == expectedPoints);
            Assert.That(leagueTable.LeagueTableEntries.First(team => team.TeamName == teamName).TeamPosition == expectedPosition);
        }

        [TestCase("Team A", 6, 2)] 
        [TestCase("Team B", 1, 4)]
        [TestCase("Team C", 6, 1)] // top by goals scored.
        [TestCase("Team D", 2, 3)]
        [Test]
        public void CustomUpload_TeamHasCorrectNumberOfPointsAndPosition_GoalsScored(string teamName, int expectedPoints, int expectedPosition)
        {
            // amend team C's win over team B to ensure GD is level...
            _fileInputStrategyMock
                .Setup(x => x.GetFixturesFromFileUpload(It.IsAny<IFormFile>()))
                .Returns<IFormFile>(fu =>
                {
                    return GenerateFixturesAlternative();
                });

            var leagueTable = _leagueTableCalculator.GetFixtureUpload(_fileMock.Object);            

            Assert.That(leagueTable.LeagueTableEntries.First(team => team.TeamName == teamName).Points == expectedPoints);
            Assert.That(leagueTable.LeagueTableEntries.First(team => team.TeamName == teamName).TeamPosition == expectedPosition);
        }

        [TestCase("Team A", 6, 1)] // level on position as both have same points, GD _and_ goals scored
        [TestCase("Team B", 1, 4)]
        [TestCase("Team C", 6, 1)] // level on position as both have same points, GD _and_ goals scored
        [TestCase("Team D", 2, 3)]
        [Test]
        public void CustomUpload_TeamHasCorrectNumberOfPointsAndPosition_JointFinish(string teamName, int expectedPoints, int expectedPosition)
        {
            // amend team C's win over team B to ensure GD is level...
            _fileInputStrategyMock
                .Setup(x => x.GetFixturesFromFileUpload(It.IsAny<IFormFile>()))
                .Returns<IFormFile>(fu =>
                {
                    return GenerateFixturesAlternative2();
                });

            var leagueTable = _leagueTableCalculator.GetFixtureUpload(_fileMock.Object);

            Assert.That(leagueTable.LeagueTableEntries.First(team => team.TeamName == teamName).Points == expectedPoints);
            Assert.That(leagueTable.LeagueTableEntries.First(team => team.TeamName == teamName).TeamPosition == expectedPosition);
        }

        private List<Fixture> GenerateFixtures() =>
            new List<Fixture>
            {
                new Fixture() { HomeTeam = "Team A", AwayTeam = "Team B", FTHG = 1, FTAG = 0, FTR = "H" },
                new Fixture() { HomeTeam = "Team B", AwayTeam = "Team A", FTHG = 0, FTAG = 1, FTR = "A" },
                new Fixture() { HomeTeam = "Team C", AwayTeam = "Team D", FTHG = 1, FTAG = 1, FTR = "D" },
                new Fixture() { HomeTeam = "Team B", AwayTeam = "Team C", FTHG = 1, FTAG = 2, FTR = "A" },
                new Fixture() { HomeTeam = "Team D", AwayTeam = "Team C", FTHG = 1, FTAG = 1, FTR = "D" },
                new Fixture() { HomeTeam = "Team C", AwayTeam = "Team B", FTHG = 2, FTAG = 2, FTR = "D" },
            };

        private List<Fixture> GenerateFixturesAlternative() =>
            new List<Fixture>
            {
                new Fixture() { HomeTeam = "Team A", AwayTeam = "Team B", FTHG = 1, FTAG = 0, FTR = "H" },
                new Fixture() { HomeTeam = "Team B", AwayTeam = "Team A", FTHG = 0, FTAG = 1, FTR = "A" },
                new Fixture() { HomeTeam = "Team C", AwayTeam = "Team D", FTHG = 1, FTAG = 1, FTR = "D" },
                new Fixture() { HomeTeam = "Team B", AwayTeam = "Team C", FTHG = 1, FTAG = 3, FTR = "A" },
                new Fixture() { HomeTeam = "Team D", AwayTeam = "Team C", FTHG = 1, FTAG = 1, FTR = "D" },
                new Fixture() { HomeTeam = "Team C", AwayTeam = "Team B", FTHG = 2, FTAG = 2, FTR = "D" },
            };

        private List<Fixture> GenerateFixturesAlternative2() =>
            new List<Fixture>
            {
                new Fixture() { HomeTeam = "Team A", AwayTeam = "Team B", FTHG = 6, FTAG = 5, FTR = "H" },
                new Fixture() { HomeTeam = "Team B", AwayTeam = "Team A", FTHG = 0, FTAG = 2, FTR = "A" },
                new Fixture() { HomeTeam = "Team C", AwayTeam = "Team D", FTHG = 1, FTAG = 1, FTR = "D" },
                new Fixture() { HomeTeam = "Team B", AwayTeam = "Team C", FTHG = 1, FTAG = 4, FTR = "A" },
                new Fixture() { HomeTeam = "Team D", AwayTeam = "Team C", FTHG = 1, FTAG = 1, FTR = "D" },
                new Fixture() { HomeTeam = "Team C", AwayTeam = "Team B", FTHG = 2, FTAG = 2, FTR = "D" },
            };
    }
}