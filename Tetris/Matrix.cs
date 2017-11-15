using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
	//класс объектов типа двумерная матрица поддерживающий сложение и вычитание
	class Matrix
	{
		protected int[,] field;

		//Конструкторы

		//создание матрицы по двумерному массиву
		public Matrix(int[,] field) {
			this.field = field;
		}
		//создание матрицы по другой матрице
		public Matrix(Matrix input)
		{
			this.field = input.field;
		}
		//создание пустой матрицы по размерам
		public Matrix(int[] size)
		{
			if(size.GetLength(0) == 2)
			{
				this.field = new int[size[0], size[1]];
			}
			else { }//error
		}

		//Публичные методы

		//копирование
		public Matrix copy()
		{
			return new Matrix(this);
		}
		//сложение
		public void add(Matrix addable)
		{
			int a = Math.Max(this.field.GetLength(0), addable.field.GetLength(0));
			int b = Math.Max(this.field.GetLength(1), addable.field.GetLength(1));
			int[,] buff = new int[a,b];
			buff = sum(buff, this.field);
			buff = sum(buff, addable.field);
			this.field = buff;
		}
		//вычитание
		public void sub(Matrix subtrahend)
		{
			int a = Math.Max(this.field.GetLength(0), subtrahend.field.GetLength(0));
			int b = Math.Max(this.field.GetLength(1), subtrahend.field.GetLength(1));
			int[,] buff = new int[a, b];
			buff = sum(buff, this.field);
			buff = diff(buff, subtrahend.field);
			this.field = buff;
		}
		private int[,]sum(int[,] first, int[,] second)
		{
			int[,] buff = first;
			for (int i = 0; i < second.GetLength(0); i++)
			{
				for (int j = 0; j < second.GetLength(1); j++)
				{
					this.field[i, j] = first[i, j] - second[i, j];
				}
			}
			return buff;
		}
		private int[,] diff(int[,] first, int[,] second)
		{
			int[,] buff = first;
			for (int i = 0; i < second.GetLength(0); i++)
			{
				for (int j = 0; j < second.GetLength(1); j++)
				{
					this.field[i, j] = first[i, j] - second[i, j];
				}
			}
			return buff;
		}
	}
}
