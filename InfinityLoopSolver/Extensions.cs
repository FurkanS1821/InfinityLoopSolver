using InfinityLoopSolver.GameItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityLoopSolver
{
    public static class Extensions
    {
        private static uint _lastId;

        public static uint GetNewId()
        {
            return _lastId++;
        }

        public static bool IsLookingAt(this byte i, Direction dir)
        {
            var b = (byte)dir;
            return (i & b) == b;
        }

        public static bool IsLookingAt(this IGameItem i, Direction dir)
        {
            return i.DirectionFlags.IsLookingAt(dir);
        }

        public static byte GetAllPossibleDirections(this IGameItem i)
        {
            return i.PossibleDirections.GetAllPossibleDirections();
        }

        public static byte GetAllPossibleDirections(this byte[] a)
        {
            byte sum = 0;
            foreach (var b in a)
            {
                sum |= b;
            }

            return sum;
        }

        public static bool CanBeLookingAt(this IGameItem i, Direction dir)
        {
            var b = (byte)dir;

            return (b & i.GetAllPossibleDirections()) == b;
        }
    }
}
