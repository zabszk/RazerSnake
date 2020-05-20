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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await Chroma.Init();
        }
        
        private async void button1_Click(object sender, EventArgs e)
        {
            await Chroma.MapTest();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            
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
            }
        }
    }
}