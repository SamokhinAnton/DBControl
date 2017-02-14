using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBControl
{
    public class ControlRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Control"].ConnectionString;

        public IEnumerable<MatchDto> GetMatches()
        {
            //--ALTER TABLE dbo.GemaPlayers ADD Id Int not NULL identity(1, 1) primary key;
            //--Create table GameScoredPlayers (
            //--GamePlayersId int Foreign key REFERENCES GemaPlayers not null,
            //--Minute int not null
            //--)
            //--go
            string sql = @"select m.Id, m.Stadium, m.TeamA, ta.Name, m.TeamAScore, m.TeamB, tb.Name, m.TeamBScore, p.Id, p.TeamId, p.Name, gsp.Minute
                from Matches m
                 left join GemaPlayers gp on gp.MatchId = m.Id
                 left join Players p on p.Id = gp.PlayerId
                 left join Teams ta on m.TeamA = ta.Id
                 left join Teams tb on m.TeamB = tb.Id
                 left join GameScoredPlayers gsp on gsp.GamePlayersId = gp.Id
                Where m.TeamA is not null and m.TeamB is not null and p.TeamId is not null
                Order By m.Id";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                Dictionary<int, MatchDto> matches = new Dictionary<int, MatchDto>();
                connection.Open();
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var matchId = reader.SafeGetInt32(0);
                        var playerId = reader.SafeGetInt32(8);
                        var playerTeam = reader.SafeGetInt32(9);
                        var k = ((reader.SafeGetInt32(2) - reader.SafeGetInt32(5)) / 2 + reader.SafeGetInt32(5));
                        var playerScored = reader.SafeGetInt32(11);
                        MatchDto match;
                        PlayerDto player;
                        if (!matches.TryGetValue(matchId, out match))
                        {
                            match = new MatchDto
                            {
                                MatchId = matchId,
                                Stadium = reader.GetString(1),
                                TeamAId = reader.SafeGetInt32(2),
                                TeamAName = reader.SafeGetString(3, "TeamB default name"),
                                TeamAScore = reader.SafeGetInt32(4),
                                TeamBId = reader.SafeGetInt32(5),
                                TeamBName = reader.SafeGetString(6, "TeamB default name"),
                                TeamBScore = reader.SafeGetInt32(7),
                                TeamAPlayers = new List<PlayerDto>(),
                                TeamBPlayers = new List<PlayerDto>()
                            };
                            player = new PlayerDto
                            {
                                Id = playerId,
                                Name = reader.SafeGetString(10),
                                TeamId = playerTeam,
                                ScoreMinutes = new List<int>()
                            };
                            if (playerTeam > k)
                            {
                                match.TeamAPlayers.Add(player);
                            }
                            else
                            {
                                match.TeamBPlayers.Add(player);
                            }
                            matches.Add(matchId, match);
                        }
                        else
                        {
                            if(match.TeamAPlayers.Any(p => p.Id == playerId))
                            {
                                if (playerScored != 0)
                                {
                                    match.TeamAPlayers.First(p => p.Id == playerId).ScoreMinutes.Add(playerScored);
                                }
                            } else if(match.TeamBPlayers.Any(p => p.Id == playerId))
                            {
                                if (playerScored != 0)
                                {
                                    match.TeamBPlayers.First(p => p.Id == playerId).ScoreMinutes.Add(playerScored);
                                }
                            } else
                            {
                                player = new PlayerDto
                                {
                                    Id = playerId,
                                    Name = reader.SafeGetString(10),
                                    TeamId = playerTeam,
                                    ScoreMinutes = new List<int>()
                                };
                                if (playerScored != 0)
                                {
                                    player.ScoreMinutes.Add(playerScored);
                                }
                                if (playerTeam > k)
                                {
                                    match.TeamAPlayers.Add(player);
                                }
                                else
                                {
                                    match.TeamBPlayers.Add(player);
                                }
                            }
                        }
                    }
                }
                return matches.Values;
            }
        }
    }
}
