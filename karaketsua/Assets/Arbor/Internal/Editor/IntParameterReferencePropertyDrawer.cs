using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	[CustomPropertyDrawer(typeof(IntParameterReference))]
	public class IntParameterReferencePropertyDrawer : ParameterReferencePropertyDrawer
	{
		protected override bool CheckType(Parameter.Type type)
		{
			return type == Parameter.Type.Int;
		}
	}
}
