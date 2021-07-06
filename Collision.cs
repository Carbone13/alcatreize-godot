using System.Security.Cryptography;
using Alcatreize.Maths;
using Godot;

namespace Alcatreize.alcatreize
{
    public class Collision
    {
        public static bool RectangleVsRectangleSAT (Rectangle a, Rectangle b)
        {
            sfloat2[] axisToTest = new sfloat2[2]
            {
                new sfloat2(1, 0),
                new sfloat2(0, 1)
            };

            for (int i = 0; i < 2; i++)
            {
                if (!Shape.OverlapOnAxis(a, b, axisToTest[i]))
                {
                    return false;
                }
            }

            return true;
        }
        public static bool RectangleVsRectangle (PrimitiveRectangle a, PrimitiveRectangle b)
        {
            // AABB Case
            if (a.Rotation == sfloat.Zero)
            {
                if (a.Max.X < b.Min.X || a.Min.X > b.Max.X) return false;
                if (a.Max.Y < b.Min.Y || a.Min.Y > b.Max.Y) return false;
            
                return true;
            }
            // OBB Case
            else
            {
                return false;
            }
        }

        public static bool CircleVsCircle (PrimitiveCircle a, PrimitiveCircle b)
        {
            sfloat r = a.Radius + b.Radius;
            r *= r;

            return r < (a.Position.X + b.Position.X) * (a.Position.X + b.Position.X)
                + (a.Position.Y + b.Position.Y) * (a.Position.Y + b.Position.Y); 
        }

        public static bool RectangleVsCircle (PrimitiveRectangle rect, PrimitiveCircle circle)
        {
            if (rect.Rotation == sfloat.Zero)
            {
                sfloat2 closestPoint = circle.Position;

                if (closestPoint.X < rect.Min.X)
                    closestPoint.X = rect.Min.X;
                else if (closestPoint.X > rect.Max.X)
                    closestPoint.X = rect.Max.X;

                closestPoint.Y = (closestPoint.Y < rect.Min.Y) ? rect.Min.Y : closestPoint.Y;
                closestPoint.Y = (closestPoint.Y > rect.Max.Y) ? rect.Max.Y : closestPoint.Y;

                sfloat2 line = circle.Position - closestPoint;
                return line.SquaredLength <= circle.Radius * circle.Radius;
            }
            // OBB Case
            else
            {
                sfloat2 r = circle.Position - rect.Position;
                sfloat theta = -rect.Rotation;
                
                Matrix zRotation = new Matrix(libm.cosf(theta), libm.sinf(theta), -libm.sinf(theta), libm.cosf(theta),
                    sfloat.Zero, sfloat.Zero);

                Matrix l = new Matrix();
                l.Translate(r.X, r.Y);
                l.Rotate(theta);
                
                r = r.Transform(zRotation);

                PrimitiveCircle lCircle = new PrimitiveCircle()
                    {Position = r + rect.HalfExtents, Radius = circle.Radius};
                PrimitiveRectangle lRectangle = new PrimitiveRectangle()
                {
                    Position = sfloat2.Zero,
                    Rotation = sfloat.Zero,
                    HalfExtents = rect.HalfExtents * (sfloat)2,
                    Max = rect.HalfExtents * (sfloat)2,
                    Min = sfloat2.Zero,
                };
                
                bool final = RectangleVsCircle(lRectangle, lCircle);

                return final;
            }
        }

        public static bool CircleVsRectangle (PrimitiveCircle a, PrimitiveRectangle b)
        {
            return RectangleVsCircle(b, a);
        }
    }
}