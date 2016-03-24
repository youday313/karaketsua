using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using FieldScene;

namespace FieldScene{

public class FFollowPlayer : MonoBehaviour {

		public Transform target;    // ターゲットへの参照
	[SerializeField]	
    private Vector3 offset;     // 相対座標

		void Start ()
		{
			//自分自身とtargetとの相対距離を求める
			//offset = GetComponent<Transform>().position - target.position;
		}

		void Update ()
		{
			// 自分自身の座標に、targetの座標に相対座標を足した値を設定する
			GetComponent<Transform>().position = target.position + offset;
		}
}

}