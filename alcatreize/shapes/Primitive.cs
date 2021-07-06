using Alcatreize.Maths;
using Alcatreize.SAT;
using Godot;

namespace Alcatreize
{
    public class Primitive {}
    public class AABB : Primitive
    {
        public sfloat2 Center;
        public sfloat2 HalfExtents;

        public sfloat2 Min => new sfloat2(Center.X - HalfExtents.X, Center.Y - HalfExtents.Y);
        public sfloat2 Max => new sfloat2(Center.X + HalfExtents.X, Center.Y + HalfExtents.Y);
        
        public AABB () {}

        public AABB (sfloat2 center, sfloat2 halfExtents)
        {
            Center = center;
            HalfExtents = halfExtents;
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
    
    public class OBB : Primitive
    {
        public sfloat2 Center;
        public sfloat2 HalfExtents;

        public sfloat Rotation;
        
        public sfloat2 Min => new sfloat2(Center.X - HalfExtents.X, Center.Y - HalfExtents.Y);
        public sfloat2 Max => new sfloat2(Center.X + HalfExtents.X, Center.Y + HalfExtents.Y);
        
        public OBB () {}

        public OBB (sfloat2 center, sfloat2 halfExtents, sfloat rotation)
        {
            Center = center;
            HalfExtents = halfExtents;
            Rotation = rotation;
        }

        public Interval GetInterval (sfloat2 axis)
        {
            AABB r = new AABB(Center, HalfExtents);
            sfloat2[] verts = new sfloat2[4]
            {
                r.Min, r.Max, new sfloat2(r.Min.X, r.Max.Y), new sfloat2(r.Max.X, r.Min.Y)
            };

            sfloat theta = sfloat.Deg2Rad(Rotation);
            Matrix zRotation = new Matrix(libm.cosf(theta), libm.sinf(theta), -libm.sinf(theta), libm.cosf(theta),
                sfloat.Zero, sfloat.Zero);

            for (int i = 0; i < 4; i++)
            {
                sfloat2 v = verts[i] - Center;
                v = v.Transform(zRotation);

                verts[i] = v + Center;
            }

            Interval res = new Interval();
            res.Min = axis.Dot(verts[0]);
            res.Max = axis.Dot(verts[0]);
            
            for (int i = 0; i < 4; i++)
            {
                sfloat proj = axis.Dot(verts[i]);

                res.Min = (proj < res.Min) ? proj : res.Min;
                res.Max = (proj > res.Max) ? proj : res.Max;
            }

            return res;
        }
    }

    public class Circle : Primitive
    {
        public sfloat2 Center;
        public sfloat Radius;

        public Circle () { }

        public Circle (sfloat2 center, sfloat radius)
        {
            Center = center;
            Radius = radius;
        }
    }

    public class Capsule : Primitive
    {
        public sfloat2 Center;
        public sfloat Radius, Height;
        public sfloat Rotation;

        public OBB Body;
        public Circle Top, Bottom;
        
        public Capsule () {}
        
        public Capsule (sfloat2 center, sfloat radius, sfloat height, sfloat rotation)
        {
            Center = center;
            Radius = radius;
            Height = height;
            Rotation = rotation;

            sfloat theta = sfloat.Deg2Rad(Rotation);
            Matrix zRotation = new Matrix(libm.cosf(theta), libm.sinf(theta), -libm.sinf(theta), libm.cosf(theta),
                sfloat.Zero, sfloat.Zero);
            
            sfloat2 topPosition = new sfloat2(sfloat.Zero, height / (sfloat)2);
            topPosition = topPosition.Transform(zRotation);

            Body = new OBB(center, new sfloat2(radius, height / (sfloat)2), rotation);
            
            Top = new Circle(topPosition + Center, Radius);
            Bottom = new Circle(Center - topPosition, Radius);
        }
    }
}