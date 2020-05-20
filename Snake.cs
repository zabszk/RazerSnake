using System;

namespace RazerSnake
{
    internal static class Snake
    {
        internal enum Direction : byte
        {
            Up,
            Down,
            Left,
            Right
        }
        
        internal const byte Food = 0xFD, Right = 0xFE, DoubleRight = 0xFF;
        
        internal static byte[][] Board = new byte[14][];

        internal static bool GameOver;
        private static byte SnakeLength;
        private static readonly Random Rng = new Random();
        private static Direction Dir;
        
        static Snake()
        {
            for (byte i = 0; i < Board.Length; i++)
            {
                switch (i)
                {
                    default:
                        Board[i] = new byte[] {0, 0, 0, 0};
                        break;
                    
                    case 11:
                        Board[i] = new byte[] {DoubleRight, 0, 0, 0};
                        break;
                    
                    case 12:
                        Board[i] = new byte[] {Right, Right, 0, 0};
                        break;
                }
            }
        }
        
        internal static void InitSnake()
        {
            ClearBoard();
            GameOver = false;
            SnakeLength = 3;
            Dir = Direction.Right;

            Board[1][1] = 3;
            Board[2][1] = 2;
            Board[3][1] = 1;
            
            RefreshFood();
        }

        internal static void NextStep()
        {
            sbyte x = 0, y = 0;
            byte x2 = 0, y2 = 0;
            
            for (byte i = 0; i < Board.Length; i++)
                for (byte j = 0; j < Board[i].Length; j++)
                {
                    if (Board[i][j] == 0 || Board[i][j] >= Food) continue;
                    if (Board[i][j] == 1)
                    {
                        x = (sbyte)i;
                        y = (sbyte)j;
                    }

                    if (Board[i][j] == SnakeLength)
                    {
                        x2 = i;
                        y2 = j;
                        
                        Board[i][j] = 0;
                    }
                    else Board[i][j]++;
                }

            switch (Dir)
            {
                case Direction.Up:
                    y++;
                    break;
                
                case Direction.Down:
                    y--;
                    break;
                
                case Direction.Left:
                    x--;
                    break;
                
                case Direction.Right:
                    x++;
                    break;
            }

            if (x < 0 || y < 0 || x >= Board.Length || y >= Board[x].Length)
            {
                GameOver = true;
                return;
            }

            switch (Board[x][y])
            {
                case Right:
                    x++;
                    break;
                
                case DoubleRight:
                    x += 2;
                    break;
            }

            switch (Board[x][y])
            {
                case 0:
                    Board[x][y] = 1;
                    break;
                
                case Food:
                    Board[x][y] = 1;
                    Board[x2][y2] = SnakeLength;
                    SnakeLength++;
                    break;
                
                default:
                    GameOver = true;
                    return;
            }
        }

        private static void RefreshFood()
        {
            byte x, y;

            do
            {
                x = (byte) Rng.Next(Board.Length);
                y = (byte) Rng.Next(Board[x].Length);
            } while (Board[x][y] != 0);

            Board[x][y] = Food;
        }

        private static void ClearBoard()
        {
            for (byte i = 0; i < Board.Length; i++)
                for (byte j = 0; j < Board[i].Length; j++)
                    if (Board[i][j] != Right && Board[i][j] != DoubleRight)
                        Board[i][j] = 0;
        }
    }
}