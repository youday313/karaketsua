using UnityEngine;
using System.Collections;

public class ActiveTimeCreater : Singleton<ActiveTimeCreater> {


    public ActiveTime activeTimePrefab;

    public ActiveTime CreateActiveTime(Character chara)
    {
        var newActiveTime = Instantiate(activeTimePrefab) as ActiveTime;
        newActiveTime.Init(chara);
        //waitTimes.Add(wTime);
        newActiveTime.transform.SetParent(transform, false);
        return newActiveTime;
    }
}
