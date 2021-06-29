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

        [Signal]
        public delegate void OnHitByHitbox (Hitbox hitbox);


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


        // Make your hitboxes tick, and damage hurtbox in their way
        public void TickHitbox ()
        {
            
        }
    }
}