using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBControl
{
    public class MatchDto
    {
        public int MatchId { get; set; }
        public string Stadium { get; set; }
        public int TeamAId { get; set; }
        public string TeamAName { get; set; }
        public int TeamBId { get; set; }
        public string TeamBName { get; set; }

        public List<PlayerDto> Players { get; set; }

        public List<ScoresDto> ScoredPlayers { get; set; }
    }
}
