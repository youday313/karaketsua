using UnityEngine;
using System.Collections;


using BattleScene;

namespace BattleScene
{
    public class BActiveTimeCreater: SingletonMonoBehaviour<BActiveTimeCreater>
    {
        [SerializeField]
        private BActiveTime activeTimePrefab;

        [SerializeField]
        private float enemyOffset;

        // アクティブタイムの生成
        public BActiveTime Initialize(BCharacterBase character)
        {
            var newActiveTime = Instantiate(activeTimePrefab) as BActiveTime;
            newActiveTime.transform.SetParent(transform, false);
            newActiveTime.gameObject.SetActive(true);
            newActiveTime.initialize(character);
            if(character.isEnemy) {
                setEnemyPosition(newActiveTime);
            }
            return newActiveTime;
        }

        private void setEnemyPosition(BActiveTime aTime)
        {
            var rectTransform = aTime.GetComponent<RectTransform>();

            rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y - enemyOffset, rectTransform.anchoredPosition3D.z);

        }
    }
}
