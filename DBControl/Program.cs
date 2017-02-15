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
                Console.WriteLine($"{match.TeamAName} - {match.TeamBName} {match.TeamAScore}:{match.TeamBScore}");
                var scoredPlayers = match.TeamAPlayers
                    .Where(p => p.ScoreMinutes.Any())
                    .Concat(match.TeamBPlayers
                        .Where(p => p.ScoreMinutes.Any()))
                    .SelectMany(p => p.ScoreMinutes, (p, s) => new
                    {
                        PlayerId = p.Id,
                        PlayerName = p.Name,
                        Minute = s,
                        TeamId = p.TeamId
                    })
                    .OrderBy(m => m.Minute);
                foreach(var scoredPlayer in scoredPlayers)
                {
                    if (scoredPlayer.TeamId == match.TeamBId)
                        Console.Write('\t');
                    Console.WriteLine($"{scoredPlayer.Minute}' {scoredPlayer.PlayerName}");
                }
                Console.WriteLine();
                Console.WriteLine($"Team {match.TeamAName} players");
                foreach (var player in match.TeamAPlayers)
                {
                    Console.WriteLine($"{player.Name}");
                }
                Console.WriteLine();
                Console.WriteLine($"Team {match.TeamBName} players");
                foreach (var player in match.TeamBPlayers)
                {
                    Console.WriteLine($"{player.Name}");
                }
                Console.WriteLine("---------------------------------------------------------------------------------------------------");
            }
            
        }
    }
}
