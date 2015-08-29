using UnityEngine;
using System.Collections;


//set_positionの拡張版
public static class CSTransform {
    //一つのVectorの値を変更
    public static void SetX(this Transform transform, float x)
    {
        Vector3 newPosition =
        new Vector3(x, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }
    public static void SetY(this Transform transform, float y)
    {
        Vector3 newPosition =
        new Vector3(transform.position.x, y, transform.position.z);
        transform.position = newPosition;
    }
    public static void SetZ(this Transform transform, float z)
    {
        Vector3 newPosition =
        new Vector3(transform.position.x, transform.position.y, z);
        transform.position = newPosition;
    }

    //x,yのみ変更しzは変えない
    public static void Set2DVector(this Transform transform,Vector2 v)
    {
        Vector3 newPosition =
            new Vector3(v.x, v.y, transform.position.z);
        transform.position = newPosition;
    }
    //position=new Vector3(v.x,v.y,v.z)をposition=CSTransform.CopyVector3(v)で作成
    public static Vector3 CopyVector3(Vector3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }
}
