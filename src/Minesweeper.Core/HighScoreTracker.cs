using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core
{
    public class HighScoreTracker
    {
        private const string dir = "data";
        private const string file = "highscores.csv";
        public string path = Path.Combine(dir, file);

        public List<HighScore> scores = new List<HighScore>();
        public HighScoreTracker()
        {
            LoadScores();
        }

        public void LoadScores ()
        {
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, "size,seconds,moves,seed,timestamp\n");
                    return;
                }
                foreach (var line in File.ReadLines(path).Skip(1))
                {
                    var values = line.Split(',');

                    if (values.Length != 5)
                        continue;

                    bool trySize = int.TryParse(values[0], out int Size);
                    bool trySeconds = int.TryParse(values[1], out int Seconds);
                    bool tryMoves = int.TryParse(values[2], out int Moves);
                    bool trySeed = int.TryParse(values[3], out int Seed);
                    bool tryTime = DateTime.TryParse(values[4], out DateTime Timestamp);

                    if (trySize && trySeconds && tryMoves && trySeed && tryTime)
                    {
                        HighScore score = new HighScore
                        {
                            size = Size,
                            seconds = Seconds,
                            moves = Moves,
                            seed = Seed,
                            timestamp = Timestamp
                        };

                        scores.Add(score);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading high scores to list: {e.Message}");
            }
        }

        public void AddNewScore(int size, int seconds, int moves, int seed, DateTime timestamp)
        {
            scores.Add(new HighScore
            {
                size = size,
                seconds = seconds,
                moves = moves,
                seed = seed,
                timestamp = timestamp
            });

        }

        public void SaveScoresToFile ()
        {
            try
            {
                var sortedScores = scores.GroupBy(s => s.size)
                    .SelectMany(g =>
                    g.OrderBy(s => s.seconds)
                    .ThenBy(s => s.moves)
                    .Take(5))
                    .ToList();

                using (var writer = new StreamWriter(path))
                {
                    writer.WriteLine("size,seconds,moves,seed,timestamp");
                    foreach (var score in sortedScores)
                    {
                        writer.WriteLine($"{score.size},{score.seconds},{score.moves},{score.seed},{score.timestamp}");
                    }    
                }
                scores = sortedScores;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error saving scores: {e.Message}");
            }
        }
        
        public List<HighScore> TopFive (int size)
        {
            List<HighScore> topFive = scores
                .Where(s => s.size == size)
                .OrderBy(s => s.seconds)
                .ThenBy(s => s.moves)
                .Take(5)
                .ToList();
            return topFive;
        }
    }
}
