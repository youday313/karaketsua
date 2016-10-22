using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using BattleScene;

namespace BattleScene
{
    public class BCharacterLife : MonoBehaviour
    {
        private CharacterMasterParameter characterParameter;
        [SerializeField]
        private BCharacterAnimator animator;
        [SerializeField]
        private BCharacterBase character;

        public void Initialize(CharacterMasterParameter param)
        {
            characterParameter = param;
        }

        //攻撃力と他のステータスを受け取り、防御力を適用しダメージ算出
        public void Damage(int attackPower, float rate)
        {
            //ここを変えるとダメージが変わる
            //計算式(ATK-1/2DEF)*rate
            var deffence = character.characterParameter.deffence;
            var damage = Mathf.Max(0, (int)((attackPower - (1 / 2 * deffence)) * rate));
            characterParameter.hp -= damage;
            createDamageText(damage);

            character.StatusUpdate();
            animator.SetDamage();
        }
        public void CheckDestroy()
        {
            if (characterParameter.hp <= 0)
            {
                animator.SetDeath();
                character.DeathMyself();
            }
        }
        private void createDamageText(float damage)
        {
            //ダメージ表示
            var popupPosition = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
            var damageText = Instantiate(Resources.Load<Text>("DamageText"), Camera.main.WorldToScreenPoint(popupPosition), Quaternion.identity) as Text;
            damageText.text = damage.ToString();
            damageText.transform.SetParent(GameObject.FindGameObjectWithTag("EffectCanvas").transform);
        }
    }
}
