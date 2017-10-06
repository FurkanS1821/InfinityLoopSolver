using System.Drawing;
using System.Numerics;
using System.Linq;

namespace InfinityLoopSolver.GameItems
{
    public class TwoDirectionalThing : IGameItem
    {
        public override Vector2 Position { get; set; }
        public override byte DirectionFlags { get; set; }

        public override byte[] PossibleDirections => new byte[]
        {
            9, // up, right
            5, // down, right
            6, // down, left
            10 // up, left
        };

        public override uint ItemId { get; }

        public TwoDirectionalThing(Vector2 position, byte bitfield = 0)
        {
            Position = position;
            DirectionFlags = PossibleDirections.Contains(bitfield) ? bitfield : PossibleDirections[0];
            ItemId = Extensions.GetNewId();
        }
    }
}
