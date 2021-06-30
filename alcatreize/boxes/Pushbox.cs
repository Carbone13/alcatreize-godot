using System.Collections.Generic;
using Alcatreize.maths;
using Godot;

namespace Alcatreize
{
    public class Pushbox : AABB
    {
        // Allow to cache more efficiently static pushbox
        [Export] public bool IsStatic;
        public override void _PhysicsProcess (float delta)
        {
            Physics.GetInRange<Hitbox>(new Rect2());
            
            Vector2 previous = GlobalPosition;
            
            if (Input.IsKeyPressed((int) KeyList.Q))
            {
                GlobalPosition = new Vector2(GlobalPosition.x - 50 * delta, GlobalPosition.y);
            }
            if (Input.IsKeyPressed((int) KeyList.D))
            {
                GlobalPosition = new Vector2(GlobalPosition.x + 50 * delta, GlobalPosition.y);
            }
            if (Input.IsKeyPressed((int) KeyList.Z))
            {
                GlobalPosition = new Vector2(GlobalPosition.x, GlobalPosition.y - 50 * delta);
            }
            if (Input.IsKeyPressed((int) KeyList.S))
            {
                GlobalPosition = new Vector2(GlobalPosition.x, GlobalPosition.y + 50 * delta);
            }
            
            Physics.Update(this);
        }

        /// <summary>
        /// Return the first pushbox who intersect this hitbox at the specified location
        /// </summary>
        /// <returns></returns>
        public Pushbox GetFirstHit (Vector2 position)
        {
            var queriedBox =
                Physics.GetInRange<Pushbox>(GetBounds(position).Inflate(2));
            
            foreach (Pushbox pushbox in queriedBox)
            {
                if (pushbox.GetBounds().Intersects(GetBounds(position)))
                {
                    return pushbox;
                }
            }

            return null;
        }
        
        /// <summary>
        /// Return the first pushbox who intersect this hitbox at its scene Position
        /// </summary>
        /// <returns></returns>
        public Pushbox GetFirstHit ()
        {
            return GetFirstHit(GlobalPosition);
        }
        
        /// <summary>
        /// Return all the pushbox who intersect this hitbox at the specified location
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Pushbox> GetAllHit (Vector2 position)
        {
            var queriedBox =
                Physics.GetInRange<Pushbox>(GetBounds(position).Inflate(2));

            List<Pushbox> found = new List<Pushbox>();
            
            foreach (Pushbox pushbox in queriedBox)
            {
                if (pushbox.GetBounds().Intersects(GetBounds(position)))
                {
                    found.Add(pushbox);
                }
            }

            return found;
        }
        
        /// <summary>
        /// Return all the pushbox who intersect this hitbox at its scene Position
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Pushbox> GetAllHit ()
        {
            return GetAllHit(GlobalPosition);
        }
    }
}