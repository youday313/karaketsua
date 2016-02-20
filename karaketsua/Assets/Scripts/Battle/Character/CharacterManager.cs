using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{

    public class CharacterManager : Singleton<CharacterManager>
    {


        [System.NonSerialized]
        public List<BCharacter> characters = new List<BCharacter>();
        BCharacter activeCharacter;
        // Use this for initialization
        void Start()
        {
            foreach (var chara in BStageData.Instance.playerCharacters)
            {
                var cha = Instantiate(chara.prefab) as BCharacter;
                cha.Init(new IntVect2D((int)chara.position.x, (int)chara.position.y), false);
                characters.Add(cha);
                cha.transform.SetParent(transform);
            }
            foreach (var chara in BStageData.Instance.enemyCharacters)
            {
                var cha = Instantiate(chara.prefab) as BCharacter;
                //向き変更
                cha.Init(new IntVect2D((int)chara.position.x, (int)chara.position.y), true);
                characters.Add(cha);
                cha.transform.SetParent(transform);
            }
            activeCharacter = null;
        }

        public void DestroyCharacter(BCharacter chara)
        {
            characters.Remove(chara);
            //Destroy(chara.gameObject);
            var count = characters.Where(t => t.isEnemy == true).Count();
            if (count == 0)
            {
                SceneManager.Instance.SetNextScene("ResultScene");
            }
        }


        public BCharacter GetActiveCharacter()
        {
            return activeCharacter;
        }
        public void SetNowActiveCharacter(BCharacter chara)
        {
            activeCharacter = chara;
        }

        #region::Utility
        //タイル上のキャラを取得
        //いない時はnull
        BCharacter GetCharacterFromVect2D(IntVect2D toPos)
        {
            return characters.
            Where(t => IntVect2D.IsEqual(toPos, t.positionArray)).
            FirstOrDefault();
        }

        //スクリーン座標からキャラクター取得
        public BCharacter GetCharacterOnTile(Vector2 screenPos)
        {
            //タイルをスクリーン座標から取得
            var targetPosition = BBattleStage.instance.GetTilePositionFromScreenPosition(screenPos);
            if (targetPosition == null) return null;
            return GetCharacterFromVect2D(targetPosition);

        }
        //Vect2Dからキャラクター取得
        public BCharacter GetCharacterOnTile(IntVect2D vect)
        {
            //タイル以外
            if (vect == null) return null;
            //ターゲットの検索
            var target = GetCharacterFromVect2D(vect);

            //ターゲットが存在しないマスをタップ
            if (target == null) return null;
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

        #endregion::Utillity


    }
}