using System;
using Godot;

namespace Alcatreize
{
    public abstract class Actor : Entity
    {
        public sfloat2 Remainder;

        #region Virtual/Overridable functions
        
        /// <summary>
        /// Called when a Solid push ourself onto another Pushbox
        /// You should override it to provide your own logic when this happens
        /// </summary>
        public virtual void Squish () 
        {
            GD.Print("Squished between 2 colliders !");
        }

        /// <summary>
        /// A check called by a Solid, you shoud tell him if you are riding it
        /// If you ride it, you will move with him
        /// example: in a platformer you would return true if you are touching its top edge
        /// Always return false by default
        /// </summary>
        /// <param name="solid">The solid we are checking</param>
        /// <returns></returns>
        public virtual bool IsRiding (Solid solid)
        {
            return false;
        }
        
        #endregion
        
        #region Moving
        
        /// <summary>
        /// Move this actor by the specified amount
        /// </summary>
        /// <param name="amount">The amount to move</param>
        /// <param name="collideX">A callback when colliding on the X axis</param>
        /// <param name="collideY">A callback when colliding on the Y axis</param>
        /// <returns>True if we did move, or false if we did not</returns>
        public bool Move (sfloat2 amount, Action<int> collideX, Action<int> collideY)
        {
            bool x = HandleHorizontalMoves(amount.X, collideX);
            bool y = HandleVerticalMoves(amount.Y, collideY);

            return x || y;
        }

        // Move on the X Axis
        private bool HandleHorizontalMoves (sfloat amount, Action<int> collide)
        {
            bool hasMoved = false;
            Remainder.X += amount;
            int move = (int)libm.roundf(Remainder.X);
            
            if (move != 0)
            {
                Remainder.X -= (sfloat) move;
                int sign = Math.Sign(move);

                while (move != 0)
                {
                    if (IsPositionValid(GlobalPosition + new Vector2(sign, 0)))
                    {
                        GlobalPosition = new Vector2(GlobalPosition.x + sign, GlobalPosition.y);
                        move -= sign;
                        hasMoved = true;
                    }
                    else
                    {
                        collide?.Invoke(sign);
                        break;
                    }
                }
            }

            if(hasMoved)
                OnPostMove();
            
            return hasMoved;
        }

        // Move on the Y Axis
        private bool HandleVerticalMoves (sfloat amount, Action<int> collide)
        {
            bool hasMoved = false;
            Remainder.Y += amount;
            int move = (int)libm.roundf(Remainder.Y);
            
            if (move != 0)
            {
                Remainder.Y -= (sfloat) move;
                int sign = Math.Sign(move);

                while (move != 0)
                {
                    if (IsPositionValid(GlobalPosition + new Vector2(0, sign)))
                    {
                        GlobalPosition = new Vector2(GlobalPosition.x, GlobalPosition.y + sign);
                        move -= sign;
                        hasMoved = true;
                    }
                    else
                    {
                        collide?.Invoke(sign);
                        break;
                    }
                }
            }

            if(hasMoved)
                OnPostMove();
            
            return hasMoved;
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Clear all the Remainder
        /// </summary>
        public void ClearRemainder ()
        {
            Remainder = sfloat2.Zero;
        }

        /// <summary>
        /// Clear the X axis of the Remainder
        /// </summary>
        public void ClearRemainderX ()
        {
            Remainder.SetX(sfloat.One);
        }
        
        /// <summary>
        /// Clear the Y axis of the Remainder
        /// </summary>
        public void ClearRemainderY ()
        {
            Remainder.SetY(sfloat.One);
        }

        /// <summary>
        /// Return true if the specified position is valid, i.e if we won't intersect anything at the specified position
        /// </summary>
        /// <returns></returns>
        public bool IsPositionValid (Vector2 position)
        {
            foreach (Pushbox pushbox in GetAllIntersecting<Pushbox>(position))
            {
                if (pushbox.GetBoundsInScene().Intersects(Pushbox.GetBoundsAtPosition(position)))
                {
                    return false;
                }
            }

            return true;
        }
        

        #endregion

    }
}