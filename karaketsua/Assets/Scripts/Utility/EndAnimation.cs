//EndAnimation
//作成日
//2015/6/21
//<summary>
//アニメーションが終了したら削除する
//
//</summary>
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EndAnimation : MonoBehaviour
{
	void Awake ()
	{
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        Destroy(this.gameObject, particleSystem.duration);
	}
}