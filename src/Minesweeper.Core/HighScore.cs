using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core
{
    // HighScore contains the values for an individual high score entry.
    public class HighScore
    {
        public int size { get; set; }
        public int seconds { get; set; }
        public int moves { get; set; }
        public int seed { get; set; }
        public DateTime timestamp { get; set; }
    }
}
