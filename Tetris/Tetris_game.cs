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
		// свойства
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
		//конец свойств
		//методы
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
					if (matrix[i, j] > 0)
					{
						field.new_cell(coordinates, figure1.return_color());
					}
					if (matrix[i, j] < 0)
					{
						field.remove_cell(coordinates);
					}
				}
			}
		}
		
		public void direction_key(orientation key)
		{
			Figure change_matrix;
			switch (key)
			{
				case (orientation.down):
					change_matrix = new Figure(figure1.move(0, 1));
					break;
				case (orientation.up):
					change_matrix = new Figure(figure1.move());
					break;
				case (orientation.right):
					change_matrix = new Figure(figure1.move(1, 0));
					break;
				case (orientation.left):
					change_matrix = new Figure(figure1.move(-1, 0));
					break;
				default:
					change_matrix = figure1;
					break;
			}
			handle_changes(field1, change_matrix);
			
		}

		private void handle_changes(Field field, Figure figure)//to do сделать нормально
		{
			if (figure_collisions(field, figure))
			{
				figure1.rebild(figures[random.Next(6)], colors[random.Next(6)], new int[2] { 4, 0 });//to do сделать нормально
				cell_transfer(field, figure1);
				return;
			}
			cell_transfer(field, figure);

			figure_out_coordinates(field, figure);

			if ( this.stop)
			{
				this.stop = false;
				figure1.rebild(figures[random.Next(6)], colors[random.Next(6)], new int[2] { 4, 0 });//to do сделать нормально
				cell_transfer(field, figure1);
			}
		}
		//проверка столкновения для всей фигуры
		private bool figure_collisions(Field field, Figure figure)
		{
			int[,] matrix = figure.return_matrix();
			int[] coordinates = figure.return_coordinates();
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
		// проверка нахождения в поле для всей фигуры
		private void figure_out_coordinates( Field field, Figure figure)//to do сделать нормально
		{
			int[,] matrix = figure.return_matrix();
			int[] coordinates = figure.return_coordinates();
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
						//если уперлись в дно фиксируем
						if (differense[1] < 0) this.stop = true;
					}
				}
			}
			if (diff_buff[0] == 0 && diff_buff[1] == 0) { }
			else
			{
				//если клетка вышла из поля сдвигаем фигуру вовнутрь 
				cell_transfer(field, figure1.move(diff_buff[0], diff_buff[1]));	//to do сделать нормально
			}
		}
		// проверяем столкновение
		private bool collision(int[] coordinates, Field field )
		{
			if (field.cell_exist(coordinates))
				return true;
			else return false;
		}
		// проверяем не  вышла ли клетка за границы поля
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
