using Godot;

public class TopDownActor : Actor
{
    [Export] private float MoveSpeed;
    
    public override void _PhysicsProcess (float delta)
    {
        Vector2 joypad = new Vector2(Input.GetJoyAxis(0, 0), Input.GetJoyAxis(0, 1));
        int x = (int)Input.GetActionStrength("ui_left") * -1 + (int)Input.GetActionStrength("ui_right");
        int y = (int)Input.GetActionStrength("ui_up") * -1 + (int)Input.GetActionStrength("ui_down");

        MoveAndSlide((sfloat2)(new Vector2(x, y) + joypad).Normalized() * (sfloat)MoveSpeed * (sfloat)delta);
    }
}
