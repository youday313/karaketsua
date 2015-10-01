using UnityEngine;
using System.Collections;

namespace MBS {
	/// <summary>
	/// The base component for the various classes in this kit. Contains the bulk of the work of the derived classes
	/// </summary>
	public class sfxBaseAction : MonoBehaviour {

		public enum 
			/// <summary>
			/// Should this action continue until the object is destroyed or play and wait or reverse when it is done?
			/// </summary>
		eSfxPlayMode { KeepGoing, UnoDirectional, BiDirectional}

		protected enum 
		eSfxStates { Waiting, Starting, Delaying, Reversing, Done}

		protected mbsStateMachine<eSfxStates>
			state;

		protected Vector3
			value_current,
			value_this_update;

		/// <summary>
		/// Select how this animation will behav. Play forever, play once or play and reverse
		/// </summary>
		public eSfxPlayMode
			play_mode;

		public float
			/// <summary>
			/// How long before this effect starts?
			/// </summary>
			start_delay,			
			/// <summary>
			/// How long before the effect reverses
			/// </summary>
			pause,				
			/// <summary>
			/// How long to take to reach the target_state
			/// </summary>
			time_up,
			/// <summary>
			/// How long to take to reach original state
			/// </summary>
			time_down,
			/// <summary>
			/// Accept this distance from destination as "destination reached"
			/// </summary>
			close_enough = 0.01f;	

		public Vector3
			/// <summary>
			/// The starting value of this animation
			/// </summary>
			value_start,
			/// <summary>
			/// The target value for this animation
			/// </summary>
			value_end,

			/// <summary>
			/// If play_mode is keep going, how fast should the animation play?
			/// </summary>
			speed_up;

		public System.Action
			onInit,
			onWaitingStart,
			onStarting,
			onDelayStarting,
			onDelayDone,
			onDone;

		float
			timer;

		Vector3
			dampvalue;

		// Use this for initialization
		void Start () {

			state = new mbsStateMachine<eSfxStates>();
			state.AddState(eSfxStates.Waiting	, doWaiting);
			state.AddState(eSfxStates.Starting	, doStarting);
			state.AddState(eSfxStates.Done		, doDone);

			if (play_mode == eSfxPlayMode.BiDirectional)
			{
				state.AddState(eSfxStates.Delaying	, doDelaying);
				state.AddState(eSfxStates.Reversing	, doReversing);
			}
			else
			{
				state.AddState(eSfxStates.Delaying);
				state.AddState(eSfxStates.Reversing);
			}

			if (null != onInit) onInit();

			value_current = value_start;
			dampvalue = Vector3.zero;

			timer = Time.time + start_delay;
			if (null != onWaitingStart) onWaitingStart();
			Setup();
		}

		void Update()
		{
			state.PerformAction();
			OnUpdate ();
		}

		public void PrimeTimerForPause()
		{
			timer = Time.time + pause;
		}

		virtual protected void doWaiting()
		{
			if (Time.time >= timer)
			{
				state.SetState(eSfxStates.Starting);
				if (null != onStarting) onStarting();
			}
		}

		virtual protected void doStarting()
		{
			if (play_mode == eSfxPlayMode.KeepGoing)
			{
				value_this_update = speed_up * Time.deltaTime;
				value_current += value_this_update;
			}
			else
			{
				value_current = Vector3.SmoothDamp(value_current, value_end, ref dampvalue, time_up);

				if (Vector3.Distance(value_current, value_end) < close_enough)
				{
					if (play_mode == eSfxPlayMode.BiDirectional)
					{
						state.SetState(eSfxStates.Delaying);
						timer = Time.time + pause;
						if (null != onDelayStarting)
							onDelayStarting();
					}
					else 
					{
						state.SetState(eSfxStates.Done);
						if (null != onDone)
							onDone();
					}
				}
			}
		}

		virtual protected void doDelaying()
		{
			if (Time.time >= timer)
			{
				dampvalue = Vector3.zero;
				state.SetState(eSfxStates.Reversing);
				if (null != onDelayDone) onDelayDone();
			}
		}

		virtual protected void doReversing()
		{
			value_current = Vector3.SmoothDamp(value_current, value_start, ref dampvalue, time_down);
			if (Vector3.Distance(value_current, value_end) < close_enough)
			{
				state.SetState(eSfxStates.Done);
				if (null != onDone)
						onDone();
			}
		}

		virtual protected void doDone()
		{
			enabled = false;
		}

		virtual public void Setup(){}
		virtual public void OnUpdate(){}
	}
}
