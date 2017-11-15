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
                if (item.IsLookingAt(Direction.Up))
                {
                    var otherItem = items.FirstOrDefault(x => x.Position == item.Position + new Vector2(0, 1));
                    if (otherItem == null || !otherItem.IsLookingAt(Direction.Down))
                    {
                        return false;
                    }
                }
                if (item.IsLookingAt(Direction.Down))
                {
                    var otherItem = items.FirstOrDefault(x => x.Position == item.Position - new Vector2(0, 1));
                    if (otherItem == null || !otherItem.IsLookingAt(Direction.Up))
                    {
                        return false;
                    }
                }
                if (item.IsLookingAt(Direction.Left))
                {
                    var otherItem = items.FirstOrDefault(x => x.Position == item.Position - new Vector2(1, 0));
                    if (otherItem == null || !otherItem.IsLookingAt(Direction.Right))
                    {
                        return false;
                    }
                }
                if (item.IsLookingAt(Direction.Right))
                {
                    var otherItem = items.FirstOrDefault(x => x.Position == item.Position + new Vector2(1, 0));
                    if (otherItem == null || !otherItem.IsLookingAt(Direction.Left))
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

        public static bool RemoveImpossibleConditions(List<IGameItem> items)
        {
            var changed = false;

            foreach (var item in items)
            {
                IGameItem otherItem;
                var newDirs = new List<byte>();
                foreach (var dir in item.PossibleDirections)
                {
                    if (dir.IsLookingAt(Direction.Up))
                    {
                        if ((otherItem = items.FirstOrDefault(x => x.Position == item.Position + new Vector2(0, 1))) == null)
                        {
                            changed = true;
                            continue;
                        }

                        if (!otherItem.CanBeLookingAt(Direction.Down))
                        {
                            changed = true;
                            continue;
                        }
                    }

                    if (dir.IsLookingAt(Direction.Down))
                    {
                        if ((otherItem = items.FirstOrDefault(x => x.Position == item.Position - new Vector2(0, 1))) == null)
                        {
                            changed = true;
                            continue;
                        }

                        if (!otherItem.CanBeLookingAt(Direction.Up))
                        {
                            changed = true;
                            continue;
                        }
                    }

                    if (dir.IsLookingAt(Direction.Right))
                    {
                        if ((otherItem = items.FirstOrDefault(x => x.Position == item.Position + new Vector2(1, 0))) == null)
                        {
                            changed = true;
                            continue;
                        }

                        if (!otherItem.CanBeLookingAt(Direction.Left))
                        {
                            changed = true;
                            continue;
                        }
                    }

                    if (dir.IsLookingAt(Direction.Left))
                    {
                        if ((otherItem = items.FirstOrDefault(x => x.Position == item.Position - new Vector2(1, 0))) == null)
                        {
                            changed = true;
                            continue;
                        }

                        if (!otherItem.CanBeLookingAt(Direction.Right))
                        {
                            changed = true;
                            continue;
                        }
                    }

                    newDirs.Add(dir);
                }
                item.PossibleDirections = newDirs.ToArray();
            }

            foreach (var item in items.Where(x => x.PossibleDirections.Length == 1))
            {
                IGameItem otherItem;
                if (item.CanBeLookingAt(Direction.Up))
                {
                    otherItem = items.First(x => x.Position == item.Position + new Vector2(0, 1));
                    var list = otherItem.PossibleDirections.ToList();
                    list.RemoveAll(x => !x.IsLookingAt(Direction.Down));
                    if (!list.SequenceEqual(otherItem.PossibleDirections))
                    {
                        changed = true;
                    }
                    otherItem.PossibleDirections = list.ToArray();
                }
                if (item.CanBeLookingAt(Direction.Down))
                {
                    otherItem = items.First(x => x.Position == item.Position - new Vector2(0, 1));
                    var list = otherItem.PossibleDirections.ToList();
                    list.RemoveAll(x => !x.IsLookingAt(Direction.Up));
                    if (!list.SequenceEqual(otherItem.PossibleDirections))
                    {
                        changed = true;
                    }
                    otherItem.PossibleDirections = list.ToArray();
                }
                if (item.CanBeLookingAt(Direction.Left))
                {
                    otherItem = items.First(x => x.Position == item.Position - new Vector2(1, 0));
                    var list = otherItem.PossibleDirections.ToList();
                    list.RemoveAll(x => !x.IsLookingAt(Direction.Right));
                    if (!list.SequenceEqual(otherItem.PossibleDirections))
                    {
                        changed = true;
                    }
                    otherItem.PossibleDirections = list.ToArray();
                }
                if (item.CanBeLookingAt(Direction.Right))
                {
                    otherItem = items.First(x => x.Position == item.Position + new Vector2(1, 0));
                    var list = otherItem.PossibleDirections.ToList();
                    list.RemoveAll(x => !x.IsLookingAt(Direction.Left));
                    if (!list.SequenceEqual(otherItem.PossibleDirections))
                    {
                        changed = true;
                    }
                    otherItem.PossibleDirections = list.ToArray();
                }
            }

            return changed;
        }
    }
}
