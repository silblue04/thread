
public struct DefsDefault
{
    public const int VALUE_NONE = -1;
    public const int VALUE_ZERO = 0;
    public const int VALUE_ONE = 1;
    public const int VALUE_TWO = 2;
    public const int VALUE_MAX = 999;
    
    public const float DEFAULT_COMPARATIVE_TARGET_FLOAT = -1.0f;
    public const float DEFAULT_FLOAT = -2.0f;

    public const float MIN_PERCENTAGE = 0.0f;
    public const float MAX_PERCENTAGE = 100.0f;

    public const float MIN_RATIO = 0.0f;
    public const float MAX_RATIO = 1.0f;


    public const float DEFAULT_TIME_SCALE = 1.0f;

    public const int MAX_FRAME_RATE = 45;

    public const float DEFAULT_SCREEN_WIDTH = 720.0f;
    public const float DEFAULT_SCREEN_HEIGHT = 1280.0f;

    public const int MAX_SHADER_ARRAY_SIZE = 1023;
}

public class Param { }

public class ParamString : Param
{
    public string value;

    public ParamString()
    {
        value = null;
    }

    public ParamString(string newString)
    {
        value = newString;
    }

    public static implicit operator string(ParamString paramString)
    {
        return paramString.value;
    }
}

public class ParamInt : Param
{
    public int value;

    public ParamInt()
    {
        value = 0;
    }

    public ParamInt(int newValue)
    {
        value = newValue;
    }

    public static implicit operator int(ParamInt paramInt)
    {
        return paramInt.value;
    }
}

public class ParamFloat : Param
{
    public float value;

    public ParamFloat()
    {
        value = 0.0f;
    }

    public ParamFloat(float newValue)
    {
        value = newValue;
    }

    public static implicit operator float(ParamFloat paramFloat)
    {
        return paramFloat.value;
    }
}

public class Pair<T, U>
{
    public T first;
    public U second;

    public Pair()
    {

    }

    public Pair(T first, U second)
    {
        this.first = first;
        this.second = second;
    }

    public override bool Equals(object obj)
    {
        if(obj == null)
        {
            return false;
        }

        // if(obj == this)
        // {
        //     return true;
        // }

        Pair<T, U> other = obj as Pair<T, U>;
        if(other == null)
        {
            return false;
        }

        return (((first == null) && (other.first == null))
                || ((first != null) && first.Equals(other.first)))
              &&
            (((second == null) && (other.second == null))
                || ((second != null) && second.Equals(other.second)));
    }

    public static bool operator ==(Pair<T, U> a, Pair<T, U> b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Pair<T, U> a, Pair<T, U> b)
    {
        return !(a.Equals(b));
    }

    public override int GetHashCode()
    {
        int hashcode = 0;
        if (first != null)
            hashcode += first.GetHashCode();
        if (second != null)
            hashcode += second.GetHashCode();

        return hashcode;
    }
}