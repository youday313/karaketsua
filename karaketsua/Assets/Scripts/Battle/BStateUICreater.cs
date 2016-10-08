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
        public BCharacterStateUI Create(BCharacterBase character)
        {
            var stateUi = Instantiate(stateUiPrefab) as BCharacterStateUI;
            if(character.IsEnemy) {
                stateUi.transform.SetParent(enemyParent, worldPositionStays: false);
            }
            else {
                stateUi.transform.SetParent(playerParent, worldPositionStays: false);
            }
            stateUi.gameObject.SetActive(true);
            stateUi.Initialize(character);
            return stateUi;
        }
    }
}