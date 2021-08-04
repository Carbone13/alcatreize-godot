public class Hit
{
    public readonly AABB Collider;
    public sfloat2 Position;
    public sfloat2 Delta;
    public sfloat2 Normal;
    public sfloat Time;

    public Hit (AABB collider)
    {
        Collider = collider;
        Position = new sfloat2();
        Delta = new sfloat2();
        Normal = new sfloat2();
        Time = sfloat.Zero;
    }
}
