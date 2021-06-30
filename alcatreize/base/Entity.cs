using System.Collections.Generic;
using Godot;

namespace Alcatreize
{
    public class Entity : Node2D
    {
        // The entity pushbox
        // TODO support multiple pushbox per entity ?
        public Pushbox Pushbox;
        // Our hitboxes
        public List<Hitbox> Hitboxes;
        // Our hurtboxes
        public List<Hurtbox> Hurtboxes;

        [Signal] public delegate void OnHurtboxTicked (Hitbox hitbox);
        [Signal] public delegate void CollisionX (int direction, Entity hit);
        


        public override void _Ready ()
        {
            Hitboxes = new List<Hitbox>();
            Hurtboxes = new List<Hurtbox>();
            
            foreach(Node child in GetChildren())
            {
                switch (child)
                {
                    case Pushbox pushbox when Pushbox == null:
                        Pushbox = pushbox;
                        break;
                    case Pushbox pushbox:
                        GD.PrintErr("Found more than 1 hitbox associated with " + Name + " this is not allowed");
                        break;
                    case Hitbox hitbox:
                        Hitboxes.Add(hitbox);
                        break;
                    case Hurtbox hurtbox:
                        Hurtboxes.Add(hurtbox);
                        break;
                }
            }
        }
        
        /// <summary>
        /// Return the first pushbox hit at the specified position
        /// You can use it to check for future position, or just pass GlobalPosition to check at the current position
        /// You can specify the box type, or simply use AABB
        /// NOTE: This will only return boxes on the right layer !
        /// </summary>
        /// <param name="position"></param>
        public void FirstHitAtPosition<T> (Vector2 position)
        {
            
        }

        public void GetAllIntersecting<T> ()
        {
            
        }

        public void GetFirstIntersecting<T> ()
        {
            
        }

        /// <summary>
        /// Make your Hitboxes ticks any hurtboxes they overlap
        /// Call the signal "Ticked" on these Hurtboxes
        /// </summary>
        public void TickHitboxes ()
        {
            foreach (Hitbox hitbox in Hitboxes)
            {
                hitbox.Tick();
            }
        }
    }
}