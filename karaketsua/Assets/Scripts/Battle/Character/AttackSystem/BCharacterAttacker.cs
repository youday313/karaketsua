using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BattleScene
{
    public class BCharacterAttacker : BCharacterBaseAction
    {
        //ターゲット選択済み
        [System.NonSerialized]
        public bool isSetTarget = false;
        //ターゲット
        protected List<Character> attackTarget = new List<Character>();


        public override void Enable()
        {
            base.Enable();
            OnActiveCharacter();

        }
        protected void OnActiveCharacter()
        {
            isSetTarget = false;
            attackTarget = new List<Character>();
        }
        public override void Disable()
        {

            BattleStage.Instance.ResetAllTileColor();
            base.Disable();
        }

        //タイル上のキャラが自身にとっての敵キャラなら取得
        protected Character GetOpponentCharacterOnTile(IntVect2D toPos)
        {
            var chara = CharacterManager.Instance.GetCharacterOnTile(toPos);
            if (chara == null) return null;
            if (chara.isEnemy != this.character.isEnemy) return chara;
            return null;
        }
        protected Character GetOpponentCharacterFromTouch(Vector2 touchPosition)
        {
            var chara = CharacterManager.Instance.GetCharacterOnTile(touchPosition);
            if (chara == null) return null;
            if (chara.isEnemy != this.character.isEnemy) return chara;
            return null;

        }
    }
}
