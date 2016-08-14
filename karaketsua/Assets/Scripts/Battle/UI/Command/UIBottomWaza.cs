using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using BattleScene;

namespace BattleScene
{

    public class UIBottomWaza: UIBottomBase
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private Text wazaName;
        [SerializeField]
        private int attackNumber;

        // Update is called once per frame
        void Update()
        {

        }
        public override void UpdateUI()
        {

            wazaName.text = "なし";
            button.interactable = false;

            //テキストの変更
            var chara = BCharacterManager.Instance.ActiveCharacter;
            if(chara == null) return;

            var param = chara.characterParameter.singleAttackParameters;
            //技がある
            if(param.Count > attackNumber) {
                var waza = param[attackNumber];
                wazaName.text = waza.wazaName;
                button.interactable = true;
            }
        }
        public void OnClick()
        {
            //キャラクターに攻撃選択通知
            var chara = BCharacterManager.Instance.ActivePlayer;
            if(chara == null) return;
            chara.SelectSingleAttack(attackNumber);

            //カメラが上から
            BCameraManager.Instance.ActiveUpMode();

            //UI
            UIBottomCommandParent.UICommandState = EUICommandState.ExecuteAttack;
            UIBottomAllManager.Instance.UpdateUI();
        }
    }
}