using Rock_Paper_Scissors.Models;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace RockPaperScissors
{
    public class Game
    {
        private readonly Player _player;
        private readonly Player _application;
        private int _turns;
        private Move _mostUsedMove;
        private readonly GameContext _context;

        public Game(string playerName)
        {
            _player = new Player
            {
                Name = playerName,
                Wins = 0,
                LastMove = Move.Rock
            };

            _application = new Player
            {
                Name = "Application",
                Wins = 0,
                LastMove = Move.Rock
            };

            _turns = 0;
            _mostUsedMove = Move.Rock;

            _context = new GameContext();
        }

        public void Start()
        {
            Console.WriteLine("Welcome to Rock Paper Scissors!");
            Console.WriteLine($"Player: {_player.Name}");
            Console.WriteLine($"Application: {_application.Name}");

            var existingPlayer = _context.Players.FirstOrDefault(p => p.Name == _player.Name);
            if (existingPlayer != null)
            {
                Console.WriteLine($"High Score: {existingPlayer.Wins}");
                Console.WriteLine($"Last Move: {existingPlayer.LastMove}");
            }

            while (true)
            {
                Console.WriteLine("\nChoose your move: (1) Rock, (2) Paper, (3) Scissors");
                Move playerMove = GetPlayerMove();

                if (playerMove == Move.Rock || playerMove == Move.Paper || playerMove == Move.Scissors)
                {
                    _player.LastMove = playerMove;
                    _turns++;

                    Console.Write("Waiting for the application's move... ");

                    Task loadingTask = AnimateLoading();

                    _application.LastMove = GenerateApplicationMove();

                    Random rnd = new Random();
                    int number = rnd.Next(1000, 3000);
                    Thread.Sleep(number);

                    int result = DetermineWinner();
                    DisplayStats();
                    SavePlayer(result);
                }
                else
                {
                    Console.WriteLine("Invalid move. Please try again.");
                }

                Console.WriteLine("Play again? (Y/N)");
                string playAgain = Console.ReadLine().Trim().ToUpper();

                if (playAgain != "Y")
                    break;
            }

            Console.WriteLine("Thanks for playing my C# Console Rock Paper Scissors!");
        }

        private async Task AnimateLoading()
        {
            string[] loadingFrames = { "/", "-", "\\", "|" };
            int frameIndex = 0;

            while (!Console.KeyAvailable)
            {
                Console.Write(loadingFrames[frameIndex]);
                await Task.Delay(250); // Delay between frames
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                frameIndex = (frameIndex + 1) % loadingFrames.Length;
            }

            Console.Clear();
        }

        private Move GetPlayerMove()
        {
            while (true)
            {
                Console.Write("Enter your move (1-3): ");
                if (Enum.TryParse(Console.ReadLine(), out Move move))
                {
                    return move;
                }
                else
                {
                    Console.WriteLine("Invalid move. Please try again.");
                }
            }
        }

        private Move GenerateApplicationMove()
        {
            Random random = new Random();
            return (Move)random.Next(1, 4);
        }

        private int DetermineWinner()
        {
            int result = (((int)_player.LastMove - (int)_application.LastMove) + 3) % 3;

            if (result == 1)
            {
                _application.Wins++;
            }
            else if (result == 2)
            {
                _player.Wins++;
            }
            return result;
        }

        private void DisplayStats()
        {
            Console.WriteLine("\n--- Game Results ---");
            if (_player.LastMove == _application.LastMove)
                Console.WriteLine("Nobody Wins.");
            Console.WriteLine($"Player: {_player.Name}, Wins: {_player.Wins}");
            Console.WriteLine($"Application: {_application.Name}, Wins: {_application.Wins}");
            Console.WriteLine($"Total Turns: {_turns}");
            Console.WriteLine($"Most Used Move: {_mostUsedMove}");
        }

        private void SavePlayer(int result)
        {
            var existingPlayer = _context.Players.FirstOrDefault(p => p.Name == _player.Name);
            if (existingPlayer != null)
            {
                if (result == 2)
                {
                    existingPlayer.Wins += 1;
                }
                existingPlayer.LastMove = _player.LastMove;
            }
            else
            {
                _context.Players.Add(_player);
            }
            _context.SaveChanges();
        }
    }
}
