using Godot;

namespace Alcatreize.maths
{
    // help classes for Godot Rect2
    public static class rect2
    {
        /// <summary>
        /// Inflate by a specified amount, inflating by 1 will add 2 unit wide to the X axis, same for the Y
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Rect2 Inflate (this Rect2 input, int amount)
        {
            return new Rect2(input.Position - new Vector2(amount, amount), input.Size + new Vector2(amount, amount));
        }
    }
}