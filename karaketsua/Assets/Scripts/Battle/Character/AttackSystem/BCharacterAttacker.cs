using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BattleScene
{
    public enum AttackDistance { 近, 中, 遠 }
    //属性
    public enum ElementKind { なし, 火, 水, 地, 風, 雷 }

    public class BCharacterAttacker : BCharacterBaseAction
    {
        //ターゲット選択済み
        [System.NonSerialized]
        protected bool isSetTarget = false;
        public bool IsSetTarget
        {
            get { return isSetTarget; }
            private set
            {
                isSetTarget = value;
            }
        }

        //ターゲット
        protected List<BCharacterPlayer> attackTarget = new List<BCharacterPlayer>();


        public override void Enable()
        {
            base.Enable();
            OnActiveCharacter();

        }
        protected void OnActiveCharacter()
        {
            isSetTarget = false;
            attackTarget = new List<BCharacterPlayer>();
        }
        public override void Disable()
        {
            BBattleStage.Instance.ResetAllTileColor();
            isSetTarget = false;
            attackTarget = new List<BCharacterPlayer>();
            base.Disable();
        }

        //タイル上のキャラが自身にとっての敵キャラなら取得
        protected BCharacterPlayer GetOpponentCharacterOnTile(IntVect2D toPos)
        {
            var chara = CharacterManager.Instance.GetCharacterOnTile(toPos);
            if (chara == null) return null;
            if (chara.isEnemy != this.character.isEnemy) return chara;
            return null;
        }
        protected BCharacterPlayer GetOpponentCharacterFromTouch(Vector2 touchPosition)
        {
            var chara = CharacterManager.Instance.GetCharacterOnTile(touchPosition);
            if (chara == null) return null;
            if (chara.isEnemy != this.character.isEnemy) return chara;
            return null;

        }
    }
}
