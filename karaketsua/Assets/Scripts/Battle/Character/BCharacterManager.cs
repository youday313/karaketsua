using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{

    public class BCharacterManager: SingletonMonoBehaviour<BCharacterManager>
    {
        public BCharacterBase ActiveCharacter { get; set; }
        public BCharacterPlayer ActivePlayer { get; set; }
        public bool IsExitEnemy { get { return characters.Any(c => c.IsEnemy); } }

        private List<BCharacterBase> characters = new List<BCharacterBase>();

        [SerializeField]
        private BCharacterCreater characterCreater;

        public void Initialze()
        {
            // バトルキャラクター作成
            characters = characterCreater.CreateBattleCharacters();
            ActiveCharacter = null;
        }

        public void SetActivePlayer(BCharacterPlayer chara)
        {
            ActivePlayer = chara;
            ActiveCharacter = chara;
        }
        public void SetActiveEnemy(BCharacterEnemy chara)
        {
            ActiveCharacter = chara;
        }
        public void ResetActiveCharacter()
        {
            ActiveCharacter = null;
            ActivePlayer = null;
            // 全員表示
            foreach(var chara in characters) {
                chara.SetActive(true);
            }
        }

        // バトルに関係するキャラクター以外をOff
        public void HideCharacter(List<BCharacterBase> shows)
        {
            var hideCharacters = characters.Except(shows);
            foreach(var chara in hideCharacters) {
                chara.SetActive(false);
            }
        }

        #region::Utility
        public void Remove(BCharacterBase chara)
        {
            characters.Remove(chara);
        }
            
        //タイル上のキャラを取得
        //いない時はnull
        public BCharacterBase GetCharacterFromVect2D(IntVect2D toPos)
        {
            return characters.
            Where(t => IntVect2D.IsEqual(toPos, t.PositionArray)).
            FirstOrDefault();
        }

        //スクリーン座標からキャラクター取得
        public BCharacterBase GetCharacterOnTile(Vector2 screenPos)
        {
            //タイルをスクリーン座標から取得
            var targetPosition = BBattleStage.Instance.GetTilePositionFromScreenPosition(screenPos);
            if(targetPosition == null) return null;
            return GetCharacterFromVect2D(targetPosition);

        }
        //Vect2Dからキャラクター取得
        public BCharacterBase GetCharacterOnTile(IntVect2D vect)
        {
            //タイル以外
            if(vect == null) return null;
            //ターゲットの検索
            var target = GetCharacterFromVect2D(vect);

            ////ターゲットが存在しないマスをタップ
            //if (target == null) return null;
            return target;
        }
        //キャラクターがタイル上にいるかを取得
        //タイルがないときもfalse
        public bool IsExistCharacterOnTile(Vector2 pos)
        {
            return GetCharacterOnTile(pos) != null;
        }
        public bool IsExistCharacterOnTile(IntVect2D pos)
        {
            return GetCharacterOnTile(pos) != null;
        }

        //タイル上のキャラが自身にとっての敵キャラなら取得
        public BCharacterBase GetOpponentCharacterOnTileFormVect2D(IntVect2D toPos, bool isEnemy)
        {
            var chara = GetCharacterOnTile(toPos);
            if(chara == null) return null;
            if(chara.IsEnemy != isEnemy) return chara;
            return null;
        }
        public BCharacterBase GetOpponentCharacterOnTileFromTouch(Vector2 touchPosition, bool isEnemy)
        {
            var chara = GetCharacterOnTile(touchPosition);
            if(chara == null) return null;
            if(chara.IsEnemy != isEnemy) return chara;
            return null;
        }
        public List<BCharacterBase> GetOpponentCharacters(bool isEnemy)
        {
            return characters.Where(x => x.IsEnemy != isEnemy).ToList();
        }

        #endregion::Utillity
    }
}