using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using BattleScene;

namespace BattleScene
{

    public class UIBottomWaza : UIBottomBase
    {
        Button button;
        Text wazaName;
        public UIBottomCommandParent commandParent;
        public int attackNumber;
        // Use this for initialization
        void Awake(){
            button=GetComponent<Button>();
            wazaName = GetComponentInChildren<Text>();
        }

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
            if (chara == null) return;

            var param=chara.characterParameter.singleAttackParameters;
            //技がある
            if(param.Count>attackNumber){
                var waza = param[attackNumber];
                wazaName.text = waza.wazaName;
                button.interactable = true;
            }
        }
        public void OnClick()
        {
            //キャラクターに攻撃選択通知
            var chara = BCharacterManager.Instance.ActivePlayer;
            if (chara == null) return;
            chara.SelectSingleAttack(attackNumber);

            //カメラが上から
            BCameraChange.Instance.ActiveUpMode();

            //UI
            UIBottomCommandParent.UICommandState = EUICommandState.ExecuteAttack;
            UIBottomAllParent.Instance.UpdateUI();
        }
    }
}