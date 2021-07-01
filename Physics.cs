using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using Godot;
using Alcatreize.Broadphase;

// We use SuperGrid2D for the BroadPhase, because I was too lazy to make my own spatial hashing solution
// (I have renamed namespaces and some classes for cleaner structure, but everything inside the Broadphase namespace come from it)
// Credit to : https://github.com/bartofzo/SuperGrid2D
namespace Alcatreize
{
    /// <summary>
    /// How do we handle space partitioning ?
    /// One Grid Only : We use one big grid for everything, and then sort the results
    /// One Grid Per Layer : We use multiple grid foreach collider types
    /// The first one is slower but take less memory
    /// The second one is the fastest but take more memory
    /// </summary>
    public enum HashingMode
    {
        ONE_GRID_ONLY,
        ONE_GRID_PER_LAYER
    }
    /// <summary>
    /// Act as a Global Manager.
    /// It handle the Grid, and allow to query near colliders
    /// </summary>
    public class Physics : Node2D
    {
        public static Physics singleton;
        
        private StaticGrid2D<Pushbox> statisPushboxGrid;
        private DynamicGrid2D<uint, Pushbox> pushboxGrid;
        private DynamicGrid2D<uint, Hitbox> hitboxGrid;
        private DynamicGrid2D<uint, Hurtbox> hurtboxGrid;
        
        private uint registeredPushboxCount;
        private uint registeredHitboxCount;
        private uint registeredHurtboxCount;
        
        public override void _Ready ()
        {
            pushboxGrid = new DynamicGrid2D<uint, Pushbox>(Vector2.Zero, 1500, 1500);
            statisPushboxGrid = new StaticGrid2D<Pushbox>(Vector2.Zero, 1500, 1500);
            hitboxGrid = new DynamicGrid2D<uint, Hitbox>(Vector2.Zero, 1500, 1500);
            hurtboxGrid = new DynamicGrid2D<uint, Hurtbox>(Vector2.Zero, 1500, 1500);
            
            singleton = this;
        }

        public override void _Draw ()
        {
            for (int c = 0; c <= 800; c += 800 / 16)
            {
                DrawLine(new Vector2(c, 0), new Vector2(c, 600), Colors.Blue);
            }
            
            for (int r = 0; r <= 800; r += 800 / 16)
            {
                DrawLine(new Vector2(0, r), new Vector2(800, r), Colors.Blue);
            }
        }

        /// <summary>
        /// Register an AABB on the manager
        /// You should register every AABBs you want to use
        /// </summary>
        /// <param name="AABB">The specified AABB</param>
        public static void Register (AABB AABB)
        {
            switch (AABB)
            {
                case Pushbox pushbox:
                    
                    if (pushbox.IsStatic)
                    {
                        singleton.statisPushboxGrid.Add(pushbox, pushbox.GetBoundsInScene());
                    }
                    else
                    {
                        singleton.pushboxGrid.Add(singleton.registeredPushboxCount, pushbox, pushbox.GetBoundsInScene());
                    }

                    pushbox.GridID = (int)singleton.registeredPushboxCount;
                    singleton.registeredPushboxCount++;
                    break;
                case Hitbox hitbox:
                    singleton.hitboxGrid.Add(singleton.registeredHitboxCount, hitbox, hitbox.GetBoundsInScene());
                    hitbox.GridID = (int) singleton.registeredHitboxCount;
                    singleton.registeredHitboxCount++;
                    break;
                case Hurtbox hurtbox:
                    singleton.hurtboxGrid.Add(singleton.registeredHurtboxCount, hurtbox, hurtbox.GetBoundsInScene());
                    hurtbox.GridID = (int) singleton.registeredHurtboxCount;
                    singleton.registeredHurtboxCount++;
                    break;
            }
        }

        /// <summary>
        /// Unregister the aabb
        /// </summary>
        /// <param name="AABB"></param>
        public static void Unregister (AABB AABB)
        {
            switch (AABB)
            {
                case Pushbox pushbox:
                    if (pushbox.IsStatic)
                    {
                        GD.PrintErr("Can't remove a static box !");
                    }
                    else
                    {
                        singleton.pushboxGrid.Remove( (uint)pushbox.GridID);
                    }

                    pushbox.GridID = -1;
                    break;
                case Hitbox hitbox:
                    singleton.hitboxGrid.Remove((uint)hitbox.GridID);
                    hitbox.GridID = -1;
                    break;
                case Hurtbox hurtbox:
                    singleton.hurtboxGrid.Remove((uint) hurtbox.GridID);
                    hurtbox.GridID = -1;
                    break;
            }
        }

        /// <summary>
        /// Update an AABB, should be called when you have moved or resized it
        /// </summary>
        /// <param name="AABB">The specified AABB</param>
        public static void Update (AABB AABB)
        {
            switch (AABB)
            {
                case Pushbox pushbox:
                    if (pushbox.IsStatic)
                    {
                        GD.PrintErr("Can't update a static box !");
                        break;
                    }

                    if (pushbox.GridID == -1)
                    {
                        GD.Print("You tried to update a non-registered box, box has been automatically registered");
                        Register(pushbox);
                    }
                    else
                    {
                        singleton.pushboxGrid.Update((uint)pushbox.GridID, pushbox.GetBoundsInScene());
                    }
                    break;
                case Hitbox hitbox:
                    if (hitbox.GridID == -1)
                    {
                        GD.Print("You tried to update a non-registered box, box has been automatically registered");
                        Register(hitbox);
                    }
                    else
                    {
                        singleton.hitboxGrid.Update((uint)hitbox.GridID, hitbox.GetBoundsInScene());
                    }
                    break;
                case Hurtbox hurtbox:
                    if (hurtbox.GridID == -1)
                    {
                        GD.Print("You tried to update a non-registered box, box has been automatically registered");
                        Register(hurtbox);
                    }
                    else
                    {
                        singleton.hurtboxGrid.Update((uint)hurtbox.GridID, hurtbox.GetBoundsInScene());
                    }
                    break;
            }
        }

        public static IEnumerable<T> GetInRange<T> (Rect2 range) where T : AABB
        {
            if (typeof(T) == typeof(Pushbox))
            {
                return singleton.statisPushboxGrid.Contact(range).Concat(
                    singleton.pushboxGrid.Contact(range)) as IEnumerable<T>;
            }
            if (typeof(T) == typeof(Hitbox))
            {
                return singleton.hitboxGrid.Contact(range) as IEnumerable<T>;
            }
            if (typeof(T) == typeof(Hurtbox))
            {
                return singleton.hurtboxGrid.Contact(range) as IEnumerable<T>;
            }

            return null;
        }

    }
}