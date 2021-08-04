using Godot;

public class SceneManager : Node
{
    public override void _Input (InputEvent @event)
    {
        if (@event is InputEventKey key)
        {
            if (!key.Echo && key.Pressed)
            {
                if (key.Scancode == (int) KeyList.Key1 || key.Scancode == (int) KeyList.Ampersand)
                {
                    GetTree().ChangeScene("res://scenes/point_aabb.tscn");
                }
                if (key.Scancode == (int) KeyList.Key2 || key.Scancode == (int) KeyList.Eacute)
                {
                    GetTree().ChangeScene("res://scenes/segment_aabb.tscn");
                }
                if (key.Scancode == (int) KeyList.Key3 || key.Scancode == (int) KeyList.Quotedbl)
                {
                    GetTree().ChangeScene("res://scenes/aabb_aabb.tscn");
                }
                if (key.Scancode == (int) KeyList.Key4 || key.Scancode == (int) KeyList.Apostrophe)
                {
                    GetTree().ChangeScene("res://scenes/sweepaabb_aabb.tscn");
                }
                if (key.Scancode == (int) KeyList.Key5 || key.Scancode == (int) KeyList.Parenleft)
                {
                    GetTree().ChangeScene("scenes/free_play.tscn");
                }
                if (key.Scancode == (int) KeyList.Key6 || key.Scancode == (int) KeyList.Minus)
                {
                    GetTree().ChangeScene("scenes/platformer.tscn");
                }
            }
        }
    }
}
