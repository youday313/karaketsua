using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{

    public class BCharacterManager : Singleton<BCharacterManager>
    {


        BCharacterBase activeCharacter;
        List<BCharacterBase> characters=new List<BCharacterBase>();
        // Use this for initialization

        public void Start()
        {

            //味方セット
            foreach (var chara in BStageData.Instance.playerCharacters)
            {
                var cha = Instantiate(chara.prefab) as BCharacterPlayer;
                cha.Init(new IntVect2D((int)chara.position.x, (int)chara.position.y));
                characters.Add(cha);
            }
            //敵セット
            foreach (var chara in BStageData.Instance.enemyCharacters)
            {
                var cha = Instantiate(chara.prefab) as BCharacterEnemy;
                cha.Init(new IntVect2D((int)chara.position.x, (int)chara.position.y));
                characters.Add(cha);
            }

            //共通初期化
            foreach (var chara in characters)
            {
                chara.OnActiveE += SetActiveCharacter;
                chara.OnEndActiveE += ResetActiveCharacter;
                chara.transform.SetParent(transform);
            }
            activeCharacter = null;
        }


        public BCharacterBase GetActiveCharacter()
        {
            return activeCharacter;
        }
        public void SetActiveCharacter(BCharacterBase chara)
        {
            activeCharacter = chara;
        }
        public void ResetActiveCharacter(BCharacterBase chara)
        {
            activeCharacter = null;
        }




        #region::Utility

        public void Add(BCharacterBase chara)
        {
            characters.Add(chara);
        }

        public void Remove(BCharacterBase chara)
        {
            characters.Remove(chara);
        }


        //タイル上のキャラを取得
        //いない時はnull
        public BCharacterBase GetCharacterFromVect2D(IntVect2D toPos)
        {
            return characters.
            Where(t => IntVect2D.IsEqual(toPos, t.positionArray)).
            FirstOrDefault();
        }

        //スクリーン座標からキャラクター取得
        public BCharacterBase GetCharacterOnTile(Vector2 screenPos)
        {
            //タイルをスクリーン座標から取得
            var targetPosition = BBattleStage.Instance.GetTilePositionFromScreenPosition(screenPos);
            if (targetPosition == null) return null;
            return GetCharacterFromVect2D(targetPosition);

        }
        //Vect2Dからキャラクター取得
        public BCharacterBase GetCharacterOnTile(IntVect2D vect)
        {
            //タイル以外
            if (vect == null) return null;
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
        public BCharacterBase GetOpponentCharacterOnTileFormVect2D(IntVect2D toPos,bool isEnemy)
        {
            var chara = GetCharacterOnTile(toPos);
            if (chara == null) return null;
            if (chara.isEnemy != isEnemy) return chara;
            return null;
        }
        public BCharacterBase GetOpponentCharacterOnTileFromTouch(Vector2 touchPosition,bool isEnemy)
        {
            var chara = GetCharacterOnTile(touchPosition);
            if (chara == null) return null;
            if (chara.isEnemy != isEnemy) return chara;
            return null;
        }

        #endregion::Utillity





    }
}