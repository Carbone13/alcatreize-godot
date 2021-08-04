using Godot;

public struct sfloat2
{
    public bool Equals (sfloat2 other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y);
    }

    public sfloat X { get; set; }
    public sfloat Y { get; set; }

    public override string ToString () => "x: " + X + "; y: " + Y;

    #region Constants

    public static readonly sfloat2 Zero = new sfloat2(sfloat.Zero, sfloat.Zero);
    public static readonly sfloat2 One = new sfloat2(sfloat.One, sfloat.One);
    
    public static readonly sfloat2 Right = new sfloat2(sfloat.One, sfloat.Zero);
    public static readonly sfloat2 Left = new sfloat2(sfloat.MinusOne, sfloat.Zero);
    public static readonly sfloat2 Up = new sfloat2(sfloat.Zero, sfloat.One);
    public static readonly sfloat2 Down = new sfloat2(sfloat.Zero, sfloat.MinusOne);
    
    #endregion
    
    #region Basic Constructor
    
    public sfloat2 (sfloat x, sfloat y)
    {
        X = x;
        Y = y;
    }

    public sfloat2 (float x, float y)
    {
        X = (sfloat) x;
        Y = (sfloat) y;
    }
    
    public sfloat2 (int x, int y)
    {
        X = (sfloat) x;
        Y = (sfloat) y;
    }

    #endregion

    #region Static Constructor
    
    public static sfloat2 FromRaw (uint _x, uint _y)
    {
        return new sfloat2
        (
            sfloat.FromRaw(_x),
            sfloat.FromRaw(_y)
        );
    }
    
    #endregion
    
    #region Serialization
    // So Godot don't really like sending custom types, but we can send string, so these functions
    // allow you the serialize and deserialize this struct as string
    public string Serialize ()
    {
        return X.RawValue + ";" + Y.RawValue;
    }

    public static sfloat2 FromString (string input)
    {
        return new sfloat2
        (
            sfloat.FromRaw( uint.Parse(input.Split(';')[0]) ),
            sfloat.FromRaw( uint.Parse(input.Split(';')[1]) )
        );
    }
    
    #endregion

    #region Functions
    
    public sfloat SquaredLength => X * X + Y * Y;

    public sfloat Length
    {
        get => libm.sqrtf(SquaredLength);
        set
        {
            sfloat eps = sfloat.Epsilon;
            sfloat angle = libm.atan2f(Y, X);

            X = libm.cosf(angle) * value;
            Y = libm.sinf(angle) * value;

            if (sfloat.Abs(X) < eps) X = sfloat.Zero;
            if (sfloat.Abs(Y) < eps) Y = sfloat.Zero;
        }
    }
    
    /// <summary>
    /// Return a sfloat2 corresponding to the normalized vector if this one
    /// /!\ Won't change the original sfloat2 at all
    /// </summary>
    public sfloat2 normalized
    {
        get
        {
            var len = Length;

            if (len == sfloat.Zero)
                return this;
            return new sfloat2(X / len, Y / len);
        }
    }

    /// <summary>
    /// Normalize this sfloat2, making its length = 1
    /// </summary>
    public void Normalize ()
    {
        this = this.normalized;
    }

    /// <summary>
    /// Truncate this sfloat2, so its length don't exceed the specified max length
    /// </summary>
    /// <param name="max">The specified max length</param>
    /// <returns></returns>
    public void Truncate (sfloat max)
    {
        Length = sfloat.Min(max, Length);
    }

    /// <summary>
    /// Return the inverted version of this sfloat2
    /// </summary>
    public sfloat2 inverted => new sfloat2(-X, -Y);
    
    /// <summary>
    /// Invert the X and Y component of this sfloat2
    /// </summary>
    public void Invert ()
    {
        X = -X;
        Y = -Y;
    }

    /// <summary>
    /// Return the Dot product between 2 sfloat2
    /// </summary>
    /// <param name="first">The first sfloat2</param>
    /// <param name="other">The second sfloat2</param>
    public static sfloat Dot (sfloat2 first, sfloat2 other)
    {
        return first.X * other.X + first.Y * other.Y;
    }
    
    /// <summary>
    /// Return the Cross product between 2 sfloat2
    /// </summary>
    /// <param name="first">The first sfloat2</param>
    /// <param name="other">The second sfloat2</param>
    public static sfloat Cross (sfloat2 first, sfloat2 other)
    {
        return first.X * other.X - first.Y * other.Y;
    }

    /// <summary>
    /// Return the signed vector
    /// </summary>
    /// <returns></returns>
    public static sfloat2 Sign (sfloat2 vector)
    {
        return new sfloat2(sfloat.Sign(vector.X), sfloat.Sign(vector.Y));
    }

    #endregion

    #region Operators

    public static implicit operator Vector2 (sfloat2 input)
    {
        return new Vector2((float)input.X, (float)input.Y);
    }

    public static implicit operator sfloat2 (Vector2 input)
    {
        return new sfloat2(input.x, input.y);
    }
    
    public static sfloat2 operator *(sfloat2 input, sfloat factor)
    {
        return new sfloat2(input.X * factor, input.Y * factor);
    }
    
    public static sfloat2 operator *(sfloat2 input, sfloat2 factor)
    {
        return new sfloat2(input.X * factor.X, input.Y * factor.Y);
    }
    
    public static sfloat2 operator /(sfloat2 input, sfloat factor)
    {
        return new sfloat2(input.X / factor, input.Y / factor);
    }
    
    public static sfloat2 operator /(sfloat2 input, sfloat2 factor)
    {
        return new sfloat2(input.X / factor.X, input.Y / factor.Y);
    }

    public static sfloat2 operator - (sfloat2 one, sfloat2 two)
    {
        return new sfloat2(one.X - two.X, one.Y - two.Y);
    }
    
    public static sfloat2 operator + (sfloat2 one, sfloat2 two)
    {
        return new sfloat2(one.X + two.X, one.Y + two.Y);
    }

    public static sfloat2 operator -(sfloat2 input)
    {
        return new sfloat2(-input.X, -input.Y);
    }
    
    public static bool operator ==(sfloat2 a, sfloat2 b)
    {
        return (a.X == b.X && a.Y == b.Y);
    }
    
    public static bool operator !=(sfloat2 a, sfloat2 b)
    {
        return (a.X != b.X || a.Y != b.Y);
    }

    #endregion
}
