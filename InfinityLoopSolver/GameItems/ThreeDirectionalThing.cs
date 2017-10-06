using System.Drawing;
using System.Linq;
using System.Numerics;

namespace InfinityLoopSolver.GameItems
{
    public class ThreeDirectionalThing : IGameItem
    {
        public override Vector2 Position { get; set; }
        public override byte DirectionFlags { get; set; }

        public override byte[] PossibleDirections => new byte[]
        {
            11, // up, left, right
            13, // up, down, right
            7, // down, left, right
            14 // up, down, left
        };

        public override uint ItemId { get; }

        public ThreeDirectionalThing(Vector2 position, byte bitfield = 0)
        {
            Position = position;
            DirectionFlags = PossibleDirections.Contains(bitfield) ? bitfield : PossibleDirections[0];
            ItemId = Extensions.GetNewId();
        }
    }
}
