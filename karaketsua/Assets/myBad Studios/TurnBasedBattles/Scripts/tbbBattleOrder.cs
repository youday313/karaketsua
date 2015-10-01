using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MBS {
	/// <summary>
	/// This component drives the entire battle system by keeping track of which character should attack in what order
	/// as well as providing an interface through which to modify the target selection and attack selection on the
	/// appropriate character.
	/// 
	/// It is also in charge of scheduling attacks and enqueuing attacks that are simply not ready yet.
	/// 
	/// Furthermore, it also runs the entire attack system from starting to ending phase, triggers the prefabs to spawn, 
	/// plays the music, launches the cinematic cameras...
	/// </summary>
	public class tbbBattleOrder : MonoBehaviour{
		
		
		/// <summary>
		/// Holds a reference to the character this BattleOrder will configure
		/// </summary>
		public tbbPlayerInfo	character;
		/// <summary>
		/// Holds a reference to the target the player has chosen to attack
		/// </summary>
		public tbbPlayerInfo	target;
		
		/// <summary>
		/// What action has the player decided to take? Attack? Magic? Item? Flee? Other? 
		/// </summary>
		public tbbeActionType
			action;
		
		public tbbeAttackState	attack_state	{ get { return character.attack_state; } set { character.attack_state = value; } } 
		public tbbAttackPhase	AttackPhase		{ get { return attack_phase.CurrentState; } }
		public bool 			Configured		{ get { return configured_for_battle; } }
		
		[System.NonSerialized] public bool 				Inverted;
		[System.NonSerialized] public tbbAttackInfo		SelectedAttack;
		[System.NonSerialized] public tbbSpellInfo		SelectedSpell;
		
		public Action<tbbBattleOrder>
			onTargetSelected,
			onTurnCompleted,
			onAttackStarting,
			onReachedTarget,
			onAttackCompleted;
		
		/// <summary>
		/// Is this an AI or Human controlled character?
		/// </summary>
		public tbbeControlMethod 
			intelligence;
		
		bool
			configured_for_battle = false;
		
		Vector3 
			velocity = Vector3.zero,
			velocity2;
		
		GameObject
			origin,
			destination;
		
		float
			speed = 0.2f,	// speed to move towards target
			move_start;		// how far along the transition to target destination are we?
		
		mbsStateMachine<tbbAttackPhase>
			attack_phase;
		
		tbbCinematicCamera
			cine_camera;
		
		/// <summary>
		/// Points to the tbbAttackInfo base class so we can fetch the attack animation info for melee and magic attacks.
		/// Also, probe this to determine if all damage has already been dealt
		/// </summary>
		private tbbAttackInfo attack_comp { get { return action == tbbeActionType.Attack ? selected_attack : selected_spell; } }
		
		bool TargetIsDead { get { return target.attack_state == tbbeAttackState.Dead; } }
		
		tbbSpellInfo selected_spell;
		tbbAttackInfo selected_attack;
		
		/*Attack phases:
		 * Idle - When not busy actively attacking
		 * SelectTarget - For AI characters, use this mode to select a target
		 * PreOpeningCinematic - If this is to show a cinematic but another is showing one already, queue this...
		 * OpeningCinematic - Optional, for moves that require camera animation, start that here
		 * OpeningCinematicPlaying - Wait for it to complete
		 * MoveToAttackPosition - When not attacking in place, move to the target location
		 * Attack - Do all attack animation and SFX spawning now
		 * Attacking - Wait for animation and sfx to finish
		 * ReturnToStand - Attack is over, return to starting position
		*/
		
		/// <summary>
		/// Construct the specified character by building it's target and origin movment markers, 
		/// setting up it's state machine and default event listeners. 
		/// </summary>
		/// <param name="character">Character.</param>
		/// <param name="enemy">Enemy.</param>
		/// <param name="action_type">Action_type.</param>
		public void Construct(tbbPlayerInfo character, tbbPlayerInfo enemy, tbbeActionType action_type)
		{
			//create position markers...
			origin = new GameObject("origin");
			destination = new GameObject("destination");
			
			Inverted = true;
			
			//assign character and target...
			AssignCharacter ( character );
			AssignTarget( enemy );
			
			//position position markers at character origin...
			PositionMarkersAtCharacter();
			
			destination.transform.rotation = Quaternion.identity;
			origin.transform.rotation = Quaternion.identity;
			
			//indicate that the character is created but still need to be tld what to do...
			attack_state = tbbeAttackState.Config;
			
			//indicate what type of action this character will perform: melee, magic, item, flee, etc...
			action = action_type;
			
			attack_phase = new mbsStateMachine<tbbAttackPhase>();
			attack_phase.AddState(tbbAttackPhase.Idle );
			attack_phase.AddState(tbbAttackPhase.SelectTarget				, SelectTarget);
			attack_phase.AddState(tbbAttackPhase.PreOpeningCinematic		, PreOpeningCinematic);
			attack_phase.AddState(tbbAttackPhase.OpeningCinematic			, OpeningCinematic);
			attack_phase.AddState(tbbAttackPhase.OpeningCinematicPlaying	, OpeningCinematicPlaying);
			attack_phase.AddState(tbbAttackPhase.MoveToAttackPosition		, MoveToAttackPosition);
			attack_phase.AddState(tbbAttackPhase.Attack						, Attack);
			attack_phase.AddState(tbbAttackPhase.Attacking					, Attacking);
			attack_phase.AddState(tbbAttackPhase.ReturnToStand				, ReturnToStand);
			
			attack_phase.SetState(tbbAttackPhase.Idle);
			
			onTargetSelected	+= __ontargetselected;
			onAttackStarting	+= __onattackstarting;
			onReachedTarget		+= __onreachedtarget;
			onReachedTarget 	+= __initiateattack;
			onAttackCompleted	+= __onattackcompleted;
			onTurnCompleted		+= __onturncompleted;
		}
		
		/// <summary>
		/// Position the internal position markers at the character's position.
		/// Used when changing the character's home tile in the grid after initial spawning
		/// </summary>
		public void PositionMarkersAtCharacter()
		{
			origin.transform.position = character.transform.position;
			destination.transform.localPosition = character.transform.position;
		}
		
		//create a pointer the the character to move and set it's origin and destination markers...
		void AssignCharacter(tbbPlayerInfo character)
		{
			this.character = character;
			
			if (null != character)
			{
				origin.transform.parent = character.transform.parent;
				destination.transform.parent = character.transform.parent;
			}
		}
		
		/// <summary>
		/// Assigns the target.
		/// </summary>
		/// <param name="target">Target.</param>
		public void AssignTarget( tbbPlayerInfo target )
		{
			this.target = target;
		}
		
		/// <summary>
		/// Assigns the attack to perform during melee type attacks
		/// </summary>
		/// <param name="attack">Attack.</param>
		public void AssignAttack(tbbAttackInfo attack)
		{
			SelectedAttack = attack;
		}
		
		/// <summary>
		/// Assigns the spell to cast for magic type attacks
		/// </summary>
		/// <param name="spell">Spell.</param>
		public void AssignSpell(tbbSpellInfo spell )
		{
			SelectedSpell = spell;
		}
		
		/// <summary>
		/// The character chooses a random attack from it's available moves.
		/// Used by the AI system.
		/// </summary>
		/// <returns>The attack.</returns>
		public tbbAttackInfo RandomAttack()
		{
			return character.RandomAttack();
		}
		
		/// <summary>
		/// Sets the state of the character attack.
		/// </summary>
		/// <param name="state">State.</param>
		public void SetCharacterAttackState(tbbeAttackState state)
		{
			attack_state = state;
		}
		
		void Update()
		{
			attack_phase.PerformAction();
		}
		
		/// <summary>
		/// Set whether or not this character is ready to perform his attack
		/// </summary>
		/// <param name="configured">If set to <c>true</c> configured.</param>
		public void MarkAsConfigured(bool configured)
		{
			configured_for_battle = configured;
		}
		
		/// <summary>
		/// Begins the attack process starting from the attack state
		/// </summary>
		public void BeginAttack()
		{
			BeginAttack(tbbAttackPhase.SelectTarget, attack_state);
		}
		
		/// <summary>
		/// Begins the attack process starting from the attack state.
		/// If the selected target is already dead (i.e. killed by another character)
		/// this function will assign the first non-dead character for this turn's attack.
		/// If no target is left, it will forfeit it's turn. So will all remaining characters
		/// and thus eventually the round will end and the game will be won or lost
		/// </summary>
		/// <param name="phase">Phase.</param>
		/// <param name="state">State.</param>
		public void BeginAttack(tbbAttackPhase phase, tbbeAttackState state)
		{
			attack_state = state;
			if (attack_state != tbbeAttackState.Ready || !character.ready_for_attack)
				return;
			
			if (TargetIsDead)
			{
				target = null;
				int count = tbbBattleField.opponent.participants.Count;
				if ( count == 1 )
					target = tbbBattleField.opponent.participants[0];
				else if (count > 1)
					target = tbbBattleField.opponent.participants[UnityEngine.Random.Range(0, count)];
				
				if (null == target)
				{
					OnTurnCompleted(this);
					return;
				}
			}
			
			OnAttackStarting(this);
			attack_phase.SetState(phase);
		}
		
		void FixCharacterFacingDirection()
		{
			if (target.facing_away)
			{
				target.transform.Rotate(0,-180,0);
				target.facing_away = false;
			}
			character.facing_away = false;
		}
		
		void StartMovingToTargetDestination()
		{
			//first we need to spawn the attack prefab so we can call functions on it's instance
			switch(action)
			{
			case tbbeActionType.Attack:
				selected_attack = (tbbAttackInfo)Instantiate(SelectedAttack);
				break;
				
			case tbbeActionType.Magic:
				selected_spell = (tbbSpellInfo)Instantiate (SelectedSpell);
				break;
			}
			
			//determine wether or not to override the battlefield's default attack mode
			//also, if the character has no attack animation to play upon reaching the target
			//don't even bother sending him over just to return immediately...
			tbbeAttackMode e = (attack_comp.attack_animation_name == string.Empty) ? tbbeAttackMode.InPlace : attack_comp.AttackMode;
			
			switch(e)
			{
			case tbbeAttackMode.InPlace:
				OnReachedTarget(this);
				return;
				
			case tbbeAttackMode.NeutralArea:
				SetFieldCenterAsNextTarget();
				break;
				
			case tbbeAttackMode.AtTarget:
				SetCharacterAsNextTarget();
				break;
			}
			attack_phase.SetState(tbbAttackPhase.MoveToAttackPosition);
		}
		
		/// <summary>
		/// This function sets the character's destination to it's base tile
		/// </summary>
		public void SetBaseTileAsNextTarget()
		{
			SelectTargetDestination(target: origin.transform, 
			                        lookat: tbbBattleField.active_faction.battlefield_side == tbbeBattlefieldSide.Lower ? 
			                        tbbBattleField.Instance.grid.North.localPosition:
			                        tbbBattleField.Instance.grid.South.localPosition,
			                        anim:	character.run_back_animation_name );
		}
		
		/// <summary>
		/// This function sets the character's destination to the center of thebattlefield
		/// </summary>
		public void SetFieldCenterAsNextTarget()
		{
			SelectTargetDestination(target: tbbBattleField.Instance.grid.Middle, 
			                        lookat: tbbBattleField.active_faction.battlefield_side == tbbeBattlefieldSide.Lower ?
			                        tbbBattleField.Instance.grid.North.localPosition:
			                        tbbBattleField.Instance.grid.South.localPosition,
			                        anim: character.run_animation_name);
		}
		
		/// <summary>
		/// This function sets the character's destination to an offset from a target object
		/// </summary>
		public void SetCharacterAsNextTarget()
		{
			SelectTargetDestination(target: target.transform,
			                        lookat: Vector3.zero,
			                        offset: new Vector3(0, 0, target.facing_away ? -1.5f : 1.5f),
			                        anim:	character.run_animation_name);
		}
		
		void SelectTargetDestination(Transform target, Vector3 lookat, string anim) { SelectTargetDestination(target, lookat, Vector3.zero, anim);}
		void SelectTargetDestination(Transform target, Vector3 lookat, Vector3 offset, string anim)
		{
			destination.transform.position = target.TransformPoint( offset );
			destination.transform.LookAt( target.TransformPoint(new Vector3(0,0,lookat.z)) );
			velocity = velocity2 = Vector2.zero;
			move_start = Time.time;
			PlayAnimation(anim);
		}
		
		void PlayAnimation(string name)	{ PlayAnimation(name, false); }
		void PlayAnimation(string name, bool fade_to_idle)
		{
			if (name == string.Empty)
			{
				Debug.Log("trying to play an empty animation");
				return;
			}
			Animation anim = character.transform.GetComponentInChildren<Animation>();
			move_start = Time.time;
			if (anim && name != string.Empty)
			{
				anim.Play (name);
				if (fade_to_idle) anim.CrossFadeQueued(character.idle_animation_names[0],0.5f,QueueMode.CompleteOthers);
			}
			else
				character.controller.Play(name);
		}
		
		bool TransitionToTarget()
		{
			//prevent both dampening delays upon destination as well as visible snapping by keeping this value low...
			float temp = Vector3.Distance(character.transform.position, destination.transform.position );
			bool reached_destination = (temp < 0.075f);
			
			if (!reached_destination)
			{
				character.transform.position = Vector3.SmoothDamp(character.transform.position, destination.transform.position, ref velocity, speed);				
				character.transform.eulerAngles = Vector3.SmoothDamp(character.transform.eulerAngles, destination.transform.eulerAngles, ref velocity2, speed);
			} else
			{
				character.transform.position = destination.transform.position;
				character.transform.rotation = destination.transform.rotation;
			}
			
			return reached_destination;
		}
		
		/* ------ PUBLIC EVENT HANDLERS -------------------------------
		 * ------------------------------------------------------------ */
		#region public event handler definitions
		public void OnTurnCompleted(tbbBattleOrder entry)
		{
			if  (null != onTurnCompleted)
				onTurnCompleted(entry);
		}
		
		public void OnAttackCompleted(tbbBattleOrder entry)
		{
			if ( null != onAttackCompleted && null != entry)
				onAttackCompleted(entry);
		}
		
		
		public void OnReachedTarget(tbbBattleOrder entry)
		{
			if ( null != onReachedTarget && null != entry)
				onReachedTarget(entry);
		}
		
		
		public void OnAttackStarting(tbbBattleOrder entry)
		{
			if ( null != onAttackStarting && null != entry)
				onAttackStarting(entry);
		}
		
		
		public void OnTargetSelected(tbbBattleOrder entry)
		{
			if ( null != onTargetSelected && null != entry)
				onTargetSelected(entry);
		}
		/* ------ END PUBLIC EVENT HANDLERS ---------------------------
		 * ------------------------------------------------------------ */
		#endregion
		
		/* ------ DEFAULT EVENT HANDLERS -------------------------------
		 * ------------------------------------------------------------ */
		#region default event handlers
		void __ontargetselected(tbbBattleOrder entry)
		{
			switch (tbbBattleField.Instance.battle_type)
			{
			case tbbeBattleType.PerFaction:
				break;
				
				/*			case tbbeBattleType.Realtime:
				SetCharacterAttackState(tbbeAttackState.Ready);
				break;
*/
			case tbbeBattleType.Immediate:
				tbbBattleField.active_faction.SendCharacterIntoBattle(this);
				break;
			}
		}
		
		void __onattackstarting(tbbBattleOrder entry)
		{
			attack_phase.SetState(tbbAttackPhase.Attacking);
			attack_state = tbbeAttackState.Fighting;
		}
		
		void __onattackcompleted(tbbBattleOrder entry)
		{
			//determine wether or not the character moved away from his tile
			//and play the "run back" animation only if so
			tbbeAttackMode e = (attack_comp.attack_animation_name == string.Empty) ? tbbeAttackMode.InPlace : attack_comp.AttackMode;
			
			if (e == tbbeAttackMode.AtTarget || e == tbbeAttackMode.NeutralArea)
				SetBaseTileAsNextTarget();
			
			attack_phase.SetState(tbbAttackPhase.ReturnToStand);
		}
		
		void __onreachedtarget(tbbBattleOrder entry)
		{
			attack_phase.SetState(tbbAttackPhase.Attack);
		}
		
		void __onturncompleted(tbbBattleOrder entry)
		{
			PlayAnimation(character.idle_animation_names[0]);
			attack_state = tbbeAttackState.Done;
			attack_phase.SetState(tbbAttackPhase.Idle);
		}
		
		/// <summary>
		/// Tell the actual attack prefab who is who in this battle...
		/// </summary>
		/// <param name="bo">Bo.</param>
		void __initiateattack(tbbBattleOrder bo)
		{
			attack_comp.attacker = character;
			attack_comp.defender = target;
			attack_comp.onAllDamageDealt += __destroydamageobject;
			attack_comp.DealDamage(character.Level);
		}
		
		/// <summary>
		/// After the damage is dealt, destroy the attack prefab since it will no longer be needed
		/// </summary>
		void __destroydamageobject()
		{
			Destroy(attack_comp.gameObject);
		}
		
		/* ------ END DEFAULT EVENT HANDLERS ---------------------------
		 * ------------------------------------------------------------ */
		#endregion
		
		/// <summary>
		/// Selects the target.
		/// Overloaded function to compensate for Unity's lack of support for default values when using namespaces
		/// </summary>
		public void SelectTarget()
		{
			SelectTarget(null);
		}
		
		/// <summary>
		/// Selects the target.
		/// Overloaded function to compensate for Unity's lack of support for default values when using namespaces
		/// </summary>
		/// <param name="target">Target.</param>
		public void SelectTarget(tbbPlayerInfo target)
		{
			if (intelligence == tbbeControlMethod.AI)
				PerformAITargetSelect();
			else
			{
				if (null != target)
					AssignTarget(target);
			}
			MarkAsConfigured(true);
			
			OnTargetSelected(this);
		}
		
		/// <summary>
		/// Performs the AI's target select. This function is virtual so you are free to create your own target
		/// select routines to replace this rudamentary system of mine...
		/// </summary>
		virtual public void PerformAITargetSelect()
		{
			AssignTarget( SelectRandomOpponent() );
			if (null == target)
			{
				//this should never happen as this means the player has no characters left and then the game switched to the enemy's turn...
				//alternatively, it could mean the last attack killed the last opponent...
				OnTurnCompleted(this);
				return;
			}
		}
		
		/// <summary>
		/// Selects a random opponent from the opposing faction's participants
		/// </summary>
		/// <returns>The random opponent.</returns>
		tbbPlayerInfo SelectRandomOpponent()
		{
			List<tbbPlayerInfo> available = tbbBattleField.opponent.participants;
			
			if (available.Count == 0)
				return null;
			
			if (available.Count == 1)
				return available[0];
			else
				return available[ UnityEngine.Random.Range(0,available.Count) ];
		}
		
		
		
		/* ------ STATE MACHINE ACTIONS -------------------------------
		 * ------------------------------------------------------------ */
		#region state machine actions
		void PreOpeningCinematic()
		{
			attack_phase.SetState(tbbAttackPhase.OpeningCinematic);
			Debug.Log("Set mode to " + attack_phase.CurrentState);
		}
		
		void OpeningCinematic()
		{
			switch (action)
			{
			case tbbeActionType.Attack:
				StartMovingToTargetDestination();
				break;
				
			case tbbeActionType.Magic:
				if (null == SelectedSpell || null == tbbBattleField.Instance.cinema_cam || string.Empty == SelectedSpell.cinematic_animation_camera)
				{
					StartMovingToTargetDestination();
					return;
				}
				cine_camera = (tbbCinematicCamera)Instantiate(tbbBattleField.Instance.cinema_cam, character.transform.position, character.transform.rotation);
				cine_camera.transform.parent = character.transform;
				cine_camera.animation_name = SelectedSpell.cinematic_animation_camera;
				cine_camera.onCamDone += SwitchToPostCinematicMode;
				
				if (SelectedSpell.cinematic_animation_character != string.Empty)
					PlayAnimation(SelectedSpell.cinematic_animation_character, true);
				
				if (null != SelectedSpell.special_effect)
					StartCoroutine(SpawnInSeconds(SelectedSpell.magic_FX_spawn_delay,
					                              SelectedSpell.special_effect,
					                              SelectedSpell.magic_FX_origin == tbbeSpawnOrigin.Destination ? target.transform.position : character.transform.position,
					                              character.transform.rotation,
					                              new object[]{character, target, SelectedSpell}));					
				attack_phase.SetState(tbbAttackPhase.OpeningCinematicPlaying);
				break;
			}
		}
		
		IEnumerator SpawnInSeconds(float seconds, Transform obj, Vector3 position, Quaternion rotation, object[] extras)
		{
			yield return new WaitForSeconds(seconds);
			Transform o = (Transform)Instantiate(obj, position, rotation);
			o.gameObject.BroadcastMessage("OnReceivedParams", extras, SendMessageOptions.DontRequireReceiver);
		}
		
		void SwitchToPostCinematicMode()
		{
			StartMovingToTargetDestination();
		}
		
		void OpeningCinematicPlaying()
		{
		}
		
		void MoveToAttackPosition()
		{
			if ( TransitionToTarget() )
				OnReachedTarget(this);
		}
		
		void Attack()
		{
			if(null != attack_comp.attack_special_effect)
				StartCoroutine(SpawnInSeconds(attack_comp.attack_FX_spawn_delay,
				                              attack_comp.attack_special_effect,
				                              attack_comp.attack_FX_origin == tbbeSpawnOrigin.Destination ? target.transform.position : character.transform.position,
				                              character.transform.rotation,
				                              new object[]{character, target, attack_comp}));
			
			PlayAnimation(attack_comp.attack_animation_name);
			FixCharacterFacingDirection();
			
			if (action == tbbeActionType.Magic)
			{
				character.MP -= SelectedSpell.mp_required;
			}
			OnAttackStarting(this);
		}
		
		/// <summary>
		/// During the Attacking state, we basically just wait for the attack animations and damage to be dealt.
		/// As such, all we do is constantly scan to see if this is the case and when this is true, we call
		/// OnAttackCompleted to change to the next state
		/// </summary>
		void Attacking()
		{
			if (Time.time <= move_start + character.CurrentAnimationLength())
				return;
			
			if (null != attack_comp)
			{
				if (attack_comp.done_attacking)
					OnAttackCompleted(this);
			} else
				OnAttackCompleted(this);
		}
		
		/// <summary>
		/// During this state we are just waiting for the character to return to it's tile so we just scan for
		/// this and once the character is back at it's origin we call OnTurnCompleted to signal the faction
		/// that the next character can be sent in to battle...
		/// </summary>
		void ReturnToStand()
		{
			if (TransitionToTarget())
				OnTurnCompleted(this);
		}
		/* ------ STATE MACHINE ACTIONS -------------------------------
		 * ------------------------------------------------------------ */
		#endregion
		
	}
}
