using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Tetris
{
	public partial class Form1 : Form
	{
		bool paused = false;
		int[] border = new int[10] {10, 30, 50, 70, 90, 110, 130, 150, 170, 200 };
		string game_over_msg = "GAME IS OVER\nYOUR SCORE:\n ";

		public Form1()
		{
			InitializeComponent();
		}
		Tetris_game game = new Tetris_game();

		private void Form1_Load(object sender, EventArgs e)
		{
			
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case (Keys.Up):
					if (!this.paused)
					{
					game.direction_key(orientation.up);
					}
					break;
				case (Keys.Down):
					if (!this.paused)
					{
					game.direction_key(orientation.down);
					}
					break;
				case (Keys.Right):
					if (!this.paused)
					{
					game.direction_key(orientation.right);
					}
					break;
				case (Keys.Left):
					if (!this.paused)
					{
					game.direction_key(orientation.left);
					}
					break;
				case (Keys.Space):
					pause();
					break;

			}
			turn();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (!this.paused)
			{
				//game.direction_key(orientation.down);
				turn();
			}

		}

		private void turn()
		{
			pictureBox1.Image = game.output();
			pictureBox2.Image = game.output2();
			score_summ();
			if (game.is_over())
			{
				pause();
				pause_button.Enabled = false;
				game_over_msg = String.Concat(game_over_msg, label5.Text);
				MessageBox.Show(game_over_msg);
			}

		}

		private void pause_button_Click(object sender, EventArgs e)
		{
			pause();
		}

		private void start_button_Click(object sender, EventArgs e)
		{
			start();
		}

		private void start()
		{
			game.new_game();
			timer1.Enabled = true;
			timer1.Interval = 1000;
			label4.Text = "0";
			label5.Text = "1";
			pictureBox1.Image = game.output();
			pictureBox2.Image = game.output2();
			start_button.Enabled = false;
			pause_button.Enabled = true;
			paused = false;
		}

		private void pause()
		{
			if (pause_button.Enabled)
			{
				if (this.paused)
				{
					this.paused = false;
					timer1.Enabled = true;
					start_button.Enabled = false;
				}
				else
				{
					this.paused = true;
					timer1.Enabled = false;
					start_button.Enabled = true;
				}
			}
		}

		private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			switch (e.KeyCode)
			{
				case (Keys.Up):
					e.IsInputKey = true;
					break;
				case (Keys.Down):
					e.IsInputKey = true;
					break;
				case (Keys.Right):
					e.IsInputKey = true;
					break;
				case (Keys.Left):
					e.IsInputKey = true;
					break;
			}
		}

		private void score_summ()
		{
			int buffer = Convert.ToInt32(label4.Text);
			buffer += game.return_score();
			enlarge_speed(buffer);
			label4.Text = buffer.ToString();
		}
		private void enlarge_speed(int score)
		{
			if (score > border[9])
			{
				timer1.Interval = 107;
				label5.Text = "11";
			}
			else if (score > border[8])
			{
				timer1.Interval = 134;
				label5.Text = "10";

			}
			else if (score > border[7])
			{
				timer1.Interval = 167;
				label5.Text = "9";
			}
			else if (score > border[6])
			{
				timer1.Interval = 209;
				label5.Text = "8";
			}
			else if (score > border[5])
			{
				timer1.Interval = 262;
				label5.Text = "7";
			}
			else if (score > border[4])
			{
				timer1.Interval = 327;
				label5.Text = "6";
			}
			else if (score > border[3])
			{
				timer1.Interval = 410;
				label5.Text = "5";
			}
			else if (score > border[2])
			{
				timer1.Interval = 512;
				label5.Text = "4";
			}
			else if (score > border[1])
			{
				timer1.Interval = 640;
				label5.Text = "3";
			}
			else if (score > border[0])
			{
				timer1.Interval = 800;
				label5.Text = "2";
			}
		}
	}
}
