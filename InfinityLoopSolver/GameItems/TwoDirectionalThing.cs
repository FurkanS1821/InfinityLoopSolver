using System.Drawing;
using System.Numerics;
using System.Linq;

namespace InfinityLoopSolver.GameItems
{
    public class TwoDirectionalThing : IGameItem
    {
        public override Vector2 Position { get; set; }
        public override byte DirectionFlags { get; set; }

        public override byte[] PossibleDirections { get; set; }

        public override uint ItemId { get; }

        public TwoDirectionalThing(Vector2 position, byte bitfield = 0)
        {
            PossibleDirections = new byte[] {9, 5, 6, 10};
            Position = position;
            DirectionFlags = PossibleDirections.Contains(bitfield) ? bitfield : PossibleDirections[0];
            ItemId = Extensions.GetNewId();
        }
    }
}
