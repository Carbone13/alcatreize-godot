namespace Alcatreize.SAT
{
    public class SAT
    {
        public static bool OverlapOnAxis (AABB rect1, AABB rect2, sfloat2 axis)
        {
            Interval a = rect1.GetInterval(axis);
            Interval b = rect2.GetInterval(axis);

            return (b.Min <= a.Max) && (a.Min <= b.Max);
        }

        public static bool OverlapOnAxis (AABB rect1, OBB rect2, sfloat2 axis)
        {
            Interval a = rect1.GetInterval(axis);
            Interval b = rect2.GetInterval(axis);

            return (b.Min <= a.Max) && (a.Min <= b.Max);
        }
    }
}