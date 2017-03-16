using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Field
    {
		int[] cell_size;
		Cell[,] field;
		int width;
		int height;

		public Field(int width, int height, int[] cell_size)
		{
			this.cell_size = cell_size;
			this.width = width;
			this.height = height;
			this.field = new Cell[width, height];
		}

		public int[] return_size()
		{
			int[] size = new int[2] { width, height };
			return size ;
		}

		public void new_cell(int[] coordinates, Brush color)
		{
			if (coordinates_check(coordinates))
			{
			this.field[coordinates[0], coordinates[1]] = new Cell(cell_size[0], cell_size[1], color);
			}
		}

		public void remove_cell(int[] coordinates)
		{
			if (coordinates_check(coordinates))
			{
			this.field[coordinates[0], coordinates[1]] = null;
			}
		}

		public void move_cell(int[] coordinates, int x, int y)//ошибка если ячейка пуста, необходима проверка перед вызовом
		{
			field[coordinates[0] + x, coordinates[1] + y] = field[coordinates[0], coordinates[1]];
			remove_cell(coordinates);
		}

		public Cell extract_cell(int[] coordinates) //ошибка если ячейка пуста, необходима проверка перед вызовом
		{
			return this.field[coordinates[0], coordinates[1]];
		}

		public bool cell_exist(int[] coordinates)
		{
			if (coordinates_check(coordinates))
			{
				if (field[coordinates[0], coordinates[1]] != null) return true;
			}
			return false;
		}

		public void clear()
		{
			Array.Clear(field, 0, (height * width));
		}

		private bool coordinates_check(int[] coordinates)
		{
			if (coordinates[0] < 0) return false;
			if (coordinates[1] < 0) return false;
			if (coordinates[0] > width - 1) return false;
			if (coordinates[1] > height - 1) return false;
			else return true;
		}

		/*public void add_cell(int[] coordinates, Cell cell)
		{
			this.field[coordinates[0], coordinates[1]] = cell;
		}

		public void move_cell(int[] coordinates1, int[] coordinates2)
		{
			Cell buffer = this.field[coordinates1[0], coordinates1[1]];
			remove_cell(coordinates1);
			add_cell(coordinates2, buffer);
		}*/

		public Bitmap draw()
		{
			Bitmap image;
			Bitmap gamefield = new Bitmap(width * cell_size[0], height* cell_size[1]);
			Graphics gamefieldg = Graphics.FromImage(gamefield);
			for (int j = 0; j < height ; j++)
			{
				for (int i = 0; i < width; i++)
				{
					if (field[i,j] != null)
					{
						image = field[i, j].return_cell();
						gamefieldg.DrawImage(image, (cell_size[0] * i), (cell_size[1] * j));
					}
				}
			}
			return gamefield;
		}

	}
}
