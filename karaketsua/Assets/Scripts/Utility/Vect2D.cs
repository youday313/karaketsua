using UnityEngine;


#region::vect2D
//C#のジェネリックはC++のテンプレートと異なり演算子オーバーロードはできない
[System.Serializable]
public struct Vect2D<T>
    where T : struct
{
    public T x;
    public T y;
    public Vect2D(T _x, T _y)
    {
        x = _x;
        y = _y;
    }
}

public class IntVect2D
{
    public int x;
    public int y;
    public IntVect2D(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
    public IntVect2D(IntVect2D vect)
    {
        x = vect.x;
        y = vect.y;
    }
    public static bool IsEqual(IntVect2D t1,IntVect2D t2)
    {
        return t1.x == t2.x&&t1.y==t2.y ;
    }
    public bool IsEqual(IntVect2D t)
    {
        return x == t.x && y == t.y;
    }
    //null判別用
    public const int nullNumber = 1000;
    public static bool IsNull(IntVect2D t)
    {
        return t.x == nullNumber && t.y == nullNumber;
    }
    public static bool IsNeighbor(IntVect2D t1,IntVect2D t2)
    {
        return (Mathf.Abs(t1.x - t2.x) + Mathf.Abs(t1.y - t2.y)) <=1;
    }
}

#endregion
