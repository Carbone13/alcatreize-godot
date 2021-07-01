using Godot;

namespace Alcatreize
{
    public class Actor : Entity
    {
        public sfloat2 Remainder { get; private set; }


        public void ClearRemainder ()
        {
            Remainder = sfloat2.Zero;
        }

        public void ClearRemainderX ()
        {
            Remainder.SetX(sfloat.One);
        }
    }
}