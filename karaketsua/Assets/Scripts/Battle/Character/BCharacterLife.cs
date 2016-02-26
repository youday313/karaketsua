using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using BattleScene;

namespace BattleScene
{
    public class BCharacterLife : MonoBehaviour
    {
        CharacterParameter characterParameter;
        BCharacterAnimator animator;
        BCharacterBase character;
        void Start()
        {
            character = GetComponent<BCharacterBase>();
            animator = GetComponent<BCharacterAnimator>();

        }

        public void Init(CharacterParameter param)
        {
            characterParameter = param;
        }

        public void Damage(int enemyPower)
        {
            var calcDamage = CalcDamage(enemyPower);
            characterParameter.HP -= calcDamage;
            CreateDamageText(calcDamage);

            animator.SetDamage();
            //character.StateUI.UpdateUI();

        }
        public void CheckDestroy()
        {
            if (characterParameter.HP <= 0)
            {
                animator.SetDeath();
                character.DeathMyself();
            }
        }
        void CreateDamageText(float damage)
        {
            //ダメージ表示
            var popupPosition = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
            var damageText = Instantiate(Resources.Load<Text>("DamageText"), Camera.main.WorldToScreenPoint(popupPosition), Quaternion.identity) as Text;
            damageText.text = damage.ToString();
            damageText.transform.SetParent(GameObject.FindGameObjectWithTag("EffectCanvas").transform);

        }
        //ここを変えるとダメージが変わる
        //計算式
        int CalcDamage(int power)
        {
            //相手攻撃力 - 自分防御力 がダメージ量
            //相手攻撃力<自分防御力 だったらダメージ0
            return Mathf.Max(0, power - (1 / 2 * characterParameter.deffence));
        }


    }
}
