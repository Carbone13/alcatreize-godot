using System.Collections.Generic;
using Alcatreize.Broadphase;
using Alcatreize.Broadphase.GridShape;
using Godot;

namespace Alcatreize.alcatreize
{
    public class Physics : Node2D
    {
        public static Physics singleton;
        
        public DynamicGrid2D<int, Shape> grid;
        private int shapeCount;
        
        public override void _Ready ()
        {
            singleton = this;
            grid = new DynamicGrid2D<int, Shape>(Vector2.Zero, 1200, 700, new Vector2(50, 50));
        }
    }
}