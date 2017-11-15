using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris
{
	enum orientation { up, right, left, down};
	struct coordinates { public int x, y; };

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

		bool moved_down;
		int score;
		bool game_over;
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
			game_over = false;
			int[] cell_size = new int[2] { 20, 20 };
			field1.clear();
			field2.clear();
			figure1 = null;
			figure2 = null;
			int type = random.Next(7);
			figure1 = new Figure(figures[type], colors[type], new int[2] { 4, 0 });
			type = random.Next(7);
			figure2 = new Figure(figures[type], colors[type], new int[2] { 0, 0 });
			cell_transfer(field1, figure1);
			cell_transfer(field2, figure2);
			
		}

		private Brush get_color(int color)
		{
			return this.colors[color];
		}
		// использовать методы класса матрица
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
						field.new_cell(coordinates, figure.return_color());
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
					moved_down = true;
					break;
				case (orientation.up):
					change_matrix = new Figure(figure1.move());
					moved_down = false;
					break;
				case (orientation.right):
					change_matrix = new Figure(figure1.move(1, 0));
					moved_down = false;
					break;
				case (orientation.left):
					change_matrix = new Figure(figure1.move(-1, 0));
					moved_down = false;
					break;
				default:
					change_matrix = figure1;
					moved_down = false;
					break;
			}
			handle_changes(field1, change_matrix, figure1);
			
		}

		//очень сложный и запутанный метод управления движением
		private void handle_changes(Field field, Figure change_matrix, Figure mother_fig)
		{
			//проверяем пройзошло ли столкновение
			List<int[]> collisions = new List<int[]>();
			collisions.AddRange (figure_collisions(field, change_matrix));
			if (collisions.Count > 0)
			{
				//возвращаем фигуру в прежнее состояние
				mother_fig.make_backup();
				//если столкнулись сверху
				if (moved_down)
				{
					moved_down = false;
					next_figure();
				}

				return;
			}
			//проверяем вышла ли фигура за пределы поля
			int[]outrange = figure_out_coordinates(field, change_matrix);
			if (outrange[0] == 0 && outrange[1] == 0) { }
			else
			{
				//если фигура вышла из поля сдвигаем фигуру вовнутрь 
				Figure change_matrix2 = new Figure(mother_fig.move(outrange[0], outrange[1]));
				
				//TODO ПЕРЕДЕЛАТЬ
				
				//НАУЧИТЬ СКЛАДЫВАТЬ СО СМЕЩЕНИЕМ
				/*change_matrix2.addition(change_matrix);
				change_matrix = change_matrix2;*/
				//опять проверяем на столкновения
				collisions.Clear();
				collisions.AddRange(figure_collisions(field, change_matrix));
				if (collisions.Count > 0)
				{
					//возвращаем фигуру в прежнее состояние
					mother_fig.make_backup();
					return;
				}
			}
			//если фигура уперлась в дно
			if (outrange[1] < 0)
			{
				next_figure();
			}
			//вносим изменения в поле
			cell_transfer(field, change_matrix);
		
		}
		//проверка столкновения для всей фигуры
		private List<int[]> figure_collisions(Field field, Figure figure)
		{
			List < int[] >collisions = new List<int[]>();
			collisions.Clear();
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
						if (collision(cell_coordinates, field)) collisions.Add(new int[2] { cell_coordinates[0], cell_coordinates[1] });
					}
				}
			}
			return collisions;
		}
		// проверка нахождения в поле для всей фигуры  и вычисление выхода за пределы (предполагается, что размер фигуры меньше размера поля)
		private int[] figure_out_coordinates( Field field, Figure figure)
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
					}
				}
			}
			return diff_buff;
		}
		// проверяем столкновение
		private bool collision(int[] coordinates, Field field )
		{
			if (field.cell_exist(coordinates))return true;
			else return false;
		}
		// проверяем не  вышла ли клетка за границы поля
		private int[] out_coordinates(int[] coordinates, Field field)
		{
			return field.coordinates_check(coordinates);
		}
		//
		private void next_figure()
		{
			remove_full_lines(field1);
			figure1.rebild(figure2);
			figure1.move(4, 0);
			if (figure_collisions(field1, figure1).Count > 0)
			{
				this.game_over = true;
			}
			cell_transfer(field1, figure1);
			int type = random.Next(7);
			figure2.rebild(figures[type], colors[type], new int[2] { 0, 0 });
			for(int i = random.Next(4); i > 0; i--)
			{
				figure2.move();
			}
			field2.clear();
			cell_transfer(field2, figure2);
		}

		private void remove_full_lines(Field field)
		{
			int shift = 0;
			for (int i = field.return_size()[1] - 1; i >= 0; i--)
			{
				
				int quantity = 0;
				for (int j = 0; j < field.return_size()[0]; j++)
				{
					if (field.cell_exist(new int[2] { j, i }))
					{
						quantity++;
					}
				}
				if (quantity == 10)
				{
					for (int j = 0; j < field.return_size()[0]; j++)
					{
						field.remove_cell(new int[2] { j, i });
					}
					shift++;
				}
				else if(shift > 0)
				{
					for (int j = 0; j < field.return_size()[0]; j++)
					{
						if(field.cell_exist(new int[2] { j, i }))
						{
							field.move_cell(new int[2] { j, i }, 0, shift);
						}
					}
				}
			}
			score = Convert.ToInt32(Math.Pow(shift, 2));
		}
		
		public Bitmap output()
		{
			figure1.save_backup();
			return field1.draw();
		}

		public Bitmap output2()
		{
			return field2.draw();
		}

		public int return_score()
		{
			int buffer = score;
			score = 0;
			return buffer;
		}

		public bool is_over()
		{
			return game_over;
		}
	}
}
