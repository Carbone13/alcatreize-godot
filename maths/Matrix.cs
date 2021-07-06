using System;

namespace Alcatreize.Maths
{
	//NOTE : Only implements the basics required for the shape transformations
	//NOTE : Not a full implementation, some code copied from openfl/flash/geom/Matrix.hx
	public class Matrix
	{
		public sfloat A;
		public sfloat B;
		public sfloat C;
		public sfloat D;
		public sfloat TX;
		public sfloat TY;

		public Matrix ()
		{
			this.A = sfloat.One;
			this.B = sfloat.Zero;
			this.C = sfloat.Zero;
			this.D = sfloat.One;
			this.TX = sfloat.Zero;
			this.TY = sfloat.Zero;
		}
		
		public Matrix(sfloat a, sfloat b, sfloat c, sfloat d, sfloat tx, sfloat ty) 
		{
			this.A = a;
			this.B = b;
			this.C = c;
			this.D = d;
			this.TX = tx;
			this.TY = ty;
		}

		public void Identity () 
		{
			A = sfloat.One;
			B = sfloat.Zero;
			C = sfloat.Zero;
			D = sfloat.One;
			TX = sfloat.Zero;
			TY = sfloat.Zero;
		}

		public void Translate (sfloat x, sfloat y) 
		{
			TX += x;
			TY += y;
		}

		public Matrix MakeTranslation(sfloat x, sfloat y) 
		{
			TX = x;
			TY = y;

			return this;
		}

		public void Rotate (sfloat angle) 
		{
			var cos = libm.cosf(angle);
	        var sin = libm.sinf(angle);

	        var a1 = A * cos - B * sin;
	            B = A * sin + B * cos;
	            A = a1;

	        var c1 = C * cos - D * sin;
	            D = C * sin + D * cos;
	            C = c1;

	        var tx1 = TX * cos - TY * sin;
	            TY = TX * sin + TY * cos;
	            TX = tx1;
		}

		public void Scale (sfloat x, sfloat y) 
		{
			A *= x;
	        B *= y;

	        C *= x;
	        D *= y;

	        TX *= x;
	        TY *= y;
		}

		public void Compose (sfloat2 position, sfloat rotation, sfloat2 scale) 
		{
			Identity();

	        Scale (scale.X, scale.Y);
	        Rotate (rotation);
			MakeTranslation (position.X, position.Y);
		}

		public override string ToString ()
		{
			return string.Format ("[Matrix a={0} b={1] c={2} d={3} tx={4} ty={5}]", A, B, C, D, TX, TY);
		}
	}
}