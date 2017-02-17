using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBControl
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = new ControlRepository();
            var matches = repo.GetMatches();
            foreach (var match in matches)
            {
                Console.WriteLine($"Stadium - {match.Stadium}");
                Console.WriteLine($"{match.TeamAName} - {match.TeamBName} {match.ScoredPlayers.Count(sp => sp.TeamId == match.TeamAId)}:{match.ScoredPlayers.Count(sp => sp.TeamId == match.TeamBId)}");
                var scoredPlayers = match.ScoredPlayers
                    .OrderBy(m => m.Minute);
                foreach(var scoredPlayer in scoredPlayers)
                {
                    if (scoredPlayer.TeamId == match.TeamBId)
                        Console.Write('\t');
                    Console.WriteLine($"{scoredPlayer.Minute}' {scoredPlayer.Name}");
                }
                Console.WriteLine();
                Console.WriteLine($"Team {match.TeamAName} players");
                foreach (var player in match.Players.Where(p => p.TeamId == match.TeamAId))
                {
                    Console.WriteLine($"{player.Name}");
                }
                Console.WriteLine();
                Console.WriteLine($"Team {match.TeamBName} players");
                foreach (var player in match.Players.Where(p => p.TeamId == match.TeamBId))
                {
                    Console.WriteLine($"{player.Name}");
                }
                Console.WriteLine("---------------------------------------------------------------------------------------------------");
            }
        }
    }
}
