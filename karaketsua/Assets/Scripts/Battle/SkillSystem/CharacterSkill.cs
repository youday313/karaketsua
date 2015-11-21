using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CharacterSkill : CharacterAttacker {

	public List<SkillTileWave> skillTileWaves=new List<SkillTileWave>();

    void OnActiveCharacter()
    {
        isSetTarget = false;
        isNowAction = false;
    }

    void Enable()
    {
        OnActiveCharacter();
        //IT_Gesture.onTouchDownE+=OnTouchDown;
        //IT_Gesture.onMouse1DownE += OnMouseDown;
        IT_Gesture.onShortTapE += OnShortTap;
        BattleStage.Instance.UpdateTileColors(this.character, TileState.Skill);
    }
    void Disable()
    {
        //IT_Gesture.onTouchDownE -= OnTouchDown;
        //IT_Gesture.onMouse1DownE -= OnMouseDown;
        IT_Gesture.onShortTapE -= OnShortTap;
        BattleStage.Instance.ResetTileColor();
    }

    void OnShortTap(Vector2 pos)
    {
        UpdateAttackState(pos);

    }
    void UpdateAttackState(Vector2 position)
    {
        if (isNowAction == true) return;
        if (isSetTarget == false)
        {
            SetTarget(position);
        }
    }
    //public void SetTarget(Vector2 touchPosition)
    //{
    //    //ターゲットの検索
    //    var target = GetOpponentCharacterFromTouch(touchPosition);

    //    //ターゲットが存在しないマスをタップ
    //    if (target == null) return;

    //    //攻撃範囲内
    //    if (Mathf.Abs(target.positionArray.x - character.positionArray.x) + Mathf.Abs(target.positionArray.y - character.positionArray.y) > character.characterParameter.attackRange) return;


    //    attackTarget = target;
    //    //ターゲットのタイル変更
    //    //BattleStage.Instance.UpdateTileColors(target, TileState.Attack);


    //    SetAttackMode(true);
    //    //PlayerOwner.Instance.commandState = CommandState.Attack;

    //}

	IEnumerator ICreateNewWave(){
		foreach (var wave in skillTileWaves) {
			CreateNewWave (wave);

			yield return null;
		}
	}
	void CreateNewWave(SkillTileWave wave){
		wave.CreateNewTileSequence (character.positionArray);
	}

    public void SuccessWave()
    {

    }
}
