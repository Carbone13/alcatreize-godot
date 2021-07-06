using System;
using Alcatreize.alcatreize;
using Alcatreize.Maths;
using Alcatreize.SAT;
using Godot;

namespace Alcatreize
{
    [Tool]
    public class Rectangle : Shape
    {
        [Export] private Vector2 size;
        [Export] private NodePath target;
        
        public sfloat2 Max => new sfloat2(Position.X + HalfExtents.X, Position.Y + HalfExtents.Y);
        public sfloat2 Min => new sfloat2(Position.X - HalfExtents.X, Position.Y - HalfExtents.Y);
        
        public sfloat2 HalfExtents;
        
        public Rectangle ()
        {
            HalfExtents = size;
        }

        public Rectangle (sfloat2 position, sfloat2 halfExtents)
        {
            Position = position;
            HalfExtents = halfExtents;
        }
        
        public override void _Ready ()
        {
            HalfExtents = size;
        }

        public override void _PhysicsProcess (float delta)
        {
            if (target != "")
            {
                GD.Print (CollideWith(GetNode<Circle>(target)));
            }
            
            Update();
        }

        public override void _Draw ()
        {
            DrawRect(new Rect2(-HalfExtents, (HalfExtents * (sfloat)2)), Colors.Aqua);
        }

        public bool CollideWith (Rectangle rectangle)
        {
            return Collision.RectangleVsRectangle(ToPrimitive(), rectangle.ToPrimitive());
        }

        public bool CollideWith (Circle circle)
        {
            return Collision.RectangleVsCircle(ToPrimitive(), circle.ToPrimitive());
        }

        public PrimitiveRectangle ToPrimitive ()
        {
            return new PrimitiveRectangle()
            {
                Position = Position,
                Rotation = (sfloat) base.Rotation,
                HalfExtents = HalfExtents,
                Max = Max,
                Min = Min
            };
        }

        public Interval GetInterval (sfloat2 axis)
        {
            sfloat2[] verts = new sfloat2[4]
            {
                new sfloat2(Min.X, Min.Y),
                new sfloat2(Min.X, Max.Y),
                new sfloat2(Max.X, Max.Y),
                new sfloat2(Max.X, Min.Y)
            };

            Interval result = new Interval();
            result.Min = axis.Dot(verts[0]);
            result.Max = axis.Dot(verts[0]);

            for (int i = 0; i < 4; i++)
            {
                sfloat projection = axis.Dot(verts[i]);
                if (projection < result.Min)
                {
                    result.Min = projection;
                }

                if (projection > result.Max)
                {
                    result.Max = projection;
                }
            }

            return result;
        }
        
    }
}