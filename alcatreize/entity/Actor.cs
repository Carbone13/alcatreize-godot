using System.Collections.Generic;
using System.Linq;
using Alcatreize.alcatreize;
using Alcatreize.Broadphase;
using Godot;

namespace Alcatreize
{
    public class Actor : Entity
    {
        public List<Shape> Pushboxes = new List<Shape>();

        public override void _Ready ()
        {
            base._Ready();
            foreach (Node child in GetChildren())
            {
                if (child is Shape shape)
                {
                    Pushboxes.Add(shape);
                }
            }
        }
        
        public override void _PhysicsProcess (float delta)
        {
            sfloat2 dir = new sfloat2();
            
            if (Input.GetActionStrength("ui_right") != 0)
                dir.X += sfloat.One;
            if (Input.GetActionStrength("ui_left") != 0)
                dir.X -= sfloat.One;
            if (Input.GetActionStrength("ui_up") != 0)
                dir.Y -= sfloat.One;
            if (Input.GetActionStrength("ui_down") != 0)
                dir.Y += sfloat.One;
            
            Move(dir * (sfloat)100 * (sfloat)delta);
        }

        public void Move (sfloat2 amount)
        {
            sfloat2 direction = amount.normalized;
            
            GD.Print(Physics.singleton.DynamicGrid.Contact(Pushboxes[0].GetEnglobingShape()).Count());
            
            Position += amount;
            
            MovePushboxes(amount);
        }

        private void MovePushboxes (sfloat2 amount)
        {
            foreach (Shape pushbox in Pushboxes)
            {
                pushbox.Position += amount;
            }
        }

        // Return the max distance a shape can travel in a given direction
        public sfloat GetMaxAllowedDistance (Shape collider, sfloat2 direction)
        {
            return sfloat.Zero;
        }
    }
}