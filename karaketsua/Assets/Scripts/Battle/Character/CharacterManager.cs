using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class CharacterManager : Singleton<CharacterManager>
{


    [System.NonSerialized]
    public List<Character> characters = new List<Character>();

    // Use this for initialization
    void Start()
    {


        foreach (var chara in StageData.Instance.playerCharacters)
        {
            var cha = Instantiate(chara.prefab) as Character;
            cha.Init(new IntVect2D((int)chara.position.x, (int)chara.position.y), false);
            characters.Add(cha);
            cha.transform.SetParent(transform);
        }
        foreach (var chara in StageData.Instance.enemyCharacters)
        {
            var cha = Instantiate(chara.prefab) as Character;
            //向き変更
            cha.Init(new IntVect2D((int)chara.position.x, (int)chara.position.y), true);
            characters.Add(cha);
            cha.transform.SetParent(transform);
        }

    }

    public void DestroyCharacter(Character chara)
    {
        characters.Remove(chara);
        //Destroy(chara.gameObject);
        var count = characters.Where(t => t.isEnemy == true).Count();
        if (count == 0)
        {
            SceneManager.Instance.SetNextScene("ResultScene");
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    Character activeCharacter;
    public Character GetNowActiveCharacter()
    {
        return activeCharacter;
    }
    public void SetNowActiveCharacter(Character chara)
    {
        activeCharacter = chara;
    }

    #region::Utility
    //タイル上のキャラを取得
    //いない時はnull
    Character GetCharacterFromVect2D(IntVect2D toPos)
    {
        return characters.
        Where(t => IntVect2D.IsEqual(toPos, t.positionArray)).
        FirstOrDefault();
    }

    //スクリーン座標からキャラクター取得
    public Character GetCharacterOnTile(Vector2 screenPos)
    {
        //タイルをスクリーン座標から取得
        var targetPosition = BattleStage.instance.GetTilePositionFromScreenPosition(screenPos);

       return GetCharacterFromVect2D(targetPosition);

    }
    //Vect2Dからキャラクター取得
    public Character GetCharacterOnTile(IntVect2D vect)
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
        return GetCharacterOnTile(pos)!=null;
    }
    public bool IsExistCharacterOnTile(IntVect2D pos)
    {
        return GetCharacterOnTile(pos) != null;
    }

    #endregion::Utillity


}