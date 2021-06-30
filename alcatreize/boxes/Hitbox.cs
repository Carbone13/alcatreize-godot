using Alcatreize.maths;

namespace Alcatreize
{
    public class Hitbox : AABB
    {
        /// <summary>
        /// Queries near hurtboxes, and tick them if they intersect us
        /// </summary>
        public void Tick ()
        {
            // Query near hurtbox
            foreach (Hurtbox hurtbox in Physics.GetInRange<Hurtbox>(GetBounds().Inflate(2)))
            {
                // Does it intersect us ?
                if (hurtbox.GetBounds().Intersects(GetBounds()))
                {
                    // If it does, emit the signal
                    hurtbox.EmitSignal(nameof(Hurtbox.Ticked), this);
                }
            }
        }
    }
}