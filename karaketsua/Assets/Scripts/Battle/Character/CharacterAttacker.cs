using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Animator))]
public class CharacterAttacker : MonoBehaviour {

    protected Character character;
        [System.NonSerialized]
    public bool isNowAction = false;
        [System.NonSerialized]
    public bool isSetTarget = false;
    protected bool isEnable = false;
    public bool IsEnable
    {
        get { return isEnable; }
        set
        {
            if (isEnable == false&&value==true)
            {
                Enable();
            }
            else if(isEnable==true&&value==false)
            {
                Disable();
            }
            isEnable = value;
        }

    }
    protected Animator animator;

    protected List<Character> attackTarget=new List<Character>();
    protected CameraMove cameraMove;
    //選択した攻撃方法
    [System.NonSerialized]
    public AttackParameter selectAttackParameter=null;

	// Use this for initialization
	void Start () {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
        cameraMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
	}

    protected void OnActiveCharacter()
    {
        isSetTarget = false;
        isNowAction = false;
        attackTarget = new List<Character>();
    }

   public virtual void Enable()
    {
        OnActiveCharacter();
        //IT_Gesture.onTouchDownE+=OnTouchDown;
        //IT_Gesture.onMouse1DownE += OnMouseDown;
        //IT_Gesture.onShortTapE += OnShortTap;
        //BattleStage.Instance.UpdateTileColors(this.character, TileState.Attack);
        
        //仮で必ず最初の攻撃方法を取る
        //selectAttackParameter = character.characterParameter.attackParameter[0];

		//BattleStage.Instance.ChangeTileColorsToAttack(selectAttackParameter.attackRange,this.character);
    }
    public virtual void Disable()
    {
        //IT_Gesture.onTouchDownE -= OnTouchDown;
        //IT_Gesture.onMouse1DownE -= OnMouseDown;
        //IT_Gesture.onShortTapE -= OnShortTap;
        BattleStage.Instance.ResetAllTileColor();
    }



    #region::ターゲット選択




    #endregion::ターゲット選択

    #region::攻撃
    //ターゲットが選択されていた時タイルタップ
    //public void Attack()
    //{
    //    if (attackTarget.Count == 0) return;
    //    //攻撃
    //    foreach (var target in attackTarget)
    //    {
    //        target.Damage(character.characterParameter.power);
    //    }
    //    StartAttackAnimation();
    //    //攻撃時にUI非表示
    //    ActionSelect.Instance.EndActiveAction();

    //}




    #endregion::攻撃



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
