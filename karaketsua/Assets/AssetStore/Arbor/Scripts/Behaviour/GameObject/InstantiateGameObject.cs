using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("InstantiateGameObject")]
	[AddBehaviourMenu("GameObject/InstantiateGameObject")]
	[BuiltInBehaviour]
	public class InstantiateGameObject : StateBehaviour
	{
		[SerializeField] private GameObject _Prefab;
		[SerializeField] private Transform _Parent;

		public override void OnStateBegin()
		{
			if( _Prefab != null )
			{
				GameObject obj = Instantiate( _Prefab ) as GameObject;

				Vector3 localPosition = obj.transform.localPosition;
				Quaternion localRotation = obj.transform.localRotation;
				Vector3 localScale = obj.transform.localScale;

				obj.transform.parent = _Parent;

				obj.transform.localPosition = localPosition;
				obj.transform.localRotation = localRotation;
				obj.transform.localScale = localScale;
			}
		}
	}
}