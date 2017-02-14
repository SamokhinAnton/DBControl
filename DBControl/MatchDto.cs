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
        public int TeamAScore { get; set; }
        public int TeamBId { get; set; }
        public string TeamBName { get; set; }
        public int TeamBScore { get; set; }

        public List<PlayerDto> TeamAPlayers { get; set; }
        public List<PlayerDto> TeamBPlayers { get; set; }
    }
}
