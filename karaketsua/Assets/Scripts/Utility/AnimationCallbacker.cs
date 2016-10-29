using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Animation))]
public class AnimationCallbacker : MonoBehaviour {

    public Action OnPlay;
    public Action Onfinish;
    public Action[] OnTrigger;

    [SerializeField] private List<GameObject> disableObjects;

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

    public void DisableObjects()
    {
        disableObjects.ForEach(o => o.gameObject.SetActive(false));
    }
}
