using System;
using Godot;

public class Player : Actor
{
    [Export] private float JumpHeight;
    [Export] private float JumpApexTime;
    
    private sfloat2 Velocity;

    private sfloat Gravity, Jumpforce;
    private bool Grounded;
    
    public override void _Ready ()
    {
        base._Ready();
        if(!Engine.EditorHint)
            Input.SetMouseMode(Input.MouseMode.Hidden);
        else
        {
            Input.SetMouseMode(Input.MouseMode.Visible);
        }
        
        Gravity = (sfloat) (2 * JumpHeight) / (sfloat) (Math.Pow(JumpApexTime, 2));
        Jumpforce = (sfloat) Math.Sqrt(2 * (float)Gravity * JumpHeight);
    }

    public override void _PhysicsProcess (float delta)
    {
        float x = (int)Input.GetActionStrength("ui_left") * -1 + (int)Input.GetActionStrength("ui_right");
        x += Input.GetJoyAxis(0, 0);
        x = x > 1 ? 1 : x;
        x = x < -1 ? -1 : x;
        bool jump = Input.IsActionJustPressed("jump");

        Velocity.X = (sfloat)65 * (sfloat)x;

        if (jump && Grounded)
        {
            Grounded = false;
            Velocity.Y = -Jumpforce;
        }
        else if(!Grounded)
            Velocity.Y += Gravity * (sfloat) delta;

        MoveAndSlide(Velocity * (sfloat)delta, Collided);
        
        Grounded = IsGrounded();
    }

    /// <summary>
    /// Callback called when you collide while going in a direction
    /// </summary>
    /// <param name="normal">The collision normal</param>
    private void Collided (sfloat2 normal)
    {
        if (normal.X != sfloat.Zero)
        {
            Velocity.X = sfloat.Zero;
        }

        if (normal.Y != sfloat.Zero)
        {
            Velocity.Y = sfloat.Zero;
        }
    }
    
    private void Land ()
    {
        Grounded = false;
        Velocity.Y = sfloat.Zero;
    }
    
    private bool IsGrounded ()
    {
        foreach (AABB aabb in Physics.QueryBoxes())
        {
            if (aabb != Collider)
            {
                if (aabb.IntersectAABB(new AABB(Position + new sfloat2(sfloat.Zero, (sfloat) 0.05f),
                    Collider.HalfExtents)) != null)
                {
                    if(!Grounded) Land();
                    
                    return true;
                }
                    
            }
        }

        return false;
    }
}