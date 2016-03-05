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

        //BCharacterBase character;
        float holdTime;
        public float displayEnableTime=1f;

        public void Init(BCharacterBase _character)
        {
            var character = _character;
            var objectType= character.isEnemy==false?"Player":"Enemy";
            transform.SetParent(GameObject.FindGameObjectWithTag("CharacterStateUIParent").transform.FindChild(objectType).transform, false);
            character.OnDeathE += Delete;
            character.OnStatusUpdateE += UpdateUI;

            faceImage.sprite = Resources.Load<Sprite>("StatusUIIcon/Status" + _character.characterParameter.charaName);
            Debug.Log(_character.characterParameter.charaName);
            HPbar.maxValue = _character.characterParameter.HP;
            skillBar.maxValue = _character.characterParameter.skillPoint;
            UpdateUI(character);

        }
        public void Delete(BCharacterBase chara)
        {


            Destroy(this.gameObject);
        }

        public void UpdateUI(BCharacterBase chara)
        {
            var character = chara;
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