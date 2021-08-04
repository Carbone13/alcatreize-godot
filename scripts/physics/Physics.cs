using System.Collections.Generic;
using Godot;

public class Physics : Node
{
    private static Physics sg;
    
    private List<AABB> boxes = new List<AABB>();

    public override void _Ready ()
    {
        sg = this;
    }

    public static void Register (Entity entity)
    {
        if (entity is AABB box)
        {
            if (sg != null)
            {
                if(!sg.boxes.Contains(box))
                    sg.boxes.Add(box);
            }
        }
    }

    public static void Unregister (Entity entity)
    {
        if(entity is AABB box)
            if (sg != null)
            {
                if(sg.boxes.Contains(box))
                    sg.boxes.Remove(box);
            }
    }
    
    public static IEnumerable<AABB> QueryBoxes ()
    {
        return sg.boxes;
    }

    /// <summary>
    /// Cast a Ray and return every colliders hit in its way
    /// </summary>
    /// <param name="position">Starting position of the Ray</param>
    /// <param name="direction">Normalized direction the Ray</param>
    /// <param name="length">Length of the Ray</param>
    /// <returns>Return a list of hit boxes</returns>
    public static IEnumerable<Hit> CastRay (sfloat2 position, sfloat2 direction, sfloat2 length)
    {
        List<Hit> hits = new List<Hit>();
        foreach (AABB box in QueryBoxes())
        {
            Hit hit = box.IntersectSegment(position, direction * length);
            if(hit != null)
                hits.Add(hit);
        }
        
        return hits;
    }

    /// <summary>
    /// Cast a Ray and return every colliders hit in its way
    /// </summary>
    /// <param name="position">Starting position of the Ray</param>
    /// <param name="delta">Traveled distance + direction</param>
    /// <returns>Return a list of hit boxes</returns>
    public static IEnumerable<Hit> CastRay (sfloat2 position, sfloat2 delta)
    {
        List<Hit> hits = new List<Hit>();
        foreach (AABB box in QueryBoxes())
        {
            Hit hit = box.IntersectSegment(position, delta);
            if(hit != null)
                hits.Add(hit);
        }

        return hits;
    }
    
    /// <summary>
    /// Cast an AABB and return every colliders overlapping it
    /// </summary>
    /// <param name="position">Center of the AABB</param>
    /// <param name="halfExtents">Half Extents of the AABB</param>
    /// <returns>Return a list of hit boxes</returns>
    public static IEnumerable<Hit> CastAABB (sfloat2 position, sfloat2 halfExtents)
    {
        List<Hit> hits = new List<Hit>();
        AABB castedBox = new AABB(position, halfExtents);
        
        foreach (AABB box in QueryBoxes())
        {
            Hit hit = box.IntersectAABB(castedBox);
            if(hit != null)
                hits.Add(hit);
        }

        return hits;
    }
}
