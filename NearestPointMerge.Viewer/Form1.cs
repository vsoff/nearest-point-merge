using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NearestPointMerge.Viewer
{
    public partial class Form1 : Form
    {
        readonly SizeF _mergeClipSize = new SizeF(0.0002f, 0.0001f);
        readonly Size _clipSize = new Size(300, 300);

        private Image _original;
        private Image _merged;

        public Form1()
        {
            InitializeComponent();

            _original = new Bitmap(_clipSize.Width, _clipSize.Height);
            _merged = new Bitmap(_clipSize.Width, _clipSize.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawMergeSeed(new DummyPointMerger(), (int) numericUpDown1.Value, (int) numericUpDown2.Value);
            pictureBox1.Refresh();
        }


        private void DrawMergeSeed(IPointMerger merger, int seed, int generatePointsCount)
        {
            Random random = new Random(seed);
            var points = Enumerable.Range(0, generatePointsCount)
                .Select(x => new PointF((float) random.NextDouble() * _clipSize.Width, (float) random.NextDouble() * _clipSize.Height))
                .ToList();

            var resultPoints = merger.MergeNearestAsync(points, _mergeClipSize).GetAwaiter().GetResult();

            // Draw.
            using (var g = Graphics.FromImage(_original))
            {
                g.Clear(Color.Transparent);
                foreach (var point in points)
                    g.DrawLine(Pens.CornflowerBlue, point.X, point.Y, point.X + 1, point.Y + 1);
            }

            using (var g = Graphics.FromImage(_merged))
            {
                g.Clear(Color.Transparent);
                foreach (var point in resultPoints)
                    g.DrawLine(Pens.Red, point.X, point.Y, point.X + 1, point.Y + 1);
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);
            g.DrawRectangle(Pens.Black, 0, 0, _clipSize.Width, _clipSize.Height);

            if (_original != null && checkBox1.Checked) g.DrawImage(_original, 0, 0);
            if (_merged != null && checkBox2.Checked) g.DrawImage(_merged, 0, 0);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) => pictureBox1.Refresh();
    }
}