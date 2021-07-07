using System.Collections.Generic;
using Alcatreize.alcatreize;
using Godot;

namespace Alcatreize
{
    [Tool]
    public class Entity : Node2D
    {
        private sfloat2 _position;
        public new sfloat2 Position
        {
            get => _position;
            set
            {
                _position = value;
                Moved();
            }
        }

        public override void _Ready ()
        {
            if (this is Shape shape)
            {
                Physics.RegisterShape(shape);
            }
            
            Position = GlobalPosition;
        }

        public override void _Process (float delta)
        {
            if (Engine.EditorHint)
                Position = GlobalPosition;
        }

        protected virtual void Moved ()
        {
            if (this is Shape shape)
            {
                Physics.UpdateShapePosition(shape);
            }

            GlobalPosition = Position;
        }
    }
}