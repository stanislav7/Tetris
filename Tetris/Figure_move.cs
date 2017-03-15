using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

//используется для вычисления матрицы изменений

//публичные методы:
//public void changes(int[] new_coordinates)
//public void changes(int[,] new_matrix)

namespace Tetris
{
	class Figure_move : Figure
	{
		public Figure_move(Figure figure) : base(figure)
		{

		}
		
		public void changes(int[] new_coordinates)
		{
			int d_x = new_coordinates[0] - this.coordinates[0];
			int d_y = new_coordinates[1] - this.coordinates[1];
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
					diff[i2, j2] -= matrix[i, j];
				}
			}
			this.matrix = diff;
			this.coordinates[0] = Math.Min(this.coordinates[0], new_coordinates[0]);
			this.coordinates[1] = Math.Min(this.coordinates[1], new_coordinates[1]);
		}
		public void changes(int[,] new_matrix)
		{

			int[,] diff = new int[matrix.GetLength(0), matrix.GetLength(0)];
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					diff[i, j] = new_matrix[i, j];
				}
			}
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					diff[i, j] -= matrix[i, j];
				}
			}
			this.matrix = diff;
		}
	}
}
