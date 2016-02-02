using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/DestroyGameObject")]
	[BuiltInBehaviour]
    public class DestroyGameObject : StateBehaviour, ISerializationCallbackReceiver
	{
		[FormerlySerializedAs("_Target")]
		[SerializeField] private GameObject _OldTarget;

		[SerializeField] private int _SerializeVersion;
		[SerializeField] private FlexibleGameObject _Target;

		public GameObject target
		{
			get
			{
				return _Target.value;
			}
		}

		void SerializeVer1()
		{
			_Target = (FlexibleGameObject)_OldTarget;
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
			if(target != null )
			{
				GameObject.Destroy (target);
			}
		}
	}
}
