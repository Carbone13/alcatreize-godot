using Godot;

/// <summary>
/// Something that exist in the Scene with a Position
/// </summary>
public class Entity : Node2D
{
    private sfloat2 _position = sfloat2.Zero;
    public new sfloat2 Position
    {
        get => _position;
        set
        {
            _position = value;
            MoveNodeTo(_position);
        }
    }

    public override void _Ready ()
    {
        GetEditorValue();
        Physics.Register(this);
    }

    public override void _Process (float delta)
    {
        if(Engine.EditorHint)
            GetEditorValue();
        
        Update();
    }
    
    public override void _EnterTree ()
    {
        Physics.Register(this);
    }
    

    public override void _ExitTree ()
    {
        Physics.Unregister(this);
    }

    protected virtual void GetEditorValue ()
    {
        Position = GlobalPosition;
    }

    private void MoveNodeTo (sfloat2 position)
    {
        GlobalPosition = position;
    }
}
