using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris
{
	enum fig_type { I, J, L, O, S, T, Z};
	enum orientation { up, right, left, down};

	class Tetris_game
	{
		Field field1;
		Field field2;
		Figure figure1;
		Figure figure2;
		Random random;
		
		int[][,] figures = new int[7][,]
		{
			new int [4,4]	{	{ 0, 0, 0, 0 },
								{ 1, 1, 1, 1 },
								{ 0, 0, 0, 0 },
								{ 0, 0, 0, 0 }
							},
			new int [3,3]  {	{ 0, 0, 0 },
								{ 1, 1, 1 },
								{ 0, 0, 1 }
							},
			new int [3,3]  {	{ 0, 0, 1 },
								{ 1, 1, 1 },
								{ 0, 0, 0 }
							},
			new int [2,2]  {	{ 1, 1 },
								{ 1, 1 }
							},
			new int [3,3]  {	{ 0, 1, 0 },
								{ 0, 1, 1 },
								{ 0, 0, 1 }
							},
			new int [3,3]  {	{ 0, 1, 0 },
								{ 0, 1, 1 },
								{ 0, 1, 0 }
							},
			new int [3,3]  {	{ 0, 0, 1 },
								{ 0, 1, 1 },
								{ 0, 1, 0 }
							}
		};

		Brush[] colors = new Brush[7]
		{
			Brushes.Blue,
			Brushes.DarkBlue,
			Brushes.Green,
			Brushes.Orange,
			Brushes.Red,
			Brushes.Violet,
			Brushes.Yellow
		};
		public Tetris_game()
		{
			int[] cell_size = new int[2] { 20, 20 };
			field1 = new Field(10, 20, cell_size);
			field2 = new Field(4, 4, cell_size);
			random = new Random();
		}
		public void new_game()
		{
			int[] cell_size = new int[2] { 20, 20 };
			field1.clear();
			field2.clear();
			figure1 = new Figure(figures[random.Next(6)], colors[random.Next(6)], new int[2] { 0, 0 });
			figure2 = new Figure(figures[random.Next(6)], colors[random.Next(6)], new int[2] { 0, 0 });
			cell_transfer(field1, figure1);
			//cell_transfer(field2, figure2);
			
		}
		private void cell_transfer(Field field, Figure figure)
		{
			int[,] matrix = figure.return_matrix();
			int[] fig_coordinates = figure.return_coordinates();
			int[] coordinates = new int[2];
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				coordinates[0] = i + fig_coordinates[0];
				for(int j = 0; j < matrix.GetLength(1); j++)
				{
					coordinates[1] = j + fig_coordinates[1]; 
					if(matrix[i, j] == 1)
					{
						field.new_cell(coordinates, figure.return_color());
					}
				}
			}
		}

		public void direction_key(orientation key)
		{
			switch (key)
			{
				case (orientation.down):
					if (figure1.move(0, 1))handle_changes();
					break;
				case (orientation.up):
					if (figure1.move(0, -1)) handle_changes();
					break;
				case (orientation.right):
					if (figure1.move(1, 0)) handle_changes();
					break;
				case (orientation.left):
					if (figure1.move(-1, 0)) handle_changes();
					break;
			}
			
		}

		private void handle_changes()
		{
			switch (figure1.what_changed())
			{
				case (figure_changes.coordinates):
					int[,] changes_matrix = figure1.coordinates_changed();
					int[] coordinates = figure1.lowest_coordinates();
					int[] cell_coordinates = new int[2];
					for (int i = 0; i < changes_matrix.GetLength(0); i++)
					{
						cell_coordinates[0] = i + coordinates[0];
						for (int j = 0; j < changes_matrix.GetLength(1); j++)
						{
							cell_coordinates[1] = j + coordinates[1];
							if (changes_matrix[i, j] > 0)
							{
								field1.new_cell(cell_coordinates, figure1.return_color());
							}
							if (changes_matrix[i, j] < 0)
							{
								field1.remove_cell(cell_coordinates);
							}
						}
					}
					break;
			}
		}

		public void input()
		{

		}
		public Bitmap output()
		{
			return field1.draw();
		}
		private void game()
		{

		}
	}
}
