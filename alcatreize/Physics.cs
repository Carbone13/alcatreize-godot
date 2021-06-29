using Godot;
using Alcatreize.Broadphase;

// We use SuperGrid2D for the BroadPhase, because I was too lazy to make my own spatial hashing solution
// (I have renamed namespaces and some classes for cleaner structure, but everything inside the Broadphase namespace come from it)
// Credit to : https://github.com/bartofzo/SuperGrid2D
namespace Alcatreize
{
    public class Physics : Node2D
    {
        public static Physics singleton;
        
        public DynamicGrid2D<int, AABB> superGrid;

        private int registered;
        
        public override void _Ready ()
        {
            singleton = this;

            superGrid = new DynamicGrid2D<int, AABB>(Vector2.Zero, 800, 800, 50);
        }

        public override void _Draw ()
        {
            for (int c = 0; c <= 800; c += 800 / 16)
            {
                DrawLine(new Vector2(c, 0), new Vector2(c, 600), Colors.Blue);
            }
            
            for (int r = 0; r <= 800; r += 800 / 16)
            {
                DrawLine(new Vector2(0, r), new Vector2(800, r), Colors.Blue);
            }
        }

        public void Subscribe (AABB aabb)
        {
            aabb.GridID = registered;
            registered++;
            
            superGrid.Add(aabb.GridID, aabb, aabb.GetBoundsInGrid());
        }

        public void Update (AABB aabb)
        {

            superGrid.Update(aabb.GridID, aabb.GetBoundsInGrid());
        }
        
    }
}