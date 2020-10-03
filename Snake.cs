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

        internal enum GameState : byte
        {
            SelectKeyboardSize,
            SelectMode,
            Countdown,
            InProgress,
            Paused,
            Finished
        }
        
        internal const byte Food = 0xFD, Right = 0xFE, DoubleRight = 0xFF;
        
        internal static byte[][] Board = new byte[14][];

        internal static byte Speed
        {
            get => _speed;
            set
            {
                _speed = value;

                switch (_speed)
                {
                    case 1:
                        SleepTime = 800;
                        return;
                    
                    case 2:
                        SleepTime = 600;
                        return;
                    
                    case 3:
                        SleepTime = 400;
                        return;
                    
                    case 4:
                        SleepTime = 250;
                        return;
                    
                    default:
                        SleepTime = 150;
                        return;
                }
            }
        }

        internal static bool Is60Percent;
        internal static byte Countdown = 3;
        internal static ushort SleepTime = 400;
        internal static GameState State = GameState.SelectKeyboardSize;
        internal static Direction Dir;
        private static byte _snakeLength, _speed = 3;
        private static readonly Random Rng = new Random();

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
            State = GameState.Countdown;
            _snakeLength = 3;
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

                    if (Board[i][j] == _snakeLength)
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
                State = GameState.Finished;
                return;
            }

            switch (Board[x][y])
            {
                case Right when Dir == Direction.Left && Board[x-1][y] == DoubleRight:
                    x -= 2;
                    break;
                
                case Right when Dir == Direction.Left:
                    x -= 1;
                    break;
                
                case Right:
                    x++;
                    break;
                
                case DoubleRight when Dir == Direction.Left:
                    x -= 1;
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
                    Board[x2][y2] = _snakeLength;
                    _snakeLength++;
                    RefreshFood();
                    break;
                
                default:
                    State = GameState.Finished;
                    return;
            }
        }

        private static void RefreshFood()
        {
            byte x, y, i = 0;

            do
            {
                x = (byte) Rng.Next(Board.Length);
                y = (byte) Rng.Next(Board[x].Length);
                i++;
                if (i >= 80) return;
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