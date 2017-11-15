using System.Drawing;
using System.Linq;
using System.Numerics;

namespace InfinityLoopSolver.GameItems
{
    public class StraightThing : IGameItem
    {
        public override Vector2 Position { get; set; }
        public override byte DirectionFlags { get; set; }

        public override byte[] PossibleDirections { get; set; }

        public override uint ItemId { get; }

        public StraightThing(Vector2 position, byte bitfield = 0)
        {
            PossibleDirections = new byte[] {12, 3};
            Position = position;
            DirectionFlags = PossibleDirections.Contains(bitfield) ? bitfield : PossibleDirections[0];
            ItemId = Extensions.GetNewId();
        }
    }
}
