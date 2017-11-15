using System.Drawing;
using System.Linq;
using System.Numerics;

namespace InfinityLoopSolver.GameItems
{
    public class ThreeDirectionalThing : IGameItem
    {
        public override Vector2 Position { get; set; }
        public override byte DirectionFlags { get; set; }

        public override byte[] PossibleDirections { get; set; }

        public override uint ItemId { get; }

        public ThreeDirectionalThing(Vector2 position, byte bitfield = 0)
        {
            PossibleDirections = new byte[] {11, 13, 7, 14};
            Position = position;
            DirectionFlags = PossibleDirections.Contains(bitfield) ? bitfield : PossibleDirections[0];
            ItemId = Extensions.GetNewId();
        }
    }
}
