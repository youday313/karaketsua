using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Animation))]
public class AnimationCallbacker : MonoBehaviour {

    public Action OnPlay;
    public Action Onfinish;
    public Action[] OnTrigger;
    private int triggerCount;

    public void Play()
    {
        OnPlay();
    }

    public void Finish()
    {
        Onfinish();
    }

    public void Trigger()
    {
        OnTrigger[triggerCount]();
        triggerCount++;
    }

    public void ResetTrigger()
    {
        triggerCount = 0;
    }
}
