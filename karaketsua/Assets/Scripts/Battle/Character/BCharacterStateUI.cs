using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BattleScene;
namespace BattleScene
{

    public class BCharacterStateUI : MonoBehaviour
    {

        public Image faceImage;
        //public Text name;
        public Slider HPbar;
        public Slider skillBar;

        BCharacter character;
        float holdTime;
        public float displayEnableTime=1f;

        public void Init(BCharacter _character)
        {
            character = _character;
            transform.SetParent(GameObject.FindGameObjectWithTag("CharacterStateUIParent").transform, false);
            faceImage = Resources.Load<Image>("Icon/" + _character.characterParameter.charaName);
            HPbar.maxValue = _character.characterParameter.HP;
            skillBar.maxValue = _character.characterParameter.skillPoint;
            UpdateUI();

        }
        public void UpdateUI()
        {
            HPbar.value = character.characterParameter.HP;
            skillBar.value = character.characterParameter.skillPoint;
        }
        public void Update()
        {
            if (holdTime != 0)
            {
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
            if (holdTime > displayEnableTime)
            {

            }
        }

        public void OnPushUp()
        {
            holdTime = 0;
        }


    }
}