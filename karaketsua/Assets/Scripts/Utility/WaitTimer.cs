using UnityEngine;
using System;
using System.Collections;

public class WaitTimer : MonoBehaviour 
{
    public static IEnumerator WaitFrame(Action callback, int frame = 1)
    {
        for(var i = 0; i < frame; i++) {
            yield return new WaitForEndOfFrame();
        }
        callback();
    }

    public static IEnumerator WaitSecond(Action callback, float time = 1f)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
}
