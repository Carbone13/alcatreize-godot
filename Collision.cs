using System.Security.Cryptography;
using Alcatreize.Maths;
using Alcatreize.SAT;
using Godot;

namespace Alcatreize.alcatreize
{
    public class Collision
    {
        public static bool AABBvsAABBSAT (AABB a, AABB b)
        {
            sfloat2[] axisToTest = new sfloat2[2]
            {
                new sfloat2(1, 0),
                new sfloat2(0, 1)
            };

            for (int i = 0; i < 2; i++)
            {
                
                if (!SAT.SAT.OverlapOnAxis(a, b, axisToTest[i]))
                {
                    return false;
                }
            }

            return true;
        }
        public static bool AABBvsAABB (AABB a, AABB b)
        {
            if (a.Max.X < b.Min.X || a.Min.X > b.Max.X) return false;
            if (a.Max.Y < b.Min.Y || a.Min.Y > b.Max.Y) return false;
        
            return true;
        }

        public static bool OBBvsOBB (OBB a, OBB b)
        {
            return false;
        }

        public static bool CircleVsCircle (Circle a, Circle b)
        {
            sfloat2 line = b.Center - a.Center;
            sfloat r = a.Radius + b.Radius;
            r *= r;

            return line.SquaredLength <= r;
        }

        public static bool OBBvsCircle (OBB rect, Circle circle)
        {
            sfloat2 r = circle.Center - rect.Center;
            sfloat theta = -sfloat.Deg2Rad(rect.Rotation);
            
            Matrix zRotation = new Matrix(libm.cosf(theta), libm.sinf(theta), -libm.sinf(theta), libm.cosf(theta),
                sfloat.Zero, sfloat.Zero);
            
            r = r.Transform(zRotation);

            Circle lCircle = new Circle(r + rect.HalfExtents, circle.Radius);
            AABB lRectangle = new AABB(sfloat2.Zero + rect.HalfExtents, rect.HalfExtents);

            bool final = AABBvsCircle(lRectangle, lCircle);

            return final;
        }

        public static bool AABBvsCircle (AABB rect, Circle circle)
        {
            sfloat2 closestPoint = circle.Center;

            if (closestPoint.X < rect.Min.X)
                closestPoint.X = rect.Min.X;
            else if (closestPoint.X > rect.Max.X)
                closestPoint.X = rect.Max.X;

            closestPoint.Y = (closestPoint.Y < rect.Min.Y) ? rect.Min.Y : closestPoint.Y;
            closestPoint.Y = (closestPoint.Y > rect.Max.Y) ? rect.Max.Y : closestPoint.Y;

            sfloat2 line = circle.Center - closestPoint;
            return line.SquaredLength <= circle.Radius * circle.Radius;
        }

        public static bool OBBvsAABB (OBB rect1, AABB rect2)
        {
            sfloat2[] axisToTest = new sfloat2[]
            {
                new sfloat2(1, 0), new sfloat2(0, 1), new sfloat2(), new sfloat2()
            };

            sfloat theta = sfloat.Deg2Rad(rect1.Rotation);
            Matrix zRotation = new Matrix(libm.cosf(theta), libm.sinf(theta), -libm.sinf(theta), libm.cosf(theta),
                sfloat.Zero, sfloat.Zero);
            
            axisToTest[2] = new sfloat2(1, 0).Transform(zRotation);
            axisToTest[3] = new sfloat2(0, 1).Transform(zRotation);
            
            for (int i = 0; i < 4; i++)
            {
                GD.Print(i + " " + axisToTest[i]);
                if (!SAT.SAT.OverlapOnAxis(rect2, rect1, axisToTest[i]))
                    return false;
            }

            return true;
        }
    }
}