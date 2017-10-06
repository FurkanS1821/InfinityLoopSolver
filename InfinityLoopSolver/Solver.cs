using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using InfinityLoopSolver.GameItems;

namespace InfinityLoopSolver
{
    public static class Solver
    {
        public static bool GetIfComplete(List<IGameItem> items)
        {
            foreach (var item in items)
            {
                if ((item.DirectionFlags & 8) == 8) // item is looking up
                {
                    var otherItem = items.FirstOrDefault(x => x.Position == new Vector2(item.Position.X, item.Position.Y + 1));
                    if (otherItem == null || (otherItem.DirectionFlags & 4) != 4) // other item should exist and be looking down
                    {
                        return false;
                    }
                }
                if ((item.DirectionFlags & 4) == 4) // item is looking down
                {
                    var otherItem = items.FirstOrDefault(x => x.Position == new Vector2(item.Position.X, item.Position.Y - 1));
                    if (otherItem == null || (otherItem.DirectionFlags & 8) != 8) // other item should exist and be looking up
                    {
                        return false;
                    }
                }
                if ((item.DirectionFlags & 2) == 2) // item is looking left
                {
                    var otherItem = items.FirstOrDefault(x => x.Position == new Vector2(item.Position.X - 1, item.Position.Y));
                    if (otherItem == null || (otherItem.DirectionFlags & 1) != 1) // other item should exist and be looking right
                    {
                        return false;
                    }
                }
                if ((item.DirectionFlags & 1) == 1) // item is looking right
                {
                    var otherItem = items.FirstOrDefault(x => x.Position == new Vector2(item.Position.X + 1, item.Position.Y));
                    if (otherItem == null || (otherItem.DirectionFlags & 2) != 2) // other item should exist and be looking left
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static List<IGameItem> GetPassingCondition(List<IGameItem> items)
        {
            var completed = false;
            void ForEach(IGameItem currentItem, IGameItem lastItem)
            {
                if (completed)
                {
                    return;
                }

                var index = items.FindIndex(x => x == currentItem);
                foreach (var b in currentItem.PossibleDirections)
                {
                    if (completed)
                    {
                        return;
                    }
                    currentItem.DirectionFlags = b;
                    if (currentItem == lastItem)
                    {
                        if (GetIfComplete(items))
                        {
                            completed = true;
                            return;
                        }

                        continue;
                    }
                    
                    ForEach(items[index + 1], lastItem);
                }
            }
            
            ForEach(items[0], items.Last());

            return items;
        }
    }
}
