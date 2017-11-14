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
		public void add(Matrix addable)
		{

		}
	}
}
