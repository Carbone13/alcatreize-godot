
using Alcatreize.Broadphase;
using Godot;


namespace Alcatreize
{
    // Represent a Axis Aligned Bounding Box
    public abstract class AABB : Node2D
    {
        [Export] private Rect2 bounds;
        [Export (PropertyHint.Layers2dPhysics)] private int PresentOn;
        [Export (PropertyHint.Layers2dPhysics)] private int SearchOn;

        public int GridID;

        public override void _Ready ()
        {
            Physics.singleton.Subscribe(this);
        }

        // Return the shape "settings" (= the one registered in the editor)
        public Rect2 GetBoundsSettings () => bounds;
        
        // Return the shape, according to the position in the scene
        public Rect2 GetBounds () =>
            new Rect2(GlobalPosition + bounds.Position, Scale * bounds.Size);

        // Return the shape which will be used in the grid
        public GridAABB GetBoundsInGrid ()
        {
            return new GridAABB(GlobalPosition + bounds.Position,
                GlobalPosition + bounds.Position + bounds.Size);
        }
    }
}