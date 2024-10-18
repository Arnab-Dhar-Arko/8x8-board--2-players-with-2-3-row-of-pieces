using System;

namespace BoardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            GameBoard board = new GameBoard();
            board.DisplayBoard();

            while (true)
            {
                Console.WriteLine($"Current Player: {board.CurrentPlayer}");
                Console.WriteLine("Enter your move (e.g., 'P1 2 3 3 4' to move P1 from (2,3) to (3,4)): ");
                string input = Console.ReadLine();

                if (!board.ProcessMove(input))
                {
                    board.IncrementWrongAttempt();
                    if (board.IsGameOver)
                    {
                        Console.WriteLine("Game Over: Too many invalid attempts. Exiting...");
                        break;
                    }
                    Console.WriteLine("Invalid move. Try again.");
                }
                else
                {
                    board.DisplayBoard();
                }
            }
        }
    }

    public class GameBoard
    {
        private const int Size = 8;
        private string[,] board;
        private string currentPlayer;
        private int wrongAttempts;
        public bool IsGameOver { get; private set; }

        public GameBoard()
        {
            board = new string[Size, Size];
            currentPlayer = "P1"; // Start with Player 1
            wrongAttempts = 0;
            IsGameOver = false;
            InitializeBoard();
        }

        public string CurrentPlayer => currentPlayer;

        private void InitializeBoard()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (i < 3 && (i + j) % 2 != 0) //  1 
                    {
                        board[i, j] = "P1";
                    }
                    else if (i > 4 && (i + j) % 2 != 0) //  2 
                    {
                        board[i, j] = "P2";
                    }
                    else
                    {
                        board[i, j] = ".";
                    }
                }
            }
        }

        public void DisplayBoard()
        {
            Console.Clear();
            Console.WriteLine($"Current Player: {currentPlayer}");
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public bool ProcessMove(string input)
        {
            string[] parts = input.Split(' ');
            if (parts.Length != 5) return false;

            string player = parts[0];
            if (player != currentPlayer) return false;

            if (int.TryParse(parts[1], out int fromX) && int.TryParse(parts[2], out int fromY) &&
                int.TryParse(parts[3], out int toX) && int.TryParse(parts[4], out int toY))
            {
                fromX -= 1; 
                fromY -= 1; 
                toX -= 1;   
                toY -= 1;   

                if (IsValidMove(fromX, fromY, toX, toY))
                {
                    // Move the piece
                    board[toX, toY] = player;
                    board[fromX, fromY] = ".";
                    SwitchPlayer();
                    return true;
                }
            }
            return false;
        }

        private bool IsValidMove(int fromX, int fromY, int toX, int toY)
        {
            
            if (!IsWithinBounds(fromX, fromY) || !IsWithinBounds(toX, toY) || board[fromX, fromY] != currentPlayer)
            {
                return false;
            }

            
            if (board[toX, toY] != ".")
            {
                return false;
            }

            
            int deltaX = Math.Abs(toX - fromX);
            int deltaY = Math.Abs(toY - fromY);
            if (deltaX == 1 && deltaY == 1)
            {
                return true;
            }

            return false;
        }

        private bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < Size && y >= 0 && y < Size;
        }

        private void SwitchPlayer()
        {
            currentPlayer = currentPlayer == "P1" ? "P2" : "P1";
            wrongAttempts = 0; 
        }

        public void IncrementWrongAttempt()
        {
            wrongAttempts++;
            if (wrongAttempts >= 2)
            {
                IsGameOver = true;
            }
        }
    }
}
