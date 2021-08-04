using Godot;
using System;
using System.Collections.Generic;

public class Segment
{
    public sfloat2 origin;
    public sfloat2 end;
    public Hit hit;
}

// Allow you to create multiple segment
public class SegmentDemo : Node2D
{
    [Export] private Color outsideColor, insideColor, deltaColor;
    [Export] private Color dotColor;
    [Export] private float tickness;
    [Export] private NodePath target;
    
    public List<Segment> segments = new List<Segment>();
    private bool placingSegment;
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
            
            if (placingSegment)
            {
                placingSegment = false;
                AddSegment(placedOrigin, GetGlobalMousePosition());
            }
            else
            {
                AABB _tar = GetNodeOrNull<AABB>(target);
                if (_tar != null)
                {
                    Hit hit = _tar.IntersectPoint(GetGlobalMousePosition());
                    if (hit == null)
                    {
                        placingSegment = true;
                        placedOrigin = GetGlobalMousePosition();
                    }
                }
            }
        }
        
        if (target == null) return;
            
        AABB tar = GetNodeOrNull<AABB>(target);
        if (tar != null)
        {
            foreach (Segment segment in segments)
            {
                segment.hit = tar.IntersectSegment(segment.origin, segment.end - segment.origin);
            }
            
        }
        
        Update();
    }

    private void AddSegment (sfloat2 origin, sfloat2 end)
    {
        segments.Add(new Segment { origin = origin, end = end});
    }
    
    private void ClearSegments ()
    {
        segments.Clear();
        placingSegment = false;
    }

    public override void _Draw ()
    {
        // Draw all created segments
        foreach (Segment segment in segments)
        {
            if (segment.hit != null)
            {
                DrawLine(segment.origin, segment.end, insideColor, tickness);
                DrawCircle(segment.end, tickness, dotColor);
                DrawLine(segment.origin, segment.hit.Position, deltaColor, tickness);
                DrawCircle(segment.hit.Position, tickness, dotColor);
            }
            else
            {
                DrawLine(segment.origin, segment.end, outsideColor, tickness);
                DrawCircle(segment.end, tickness, dotColor);
            }
            
        }

        if (placingSegment)
        {
            Segment segment = new Segment {origin = placedOrigin, end = GetGlobalMousePosition()};
            AABB tar = GetNodeOrNull<AABB>(target);
            if (tar != null)
            {
                segment.hit = tar.IntersectSegment(segment.origin, segment.end - segment.origin);
            }
            
            if (segment.hit != null)
            {
                DrawLine(segment.origin, segment.end, insideColor, tickness);
                DrawCircle(segment.end, tickness, dotColor);
                DrawLine(segment.origin, segment.hit.Position, deltaColor, tickness);
                DrawCircle(segment.hit.Position, tickness, dotColor);
            }
            else
            {
                DrawLine(segment.origin, segment.end, outsideColor, tickness);
                DrawCircle(segment.end, tickness, dotColor);
            }
        }
    }
}
