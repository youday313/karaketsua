using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("SendMessageGameObject")]
	[AddBehaviourMenu("GameObject/SendMessageGameObject")]
	[BuiltInBehaviour]
	public class SendMessageGameObject : StateBehaviour
	{
		public enum Type
		{
			None,
			Int,
			Float,
			Bool,
			String,
		}
		[SerializeField] private GameObject _Target;
		[SerializeField] private string _MethodName;
		[SerializeField] private Type _Type;
		[SerializeField] private int _IntValue;
		[SerializeField] private float _FloatValue;
		[SerializeField] private bool _BoolValue;
		[SerializeField] private string _StringValue;

		public override void OnStateBegin()
		{
			if( _Target != null )
			{
				switch (_Type)
				{
					case Type.None:
						_Target.SendMessage(_MethodName, SendMessageOptions.DontRequireReceiver);
						break;
					case Type.Int:
						_Target.SendMessage(_MethodName, _IntValue,SendMessageOptions.DontRequireReceiver);
						break;
					case Type.Float:
						_Target.SendMessage(_MethodName, _FloatValue, SendMessageOptions.DontRequireReceiver);
						break;
					case Type.Bool:
						_Target.SendMessage(_MethodName, _BoolValue, SendMessageOptions.DontRequireReceiver);
						break;
					case Type.String:
						_Target.SendMessage(_MethodName, _StringValue, SendMessageOptions.DontRequireReceiver);
						break;
				}
			}
		}
	}
}