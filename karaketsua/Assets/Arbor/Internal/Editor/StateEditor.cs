using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Arbor;
namespace ArborEditor
{
	internal sealed class StateEditor
	{
		private State _State;
		public State state
		{
			get
			{
				return _State;
			}
		}

		public StateEditor(State state)
		{
			_State = state;
		}

		private Dictionary<StateBehaviour, Editor> _BehaviourEditors = new Dictionary<StateBehaviour, Editor>();

		public Editor GetBehaviourEditor(StateBehaviour behaviour)
		{
			Editor editor = null;
			if (!_BehaviourEditors.TryGetValue(behaviour, out editor))
			{
				editor = Editor.CreateEditor(behaviour);

				_BehaviourEditors.Add(behaviour, editor);
			}

			return editor;
		}

		public void FinalizeBehaviourEditor()
		{
			if (_BehaviourEditors == null)
			{
				return;
			}
			foreach (Editor editor in _BehaviourEditors.Values)
			{
				Object.DestroyImmediate(editor);
			}
			_BehaviourEditors.Clear();
		}
	}
}
