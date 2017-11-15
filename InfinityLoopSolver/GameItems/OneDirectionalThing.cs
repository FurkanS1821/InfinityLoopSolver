using System.Linq;
using System.Numerics;

namespace InfinityLoopSolver.GameItems
{
    public class OneDirectionalThing : IGameItem
    {
        public override Vector2 Position { get; set; }
        public override byte DirectionFlags { get; set; }

        public override byte[] PossibleDirections { get; set; }

        public override uint ItemId { get; }

        public OneDirectionalThing(Vector2 position, byte bitfield = 0)
        {
            PossibleDirections = new byte[] {8, 1, 4, 2};
            Position = position;
            DirectionFlags = PossibleDirections.Contains(bitfield) ? bitfield : PossibleDirections[0];
            ItemId = Extensions.GetNewId();
        }
    }
}
