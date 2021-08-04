using System.Collections.Generic;
using static sfloat;
using Godot;

[Tool]
public class AABB : Entity
{
    [Export] private Vector2 halfExtents;
    [Export] private Color color;
    [Export] private float thickness;

    public sfloat2 HalfExtents;

    protected override void GetEditorValue ()
    {
        Position = GlobalPosition;
        HalfExtents = halfExtents;
    }

    public override void _Draw ()
    {
        DrawRect(new Rect2(Vector2.Zero - (Vector2)HalfExtents, HalfExtents * (sfloat)2), 
            color, 
            false,
            thickness);
    }

    public AABB () {}
    
    public AABB (sfloat2 position, sfloat2 halfExtents)
    {
        Position = position;
        HalfExtents = halfExtents;
    }

    /// <summary>
    /// Check if we Intersect a given point in the Space
    /// </summary>
    /// <param name="point">A point in the scene</param>
    /// <returns>Return the collision informations or null if their is no collision</returns>
    public Hit IntersectPoint (sfloat2 point)
    {
        sfloat dx = point.X - Position.X;
        sfloat px = HalfExtents.X - Abs(dx);

        sfloat dy = point.Y - Position.Y;
        sfloat py = HalfExtents.Y - Abs(dy);

        if (px <= Zero|| py <= Zero)
            return null;

        Hit hit = new Hit(this);

        if (px < py)
        {
            sfloat sx = Sign(dx);
            hit.Delta.X = px * sx;
            hit.Normal.X = sx;
            hit.Position.X = Position.X + HalfExtents.X * sx;
            hit.Position.Y = point.Y;
        }
        else
        {
            sfloat sy = Sign(dy);
            hit.Delta.Y = py * sy;
            hit.Normal.Y = sy;
            hit.Position.X = point.X;
            hit.Position.Y = Position.Y + HalfExtents.Y * sy;
        }

        return hit;
    }

    /// <summary>
    /// Check if we Intersect a given segment
    /// </summary>
    /// <param name="position">The origin of the segment</param>
    /// <param name="delta">The direction & length of the segment</param>
    /// <param name="paddingX">Add some padding to ourself</param>
    /// <param name="paddingY">Add some padding to ourself</param>
    /// <returns>Return the collision informations or null if their is no collision</returns>
    public Hit IntersectSegment (sfloat2 position, sfloat2 delta, sfloat paddingX, sfloat paddingY)
    {
        sfloat scaleX = One / delta.X;
        sfloat scaleY = One / delta.Y;
        sfloat signX = Sign(scaleX);
        sfloat signY = Sign(scaleY);
        sfloat nearTimeX = (Position.X - signX * (HalfExtents.X + paddingX) - position.X) * scaleX;
        sfloat nearTimeY = (Position.Y - signY * (HalfExtents.Y + paddingY) - position.Y) * scaleY;
        sfloat farTimeX = (Position.X + signX * (HalfExtents.X + paddingX) - position.X) * scaleX;
        sfloat farTimeY = (Position.Y + signY * (HalfExtents.Y + paddingY) - position.Y) * scaleY;
        
        if (nearTimeY.IsNaN()) nearTimeY = PositiveInfinity * (delta.Y <= Zero ? MinusOne : One);
        if (nearTimeX.IsNaN()) nearTimeX = PositiveInfinity * (delta.X <= Zero ? MinusOne : One);
        if (farTimeY.IsNaN()) farTimeY = PositiveInfinity * (delta.Y <= Zero ? MinusOne : One);
        if (farTimeX.IsNaN()) farTimeX = PositiveInfinity * (delta.X <= Zero ? MinusOne : One);
        
        if (nearTimeX > farTimeY || nearTimeY > farTimeX)
            return null;

        sfloat nearTime = nearTimeX > nearTimeY ? nearTimeX : nearTimeY;
        sfloat farTime = farTimeX < farTimeY ? farTimeX : farTimeY;

        if (nearTime >= One || farTime <= Zero)
            return null;

        Hit hit = new Hit(this);
        hit.Time = Clamp(nearTime, Zero, One);

        if (nearTimeX > nearTimeY)
        {
            hit.Normal.X = -signX;
            hit.Normal.Y = Zero;
        }
        else
        {
            hit.Normal.X = Zero;
            hit.Normal.Y = -signY;
        }

        hit.Delta.X = (One - hit.Time) * -delta.X;
        hit.Delta.Y = (One - hit.Time) * -delta.Y;
        hit.Position.X = position.X + delta.X * hit.Time;
        hit.Position.Y = position.Y + delta.Y * hit.Time;

        return hit;
    }

    /// <summary>
    /// Check if we Intersect a given segment
    /// </summary>
    /// <param name="position">The origin of the segment</param>
    /// <param name="delta">The direction & length of the segment</param>
    /// <returns>Return the collision informations or null if their is no collision</returns>
    public Hit IntersectSegment (sfloat2 position, sfloat2 delta)
        => IntersectSegment(position, delta, Zero, Zero);

    /// <summary>
    /// Check if we Intersect with another AABB
    /// </summary>
    /// <param name="box">An AABB in the scene</param>
    /// <returns>Return the collision informations or null if their is no collision</returns>
    public Hit IntersectAABB (AABB box)
    {
        sfloat dx = box.Position.X - Position.X;
        sfloat px = box.HalfExtents.X + HalfExtents.X - Abs(dx);
        sfloat dy = box.Position.Y - Position.Y;
        sfloat py = box.HalfExtents.Y + HalfExtents.Y - Abs(dy);
        
        if (px <= Zero|| py <= Zero)
            return null;

        Hit hit = new Hit(this);
        if (px < py)
        {
            sfloat sx = (sfloat) Sign(dx);
            hit.Delta.X = px * sx;
            hit.Normal.X = sx;
            hit.Position.X = Position.X + HalfExtents.X * sx;
            hit.Position.Y = box.Position.Y;
        }
        else
        {
            sfloat sy = (sfloat) Sign(dy);
            hit.Delta.Y = py * sy;
            hit.Normal.Y = sy;
            hit.Position.X = box.Position.X;
            hit.Position.Y = Position.Y + HalfExtents.Y * sy;
        }

        return hit;
    }

    /// <summary>
    /// Check if we Intersect with another AABB that move
    /// </summary>
    /// <param name="box">An AABB in the scene</param>
    /// <param name="delta">The movement of the other box for this physic step</param>
    /// <returns>Return the sweep informations with the max allowed delta and a potential Hit informations</returns>
    public Sweep IntersectSweepAABB (AABB box, sfloat2 delta)
    {
        Sweep sweep = new Sweep();

        if (delta.X == Zero && delta.Y == Zero)
        {
            sweep.Position = box.Position;
            sweep.Hit = IntersectAABB(box);
            sweep.Time = sweep.Hit != null ? (sweep.Hit.Time = Zero) : One;
            return sweep;
        }

        sweep.Hit = IntersectSegment(box.Position, delta, box.HalfExtents.X, box.HalfExtents.Y);
        if (sweep.Hit != null)
        {
            sweep.Time = Clamp(sweep.Hit.Time - Epsilon, Zero, One);
            sweep.Position.X = box.Position.X + delta.X * sweep.Time;
            sweep.Position.Y = box.Position.Y + delta.Y * sweep.Time;
            
            sfloat2 direction = new sfloat2(delta.X, delta.Y);
            direction = direction.normalized;
            
            sweep.Hit.Position.X = Clamp
                (sweep.Hit.Position.X + direction.X * box.HalfExtents.X,
                Position.X - HalfExtents.X, Position.X + HalfExtents.X);
            sweep.Hit.Position.Y = Clamp
            (sweep.Hit.Position.Y + direction.Y * box.HalfExtents.Y,
                Position.Y - HalfExtents.Y, Position.Y + HalfExtents.Y);
        }
        else
        {
            sweep.Time = One;
            sweep.Position.X = box.Position.X + delta.X;
            sweep.Position.Y = box.Position.Y + delta.Y;
        }
        
        return sweep;
    }

    /// <summary>
    /// Use this for moving AABB, allow you to sweep test against a set of static colliders
    /// </summary>
    /// <param name="colliders">The set of colliders, i.e a broadphase query result</param>
    /// <param name="delta">The distance you want to travel</param>
    /// /// <returns>Return the sweep informations about the nearest collision or null if their is no collision</returns>
    public Sweep SweepInto (IEnumerable<AABB> colliders, sfloat2 delta)
    {
        Sweep nearest = new Sweep();
        nearest.Time = One;
        nearest.Position = Position + delta;

        foreach (AABB collider in colliders)
        {
            if (collider == this) continue;
            
            Sweep sweep = collider.IntersectSweepAABB(this, delta);
            if (sweep.Time < nearest.Time)
                nearest = sweep;
        }

        return nearest;
    }
}
