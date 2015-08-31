using UnityEngine;
using System.Collections;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// NavMeshAgentをラップしたAI用移動コンポーネント
	/// </summary>
#else
	/// <summary>
	/// AI for the movement component that wraps the NavMeshAgent
	/// </summary>
#endif
	[AddComponentMenu("Arbor/AgentController")]
	public class AgentController : MonoBehaviour
	{
		[SerializeField] private NavMeshAgent _Agent;
		[SerializeField] private AnimatorParameterReference _SpeedParameter;

		private Vector3 _StartPosition;

		// Use this for initialization
		void Start()
		{
			if (_Agent == null)
			{
				_Agent = GetComponent<NavMeshAgent>();
			}
			
			_StartPosition = transform.position;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始位置から指定半径内をうろつく
		/// </summary>
		/// <param name="speed">移動速度</param>
		/// <param name="radius">開始位置からの半径</param>
#else
		/// <summary>
		/// Prowl the within a specified radius from the start position
		/// </summary>
		/// <param name="speed">Movement speed</param>
		/// <param name="radius">Radius from the starting position</param>
#endif
		public void Patrol(float speed, float radius)
		{
			float angle = Random.Range(-180.0f, 180.0f);

			Vector3 axis = Vector3.up;

			Quaternion rotate = Quaternion.AngleAxis(angle, axis);

			Vector3 dir = rotate * Vector3.forward * Random.Range(0.0f, radius);

			_Agent.speed = speed;
			_Agent.stoppingDistance = 0.0f;
			_Agent.SetDestination(_StartPosition + dir);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したTransformの位置へ近づく
		/// </summary>
		/// <param name="speed">移動速度</param>
		/// <param name="stoppingDistance">停止距離</param>
		/// <param name="target">目標地点</param>
#else
		/// <summary>
		/// Approach to the position of the specified Transform
		/// </summary>
		/// <param name="speed">Movement speed</param>
		/// <param name="stoppingDistance">Stopping distance</param>
		/// <param name="target">Objective point</param>
#endif
		public void Follow(float speed, float stoppingDistance, Transform target)
		{
			_Agent.speed = speed;
			_Agent.stoppingDistance = stoppingDistance;
			_Agent.SetDestination(target.position);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したTransformから遠ざかる
		/// </summary>
		/// <param name="speed">移動速度</param>
		/// <param name="distance">遠ざかる距離</param>
		/// <param name="target">対象</param>
#else
		/// <summary>
		/// Away from the specified Transform
		/// </summary>
		/// <param name="speed">Movement speed</param>
		/// <param name="distance">Distance away</param>
		/// <param name="target">Target</param>
#endif
		public void Escape(float speed, float distance, Transform target)
		{
			Vector3 dir = transform.position - target.position;

			if (dir.magnitude >= distance)
			{
				return;
			}

			Vector3 pos = dir.normalized * distance + target.position;

			_Agent.speed = speed;
			_Agent.stoppingDistance = 0.0f;
			if (!_Agent.SetDestination(pos))
			{
				pos = -dir.normalized * distance + transform.position;

				_Agent.SetDestination(pos);
			}
		}

		void Update()
		{
			if (_SpeedParameter.animator != null)
			{
				_SpeedParameter.animator.SetFloat(_SpeedParameter.name, _Agent.velocity.magnitude);
			}
		}
	}
}