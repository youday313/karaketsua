using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BattleScene;
namespace BattleScene
{

    public class BCharacterStateUI: MonoBehaviour
    {
        [SerializeField]
        private Image faceImage;
        //public Text name;
        [SerializeField]
        private Slider hpbar;
        [SerializeField]
        private Slider skillBar;

        //BCharacterBase character;
        private float holdTime;
        [SerializeField]
        private float displayEnableTime = 1f;

        public void Initialize(BCharacterBase character)
        {
            // イベント登録
            character.OnDeathE += Delete;
            character.OnStatusUpdateE += UpdateUI;

            faceImage.sprite = Resources.Load<Sprite>("Character/StatusUIIcon/Status" + character.characterParameter.charaName);
            hpbar.maxValue = character.characterParameter.hp;
            skillBar.maxValue = character.characterParameter.skillPoint;
            UpdateUI(character);
        }

        public void Delete(BCharacterBase chara)
        {
            Destroy(this.gameObject);
        }

        public void UpdateUI(BCharacterBase chara)
        {
            var character = chara;
            hpbar.value = character.characterParameter.hp;
            skillBar.value = character.characterParameter.skillPoint;
        }

        public void Update()
        {
            if(holdTime != 0) {
                OnHold();
            }

        }

        //詳細表示用
        //ボタンが押されている
        public void OnPushDown()
        {
            holdTime = 0.01f;
        }

        public void OnHold()
        {
            holdTime += Time.deltaTime;
            if(holdTime > displayEnableTime) {

            }
        }

        public void OnPushUp()
        {
            holdTime = 0;
        }
    }
}