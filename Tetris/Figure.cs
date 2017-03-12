using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris
{
	enum figure_changes { matrix, color, coordinates };

	class Figure 
	{
		int[,] last_matrix;
		Brush last_color;
		int[] last_coordinates;
		int[,] matrix;
		Brush color;
		int[] coordinates;
		figure_changes changes;

		public Figure (  int[,] matrix, Brush color, int[] coordinates)	
		{
			this.matrix = matrix;
			this.color = color;
			this.coordinates = coordinates;
		}

		public void rebild(int[,] matrix, Brush color, int[] coordinates)
		{
			this.matrix = matrix;
			this.color = color;
			this.coordinates = coordinates;
		}
		public Brush return_color()
		{
			return color;
		}
		public int[] return_coordinates()
		{
			return coordinates;
		}
		public int[,] return_matrix()
		{
			return matrix;
		}
		public void move(int x, int y)
		{
			save_last_condition();
			this.changes = figure_changes.coordinates;
			coordinates[0] += x;
			coordinates[1] += y;
		}

		public void rotate()
		{
			save_last_condition();
			this.changes = figure_changes.matrix;
			int[,] buffer = new int[matrix.GetLength(0), matrix.GetLength(1)];
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					buffer[matrix.GetLength(1) - 1 - j, i] = matrix[i, j];
				}
			}
			Array.Copy(buffer, matrix, matrix.Length);
		}

		private void save_last_condition()
		{
			this.last_matrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
			Array.Copy(matrix, last_matrix, matrix.Length);

			this.last_color = this.color;
			this.last_coordinates = new int[2];
			Array.Copy(coordinates, last_coordinates, coordinates.Length);
		}
		public int[,] return_changes()
		{
			switch (changes)
			{
				case (figure_changes.coordinates):
					return coordinates_changed();
				case (figure_changes.matrix):
					return matrix_changed();
				default:
					return new int[1, 1] { { 0 } };
			}
		}
		private int[,] coordinates_changed()
		{
			int d_x = coordinates[0] - last_coordinates[0];
			int d_y = coordinates[1] - last_coordinates[1];
			int width = matrix.GetLength(0) + Math.Abs(d_x);
			int height = matrix.GetLength(1) + Math.Abs(d_y);
			int[,] diff = new int[width, height];
			int i2;
			int j2;
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for(int j = 0; j < matrix.GetLength(1); j++)
				{
					if (d_x > 0) i2 = i + d_x; else i2 = i;
					if (d_y > 0) j2 = j + d_y; else j2 = j;
					diff[i2, j2] =  matrix[i, j];
				}
			}
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					if (d_x < 0) i2 = i - d_x; else i2 = i;
					if (d_y < 0) j2 = j - d_y; else j2 = j;
					diff[i2, j2] -= last_matrix[i, j];
				}
			}

			return diff;
		}
		private int[,] matrix_changed()
		{

			int[,] diff = new int[matrix.GetLength(0), matrix.GetLength(0)];
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					diff[i, j] = matrix[i, j];
				}
			}
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					diff[i, j] -= last_matrix[i, j];
				}
			}
			return diff;
		}
		public int[] lowest_coordinates()
		{
			int[] coord = new int[2];
			coord[0] = Math.Min(coordinates[0], last_coordinates[0]);
			coord[1] = Math.Min(coordinates[1], last_coordinates[1]);
			return coord;

		}
	}
}
