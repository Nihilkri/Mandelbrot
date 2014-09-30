using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mandelbrot {
	public partial class Form1 : Form {
		#region Variables
		#region Form
		int fx, fy, fx2, fy2;
		Graphics gb, gf; Bitmap gi;

		#endregion Form
		Complex[,,] map;
		#endregion Variables
		#region Events
		public Form1() {InitializeComponent();}
		private void Form1_Load(object sender, EventArgs e) {
			fx = Width; fy = Height; fx2 = fx / 2; fy2 = fy / 2; CenterToScreen();
			gi = new Bitmap(fx, fy); gb = Graphics.FromImage(gi); gf = CreateGraphics();
			//gb.ScaleTransform(1.0F / fx2, 1.0F / fy2);

			//map = new Complex[fx, fy, 2];
			//for(int x = 0 ; x < fx ; x++) {
			//	for(int y = 0 ; y < fy ; y++) {
			//		map[x, y, 0] = new Complex(0, 0);
			//	}
			//}
			//for(int x = 0 ; x < fx ; x++) {
			//	for(int y = 0 ; y < fy ; y++) {
			//		map[x, y, 1] = new Complex((x - fx2) / (fx2 / 2), (y - fy2) / (fy2 / 2));
			//	}
			//}

			Draw();
		}
		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			gb.Dispose(); gi.Dispose(); gf.Dispose();

		}
		private void Form1_Paint(object sender, PaintEventArgs e) {gf.DrawImage(gi, 0, 0);}
		private void Form1_KeyDown(object sender, KeyEventArgs e) {
			switch(e.KeyCode) {
				case Keys.Escape: Close(); return;
				case Keys.Space: Draw(); break;

				default: break;
			}
		}

		#endregion Events
		#region Beat
		#region Calc
		private void Calc() {

		}
		#endregion Calc
		#region Draw
		int ml = 2;
		Color[] itclr = {
			Color.Black, Color.Blue, Color.Green, Color.Cyan,
			Color.Red, Color.Purple, Color.Brown, Color.White,
			Color.Black, Color.Blue, Color.Green, Color.Cyan,
			Color.Red, Color.Purple, Color.Brown, Color.White,
			Color.Black};
		private void Draw() {
			gb.Clear(Color.Black);
			Color clr; Complex z = new Complex(0,0), c = new Complex(-1,0);
			for(int l = 1 ; l < ml ; l++) {
				//clr = Color.FromArgb(255 - z * 17, 255 - z * 17, 255 - z * 17);
				for(int x = 0 ; x < fx ; x++) {
					for(int y = 0 ; y < fy ; y++) {
						z.a = (double)(x - fx2) * 4.0 / (double)fx;
						z.b = (double)(y - fy2) * 4.0 / (double)fy;
						clr = itclr[test(z, c)];
						gi.SetPixel(x, y, clr);
						if((x % 16 == 0) && (y % 16 == 0)) gf.DrawLine(Pens.White, x, y, x + 1, y);
					}
				}
				//string s = pfact(65535);
				//gb.DrawString(s, Font, Brushes.White, 0, 0);
				gb.DrawLine(Pens.DarkGray, 0, fy2, fx, fy2);
				gb.DrawLine(Pens.DarkGray, fx2, 0, fx2, fy);
				gf.DrawImage(gi, 0, 0);
			}
		}
		#endregion Draw
		#endregion Beat
		#region Functions
		public static int test(Complex z, Complex c) {
			for(int q = 0 ; q < 16 ; q++) {
				z = z * z + c;
				if(z.Abs() >= 2.0) return q;
			} return 16;
		} 

		public static string pfact(int n) {
			List<int> l = listpfact(n); string s = ""; for(int q = 0 ; q < l.Count - 1 ; q++)
			s += l[q] + ", "; return s + l[l.Count - 1]; } public static List<int> listpfact(int n) {
			List<int> l = new List<int>(); if(n == 0 || n == 1) { l.Add(n); return l; }
			if(n < 0) { n = -n; l.Add(-1); } int i = 2; while(n >= 2 && i <= Math.Sqrt(n))
			{if(n / (double)i == n / i) { l.Add(i); n /= i; } else i++;} return l;
		}
		#endregion Functions
	}
	public class Complex {
		public double a, b;
		public Complex(double na, double nb) { a = na; b = nb; }
		public override string ToString() { return "(" + a.ToString() + (b >= 0 ? " + " : " - ") + Math.Abs(b).ToString() + "i)"; }
		public double Abs() { return Math.Sqrt(a * a + b * b); }
		public Complex conj() { return new Complex(a, -b); }
		public override bool Equals(object obj) { return base.Equals(obj); }
		public override int GetHashCode() { return base.GetHashCode(); }
		public static bool operator ==(Complex l, Complex r) { return (l.a == r.a) && (l.b == r.b); }
		public static bool operator !=(Complex l, Complex r) { return !((l.a == r.a) && (l.b == r.b)); }
		public static Complex operator +(Complex l, Complex r) { return new Complex(l.a + r.a, l.b + r.b); }
		public static Complex operator +(Complex l, double r) { return new Complex(l.a + r, l.b); }
		public static Complex operator +(double l, Complex r) { return new Complex(l + r.a, r.b); }
		public static Complex operator -(Complex l, Complex r) { return new Complex(l.a - r.a, l.b - r.b); }
		public static Complex operator -(Complex l, double r) { return new Complex(l.a - r, l.b); }
		public static Complex operator -(double l, Complex r) { return new Complex(l - r.a, -r.b); }
		public static Complex operator *(Complex l, Complex r) { return new Complex(l.a * r.a - l.b * r.b, l.a * r.b + r.a * l.b); }
		public static Complex operator *(Complex l, double r) { return new Complex(l.a * r, l.b * r); }
		public static Complex operator *(double l, Complex r) { return new Complex(l * r.a, l * r.b); }
		public static Complex operator /(Complex l, Complex r) {double cd = r.a * r.a + r.b * r.b;
			return cd == 0 ? new Complex(double.NaN, double.NaN) :
			new Complex((l.a * r.a + l.b * r.b) / cd, (l.b * r.a - l.a * r.b) / cd);
		}
		public static Complex operator /(Complex l, double r) { 
			return r == 0 ? new Complex(double.NaN, double.NaN) : new Complex(l.a / r, l.b / r); }
		public static Complex operator /(double l, Complex r) {double cd = r.a * r.a + r.b * r.b;
			return cd == 0 ? new Complex(double.NaN, double.NaN) :
			new Complex((l * r.a) / cd, (-l * r.b) / cd);
		}

	}
}
