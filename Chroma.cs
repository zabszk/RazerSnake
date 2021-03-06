﻿using System.Threading.Tasks;
using Colore;
using Colore.Data;
using Colore.Effects.Keyboard;

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
        private static readonly Color Crimson = Color.FromRgb(0xDC143C);
        
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

            while (!MainForm.Stop)
            {
                if (MainForm.Testing)
                {
                    await Task.Delay(200);
                    continue;
                }
                
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
                        _grid[Snake.Is60Percent ? Key.OemPeriod: Key.Pause] = blinkState ? Color.Green : Color.Black;
                        await ShowBoard();
                        await Task.Delay(250);
                        break;
                    
                    case Snake.GameState.Finished:
                        blinkState = !blinkState;
                        _grid[Snake.Is60Percent ? Key.OemTilde : Key.Escape] = blinkState ? Color.White : Color.Black;
                        _grid[Snake.Is60Percent ? Key.OemApostrophe : Key.End] = blinkState ? Color.Green : Color.Black;
                        await ShowBoard(finishReset);
                        finishReset = false;
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
                case Snake.GameState.SelectKeyboardSize when reset:
                    _grid[Key.D6] = Color.Green;
                    _grid[Key.R] = Color.Green;
                    MainForm.Instance.statusLabel.Text =
                        // ReSharper disable once LocalizableElement
                        "Please select keyboard size.\r\nR - Regular or TE/TKL\r\n6 - 60%";
                    break;
                
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
                        _grid[Snake.Is60Percent ? Key.OemTilde : Key.Escape] = Color.White;
                        _grid[Key.Enter] = Color.Green;
                        MainForm.Instance.statusLabel.Text =
                            @"Please select speed using numeric keys (1-5) and press ENTER.";
                    }

                    break;
                
                case Snake.GameState.Countdown:
                    if (reset)
                        ShowSnakeBoard();
                    
                    MainForm.Instance.statusLabel.Text =
                        $@"Game will begin in {Snake.Countdown} second{(Snake.Countdown == 1 ? "" : "s")}.";
                    
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
                        if (Snake.Is60Percent)
                        {
                            _grid[Key.I] = DarkGold;
                            _grid[Key.J] = DarkGold;
                            _grid[Key.K] = DarkGold;
                            _grid[Key.L] = DarkGold;
                            
                            _grid[Key.OemApostrophe] = Color.Black;
                            _grid[Key.OemPeriod] = Color.Black;
                        }
                        else
                        {
                            _grid[Key.Up] = DarkGold;
                            _grid[Key.Down] = DarkGold;
                            _grid[Key.Left] = DarkGold;
                            _grid[Key.Right] = DarkGold;
                        
                            _grid[Key.End] = Color.Black;
                            _grid[Key.Pause] = Color.Black;
                        }
                    }
                    break;
                
                case Snake.GameState.InProgress:
                    if (reset)
                    {
                        _grid[Key.LeftControl] = Color.White;
                        _grid[Key.LeftWindows] = Color.White;
                        _grid[Key.LeftAlt] = Color.White;
                        _grid[Key.Space] = Color.White;
                        _grid[Key.RightAlt] = Color.White;
                        _grid[Key.Function] = Color.White;
                        _grid[Key.RightMenu] = Color.White;
                        _grid[Key.RightControl] = Color.White;

                        if (!Snake.Is60Percent)
                        {
                            _grid[Key.F12] = Color.White;
                            _grid[Key.F11] = Color.White;
                            _grid[Key.F10] = Color.White;
                            _grid[Key.F9] = Color.White;
                            _grid[Key.F8] = Color.White;
                            _grid[Key.F7] = Color.White;
                            _grid[Key.F6] = Color.White;
                            _grid[Key.F5] = Color.White;
                            _grid[Key.F4] = Color.White;
                            _grid[Key.F3] = Color.White;
                            _grid[Key.F2] = Color.White;
                            _grid[Key.F1] = Color.White;
                            
                            _grid[Key.Up] = Gold;
                            _grid[Key.Down] = Gold;
                            _grid[Key.Left] = Gold;
                            _grid[Key.Right] = Gold;
                        
                            _grid[Key.End] = DarkGreen;
                            _grid[Key.Pause] = DarkGreen;
                            
                            _grid[Key.Escape] = Color.White;
                        }
                        else _grid[Key.OemTilde] = Color.White;

                        MainForm.Instance.statusLabel.Text =
                            @"Game is in progress.";
                    }
                    
                    ShowSnakeBoard();
                    
                    if (Snake.Is60Percent)
                    {
                        if (_grid[Key.I] == Color.Black)
                            _grid[Key.I] = Gold;
                        
                        if (_grid[Key.J] == Color.Black)
                            _grid[Key.J] = Gold;
                        
                        if (_grid[Key.K] == Color.Black)
                            _grid[Key.K] = Gold;
                        
                        if (_grid[Key.L] == Color.Black)
                            _grid[Key.L] = Gold;
                        
                        if (_grid[Key.OemApostrophe] == Color.Black)
                            _grid[Key.OemApostrophe] = DarkGreen;
                        
                        if (_grid[Key.OemPeriod] == Color.Black)
                            _grid[Key.OemPeriod] = DarkGreen;
                    }
                    break;

                case Snake.GameState.Paused:
                    if (reset)
                    {
                        if (Snake.Is60Percent)
                        {
                            _grid[Key.I] = Gold;
                            _grid[Key.J] = Gold;
                            _grid[Key.K] = Gold;
                            _grid[Key.L] = Gold;
                        
                            _grid[Key.OemApostrophe] = DarkGreen;
                            _grid[Key.OemPeriod] = Color.Green;
                        }
                        else
                        {
                            _grid[Key.Up] = DarkGold;
                            _grid[Key.Down] = DarkGold;
                            _grid[Key.Left] = DarkGold;
                            _grid[Key.Right] = DarkGold;
                        
                            _grid[Key.End] = DarkGreen;
                            _grid[Key.Pause] = Color.Green;
                        }

                        ShowSnakeBoard();

                        MainForm.Instance.statusLabel.Text =
                            @"Game paused. Press PAUSE key to unpause.";
                    }
                    break;
                
                case Snake.GameState.Finished:
                    if (reset)
                    {
                        if (Snake.Is60Percent)
                        {
                            _grid[Key.I] = Color.Black;
                            _grid[Key.J] = Color.Black;
                            _grid[Key.K] = Color.Black;
                            _grid[Key.L] = Color.Black;
                        
                            _grid[Key.OemApostrophe] = Color.Green;
                            _grid[Key.OemPeriod] = Color.Black;
                            _grid[Key.OemTilde] = Color.White;
                        }
                        else
                        {
                            _grid[Key.Up] = Color.Black;
                            _grid[Key.Down] = Color.Black;
                            _grid[Key.Left] = Color.Black;
                            _grid[Key.Right] = Color.Black;

                            _grid[Key.End] = Color.Green;
                            _grid[Key.Pause] = Color.Black;
                            _grid[Key.Escape] = Color.White;
                        }

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
                        _grid[KeyboardMap[i][j]] = Crimson;
                        break;

                    case 1:
                        _grid[KeyboardMap[i][j]] = SnakeHeadColor;
                        break;

                    default:
                        _grid[KeyboardMap[i][j]] = Color.Blue;
                        break;
                }
        }

        internal static async Task Border()
        {
            const int delay = 50;
            
            await _chroma.Keyboard.SetKeyAsync(Key.F12, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.F11, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.F10, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.F9, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.F8, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.F7, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.F6, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.F5, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.F4, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.F3, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.F2, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.F1, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.Escape, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.LeftControl, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.LeftWindows, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.LeftAlt, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.Space, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.RightAlt, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.Function, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.RightMenu, Color.White);
            await Task.Delay(delay);
            await _chroma.Keyboard.SetKeyAsync(Key.RightControl, Color.White);
            await Task.Delay(delay);
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