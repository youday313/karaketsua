using UnityEngine;
using System.Collections;


using BattleScene;

namespace BattleScene
{
    public class BActiveTimeCreater : SingletonMonoBehaviour<BActiveTimeCreater>
    {

        public BActiveTime activeTimePrefab;
        public float enemyOffset;

        public BActiveTime CreateActiveTime()
        {
            var newActiveTime = Instantiate(activeTimePrefab) as BActiveTime;
            //newActiveTime.Init(chara);
            //waitTimes.Add(wTime);
            //位置補正
//			if (newActiveTime.character.isEnemy == true)
//            {
//                SetEnemyPosition(newActiveTime);
//            }
            newActiveTime.transform.SetParent(transform, false);


            return newActiveTime;
        }
        public void SetEnemyPosition(BActiveTime aTime)
        {
            var rectTransform = aTime.GetComponent<RectTransform>();

			rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y - enemyOffset, rectTransform.anchoredPosition3D.z);

        }
    }
}
