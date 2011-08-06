/*
 * Most of this code was taken from http://www.ragestorm.net/sample?id=80
 * Explanation available at http://www.ragestorm.net/tutorial?id=22
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KartAttack
{
	struct _Vector2D
	{
		public float x, y;
	};

	struct _RotRect
	{
		public _Vector2D C;
		public _Vector2D S;
		public float ang;
	};

	class RotatingRectangle
	{
		

		private void AddVectors2D(_Vector2D v1, _Vector2D v2)
		{ v1.x += v2.x; v1.y += v2.y; }

		private void SubVectors2D(_Vector2D v1, _Vector2D v2)
		{ v1.x -= v2.x; v1.y -= v2.y; }

		private void RotateVector2DClockwise(_Vector2D v, float ang)
		{
		 float t,
				 cosa = (float)Math.Cos(ang),
				 sina = (float)Math.Sin(ang);
		 t = v.x; v.x = t*cosa + v.y*sina; v.y = -t*sina + v.y*cosa;
		}

		// Rotated Rectangles Collision Detection, Oren Becker, 2001
		public bool RotRectsCollision(_RotRect rr1, _RotRect rr2)
		{
		 _Vector2D A, B,   // vertices of the rotated rr2
				 C,      // center of rr2
				 BL, TR; // vertices of rr2 (bottom-left, top-right)

		 float ang = rr1.ang - rr2.ang, // orientation of rotated rr1
				 cosa = (float)Math.Cos(ang),           // precalculated trigonometic -
				 sina = (float)Math.Sin(ang);           // - values for repeated use

		 float t, x, a;      // temporary variables for various uses
		 float dx;           // deltaX for linear equations
		 float ext1, ext2;   // min/max vertical values

		 // move rr2 to make rr1 cannonic
		 C = rr2.C;
		 SubVectors2D(C, rr1.C);

		 // rotate rr2 clockwise by rr2.ang to make rr2 axis-aligned
		 RotateVector2DClockwise(C, rr2.ang);

		 // calculate vertices of (moved and axis-aligned := 'ma') rr2
		 BL = TR = C;
		 SubVectors2D(BL, rr2.S);
		 AddVectors2D(TR, rr2.S);

		 // calculate vertices of (rotated := 'r') rr1
		 A.x = -rr1.S.y*sina; B.x = A.x; t = rr1.S.x*cosa; A.x += t; B.x -= t;
		 A.y =  rr1.S.y*cosa; B.y = A.y; t = rr1.S.x*sina; A.y += t; B.y -= t;

		 t = sina*cosa;

		 // verify that A is vertical min/max, B is horizontal min/max
		 if (t < 0)
		 {
			t = A.x; A.x = B.x; B.x = t;
			t = A.y; A.y = B.y; B.y = t;
		 }

		 // verify that B is horizontal minimum (leftest-vertex)
		 if (sina < 0) { B.x = -B.x; B.y = -B.y; }

		 // if rr2(ma) isn't in the horizontal range of
		 // colliding with rr1(r), collision is impossible
		 //if (B.x > TR.x || B.x > -BL.x) return false;

		 // if rr1(r) is axis-aligned, vertical min/max are easy to get
		 if (t == 0) {ext1 = A.y; ext2 = -ext1; }
		 // else, find vertical min/max in the range [BL.x, TR.x]
		 else
		 {
			x = BL.x-A.x; a = TR.x-A.x;
			ext1 = A.y;
			// if the first vertical min/max isn't in (BL.x, TR.x), then
			// find the vertical min/max on BL.x or on TR.x
			if (a*x > 0)
			{
			 dx = A.x;
			 if (x < 0) { dx -= B.x; ext1 -= B.y; x = a; }
			 else       { dx += B.x; ext1 += B.y; }
			 ext1 *= x; ext1 /= dx; ext1 += A.y;
			}
	
			x = BL.x+A.x; a = TR.x+A.x;
			ext2 = -A.y;
			// if the second vertical min/max isn't in (BL.x, TR.x), then
			// find the local vertical min/max on BL.x or on TR.x
			if (a*x > 0)
			{
			 dx = -A.x;
			 if (x < 0) { dx -= B.x; ext2 -= B.y; x = a; }
			 else       { dx += B.x; ext2 += B.y; }
			 ext2 *= x; ext2 /= dx; ext2 -= A.y;
			}
		 }

		 // check whether rr2(ma) is in the vertical range of colliding with rr1(r)
		 // (for the horizontal range of rr2)
		 return !((ext1 < BL.y && ext2 < BL.y) || (ext1 > TR.y && ext2 > TR.y));
		}








		/*public float angle;
		public Vector2 rrC;
		public Vector2 rrS;

		public RotatingRectangle(Vector2 rrC_, Vector2 rrS_, float angle_)
		{
			angle = angle_;
			rrC = rrC_;
			rrS = rrS_;
		}

		private void AddVectors2D(Vector2 v1, Vector2 v2)
		{ v1.X += v2.X; v1.Y += v2.Y; }

		private void SubVectors2D(Vector2 v1, Vector2 v2)
		{ v1.X -= v2.X; v1.Y -= v2.Y; }

		private void RotateVector2DClockwise(Vector2 v, float ang)
		{
			float t;
			float cosa = (float)Math.Math.Cos((double)ang);
			float sina = (float)Math.Math.Sin((double)ang);
			t = v.X; 
			v.X = (float)(t*cosa + v.Y*sina);
			v.Y = (float)(-t*sina + v.Y*cosa);
		}

		public bool Collide(RotatingRectangle rhs)
		{
			Vector2 A, B,    // vertices of the rotated rr2
			C,               // center of rr2
			BL, TR;          // vertices of rr2 (bottom-left, top-right)

			float ang = this.angle - rhs.angle,  // orientation of rotated rr1
						cosa = (float)Math.Math.Cos((double)ang),           // precalculated trigonometic -
						sina = (float)Math.Math.Sin((double)ang);           // - values for repeated use

			float t, x, a;      // temporary variables for various uses
			float dx;           // deltaX for linear equations
			float ext1, ext2;   // min/max vertical values

			// move rr2 to make rr1 cannonic
			C = rhs.rrC;
			SubVectors2D(C, this.rrC);

			// rotate rr2 clockwise by rhs.ang to make rr2 axis-aligned
			RotateVector2DClockwise(C, rhs.angle);

			// calculate vertices of (moved and axis-aligned := 'ma') rr2
			BL = TR = C;
			BL -= rr2 S / 2;
			SubVectors2D(BL, rhs.rrS);
			AddVectors2D(TR, rhs.rrS);

			// calculate vertices of (rotated := 'r') rr1
			A.X = (float)(-this.rrS.Y * sina); B.X = A.X; t = this.rrS.X * cosa; A.X += (float)t; B.X -= (float)t;
			A.Y = (float)(this.rrS.Y * cosa); B.Y = A.Y; t = this.rrS.X * sina; A.Y += (float)t; B.Y -= (float)t;

			t = sina * cosa;

			// verify that A is vertical min/max, B is horizontal min/max
			if (t < 0)
			{
				t = A.X; A.X = B.X; B.X = (float)t;
				t = A.Y; A.Y = B.Y; B.Y = (float)t;
			}

			// verify that B is horizontal minimum (leftest-vertex)
			if (sina < 0) { B.X = -B.X; B.Y = -B.Y; }

			// if rr2(ma) isn't in the horizontal range of
			// colliding with rr1(r), collision is impossible
			if (B.X > TR.X || B.X > -BL.X)
				return false;

			// if rr1(r) is axis-aligned, vertical min/max are easy to get
			if (t == 0) { ext1 = A.Y; ext2 = -ext1; }
			// else, find vertical min/max in the range [BL.X, TR.X]
			else
			{
				x = BL.X - A.X; a = TR.X - A.X;
				ext1 = A.Y;
				// if the first vertical min/max isn't in (BL.X, TR.X), then
				// find the vertical min/max on BL.X or on TR.X
				if (a * x > 0)
				{
					dx = A.X;
					if (x < 0) { dx -= B.X; ext1 -= B.Y; x = a; }
					else { dx += B.X; ext1 += B.Y; }
					ext1 *= x; ext1 /= dx; ext1 += A.Y;
				}

				x = BL.X + A.X; a = TR.X + A.X;
				ext2 = -A.Y;
				// if the second vertical min/max isn't in (BL.X, TR.X), then
				// find the local vertical min/max on BL.X or on TR.X
				if (a * x > 0)
				{
					dx = -A.X;
					if (x < 0) { dx -= B.X; ext2 -= B.Y; x = a; }
					else { dx += B.X; ext2 += B.Y; }
					ext2 *= x; ext2 /= dx; ext2 -= A.Y;
				}
			}

			// check whether rr2(ma) is in the vertical range of colliding with rr1(r)
			// (for the horizontal range of rr2)
			return !((ext1 < BL.Y && ext2 < BL.Y) ||
				 (ext1 > TR.Y && ext2 > TR.Y));
		}*/
	}
}
