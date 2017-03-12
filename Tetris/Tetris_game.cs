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
			Brushes.Purple,
			Brushes.Yellow
		};

		bool stop;
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
			figure1 = new Figure(figures[random.Next(6)], colors[random.Next(6)], new int[2] { 4, 0 });
			//figure2 = new Figure(figures[random.Next(6)], colors[random.Next(6)], new int[2] { 0, 0 });
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
					figure1.move(0, 1);
					handle_changes(field1, figure1);
					break;
				case (orientation.up):
					figure1.rotate();
					handle_changes(field1, figure1);
					break;
				case (orientation.right):
					figure1.move(1, 0);
					handle_changes(field1, figure1);
					break;
				case (orientation.left):
					figure1.move(-1, 0);
					handle_changes(field1, figure1);
					break;
			}
			
		}

		private void handle_changes(Field field, Figure figure)
		{
			if (check_collisions(field, figure))
			{
				figure.rebild(figures[random.Next(6)], colors[random.Next(6)], new int[2] { 4, 0 });
				cell_transfer(field, figure);
				return;
			}
			rewrite(field, figure);

			check_for_out(field, figure);

			if ( this.stop)
			{
				this.stop = false;
				figure.rebild(figures[random.Next(6)], colors[random.Next(6)], new int[2] { 4, 0 });
				cell_transfer(field, figure);
			}
		}

		// транслируем узменения фигуры на поле
		private void rewrite(Field field, Figure figure)
		{
			int[,] changes_matrix = figure.return_changes();
			int[] coordinates = figure.lowest_coordinates();
			int[] cell_coordinates = new int[2];
			
			//changes_matrix = figure.return_changes();
			//coordinates = figure.lowest_coordinates();
			for (int i = 0; i < changes_matrix.GetLength(0); i++)
			{
				cell_coordinates[0] = i + coordinates[0];
				for (int j = 0; j < changes_matrix.GetLength(1); j++)
				{
					cell_coordinates[1] = j + coordinates[1];
					if (changes_matrix[i, j] > 0)
					{
						field.new_cell(cell_coordinates, figure1.return_color());
					}
					if (changes_matrix[i, j] < 0)
					{
						field.remove_cell(cell_coordinates);
					}
				}
			}
		}

		private bool check_collisions(Field field, Figure figure)
		{
			int[,] matrix = figure.return_changes();
			int[] coordinates = figure.lowest_coordinates();
			int[] cell_coordinates = new int[2];
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				cell_coordinates[0] = i + coordinates[0];
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					cell_coordinates[1] = j + coordinates[1];
					if (matrix[i, j] > 0)
					{
						if (collision(cell_coordinates, field)) return true;
					}
				}
			}
			return false;
		}

		private void check_for_out( Field field, Figure figure )
		{
			int[,] matrix = figure.return_changes();
			int[] coordinates = figure.lowest_coordinates();
			int[] cell_coordinates = new int[2] { 0, 0 };
			int[] differense = new int[2] { 0, 0 };
			int[] diff_buff = new int[2] {0, 0};
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				cell_coordinates[0] = i + coordinates[0];
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					cell_coordinates[1] = j + coordinates[1];
					if (matrix[i, j] > 0)
					{
						differense = out_coordinates(cell_coordinates, field);
						if (Math.Abs(diff_buff[0]) < Math.Abs(differense[0])) diff_buff[0] = differense[0];
						if (Math.Abs(diff_buff[1]) < Math.Abs(differense[1])) diff_buff[1] = differense[1];
						//если уперлись в дно обрабатываем как столкновение
						if (differense[1] < 0) this.stop = true;
					}
				}
			}
			if (diff_buff[0] == 0 && diff_buff[1] == 0) { }
			else
			{
				//если клетка вышла из поля двигаем фигуру
				figure.move(diff_buff[0], diff_buff[1]);
				rewrite(field, figure);
			}
		}

		// проверяем столкновение
		private bool collision(int[] coordinates, Field field )
		{
			if (field.cell_exist(coordinates))
				return true;
			else return false;
		}

		// проверяем не  вышла ли фигура за границы поля
		private int[] out_coordinates(int[] coordinates, Field field)
		{
			int[] differense = new int[2];
			int[] size = field.return_size();
			if (coordinates[0] >= 0 && coordinates[1] >= 0 && coordinates[0] < size[0] && coordinates[1] < size[1] )
				return new int[2]{ 0, 0 };
			if (coordinates[0] < 0) differense[0] = 0 - coordinates[0];
			if (coordinates[1] < 0) differense[1] = 0 - coordinates[1];
			if (coordinates[0] > size[0] - 1) differense[0] = size[0] - 1 - coordinates[0];
			if (coordinates[1] > size[1] - 1) differense[1] = size[1] - 1 - coordinates[1];
			return differense;
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
