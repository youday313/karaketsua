//DestroyedAfterEndAnimation
//作成日
//<summary>
//Animatorを用いてAnimationから自身の削除を呼ぶ
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
[RequireComponent(typeof(Animator))]


public class DestroyedAfterEndAnimation : MonoBehaviour
{
	//public

	//private
    public void DestroyObject()
    {
        Destroy(transform.parent.gameObject);
    }
}