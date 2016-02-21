using UnityEngine;
using System.Collections;


using BattleScene;

namespace BattleScene
{
    public class BActiveTimeCreater : Singleton<BActiveTimeCreater>
    {

        public BActiveTime activeTimePrefab;
        public float enemyOffset;

        public BActiveTime CreateActiveTime(BCharacter chara)
        {
            var newActiveTime = Instantiate(activeTimePrefab) as BActiveTime;
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
        void SetEnemyPosition(BActiveTime aTime)
        {
            var rectTransform = aTime.GetComponent<RectTransform>();
            rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y - enemyOffset, rectTransform.position.z);
        }
    }
}
