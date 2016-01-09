using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Animator))]
public class CharacterAttacker : CharacterBaseAction
{

        [System.NonSerialized]
    public bool isSetTarget = false;
    protected List<Character> attackTarget=new List<Character>();

    //選択した攻撃方法
    [System.NonSerialized]
    public AttackParameter selectAttackParameter=null;


    protected void OnActiveCharacter()
    {
        isSetTarget = false;
        attackTarget = new List<Character>();
    }

   public override void Enable()
    {
        base.Enable();  
        OnActiveCharacter();

    }
   public override void Disable()
    {

        BattleStage.Instance.ResetAllTileColor();
        base.Disable();  
    }

    //タイル上のキャラが自身にとっての敵キャラなら取得
    protected Character GetOpponentCharacterOnTile(IntVect2D toPos)
    {

        var chara = Character.GetCharacterOnTile(toPos);
        if (chara == null) return null;
        if (chara.isEnemy != this.character.isEnemy) return chara;
        return null;
    }
    protected Character GetOpponentCharacterFromTouch(Vector2 touchPosition)
    {
        var chara = Character.GetCharacterOnTile(touchPosition);
        if (chara == null) return null;
        if (chara.isEnemy != this.character.isEnemy) return chara;
        return null;

    }
}
