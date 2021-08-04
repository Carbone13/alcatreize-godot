using Godot;
using System;
using System.Collections.Generic;

public class SweepAABB
{
    public sfloat2 origin;
    public sfloat2 end;
    public Sweep sweep;
}

public class SweepAABBDemo : Node2D
{
    [Export] private Color outsideColor, insideColor, deltaColor;
    [Export] private Color originalColor;
    [Export] private Color dotColor;
    [Export] private Vector2 Size;
    [Export] private float tickness;
    [Export] private NodePath target;
    
    public List<SweepAABB> aabb = new List<SweepAABB>();
    private bool placingAABB;
    public sfloat2 placedOrigin;

    public override void _Ready ()
    {
        Input.SetMouseMode(Input.MouseMode.Visible);
    }

    public override void _Process (float delta)
    {
        if(Input.IsMouseButtonPressed((int)ButtonList.Right))
            ClearSegments();

        if (Input.IsActionJustPressed("place_point"))
        {
            if (target == null) return;
            
            if (placingAABB)
            {
                placingAABB = false;
                AddSegment(placedOrigin, GetGlobalMousePosition());
            }
            else
            {
                AABB _tar = GetNodeOrNull<AABB>(target);
                if (_tar != null)
                {
                    Hit hit = _tar.IntersectAABB(new AABB(GetGlobalMousePosition(), Size));
                    if (hit == null)
                    {
                        placingAABB = true;
                        placedOrigin = GetGlobalMousePosition();
                    }
                }
            }
        }
        
        if (target == null) return;
            
        AABB tar = GetNodeOrNull<AABB>(target);
        if (tar != null)
        {
            foreach (SweepAABB box in aabb)
            {
                box.sweep = tar.IntersectSweepAABB(new AABB(box.origin, Size), box.end - box.origin);
            }
            
        }
        
        Update();
    }

    private void AddSegment (sfloat2 origin, sfloat2 end)
    {
        aabb.Add(new SweepAABB{ origin = origin, end = end});
    }
    
    private void ClearSegments ()
    {
        aabb.Clear();
        placingAABB = false;
    }

    public override void _Draw ()
    {
        // Draw all created segments
        foreach (SweepAABB segment in aabb)
        {
            if (segment.sweep.Hit != null)
            {
                DrawRect(new Rect2(segment.origin - (sfloat2)Size, Size * 2), originalColor, false, tickness);
                DrawRect(new Rect2(segment.end - (sfloat2)Size, Size * 2), insideColor, false, tickness);
                DrawRect(new Rect2(segment.sweep.Position - (sfloat2)Size, Size * 2), deltaColor, false, tickness);

                DrawLine(segment.origin, segment.end, insideColor, tickness);
                DrawLine(segment.origin, segment.sweep.Hit.Position, outsideColor, tickness);

                DrawCircle(segment.end, tickness, dotColor);
                DrawCircle(segment.sweep.Hit.Position, tickness, dotColor);
            }
            else
            {
                DrawRect(new Rect2(segment.origin - (sfloat2)Size, Size * 2), originalColor, false, tickness);
                DrawRect(new Rect2(segment.end - (sfloat2)Size, Size * 2), outsideColor, false, tickness);

                DrawLine(segment.origin, segment.end, outsideColor, tickness);
                DrawCircle(segment.end, tickness, dotColor);
            }
            
        }

        if (placingAABB)
        {
            SweepAABB segment = new SweepAABB {origin = placedOrigin, end = GetGlobalMousePosition()};
            AABB tar = GetNodeOrNull<AABB>(target);
            if (tar != null)
            {
                segment.sweep = tar.IntersectSweepAABB(new AABB(segment.origin, Size), segment.end - segment.origin);
            }
            
            DrawRect(new Rect2(segment.origin - (sfloat2)Size, Size * 2), originalColor, false, tickness);
            DrawRect(new Rect2(segment.end - (sfloat2)Size, Size * 2), outsideColor, false, tickness);
            
            if (segment.sweep.Hit != null)
            {
                DrawRect(new Rect2(segment.end - (sfloat2)Size, Size * 2), insideColor, false, tickness);
                DrawRect(new Rect2(segment.sweep.Position - (sfloat2)Size, Size * 2), deltaColor, false, tickness);

                DrawLine(segment.origin, segment.end, insideColor, tickness);
                DrawLine(segment.origin, segment.sweep.Hit.Position, outsideColor, tickness);
                
                DrawCircle(segment.end, tickness, dotColor);
                DrawCircle(segment.sweep.Hit.Position, tickness, dotColor);
            }
            else
            {
                DrawLine(segment.origin, segment.end, outsideColor, tickness);
                DrawCircle(segment.end, tickness, dotColor);
            }
        }
    }
}
