using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace NearestPointMerge
{
    /// <summary>
    /// Группировщик ближайших точек в 2D пространстве по указанному шагу.
    /// </summary>
    public interface IPointMerger
    {
        /// <summary>
        /// Группирует ближайшие точки.
        /// </summary>
        /// <param name="points">Коллекция точек.</param>
        /// <param name="mergeSize">Шаг для применения группировки.</param>
        /// <returns>Сгруппированные точки.</returns>
        Task<ICollection<PointF>> MergeNearestAsync(ICollection<PointF> points, SizeF mergeSize);
    }
}