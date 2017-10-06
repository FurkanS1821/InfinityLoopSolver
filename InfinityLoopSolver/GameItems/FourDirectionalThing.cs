using System.Linq;
using System.Drawing;
using System.Numerics;

namespace InfinityLoopSolver.GameItems
{
    public class FourDirectionalThing : IGameItem
    {
        public override Vector2 Position { get; set; }
        public override byte DirectionFlags { get; set; }

        public override byte[] PossibleDirections => new byte[]
        {
            15 // all 4 directions
        };

        public override uint ItemId { get; }

        public FourDirectionalThing(Vector2 position, byte bitfield = 0)
        {
            Position = position;
            DirectionFlags = PossibleDirections.Contains(bitfield) ? bitfield : PossibleDirections[0];
            ItemId = Extensions.GetNewId();
        }
    }
}
