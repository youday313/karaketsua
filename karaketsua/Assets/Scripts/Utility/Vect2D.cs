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

[System.Serializable]
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
    public static bool IsEqual(IntVect2D v1,IntVect2D v2)
    {
        return v1.x == v2.x&&v1.y==v2.y ;
    }
    public bool IsEqual(IntVect2D v)
    {
        return x == v.x && y == v.y;
    }
    //null判別用
    public const int nullNumber = 1000;
    public static bool IsNull(IntVect2D v)
    {
        return v.x == nullNumber && v.y == nullNumber;
    }
    public static bool IsNeighbor(IntVect2D v1,IntVect2D v2)
    {
        return (Mathf.Abs(v1.x - v2.x) + Mathf.Abs(v1.y - v2.y)) <=1;
    }
    public static IntVect2D Sub(IntVect2D v1,IntVect2D v2)
    {
        return new IntVect2D(v1.x-v2.x,v1.y-v2.y);
    }
	public static IntVect2D Add(IntVect2D v1,IntVect2D v2){
		return new IntVect2D (v1.x + v2.x,v1.y + v2.y);
	}
}

#endregion
