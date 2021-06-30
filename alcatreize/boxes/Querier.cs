using Alcatreize.Broadphase;
using Godot;

namespace Alcatreize
{
    public class Querier : Node2D
    {
        public override void _PhysicsProcess (float delta)
        {
            if (Input.IsKeyPressed((int) KeyList.Left))
            {
                GlobalPosition = new Vector2(GlobalPosition.x - 200 * delta, GlobalPosition.y);
            }
            if (Input.IsKeyPressed((int) KeyList.Right))
            {
                GlobalPosition = new Vector2(GlobalPosition.x + 200 * delta, GlobalPosition.y);
            }
            if (Input.IsKeyPressed((int) KeyList.Up))
            {
                GlobalPosition = new Vector2(GlobalPosition.x, GlobalPosition.y - 200 * delta);
            }
            if (Input.IsKeyPressed((int) KeyList.Down))
            {
                GlobalPosition = new Vector2(GlobalPosition.x, GlobalPosition.y + 200 * delta);
            }
            
            Physics.GetInRange<Pushbox>(new Rect2());
        }

        public override void _Draw ()
        {
            DrawRect(new Rect2(Vector2.Zero, new Vector2(60, 60)), new Godot.Color(1f, 0.6f, 0.6f, 0.6f));
        }
    }
}