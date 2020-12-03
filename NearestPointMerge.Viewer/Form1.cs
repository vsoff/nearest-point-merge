using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NearestPointMerge.Viewer
{
    public partial class Form1 : Form
    {
        private readonly Size _viewFieldSize = new Size(300, 300);

        private readonly Image _original;
        private readonly Image _merged;
        private readonly Image _grid;

        public Form1()
        {
            InitializeComponent();

            _original = new Bitmap(_viewFieldSize.Width, _viewFieldSize.Height);
            _merged = new Bitmap(_viewFieldSize.Width, _viewFieldSize.Height);
            _grid = new Bitmap(_viewFieldSize.Width, _viewFieldSize.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawMergeSeed(new DummyPointMerger(), new SizeF(7f, 7f), (int) numericUpDown1.Value, (int) numericUpDown2.Value);
            pictureBox1.Refresh();
        }


        private void DrawMergeSeed(IPointMerger merger, SizeF mergeClipSize, int seed, int generatePointsCount)
        {
            Random random = new Random(seed);
            var points = Enumerable.Range(0, generatePointsCount)
                .Select(x => new PointF((float) random.NextDouble() * _viewFieldSize.Width, (float) random.NextDouble() * _viewFieldSize.Height))
                .ToList();

            var resultPoints = merger.MergeNearestAsync(points, mergeClipSize).GetAwaiter().GetResult();

            // Draw grid.
            using (var g = Graphics.FromImage(_grid))
            {
                g.Clear(Color.Transparent);

                for (float i = 0; i < _viewFieldSize.Width; i += mergeClipSize.Width)
                    g.DrawLine(Pens.LightGray, i, 0f, i, _viewFieldSize.Height);

                for (float i = 0; i < _viewFieldSize.Width; i += mergeClipSize.Width)
                    g.DrawLine(Pens.LightGray, 0f, i, _viewFieldSize.Width, i);
            }

            // Draw original.
            using (var g = Graphics.FromImage(_original))
            {
                g.Clear(Color.Transparent);
                foreach (var point in points)
                    g.DrawRectangle(Pens.CornflowerBlue, (int) point.X, (int) point.Y, 1, 1);
            }

            // Draw merged.
            using (var g = Graphics.FromImage(_merged))
            {
                g.Clear(Color.Transparent);
                foreach (var point in resultPoints)
                    g.DrawRectangle(Pens.Red, (int) point.X, (int) point.Y, 1, 1);
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);
            Point offset = new Point((_viewFieldSize.Width - pictureBox1.Width) / 2, (_viewFieldSize.Height - pictureBox1.Height) / 2);

            g.TranslateTransform(-offset.X - 1, -offset.Y - 1);
            g.DrawRectangle(Pens.LightGray, 0, 0, _viewFieldSize.Width, _viewFieldSize.Height);

            if (_grid != null && checkBox3.Checked) g.DrawImage(_grid, 0, 0);
            if (_original != null && checkBox1.Checked) g.DrawImage(_original, 0, 0);
            if (_merged != null && checkBox2.Checked) g.DrawImage(_merged, 0, 0);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) => pictureBox1.Refresh();
    }
}