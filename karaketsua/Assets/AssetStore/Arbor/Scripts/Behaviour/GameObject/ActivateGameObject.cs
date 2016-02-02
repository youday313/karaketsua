using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/ActivateGameObject")]
	[BuiltInBehaviour]
	public class ActivateGameObject : StateBehaviour, ISerializationCallbackReceiver
	{
		[FormerlySerializedAs("_Target")]
		[SerializeField] private GameObject _OldTarget;

		[SerializeField]
		private bool _BeginActive;
		[SerializeField]
		private bool _EndActive;

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
				target.SetActive( _BeginActive );
			}
		}

		public override void OnStateEnd()
		{
			if(target != null )
			{
				target.SetActive( _EndActive );
			}
		}
	}
}
