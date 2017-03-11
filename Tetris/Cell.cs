using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris
{
	class Cell
	{
		protected int width;
		protected int height;
		protected Brush color;
		protected Bitmap img;
		protected Rectangle rectangle;

		public Cell( int width, int height, Brush color)
		{
			this.width = width;
			this.height = height;
			this.color = color;
			this.rectangle = new Rectangle(0, 0, width, height);
			this.img = draw_cell();
		}

		protected Bitmap draw_cell()
		{
			Bitmap cell = new Bitmap(width, height);
			Graphics cellg = Graphics.FromImage(cell);
			cellg.FillRectangle(this.color, this.rectangle);
			
			return cell;
		}
		public Bitmap return_cell()
		{
			return (this.img);
		}
		/*public Brush getcolor()	del?
		{
			return (this.color);
		}*/
	}
}
