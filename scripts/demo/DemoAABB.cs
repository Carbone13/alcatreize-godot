using Godot;

public class DemoAABB : AABB
{
    [Export] private Color outsideColor, insideColor, deltaColor;
    [Export] private float tickness;
    [Export] private NodePath target;
    
    private bool insideTarget;
    private sfloat2 deltaPosition;
    
    public override void _Ready ()
    {
        base._Ready();
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
                Hit hit = tar.IntersectAABB(this);
                insideTarget = hit != null;
                if (hit != null)
                {
                    deltaPosition = hit.Position;
                    deltaPosition += hit.Normal * HalfExtents;
                }
            }
        }
        
        Update();
    }

    public override void _Draw ()
    {
        if (insideTarget)
        {
            DrawRect(new Rect2(Vector2.Zero - (Vector2)HalfExtents, HalfExtents * (sfloat)2), insideColor, false, tickness);
            DrawRect(new Rect2((Vector2)deltaPosition- (Vector2)HalfExtents - GlobalPosition, HalfExtents * (sfloat)2), deltaColor, false, tickness);
        }
        else
        {
            DrawRect(new Rect2(Vector2.Zero - (Vector2)HalfExtents, HalfExtents * (sfloat)2), outsideColor, false, tickness);
        }
    }
}
