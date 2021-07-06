using Godot;
using System;
using System.Collections.Generic;
using Alcatreize.Broadphase.GridShape;

[Tool]
public class drawer : Node2D
{


    public Dictionary<Alcatreize.Shape, GridCircle> toDraw = new Dictionary<Alcatreize.Shape, GridCircle>();
    
    public override void _Process (float delta)
    {
        Update();
    }

    public override void _Draw ()
    {
        Color color = Colors.Beige;
        color.a = 0.2f;
        
        foreach(GridCircle rect in toDraw.Values)
        {
            DrawCircle(rect.center, rect.radius, color);
        }
        
        toDraw.Clear();
    }
}
