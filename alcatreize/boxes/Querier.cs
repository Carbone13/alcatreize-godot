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

            var query = Physics.singleton.superGrid.Contact(new GridAABB(GlobalPosition,
                GlobalPosition + new Vector2(60, 60)));
                //Physics.singleton.Grid.FindNear(GlobalPosition, new Vector2(60, 60));

            foreach (AABB found in query)
            {
                GD.Print(OS.GetTicksMsec() + " Found: " + found.Name);
            }
                /*
            string result = "";
            foreach (GridClient client in query)
            {
                result += " " + client.Name;
            }
            if(result != "")
                GD.Print(OS.GetTicksMsec() + " " + result);*/
        }

        public override void _Draw ()
        {
            DrawRect(new Rect2(Vector2.Zero, new Vector2(60, 60)), new Godot.Color(1f, 0.6f, 0.6f, 0.6f));
        }
    }
}