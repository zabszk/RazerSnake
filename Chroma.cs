using System;
using System.Threading.Tasks;
using Colore;
using Colore.Data;
using Colore.Effects.Keyboard;
using ColoreColor = Colore.Data.Color;

namespace RazerSnake
{
    internal static class Chroma
    {
        private static readonly Key[][] KeyboardMap =
        {
            new [] {Key.LeftShift, Key.CapsLock, Key.Tab, Key.OemTilde}, new [] {Key.Z, Key.A, Key.Q, Key.D1 }, new [] {Key.X, Key.S, Key.W, Key.D2}, //0, 1, 2
            new [] {Key.C, Key.D, Key.E, Key.D3}, new [] {Key.V, Key.F, Key.R, Key.D4}, new [] {Key.B, Key.G, Key.T, Key.D5}, //3, 4, 5
            new [] {Key.N, Key.H, Key.Y, Key.D6}, new [] {Key.M, Key.J, Key.U, Key.D7}, new [] {Key.OemComma, Key.K, Key.I, Key.D8}, //6, 7, 8
            new [] {Key.OemPeriod, Key.L, Key.O, Key.D9}, new [] {Key.OemSlash, Key.OemSemicolon, Key.P, Key.D0}, //9, 10
            new [] {Key.RightShift, Key.OemApostrophe, Key.OemLeftBracket, Key.OemMinus}, new [] {Key.RightShift, Key.Enter, Key.OemRightBracket, Key.OemEquals}, //11, 12
            new [] {Key.RightShift, Key.Enter, Key.OemBackslash, Key.Backspace} //13
        };

        private static readonly Color SnakeHeadColor = Color.FromRgb(0x00B7EB);
        private static readonly Color DarkGreen = Color.FromRgb(0x315C28);
        private static readonly Color Gold = Color.FromRgb(0xFFD300);
        private static readonly Color DarkGold = Color.FromRgb(0x6B5C12);
        
        private static KeyboardCustom _grid = KeyboardCustom.Create();
        private static IChroma _chroma;

        internal static async void Init()
        {
            _chroma = await ColoreProvider.CreateNativeAsync();
            MainLoop();
        }

        private static async void MainLoop()
        {
            bool blinkState = false, finishReset = true;

            while (!MainForm.Disposing)
            {
                switch (Snake.State)
                {
                    case Snake.GameState.SelectMode:
                        blinkState = !blinkState;
                        _grid[Key.Enter] = blinkState ? Color.Green : Color.Black;
                        await ShowBoard();
                        await Task.Delay(250);
                        break;
                    
                    case Snake.GameState.Countdown:
                        finishReset = true;
                        await Task.Delay(1000);
                        if (Snake.Countdown <= 1)
                        {
                            Snake.State = Snake.GameState.InProgress;
                            await ShowBoard(true);
                            continue;
                        }
                        Snake.Countdown -= 1;
                        await ShowBoard();
                        break;
                    
                    case Snake.GameState.InProgress:
                        Snake.NextStep();
                        await ShowBoard();
                        await Task.Delay(Snake.SleepTime);
                        break;
                    
                    case Snake.GameState.Paused:
                        blinkState = !blinkState;
                        _grid[Key.Pause] = blinkState ? Color.Green : Color.Black;
                        await ShowBoard(finishReset);
                        finishReset = false;
                        await Task.Delay(250);
                        break;
                    
                    case Snake.GameState.Finished:
                        blinkState = !blinkState;
                        _grid[Key.End] = blinkState ? Color.Green : Color.Black;
                        await ShowBoard();
                        await Task.Delay(250);
                        break;
                    
                    default:
                        await Task.Delay(250);
                        break;
                }
            }
        }
        
        internal static async Task ShowBoard(bool reset = false)
        {
            if (reset)
                _grid.Set(Color.Black);
            
            switch (Snake.State)
            {
                case Snake.GameState.SelectMode:
                    switch (Snake.Speed)
                    {
                        case 1:
                            _grid[Key.D1] = Color.Green;
                            _grid[Key.D2] = DarkGreen;
                            _grid[Key.D3] = DarkGreen;
                            _grid[Key.D4] = DarkGreen;
                            _grid[Key.D5] = DarkGreen;
                            break;
                        
                        case 2:
                            _grid[Key.D1] = DarkGreen;
                            _grid[Key.D2] = Color.Green;
                            _grid[Key.D3] = DarkGreen;
                            _grid[Key.D4] = DarkGreen;
                            _grid[Key.D5] = DarkGreen;
                            break;
                        
                        case 3:
                            _grid[Key.D1] = DarkGreen;
                            _grid[Key.D2] = DarkGreen;
                            _grid[Key.D3] = Color.Green;
                            _grid[Key.D4] = DarkGreen;
                            _grid[Key.D5] = DarkGreen;
                            break;
                        
                        case 4:
                            _grid[Key.D1] = DarkGreen;
                            _grid[Key.D2] = DarkGreen;
                            _grid[Key.D3] = DarkGreen;
                            _grid[Key.D4] = Color.Green;
                            _grid[Key.D5] = DarkGreen;
                            break;
                        
                        case 5:
                            _grid[Key.D1] = DarkGreen;
                            _grid[Key.D2] = DarkGreen;
                            _grid[Key.D3] = DarkGreen;
                            _grid[Key.D4] = DarkGreen;
                            _grid[Key.D5] = Color.Green;
                            break;
                    }

                    if (reset)
                    {
                        _grid[Key.Enter] = Color.Green;
                        MainForm.Instance.statusLabel.Text =
                            @"Please select speed using numeric keys (1-5) and press ENTER.";
                    }

                    break;
                
                case Snake.GameState.Countdown:
                    if (reset)
                        ShowSnakeBoard();
                    
                    MainForm.Instance.statusLabel.Text =
                        $@"Game will begin in {Snake.Countdown} seconds.";
                    
                    if (Snake.Countdown > 2f)
                    {
                        _grid[Key.D1] = Color.Green;
                        _grid[Key.D2] = Color.Green;
                        _grid[Key.D3] = Color.Green;
                    }
                    else if (Snake.Countdown > 1f)
                    {
                        _grid[Key.D1] = Color.Green;
                        _grid[Key.D2] = Color.Green;
                        _grid[Key.D3] = Color.Black;
                    }
                    else
                    {
                        _grid[Key.D1] = Color.Green;
                        _grid[Key.D2] = Color.Black;
                        _grid[Key.D3] = Color.Black;
                    }
                    
                    if (reset)
                    {
                        _grid[Key.Up] = DarkGold;
                        _grid[Key.Down] = DarkGold;
                        _grid[Key.Left] = DarkGold;
                        _grid[Key.Right] = DarkGold;
                        
                        _grid[Key.End] = DarkGreen;
                        _grid[Key.Pause] = DarkGreen;
                    }
                    break;
                
                case Snake.GameState.InProgress:
                    ShowSnakeBoard();

                    if (reset)
                    {
                        _grid[Key.Up] = Gold;
                        _grid[Key.Down] = Gold;
                        _grid[Key.Left] = Gold;
                        _grid[Key.Right] = Gold;
                        
                        _grid[Key.End] = DarkGreen;
                        _grid[Key.Pause] = DarkGreen;
                        
                        MainForm.Instance.statusLabel.Text =
                            @"Game is in progress.";
                    }
                    break;

                case Snake.GameState.Paused:
                    if (reset)
                    {
                        ShowSnakeBoard();

                        _grid[Key.Up] = DarkGold;
                        _grid[Key.Down] = DarkGold;
                        _grid[Key.Left] = DarkGold;
                        _grid[Key.Right] = DarkGold;

                        _grid[Key.End] = DarkGreen;
                        _grid[Key.Pause] = Color.Green;

                        MainForm.Instance.statusLabel.Text =
                            @"Game paused. Press PAUSE key to unpause.";
                    }
                    break;
                
                case Snake.GameState.Finished:
                    if (reset)
                    {
                        _grid[Key.Up] = Color.Black;
                        _grid[Key.Down] = Color.Black;
                        _grid[Key.Left] = Color.Black;
                        _grid[Key.Right] = Color.Black;

                        _grid[Key.End] = Color.Green;
                        _grid[Key.Pause] = Color.Black;
                        
                        MainForm.Instance.statusLabel.Text =
                            @"Game over. Press END key to restart.";
                    }

                    break;
            }

            await _chroma.Keyboard.SetCustomAsync(_grid);
        }

        private static void ShowSnakeBoard()
        {
            for (byte i = 0; i < Snake.Board.Length; i++)
            for (byte j = 0; j < Snake.Board[i].Length; j++)
                switch (Snake.Board[i][j])
                {
                    case 0:
                        _grid[KeyboardMap[i][j]] = Color.Black;
                        break;

                    case Snake.Food:
                        _grid[KeyboardMap[i][j]] = Color.HotPink;
                        break;

                    case 1:
                        _grid[KeyboardMap[i][j]] = SnakeHeadColor;
                        break;

                    default:
                        _grid[KeyboardMap[i][j]] = Color.Blue;
                        break;
                }
        }

        internal static async Task MapTest()
        {
            for (byte i = 0; i < Snake.Board.Length; i++)
            {
                _grid.Set(Color.Black);
                for (byte j = 0; j < Snake.Board[i].Length; j++)
                    _grid[KeyboardMap[i][j]] = Color.Green;

                await _chroma.Keyboard.SetCustomAsync(_grid);
                await Task.Delay(500);
            }
            
            for (byte i = 0; i < Snake.Board.Length; i++)
                for (byte j = 0; j < Snake.Board[i].Length; j++)
                {
                    _grid.Set(Color.Black);
                    _grid[KeyboardMap[i][j]] = Color.Green;
                    await _chroma.Keyboard.SetCustomAsync(_grid);
                    await Task.Delay(50);
                }

            _grid.Set(Color.Black);
            await _chroma.Keyboard.SetCustomAsync(_grid);
        }
    }
}