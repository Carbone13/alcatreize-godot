using System.Collections.Generic;
using System.Linq;
using Alcatreize.Broadphase;
using Alcatreize.Broadphase.GridShape;
using Godot;

namespace Alcatreize.alcatreize
{
    public class Physics : Node2D
    {
        public static Physics singleton;
        
        public DynamicGrid2D<int, Shape> DynamicGrid;
        public StaticGrid2D<Shape> StaticGrid;
        
        private int shapeCount = 0;
        
        public override void _Ready ()
        {
            DynamicGrid = new DynamicGrid2D<int, Shape>(Vector2.Zero, 1200, 700, new Vector2(20, 20));
            StaticGrid = new StaticGrid2D<Shape>(Vector2.Zero, 1200, 700, new Vector2(20, 20));
            
            singleton = this;
        }

        public override void _Draw ()
        {
            for (int i = 0; i < 1200; i += 20)
            {
                DrawLine(new Vector2(i, 0), new Vector2(i, 1200), Colors.Blue);
            }
            
            for (int i = 0; i < 1200; i += 20)
            {
                DrawLine(new Vector2(0, i), new Vector2(1200, i), Colors.Blue);
            }
        }

        public static void RegisterShape (Shape shape)
        {
            if (singleton == null) return;
            
            if (shape.Static)
            {
                singleton.StaticGrid.Add(shape, shape.GetEnglobingShape());
            }
            else
            {
                singleton.shapeCount++;
                shape.GridID = singleton.shapeCount;
                
                singleton.DynamicGrid.Add(shape.GridID, shape, shape.GetEnglobingShape());
            }
        }

        public static void UpdateShapePosition (Shape shape)
        {
            if (singleton == null) return;
            if (shape.Static) return;
            
            singleton.DynamicGrid.Update(shape.GridID, shape.GetEnglobingShape());
        }

        public static IEnumerator<Shape> GetShapeInRange (Circle range)
        {
            return singleton.DynamicGrid.Contact(range.ToGridShape()).Concat(
                singleton.StaticGrid.Contact(range.ToGridShape())) as IEnumerator<Shape>;
        }

        public static IEnumerable<Shape> GetShapeInRange (AABB range)
        {
            return singleton.DynamicGrid.Contact(range.ToGridShape()).Concat(
                singleton.StaticGrid.Contact(range.ToGridShape()));
        }
    }
}