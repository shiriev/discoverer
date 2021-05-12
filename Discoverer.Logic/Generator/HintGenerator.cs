using System;
using System.Collections.Generic;
using System.Linq;
using Discoverer.Logic.CellContract;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Hints;

namespace Discoverer.Logic.Generator
{
    internal class HintGenerator
    {
        private static bool Increase(ref int[] poses, int length)
        {
            // TODO: Описать алгоритм работы
            while (true)
            {
                for (var i = poses.Length - 1; i >= 0; i--)
                {
                    if (poses[i] + 1 < length)
                    {
                        poses[i]++;
                        break;
                    }

                    if (i > 0)
                    {
                        poses[i] = poses[i - 1];
                    }
                    else
                    {
                        return true;
                    }
                }

                var b = false;
                for (var i = 0; i < poses.Length - 1; ++i)
                {
                    if (poses[i] >= poses[i + 1])
                    {
                        b = true;
                        break;
                    }
                }
                
                if (!b)
                    return false;
            }
        }
        
        internal static IEnumerable<T[]> Combine<T>(T[] fullSet, int count)
        {
            // TODO: Описать алгоритм работы
            if (fullSet.Length < count)
                yield break;
            
            var poses = Enumerable.Range(0, count).ToArray();
            
            while (true)
            {
                if (!poses.Any())
                    yield break;

                yield return poses.Select(p => fullSet[p]).ToArray();

                var shouldEnd = Increase(ref poses, fullSet.Length);
                if (shouldEnd)
                    yield break;
            }
        }
    }

    internal class HintGenerator<TCoord> : HintGenerator where TCoord : ICoordinate
    {
        private readonly int _playerCount;
        private readonly Dictionary<EHint, Func<IGrid<Cell, TCoord>, TCoord, bool>> _hintFunctions;
        
        public HintGenerator(int playerCount, Dictionary<EHint, Func<IGrid<Cell, TCoord>, TCoord, bool>> hintFunctions)
        {
            _playerCount = playerCount;
            _hintFunctions = hintFunctions;
        }
        
        // TODO: Описать алгоритм работы
        public (TCoord, EHint[])[] Generate(
            IGrid<Cell, TCoord> grid)
        {
            var correctNums = new Dictionary<string, (TCoord, EHint[])>();
            var incorrectNums = new HashSet<string>();

            foreach (var (coord, _) in grid.Items)
            {
                var hints = _hintFunctions
                    .Where(kv => kv.Value(grid, coord))
                    .Select(kv => kv.Key)
                    .ToArray();

                foreach (var hint in Combine(hints, _playerCount))
                {
                    var key = string.Join(',', hint.Cast<int>());
                    if (incorrectNums.Contains(key))
                        continue;
                    if (correctNums.ContainsKey(key))
                    {
                        correctNums.Remove(key);
                        incorrectNums.Add(key);
                        continue;
                    }
                    correctNums[key] = (coord, hint);
                }
            }
            
            return correctNums.Values.ToArray();
        }
    }
}