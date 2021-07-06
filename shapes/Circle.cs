using System;
using Godot;

namespace Alcatreize
{
    [Tool]
    public class Circle : Shape
    {
        [Export] private float radius;
        
        public sfloat Radius;

        public Circle ()
        {
            Radius = (sfloat)radius;
        }

        public Circle (sfloat2 position, sfloat _radius)
        {
            Position = position;
            Radius = _radius;
        }
        
        public override void _Ready ()
        {
            Radius = (sfloat) radius;
        }

        public override void _Process (float delta)
        {
            Update();
        }

        public override void _Draw ()
        {
            DrawCircle(Vector2.Zero, (float)Radius, Colors.Aqua);
        }

        public bool CollideWith (Circle circle)
        {
            return false;
        }

        public PrimitiveCircle ToPrimitive ()
        {
            return new PrimitiveCircle()
            {
                Position = Position,
                Radius = Radius
            };
        }
    }
}