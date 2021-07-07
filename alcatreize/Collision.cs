using System.Security.Cryptography;
using Alcatreize.Broadphase;
using Alcatreize.Maths;
using Alcatreize.SAT;
using Godot;

namespace Alcatreize.alcatreize
{
    public class Collision
    {
        public static bool ShapeVsShape (Shape shapeA, Shape shapeB)
        {
            Primitive a = shapeA.GetShape();
            Primitive b = shapeB.GetShape();

            if (a is AABB aabb)
                return AABBvsPrimitive(aabb, b);

            if (a is OBB obb)
                return OBBvsPrimitive(obb, b);

            if (a is Circle circle)
                return CirclevsPrimitive(circle, b);

            if (a is Capsule capsule)
                return CapsulevsPrimitive(capsule, b);

            return false;
        }

        public static bool AABBvsPrimitive (AABB aabb, Primitive b)
        {
            if (b is AABB _aabb)
                return AABBvsAABB(aabb, _aabb);

            if (b is OBB obb)
                return OBBvsAABB(obb, aabb);

            if (b is Circle circle)
                return AABBvsCircle(aabb, circle);

            if (b is Capsule capsule)
                return AABBVsCapsule(aabb, capsule);

            return false;
        }
        
        public static bool OBBvsPrimitive (OBB obb, Primitive b)
        {
            if (b is AABB _aabb)
                return OBBvsAABB(obb, _aabb);

            if (b is OBB _obb)
                return OBBvsOBB(obb, _obb);

            if (b is Circle circle)
                return OBBvsCircle(obb, circle);

            if (b is Capsule capsule)
                return OBBVsCapsule(obb, capsule);

            return false;
        }
        
        public static bool CirclevsPrimitive (Circle circle, Primitive b)
        {
            if (b is AABB _aabb)
                return AABBvsCircle(_aabb, circle);

            if (b is OBB obb)
                return OBBvsCircle(obb, circle);

            if (b is Circle _circle)
                return CircleVsCircle(_circle, circle);

            if (b is Capsule capsule)
                return CircleVsCapsule(circle, capsule);

            return false;
        }
        
        public static bool CapsulevsPrimitive (Capsule capsule, Primitive b)
        {
            if (b is AABB _aabb)
                return AABBVsCapsule(_aabb, capsule);

            if (b is OBB obb)
                return OBBVsCapsule(obb, capsule);

            if (b is Circle circle)
                return CircleVsCapsule(circle, capsule);

            if (b is Capsule _capsule)
                return CapsuleVsCapsule(_capsule, capsule);

            return false;
        }
        
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
            AABB local1 = new AABB(sfloat2.Zero, a.HalfExtents);

            sfloat2 r = b.Center - a.Center;

            OBB local2 = new OBB(b.Center, b.HalfExtents, b.Rotation);
            local2.Rotation = b.Rotation - a.Rotation;

            sfloat theta = -sfloat.Deg2Rad(a.Rotation);
            Matrix zRotation = new Matrix(libm.cosf(theta), libm.sinf(theta), -libm.sinf(theta), libm.cosf(theta),
                sfloat.Zero, sfloat.Zero);

            r = r.Transform(zRotation);
            local2.Center = r;

            return OBBvsAABB(local2, local1);
            
            return false;
        }

        public static bool CircleVsCircle (Circle a, Circle b)
        {
            sfloat2 line = b.Center - a.Center;
            sfloat r = a.Radius + b.Radius;
            r *= r;

            return line.SquaredLength < r;
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
            return line.SquaredLength < circle.Radius * circle.Radius;
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
                if (!SAT.SAT.OverlapOnAxis(rect2, rect1, axisToTest[i]))
                    return false;
            }

            return true;
        }

        public static bool CapsuleVsCapsule (Capsule a, Capsule b)
        {
            if (OBBvsOBB(a.Body, b.Body)) return true;
            if (CircleVsCircle(a.Top, b.Top)) return true;
            if (CircleVsCircle(a.Top, b.Bottom)) return true;
            if (CircleVsCircle(a.Bottom, b.Bottom)) return true;
            if (CircleVsCircle(a.Bottom, b.Top)) return true;

            return false;
        }

        public static bool AABBVsCapsule (AABB a, Capsule b)
        {
            if (OBBvsAABB(b.Body, a)) return true;

            if (AABBvsCircle(a, b.Top)) return true;
            if (AABBvsCircle(a, b.Bottom)) return true;
            
            return false;
        }

        public static bool OBBVsCapsule (OBB a, Capsule b)
        {
            if (OBBvsOBB(b.Body, a)) return true;

            if (OBBvsCircle(a, b.Top)) return true;
            if (OBBvsCircle(a, b.Bottom)) return true;

            return false;
        }

        public static bool CircleVsCapsule (Circle a, Capsule b)
        {
            if (OBBvsCircle(b.Body, a)) return true;

            if (CircleVsCircle(a, b.Top)) return true;
            if (CircleVsCircle(a, b.Bottom)) return true;

            return false;
        }
    }
}