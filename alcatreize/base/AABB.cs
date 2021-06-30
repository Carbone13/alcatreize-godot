﻿using Alcatreize.Broadphase;
using Godot;

namespace Alcatreize
{
    // Represent a Axis Aligned Bounding Box
    public abstract class AABB : Node2D
    {
        [Export] private Rect2 bounds;
        [Export (PropertyHint.Layers2dPhysics)] private int PresentOn = 1;
        [Export (PropertyHint.Layers2dPhysics)] private int SearchOn = 1;

        public int GridID = -1;

        public override void _Ready ()
        {
            Physics.Register(this);
        }

        // Return the shape "settings" (= the one registered in the editor)
        public Rect2 GetBoundsSettings () => bounds;
        
        // Return the shape, according to the position in the scene
        public Rect2 GetBounds () =>
            new Rect2(GlobalPosition + bounds.Position, Scale * bounds.Size);

        public Rect2 GetBounds (Vector2 position) =>
            new Rect2(position + bounds.Position, Scale * bounds.Size);
    }
}