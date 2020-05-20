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
        
        private static KeyboardCustom _grid = KeyboardCustom.Create();
        private static IChroma _chroma;

        internal static async Task Init() =>  _chroma = await ColoreProvider.CreateNativeAsync();

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
            {
                for (byte j = 0; j < Snake.Board[i].Length; j++)
                {
                    _grid.Set(Color.Black);
                    _grid[KeyboardMap[i][j]] = Color.Green;
                    await _chroma.Keyboard.SetCustomAsync(_grid);
                    await Task.Delay(50);
                }
            }
            
            _grid.Set(Color.Black);
            await _chroma.Keyboard.SetCustomAsync(_grid);
        }
    }
}