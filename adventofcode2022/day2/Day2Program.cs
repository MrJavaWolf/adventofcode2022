using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adventofcode2022.day2;



class Day2Program
{
    private class Game
    {
        public RockScissorPaperChoice You { get; set; }
        public RockScissorPaperChoice Opponent { get; set; }

        public GameResult Result
        {
            get
            {
                if (You == Opponent)
                    return GameResult.Draw;
                if (You == RockScissorPaperChoice.Rock && Opponent == RockScissorPaperChoice.Scissors)
                    return GameResult.Win;
                if (You == RockScissorPaperChoice.Scissors && Opponent == RockScissorPaperChoice.Paper)
                    return GameResult.Win;
                if (You == RockScissorPaperChoice.Paper && Opponent == RockScissorPaperChoice.Rock)
                    return GameResult.Win;
                return GameResult.Defeat;
            }
        }

        public int GetYourScore()
        {
            if (You == RockScissorPaperChoice.Rock) return 1;
            else if (You == RockScissorPaperChoice.Paper) return 2;
            return 3;
        }

        public int GetFinalGameScore()
        {
            int score = GetYourScore();
            if (Result == GameResult.Win) score = score + 6;
            else if (Result == GameResult.Draw) score = score + 3;
            else if (Result == GameResult.Defeat) score = score + 0;
            return score;
        }
    }

    private enum GameResult
    {
        Win,
        Draw,
        Defeat
    }

    private enum RockScissorPaperChoice
    {
        Rock,
        Scissors,
        Paper
    }


    public static void Run()
    {
        List<Game> games = LoadStratigyGuidePart2();
        for(int i = 0; i < games.Count; i++)
        {
            int finalScore = games[i].GetFinalGameScore();
        }
        int totalScore = games.Sum(x => x.GetFinalGameScore());
        Console.WriteLine($"Total score from game plan: {totalScore}");
    }

    private static List<Game> LoadStratigyGuidePart1()
    {
        string inputFile = "day2/input-day2.txt";
        string[] lines = File.ReadAllLines(inputFile);
        List<Game> games = new List<Game>();
        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i])) continue;

            Game game = new Game();
            if (lines[i][0] == 'A')
                game.Opponent = RockScissorPaperChoice.Rock;
            else if (lines[i][0] == 'B')
                game.Opponent = RockScissorPaperChoice.Paper;
            else if (lines[i][0] == 'C')
                game.Opponent = RockScissorPaperChoice.Scissors;

            if (lines[i][2] == 'X')
                game.You = RockScissorPaperChoice.Rock;
            if (lines[i][2] == 'Y')
                game.You = RockScissorPaperChoice.Paper;
            if (lines[i][2] == 'Z')
                game.You = RockScissorPaperChoice.Scissors;
            games.Add(game);
        }
        return games;
    }
    private static List<Game> LoadStratigyGuidePart2()
    {
        string inputFile = "day2/input-day2.txt";
        string[] lines = File.ReadAllLines(inputFile);
        List<Game> games = new List<Game>();
        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i])) continue;

            Game game = new Game();
            if (lines[i][0] == 'A')
                game.Opponent = RockScissorPaperChoice.Rock;
            else if (lines[i][0] == 'B')
                game.Opponent = RockScissorPaperChoice.Paper;
            else if (lines[i][0] == 'C')
                game.Opponent = RockScissorPaperChoice.Scissors;

            GameResult gameResult = GameResult.Win;
            if (lines[i][2] == 'X')
                gameResult = GameResult.Defeat;
            if (lines[i][2] == 'Y')
                gameResult = GameResult.Draw;
            if (lines[i][2] == 'Z')
                gameResult = GameResult.Win;

            if(game.Opponent == RockScissorPaperChoice.Rock && gameResult == GameResult.Win)
                game.You = RockScissorPaperChoice.Paper;
            else if (game.Opponent == RockScissorPaperChoice.Rock && gameResult == GameResult.Draw)
                game.You = RockScissorPaperChoice.Rock;
            else if (game.Opponent == RockScissorPaperChoice.Rock && gameResult == GameResult.Defeat)
                game.You = RockScissorPaperChoice.Scissors;

            else if (game.Opponent == RockScissorPaperChoice.Scissors && gameResult == GameResult.Win)
                game.You = RockScissorPaperChoice.Rock;
            else if (game.Opponent == RockScissorPaperChoice.Scissors && gameResult == GameResult.Draw)
                game.You = RockScissorPaperChoice.Scissors;
            else if (game.Opponent == RockScissorPaperChoice.Scissors && gameResult == GameResult.Defeat)
                game.You = RockScissorPaperChoice.Paper;

            else if (game.Opponent == RockScissorPaperChoice.Paper && gameResult == GameResult.Win)
                game.You = RockScissorPaperChoice.Scissors;
            else if (game.Opponent == RockScissorPaperChoice.Paper && gameResult == GameResult.Draw)
                game.You = RockScissorPaperChoice.Paper;
            else if (game.Opponent == RockScissorPaperChoice.Paper && gameResult == GameResult.Defeat)
                game.You = RockScissorPaperChoice.Rock;

            games.Add(game);
        }
        return games;
    }
}
