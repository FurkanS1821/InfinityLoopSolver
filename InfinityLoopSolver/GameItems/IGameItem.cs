using System.Numerics;

namespace InfinityLoopSolver.GameItems
{
    public abstract class IGameItem
    {
        public abstract Vector2 Position { get; set; }

        /// <summary>
        /// Bits left to right: up, down, left, right
        /// </summary>
        public abstract byte DirectionFlags { get; set; }

        public abstract byte[] PossibleDirections { get; set; }
        public abstract uint ItemId { get; }

        public static bool operator ==(IGameItem left, IGameItem right)
        {
            return left?.ItemId == right?.ItemId;
        }

        public static bool operator !=(IGameItem left, IGameItem right)
        {
            return left?.ItemId != right?.ItemId;
        }
    }
}
