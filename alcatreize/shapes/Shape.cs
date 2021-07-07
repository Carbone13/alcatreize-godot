using System;
using Alcatreize.Broadphase;
using Alcatreize.Broadphase.GridShape;
using Alcatreize.SAT;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace  Alcatreize
{
    public enum ShapeType
    {
        AABB, OBB, Circle, Capsule
    }
    
    /// <summary>
    /// Shape is actually an helper class for the Godot Editor,
    /// internally we compare PrimitiveShape instead of Shapes
    /// </summary>
    [Tool]
    public class Shape : Entity
    {
        [Export] private Color color = new Color(0.094118f, 0.462745f, 0.52549f, 0.545098f);
        [Export] public bool Static = false;
        [Export] public bool Active = true;
        [Export] public NodePath path;

        private bool _ignoreDirections;
        public bool IgnoreDirections
        {
            get => _ignoreDirections;
            set
            {
                _ignoreDirections = value;
                PropertyListChangedNotify();
            }
        }
        
        private ShapeType _shapeType = ShapeType.Circle;
        public ShapeType ShapeType
        {
            get => _shapeType;
            set
            {
                _shapeType = value;
                PropertyListChangedNotify();
            }
        }

        public new float Rotation = 0;
        public Vector2 HalfExtents = new Vector2(20, 20);
        public float Radius = 20;
        public float Height = 50;
        
        public int FoundOn = 1;
        public int SearchOn = 1;

        public int GridID = -1;
        
        public override void _Process (float delta)
        {
            base._Process(delta);
            GetEnglobingShape();
            Update();
        }

        public Primitive GetShape ()
        {
            Primitive shape = null;
            
            switch (ShapeType)
            {
                case ShapeType.AABB:
                    shape = new AABB(Position, HalfExtents);
                    break;
                case ShapeType.OBB:
                    shape = new OBB(Position, HalfExtents, (sfloat)Rotation);
                    break;
                case ShapeType.Circle:
                    shape = new Circle(Position, (sfloat)Radius);
                    break;
                case ShapeType.Capsule:
                    shape = new Capsule(Position, (sfloat) Radius, (sfloat) Height, (sfloat)Rotation);
                    break;
            }

            return shape;
        }

        public IConvex2D GetEnglobingShape ()
        {
            Primitive shape = GetShape();

            GetNodeOrNull<drawer>(path).toDraw[this] = shape.ToGridShape();
            return shape.ToGridShape();
        }
        
        public override void _Draw ()
        {
            switch (ShapeType)
            {
                case ShapeType.AABB:
                    DrawRect(new Rect2(-HalfExtents, HalfExtents * 2), color);
                    break;
                case ShapeType.OBB:
                    DrawSetTransform(Vector2.Zero, (float)sfloat.Deg2Rad((sfloat)Rotation), Vector2.One);
                    
                    DrawRect(new Rect2(-HalfExtents, HalfExtents * 2), color);
                    
                    DrawSetTransform(Vector2.Zero, 0, Vector2.One);
                    break;
                case ShapeType.Circle:
                    DrawCircle(Vector2.Zero, Radius, color);
                    break;
                case ShapeType.Capsule:
                    DrawSetTransform(Vector2.Zero, (float)sfloat.Deg2Rad((sfloat)Rotation), Vector2.One);
                    
                    CapsuleShape2D shape = new CapsuleShape2D();
                    shape.Height = Height;
                    shape.Radius = Radius;
                    shape.Draw(GetCanvasItem(), color);
                    
                    DrawSetTransform(Vector2.Zero, 0, Vector2.One);
                    break;
            }
        }
        
        public override Array _GetPropertyList ()
        {
            Array properties = new Array();

            properties.Add(new Dictionary 
            { 
                {"name", nameof(IgnoreDirections)}, 
                {"type", Variant.Type.Bool},
                {"usage", PropertyUsageFlags.Default} 
            });

            if (IgnoreDirections)
            {
                properties.Add(new Dictionary 
                { 
                    {"name", "Ignored Direction"}, 
                    {"type", Variant.Type.Nil},
                    {"usage", PropertyUsageFlags.Category} 
                });
                
                properties.Add(new Dictionary 
                { 
                    {"name", "Shape Properties"}, 
                    {"type", Variant.Type.Nil},
                    {"usage", PropertyUsageFlags.Editor} 
                });
            }

            properties.Add(new Dictionary 
            { 
                {"name", "Shape Properties"}, 
                {"type", Variant.Type.Nil},
                {"usage", PropertyUsageFlags.Group} 
            });
            
            properties.Add(new Dictionary 
            { 
                {"name", nameof(ShapeType)}, 
                {"type", Variant.Type.Int}, 
                {"hint", PropertyHint.Enum},
                {"hint_string", "Rectangle, Oriented Rectangle, Circle, Capsule"},
                {"usage", PropertyUsageFlags.Default} 
            });

            switch (ShapeType)
            {
                case ShapeType.Circle:
                    properties.Add(new Dictionary 
                    { 
                        {"name", nameof(Radius)}, 
                        {"type", Variant.Type.Real},
                        {"hint_string", "Raza"},
                        {"usage", PropertyUsageFlags.Default} 
                    });
                    break;
                case ShapeType.AABB:
                    properties.Add(new Dictionary 
                    { 
                        {"name", nameof(HalfExtents)}, 
                        {"type", Variant.Type.Vector2},
                        {"usage", PropertyUsageFlags.Default} 
                    });
                    break;
                case ShapeType.OBB:
                    properties.Add(new Dictionary 
                    { 
                        {"name", nameof(HalfExtents)}, 
                        {"type", Variant.Type.Vector2},
                        {"usage", PropertyUsageFlags.Default} 
                    });
                    properties.Add(new Dictionary 
                    { 
                        {"name", nameof(Rotation)}, 
                        {"type", Variant.Type.Real},
                        {"hint", PropertyHint.Range},
                        {"hint_string", "-360,360"},
                        {"usage", PropertyUsageFlags.Default} 
                    }); break;
                case ShapeType.Capsule:
                    properties.Add(new Dictionary 
                    { 
                        {"name", nameof(Radius)}, 
                        {"type", Variant.Type.Real},
                        {"hint_string", "Raza"},
                        {"usage", PropertyUsageFlags.Default} 
                    });
                    properties.Add(new Dictionary 
                    { 
                        {"name", nameof(Height)}, 
                        {"type", Variant.Type.Real},
                        {"hint_string", "Raza"},
                        {"usage", PropertyUsageFlags.Default} 
                    });
                    properties.Add(new Dictionary 
                    { 
                        {"name", nameof(Rotation)}, 
                        {"type", Variant.Type.Real},
                        {"hint", PropertyHint.Range},
                        {"hint_string", "-360,360"},
                        {"usage", PropertyUsageFlags.Default} 
                    });
                    break;
            }
            
            properties.Add(new Dictionary 
            { 
                {"name", "Layers"}, 
                {"type", Variant.Type.Nil},
                {"usage", PropertyUsageFlags.Group} 
            });
            
            properties.Add(new Dictionary 
            { 
                {"name", nameof(FoundOn)}, 
                {"type", Variant.Type.Int},
                {"hint", PropertyHint.Layers2dPhysics},
                {"usage", PropertyUsageFlags.Default} 
            });
            
            properties.Add(new Dictionary 
            { 
                {"name", nameof(SearchOn)}, 
                {"type", Variant.Type.Int},
                {"hint", PropertyHint.Layers2dPhysics},
                {"usage", PropertyUsageFlags.Default} 
            });

            return properties;
        }
    }
}