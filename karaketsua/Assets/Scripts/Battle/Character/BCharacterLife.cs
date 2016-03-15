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

        //攻撃力と他のステータスを受け取り、防御力を適用しダメージ算出
        public void Damage(int attackPower,ElementKind element, float magnification)
        {

            var calcDamage = CalcDamage(attackPower, element, magnification);
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
        //計算式(ATK-1/2DEF)*magnification
        int CalcDamage(int power,ElementKind element, float magnification)
        {
            var deffence = element == ElementKind.なし ? character.characterParameter.deffence : character.characterParameter.elementDeffence;
            return Mathf.Max(0, (int)((power - (1 / 2 * deffence)) * magnification));
        }


    }
}
