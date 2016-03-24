using UnityEngine;
using System.Collections;

using FieldScene;

public class FFollowPlayerIn2D : FFollowPlayer
{

    void Update()
    {
        // 自分自身の座標に、targetの座標に相対座標を足した値を設定する
        CSTransform.SetX(transform, target.position.x + offset.x);
    }

}
