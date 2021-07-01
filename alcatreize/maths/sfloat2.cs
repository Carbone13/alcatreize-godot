public struct sfloat2
{
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
    
    public sfloat2 (sfloat _x, sfloat _y)
    {
        X = _x;
        Y = _y;
    }

    public sfloat2 (float _x, float _y)
    {
        X = (sfloat) _x;
        Y = (sfloat) _y;
    }
    
    public sfloat2 (int _x, int _y)
    {
        X = (sfloat) _x;
        Y = (sfloat) _y;
    }
    
    public sfloat2 (Godot.Vector2 vector)
    {
        X = (sfloat) vector.x;
        Y = (sfloat) vector.y;
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
   
    #region Setter
    public void SetX (sfloat _x)
    {
        X = _x;
    }

    public void SetY (sfloat _y)
    {
        Y = _y;
    }
    
    public void SetX (int _x)
    {
        X = (sfloat)_x;
    }

    public void SetY (int _y)
    {
        Y = (sfloat)_y;
    }
    
    public void SetX (float _x)
    {
        X = (sfloat)_x;
    }

    public void SetY (float _y)
    {
        Y = (sfloat) _y;
    }

    #endregion
    
    #region Functions
    
    public sfloat Length ()
    {
        return libm.sqrtf(X * X + Y * Y);
    }
    
        
    public sfloat2 Normalize ()
    {
        sfloat length = Length();
        
        return new sfloat2(X / length, Y / length);
    }
    
    #endregion

    #region Operators
    
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

    public static sfloat2 operator -(sfloat2 input)
    {
        return new sfloat2(-input.X, -input.Y);
    }

    #endregion
}
