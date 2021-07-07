using Godot;
using System;
using System.Collections.Generic;
using Alcatreize;
using Alcatreize.alcatreize;
using Alcatreize.Broadphase;
using Alcatreize.Broadphase.GridShape;
using Shape = Alcatreize.Shape;

[Tool]
public class drawer : Node2D
{
    public Dictionary<Shape, IConvex2D> toDraw = new Dictionary<Shape, IConvex2D>();

    public override void _Draw ()
    {
        Color col = Colors.Beige;
        col.a = 0.3f;
        foreach (IConvex2D shape in toDraw.Values)
        {
            if (shape is GridAABB rect)
            {
                DrawRect( new Rect2(rect.topLeft, rect.bottomRight - rect.topLeft), col);
            }

            if (shape is GridCircle circle)
            {
                DrawCircle(circle.center, circle.radius, col);
            }
        }
    }

    public override void _Process (float delta)
    {
        Update();
    }
    /*
    public string CheckMeAgainstAll (Alcatreize.Shape me)
    {
        string names = "";

        if (me.ShapeType == ShapeType.AABB)
        {
            foreach (Alcatreize.Shape shape in shapes)
            {
                if (shape != me)
                {
                    if (shape.ShapeType == ShapeType.AABB)
                        if(Collision.AABBvsAABB(me.GetShape() as Alcatreize.AABB, shape.GetShape() as Alcatreize.AABB))
                            names += shape.Name + "\n";
                    if (shape.ShapeType == ShapeType.OBB)
                        if (Collision.OBBvsAABB(shape.GetShape() as Alcatreize.OBB, me.GetShape() as Alcatreize.AABB))
                            names += shape.Name + "\n";
                    if(shape.ShapeType == ShapeType.Circle)
                        if(Collision.AABBvsCircle(me.GetShape() as Alcatreize.AABB, shape.GetShape() as Alcatreize.Circle))
                            names += shape.Name + "\n";
                    if(shape.ShapeType == ShapeType.Capsule)
                        if(Collision.AABBVsCapsule(me.GetShape() as Alcatreize.AABB, shape.GetShape() as Alcatreize.Capsule))
                            names += shape.Name + "\n";
                }
            }
        }
        
        if (me.ShapeType == ShapeType.OBB)
        {
            foreach (Alcatreize.Shape shape in shapes)
            {
                if (shape != me)
                {
                    if (shape.ShapeType == ShapeType.AABB)
                        if(Collision.OBBvsAABB(me.GetShape() as Alcatreize.OBB, shape.GetShape() as Alcatreize.AABB))
                            names += shape.Name + "\n";
                    if (shape.ShapeType == ShapeType.OBB)
                        if(Collision.OBBvsOBB(shape.GetShape() as Alcatreize.OBB, me.GetShape() as Alcatreize.OBB))
                            names += shape.Name + "\n";
                    if(shape.ShapeType == ShapeType.Circle)
                        if(Collision.OBBvsCircle(me.GetShape() as Alcatreize.OBB, shape.GetShape() as Alcatreize.Circle))
                            names += shape.Name + "\n";
                    if(shape.ShapeType == ShapeType.Capsule)
                        if(Collision.OBBVsCapsule(me.GetShape() as Alcatreize.OBB, shape.GetShape() as Alcatreize.Capsule))
                            names += shape.Name + "\n";
                }
            }
        }
        
        if (me.ShapeType == ShapeType.Circle)
        {
            foreach (Alcatreize.Shape shape in shapes)
            {
                if (shape != me)
                {
                    if (shape.ShapeType == ShapeType.AABB)
                        if(Collision.AABBvsCircle(shape.GetShape() as Alcatreize.AABB, me.GetShape() as Alcatreize.Circle))
                            names += shape.Name + "\n";
                    if (shape.ShapeType == ShapeType.OBB)
                        if(Collision.OBBvsCircle(shape.GetShape() as Alcatreize.OBB, me.GetShape() as Alcatreize.Circle))
                            names += shape.Name + "\n";
                    if(shape.ShapeType == ShapeType.Circle)
                        if(Collision.CircleVsCircle(me.GetShape() as Alcatreize.Circle, shape.GetShape() as Alcatreize.Circle))
                            names += shape.Name + "\n";
                    if(shape.ShapeType == ShapeType.Capsule)
                        if(Collision.CircleVsCapsule(me.GetShape() as Alcatreize.Circle, shape.GetShape() as Alcatreize.Capsule))
                            names += shape.Name + "\n";
                }
            }
        }
        
        if (me.ShapeType == ShapeType.Capsule)
        {
            foreach (Alcatreize.Shape shape in shapes)
            {
                if (shape != me)
                {
                    if (shape.ShapeType == ShapeType.AABB)
                        if(Collision.AABBVsCapsule(shape.GetShape() as Alcatreize.AABB, me.GetShape() as Alcatreize.Capsule))
                            names += shape.Name + "\n";
                    if (shape.ShapeType == ShapeType.OBB)
                        if(Collision.OBBVsCapsule(shape.GetShape() as Alcatreize.OBB, me.GetShape() as Alcatreize.Capsule))
                            names += shape.Name + "\n";
                    if(shape.ShapeType == ShapeType.Circle)
                        if(Collision.CircleVsCapsule(shape.GetShape() as Alcatreize.Circle, me.GetShape() as Alcatreize.Capsule))
                            names += shape.Name + "\n";
                    if(shape.ShapeType == ShapeType.Capsule)
                        if(Collision.CapsuleVsCapsule(me.GetShape() as Alcatreize.Capsule, shape.GetShape() as Alcatreize.Capsule))
                            names += shape.Name + "\n";
                }
            }
        }
        
        


        return names;
    }*/
}
