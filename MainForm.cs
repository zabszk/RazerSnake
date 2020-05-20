using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RazerSnake
{
    public partial class MainForm : Form
    {
        internal static MainForm Instance;
        internal static bool Ready, Disposing = false;
        
        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Instance = this;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            Chroma.Init();
            await Task.Delay(1000);
            await Chroma.ShowBoard(true);
            Ready = true;
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            
        }

        private async void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            if (!Ready) return;
            
            switch (e.KeyCode)
            {
                case Keys.Up:
                    Snake.Dir = Snake.Direction.Up;
                    break;
                
                case Keys.Down:
                    Snake.Dir = Snake.Direction.Down;
                    break;
                
                case Keys.Left:
                    Snake.Dir = Snake.Direction.Left;
                    break;
                
                case Keys.Right:
                    Snake.Dir = Snake.Direction.Right;
                    break;
                
                case Keys.D1 when Snake.State == Snake.GameState.SelectMode:
                    Snake.Speed = 1;
                    await Chroma.ShowBoard();
                    break;
                
                case Keys.D2 when Snake.State == Snake.GameState.SelectMode:
                    Snake.Speed = 2;
                    await Chroma.ShowBoard();
                    break;
                
                case Keys.D3 when Snake.State == Snake.GameState.SelectMode:
                    Snake.Speed = 3;
                    await Chroma.ShowBoard();
                    break;
                
                case Keys.D4 when Snake.State == Snake.GameState.SelectMode:
                    Snake.Speed = 4;
                    await Chroma.ShowBoard();
                    break;
                
                case Keys.D5 when Snake.State == Snake.GameState.SelectMode:
                    Snake.Speed = 5;
                    await Chroma.ShowBoard();
                    break;
                
                case Keys.Enter when Snake.State == Snake.GameState.SelectMode:
                    Snake.Countdown = 3;
                    Snake.InitSnake();
                    Snake.State = Snake.GameState.Countdown;
                    await Chroma.ShowBoard(true);
                    break;
                
                case Keys.Pause when Snake.State == Snake.GameState.InProgress:
                    Snake.State = Snake.GameState.Paused;
                    await Chroma.ShowBoard(true);
                    break;
                
                case Keys.Pause when Snake.State == Snake.GameState.Paused:
                    Snake.Countdown = 3;
                    Snake.State = Snake.GameState.Countdown;
                    await Chroma.ShowBoard(true);
                    break;
                
                case Keys.End when Snake.State ==Snake.GameState.InProgress || Snake.State ==Snake.GameState.Paused || Snake.State ==Snake.GameState.Finished:
                    Snake.State = Snake.GameState.SelectMode;
                    await Chroma.ShowBoard(true);
                    break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disposing = true;
        }
    }
}