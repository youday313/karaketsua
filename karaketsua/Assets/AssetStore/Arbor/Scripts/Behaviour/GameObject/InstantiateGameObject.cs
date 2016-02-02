using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/InstantiateGameObject")]
	[BuiltInBehaviour]
	public class InstantiateGameObject : StateBehaviour, ISerializationCallbackReceiver
	{
		[SerializeField] private GameObject _Prefab;

		[FormerlySerializedAs("_Parent")]
		[SerializeField] private Transform _OldParent;

		[FormerlySerializedAs("_InitTransform")]
		[SerializeField] private Transform _OldInitTransform;

		[SerializeField] private GameObjectParameterReference _Parameter;

		[SerializeField]
		private int _SerializeVersion;

		[SerializeField]
		private FlexibleGameObject _Parent;

		[SerializeField]
		private FlexibleGameObject _InitTransform;

		void SerializeVer1()
		{
			if (_OldParent != null)
			{
				_Parent = (FlexibleGameObject)_OldParent.gameObject;
			}
			if (_OldInitTransform != null)
			{
				_InitTransform = (FlexibleGameObject)_OldInitTransform.gameObject;
            }
		}

		public void OnBeforeSerialize()
		{
			if (_SerializeVersion == 0)
			{
				SerializeVer1();
				_SerializeVersion = 1;
			}
		}

		public void OnAfterDeserialize()
		{
			if (_SerializeVersion == 0)
			{
				SerializeVer1();
			}
		}

		public override void OnStateBegin()
		{
			if( _Prefab != null )
			{
				GameObject obj = null;
                if (_InitTransform.value == null)
				{
					obj = Instantiate(_Prefab) as GameObject;
                }
				else
				{
					obj = Instantiate(_Prefab, _InitTransform.value.transform.position, _InitTransform.value.transform.rotation) as GameObject;
				}

				if (_Parent.value != null)
				{
					obj.transform.SetParent(_Parent.value.transform, _InitTransform.value != null);
				}

				if (_Parameter.parameter != null)
				{
					_Parameter.parameter.gameObjectValue = obj;
				}
			}
		}
	}
}
