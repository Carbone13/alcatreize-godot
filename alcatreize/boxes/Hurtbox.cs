using Godot;

namespace Alcatreize
{
    public class Hurtbox : AABB
    {
        [Signal] public delegate void Ticked (Hitbox by);
    }
}