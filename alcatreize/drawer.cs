using Godot;
using System;
using System.Collections.Generic;
using Alcatreize;
using Alcatreize.alcatreize;
using Alcatreize.Broadphase.GridShape;
using Shape = Godot.Shape;

[Tool]
public class drawer : Node2D
{
    public List<Alcatreize.Shape> shapes = new List<Alcatreize.Shape>();

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
                }
            }
        }
        
        if (me.ShapeType == ShapeType.Capsule)
        {
            
        }
        
        


        return names;
    }
}
