using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

//публичные методы:
//public void rebild(int[,] matrix, Brush color, int[] coordinates)
//public void rebild(Figure figure)
//public int[,] return_matrix()
//public Brush return_color()
//public int[] return_coordinates()
//public Figure_move move(int x, int y)
//public Figure_move move()

namespace Tetris
{
	class Figure
	{

		protected int[,] matrix;
		protected Brush color;
		protected int[] coordinates;


		public Figure(int[,] matrix, Brush color, int[] coordinates)
		{
			this.matrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
			Array.Copy(matrix, this.matrix, matrix.Length);
			this.color = color;
			this.coordinates = new int[2];
			Array.Copy(coordinates, this.coordinates, 2);
		}

		public Figure( Figure figure)
		{
			int[,] matrix = figure.return_matrix();
			this.matrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
			Array.Copy(matrix, this.matrix, matrix.Length);
			this.coordinates = new int[2];
			Array.Copy(figure.return_coordinates(), this.coordinates, 2);
			this.color = figure.return_color();
		}

		//публичные методы

		public void rebild(int[,] matrix, Brush color, int[] coordinates)
		{
			this.matrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
			Array.Copy(matrix, this.matrix, matrix.Length);
			this.color = color;
			Array.Copy(coordinates, this.coordinates, 2);
		}

		public void rebild(Figure figure)
		{
			int[,] matrix = figure.return_matrix();
			this.matrix = null;
			this.matrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
			Array.Copy(matrix, this.matrix, matrix.Length);
			Array.Copy(figure.return_coordinates(), this.coordinates, 2);
			this.color = figure.return_color();
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

		public Figure_move move(int x, int y)
		{
			Figure_move fmove = figuremove();
			shift(x, y);
			fmove.changes(this.coordinates);
			return fmove;
		}

		public Figure_move move()
		{
			Figure_move fmove = figuremove();
			rotate(); ;
			fmove.changes(this.matrix);
			return fmove;
		}

		//приватные методы

		private void shift(int x, int y)
		{
			coordinates[0] += x;
			coordinates[1] += y;
		}

		private void rotate()
		{	
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

		private Figure_move figuremove()
		{
			return new Figure_move(this);
		}

		
	}
}
