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
		#endregion Variables
		#region Events
		public Form1() {InitializeComponent();}
		private void Form1_Load(object sender, EventArgs e) {
			fx = Width; fy = Height; fx2 = fx / 2; fy2 = fy / 2; CenterToScreen();
			gi = new Bitmap(fx, fy); gb = Graphics.FromImage(gi); gf = CreateGraphics();

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
		int mz = 8;
		private void Draw() {
			//gb.Clear(Color.Black);
			Color c;
			gb.DrawLine(Pens.DarkGray, 0, fy2, fx, fy2);
			gb.DrawLine(Pens.DarkGray, fx2, 0, fx2, fy);

			for(int z = 1 ; z < 8 ; z++) {
				c = Color.FromArgb(z * 32, z * 32, z * 32);
				for(double x = -fx2 ; x < fx2 ; x++) {
					for(double y = -fy2 ; y < fy2 ; y++) {
						gi.SetPixel((int)(x + fx2), (int)(y + fy2), c);
					}
				}
			}
			gf.DrawImage(gi, 0, 0);
		}
		#endregion Draw
		#endregion Beat
		#region Functions

		#endregion Functions
	}
	public class Complex {
		public double a, b;
		public Complex(double na, double nb) { a = na; b = nb; }
		public override string ToString() { return "(" + a.ToString() + (b >= 0 ? " + " : " - ") + Math.Abs(b).ToString() + "i)"; }
		public double Abs() { return Math.Sqrt(a * a + b * b); }
		public Complex conj() { return new Complex(a, -b); }
		public override bool Equals(object obj) { return base.Equals(obj); }
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
		public static Complex operator /(Complex l, double r) { return r == 0 ? new Complex(double.NaN, double.NaN) : new Complex(l.a / r, l.b / r); }
		public static Complex operator /(double l, Complex r) {double cd = r.a * r.a + r.b * r.b;
			return cd == 0 ? new Complex(double.NaN, double.NaN) :
			new Complex((l * r.a) / cd, (-l * r.b) / cd);
		}

	}
}
