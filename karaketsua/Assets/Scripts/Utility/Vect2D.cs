using UnityEngine;
using System.Collections.Generic;
using System;


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
    public static IntVect2D GetDirection(IntVect2D oldVect, IntVect2D newVect)
    {
        return new IntVect2D(newVect.x - oldVect.x, newVect.y - oldVect.y);
    }
    public static IntVect2D Clone(IntVect2D vect)
    {
        return new IntVect2D(vect.x, vect.y);
    }

    //最大方向の取得
    public static IntVect2D GetDirectionFromVector2(Vector2 delta)
    {
        //x方向
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            return new IntVect2D((int)Mathf.Sign(delta.x), 0);

        }
        //y方向
        else
        {
            return new IntVect2D(0, (int)Mathf.Sign(delta.y));
        }
    }

    //距離の取得
    public static float Distance(IntVect2D v1,IntVect2D v2)
    {
        return Vector2.Distance(new Vector2(v1.x, v1.y), new Vector2(v2.x, v2.y));
    }


}

//IntVect2D
public static class IntVect2DExtend
{
    // デバッグ表示
    public static void Log(this IntVect2D vect)
    {
        Debug.Log("x:" + vect.x);
        Debug.Log("y:" + vect.y);
    }

    //シャッフル
    public static IntVect2D[] Shuffle(this IntVect2D[] array)
    {
        return Shuffle(array, new System.Random());
    }
    public static IntVect2D[] Shuffle(this IntVect2D[] array, System.Random random)
    {
        if (array == null)
            throw new ArgumentNullException("array");
        if (random == null)
            throw new ArgumentNullException("random");

        var shuffled = (IntVect2D[])array.Clone();

        if (shuffled.Length < 2)
            return shuffled;

        for (var i = 1; i < shuffled.Length; i++)
        {
            var j = random.Next(0, i + 1);

            // swap
            var temp = shuffled[i];
            shuffled[i] = shuffled[j];
            shuffled[j] = temp;
        }

        return shuffled;
    }

}