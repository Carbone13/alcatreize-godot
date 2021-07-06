using Alcatreize.SAT;
using Godot;

namespace  Alcatreize
{
    /// <summary>
    /// Shapes are actually an helper class for the Godot Editor,
    /// internally we compare PrimitiveShape instead of Shapes
    /// </summary>
    public class Shape : Node2D
    {
        // TODO this bad
        public new sfloat2 Position
        {
            get { return (sfloat2) GlobalPosition; }
            set { GlobalPosition = value; }
        }

        [Export(PropertyHint.Layers2dPhysics)] public int Layer;
        [Export(PropertyHint.Layers2dPhysics)] public int Mask;

        public static bool OverlapOnAxis (Rectangle rect1, Rectangle rect2, sfloat2 axis)
        {
            Interval a = rect1.GetInterval(axis);
            Interval b = rect2.GetInterval(axis);

            return ((b.Min <= a.Max) && (a.Min <= b.Max));
        }
        
        
        public static sfloat OrthagonalProjectionOf (sfloat2 u, sfloat2 v)
        {
            sfloat normeU = u.Length;
            sfloat normeV = v.Length;
            sfloat dotUV = u.Dot(v);
            sfloat buffer = (dotUV / (normeU * normeV)) * normeU;
            
            if (buffer == sfloat.NaN) return sfloat.Zero;
            return buffer;
        }
        
        public static bool isOverlapping (sfloat minA, sfloat maxA, sfloat minB, sfloat maxB) 
        {
            sfloat minOverlap = sfloat.NaN;
            sfloat maxOverlap = sfloat.NaN;


            //If B contain in A
            if(minA <= minB && minB <= maxA) 
            {
                if(minOverlap == sfloat.NaN || minB < minOverlap)
                    minOverlap = minB;
            }
            if(minA <= maxB && maxB <= maxA) 
            {
                if(maxOverlap == sfloat.NaN || maxB > minOverlap)
                    maxOverlap = maxB;
            }

            //If A contain in B
            if(minB <= minA && minA <= maxB) 
            {
                if(minOverlap == sfloat.NaN || minA < minOverlap)
                    minOverlap = minA;
            }
            if(minB <= maxA && maxA <= maxB) 
            {
                if(maxOverlap == sfloat.NaN || maxA > minOverlap)
                    maxOverlap = maxA;
            }

            if(minOverlap == sfloat.NaN || maxOverlap == sfloat.NaN) return false;
            return true;
        }
    }
}