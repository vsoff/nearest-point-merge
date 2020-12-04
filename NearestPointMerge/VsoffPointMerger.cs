using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace NearestPointMerge
{
    public class VsoffPointMerger : IPointMerger
    {
        public async Task<ICollection<PointF>> MergeNearestAsync(ICollection<PointF> points, SizeF mergeSize)
        {
            var map = new Dictionary<PointF, List<PointF>>();

            float GetDiscreteValue(float value, float step) => (float) Math.Round(value / step) * step;
            float GetDiscreteValue2(float value, float step) => (int) (value / step) * step;

            PointF GetDiscrete(PointF p) => new PointF(GetDiscreteValue(p.X, mergeSize.Width), GetDiscreteValue(p.Y, mergeSize.Height));

            foreach (var point in points)
            {
                var discretePoint = GetDiscrete(point);
                if (!map.TryGetValue(discretePoint, out var list))
                {
                    list = new List<PointF>();
                    map[discretePoint] = list;
                }

                list.Add(point);
            }

            return map.Select(x => GetAveragePoint(x.Value)).ToList();
        }

        private PointF GetAveragePoint(IEnumerable<PointF> points) => new PointF(points.Average(x => x.X), points.Average(x => x.Y));

        private PointF GetMiddlePoint(IEnumerable<PointF> points)
        {
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            foreach (var point in points)
            {
                maxX = Math.Max(point.X, maxX);
                minX = Math.Min(point.X, minX);
                maxY = Math.Max(point.Y, maxY);
                minY = Math.Min(point.Y, minY);
            }

            return new PointF((maxX + minX) / 2, (maxY + minY) / 2);
        }
    }
}