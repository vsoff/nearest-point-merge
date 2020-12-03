using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using NearestPointMerge;

namespace NearestPointMerger.PerformanceTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await StartPerformanceTestAsync(new DummyPointMerger());
            Console.WriteLine("Press any key...");
            Console.ReadLine();
        }

        private static readonly SizeF DefaultMergeStep = new SizeF(0.0002f, 0.0001f);

        private static async Task StartPerformanceTestAsync(IPointMerger merger, int seed = 1337, int pointsCount = 1000, int repeatCount = 10, SizeF? mergeSize = null)
        {
            mergeSize = mergeSize ?? DefaultMergeStep;
            Random random = new Random(seed);

            var enumerableArray = Enumerable.Range(0, pointsCount).ToArray();

            Stopwatch sw = new Stopwatch();
            var elapsedTimes = new List<Tuple<int, TimeSpan, int>>();
            for (int i = 0; i < repeatCount; i++)
            {
                var points = enumerableArray
                    .Select(x => new PointF((float) random.NextDouble(), (float) random.NextDouble()))
                    .ToList();

                sw.Restart();
                var resultPoints = await merger.MergeNearestAsync(points, mergeSize.Value);
                sw.Stop();
                elapsedTimes.Add(Tuple.Create(i + 1, sw.Elapsed, resultPoints.Count));
            }

            var testResult = $"Type: {merger.GetType()}\n" +
                             $"Merge step: W {mergeSize.Value.Width} : H {mergeSize.Value.Height}\n" +
                             $"Seed: {seed}\n" +
                             $"Points count: {pointsCount}\n" +
                             $"Repeat count: {repeatCount}\n" +
                             $"\nTests:\n{string.Join("\n", elapsedTimes.Select(x => $"{x.Item1}. {x.Item2} (Result points count: {x.Item3})"))}";
            Console.WriteLine(testResult);
        }
    }
}