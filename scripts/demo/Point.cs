using Godot;

[Tool]
public class Point : Entity
{
    [Export] private Color outsideColor, insideColor, deltaColor;
    [Export] private float tickness;
    [Export] private NodePath target;

    private bool insideTarget;
    private sfloat2 deltaPosition;

    public override void _Ready ()
    {
        if(!Engine.EditorHint)
            Input.SetMouseMode(Input.MouseMode.Hidden);
        else
        {
            Input.SetMouseMode(Input.MouseMode.Visible);
        }
    }

    public override void _Process (float delta)
    {
        if (!Engine.EditorHint)
        {
            Position = GetGlobalMousePosition();
            
            if (target == null) return;
            
            AABB tar = GetNodeOrNull<AABB>(target);
            if (tar != null)
            {
                Hit hit = tar.IntersectPoint(Position);
                insideTarget = hit != null;
                if (hit != null)
                {
                    deltaPosition = hit.Position;
                }
            }
        }
        
        Update();
    }

    public override void _Draw ()
    {
        if (insideTarget)
        {
            DrawCircle(Vector2.Zero, tickness, insideColor);
            DrawCircle((Vector2)deltaPosition - GlobalPosition, tickness, deltaColor);
        }
        else
        {
            DrawCircle(Vector2.Zero, tickness, outsideColor);
        }
    }
}
