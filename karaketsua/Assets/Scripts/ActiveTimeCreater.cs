using UnityEngine;
using System.Collections;

public class ActiveTimeCreater : Singleton<ActiveTimeCreater> {


    public ActiveTime activeTimePrefab;

    public ActiveTime CreateActiveTime(Character chara)
    {
        var newActiveTime = Instantiate(activeTimePrefab) as ActiveTime;
        newActiveTime.Init(chara);
        //waitTimes.Add(wTime);
        //位置補正
        if (chara.isEnemy == true)
        {
            SetEnemyPosition(newActiveTime);
        }
        newActiveTime.transform.SetParent(transform, false);

        
        return newActiveTime;
    }
    void SetEnemyPosition(ActiveTime aTime)
    {
        var rectTransform = aTime.GetComponent<RectTransform>();
        rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y - 50f, rectTransform.position.z);
    }
}
