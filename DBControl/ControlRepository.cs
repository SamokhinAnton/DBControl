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
            string sql = @"select m.Id, m.Stadium, m.TeamA, ta.Name, m.TeamB, tb.Name, p.Id, p.TeamId, p.Name, gsp.Minute
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
                        var playerId = reader.SafeGetInt32(6);
                        var playerTeam = reader.SafeGetInt32(7);
                        var k = ((reader.SafeGetInt32(2) - reader.SafeGetInt32(4)) / 2 + reader.SafeGetInt32(4));
                        var playerScored = reader.SafeGetInt32(9);
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
                                TeamBId = reader.SafeGetInt32(4),
                                TeamBName = reader.SafeGetString(5, "TeamB default name"),
                                Players = new List<PlayerDto>(),
                                ScoredPlayers = new List<ScoresDto>()
                            };
                            matches.Add(matchId, match);
                        }
                        if (!match.Players.Any(p => p.Id == playerId))
                        {
                            player = new PlayerDto
                            {
                                Id = playerId,
                                Name = reader.SafeGetString(8),
                                TeamId = playerTeam > k ? match.TeamAId : match.TeamBId,
                            };
                            match.Players.Add(player);
                        }
                        if (playerScored != -1)
                        {
                            match.ScoredPlayers.Add(new ScoresDto()
                            {
                                Id = playerId,
                                Name = reader.SafeGetString(8),
                                Minute = playerScored,
                                TeamId = playerTeam > k ? match.TeamAId : match.TeamBId,
                            });
                        }
                    }
                }
                return matches.Values;
            }
        }
    }
}
