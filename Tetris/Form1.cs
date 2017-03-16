using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
		Tetris_game game = new Tetris_game();

		private void Form1_Load(object sender, EventArgs e)
		{
			
			game.new_game();
			pictureBox1.Image = game.output();
			pictureBox2.Image = game.output2();
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case (Keys.Up):
					game.direction_key(orientation.up);
					break;
				case (Keys.Down):
					game.direction_key(orientation.down);
					break;
				case (Keys.Right):
					game.direction_key(orientation.right);
					break;
				case (Keys.Left):
					game.direction_key(orientation.left);
					break;
			}
			pictureBox1.Image = game.output();
			pictureBox2.Image = game.output2();
		}
	}
}
