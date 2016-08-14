using UnityEngine;
using System.Collections;

namespace BattleScene
{

    public class BStateUICreater: SingletonMonoBehaviour<BStateUICreater>
    {


        [SerializeField]
        private BCharacterStateUI stateUiPrefab;

        [SerializeField]
        private Transform playerParent;
        [SerializeField]
        private Transform enemyParent;


        // アクティブタイムの生成
        public  BCharacterStateUI Initialize(BCharacterBase character)
        {
            var stateUi = Instantiate(stateUiPrefab) as BCharacterStateUI;
            if(character.isEnemy) {
                stateUi.transform.SetParent(enemyParent);
            }
            else {
                stateUi.transform.SetParent(playerParent);
            }
            stateUi.gameObject.SetActive(true);
            stateUi.Initialize(character);
            return stateUi;
        }

    }
}