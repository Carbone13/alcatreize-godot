using Godot;

namespace Alcatreize
{
    public class Pushbox : AABB
    {
        public override void _PhysicsProcess (float delta)
        {
            Vector2 previous = GlobalPosition;
            
            if (Input.IsKeyPressed((int) KeyList.Q))
            {
                GlobalPosition = new Vector2(GlobalPosition.x - 50 * delta, GlobalPosition.y);
            }
            if (Input.IsKeyPressed((int) KeyList.D))
            {
                GlobalPosition = new Vector2(GlobalPosition.x + 50 * delta, GlobalPosition.y);
            }
            if (Input.IsKeyPressed((int) KeyList.Z))
            {
                GlobalPosition = new Vector2(GlobalPosition.x, GlobalPosition.y - 50 * delta);
            }
            if (Input.IsKeyPressed((int) KeyList.S))
            {
                GlobalPosition = new Vector2(GlobalPosition.x, GlobalPosition.y + 50 * delta);
            }
            
            Physics.singleton.Update(this);
        }
        
        public override void _Draw ()
        {
            DrawRect(new Rect2(Vector2.Zero, GetBoundsSettings().Size), new Color(1, 1, 1, 0.5f));
        }

        public override void _Process (float delta)
        {
            Update();
        }
    }
}