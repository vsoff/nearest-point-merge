using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace NearestPointMerge
{
    /// <summary>
    /// Пример реализации группировщика.
    /// </summary>
    public class DummyPointMerger : IPointMerger
    {
        public async Task<ICollection<PointF>> MergeNearestAsync(ICollection<PointF> points, SizeF mergeSize)
        {
            return points;
        }
    }
}