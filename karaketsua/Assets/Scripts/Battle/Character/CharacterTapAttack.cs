using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;




public class CharacterTapAttack : CharacterAttacker
{
    public enum ActionKind
    {
        Tap,
        Swipe,
        Hold
    }
    [System.Serializable]
    public class TapActionParameter
    {
        public float judgeTime;
        public float startInterval;
        public ActionKind actionKind;
        public GameObject attackMakerPrefab;
    }
    public List<TapActionParameter> tapActionParamaters = new List<TapActionParameter>();

    #region::初期化
    public override void Enable()
    {
        base.Enable();


        IT_Gesture.onShortTapE += OnShortTap;

        //仮で必ず最初の攻撃方法を取る
        selectAttackParameter = character.characterParameter.attackParameter[0];

        BattleStage.Instance.ChangeTileColorsToAttack(selectAttackParameter.attackRange, this.character);
    }
    public override void Disable()
    {

        //IT_Gesture.onTouchDownE -= OnTouchDown;
        //IT_Gesture.onMouse1DownE -= OnMouseDown;
        IT_Gesture.onShortTapE -= OnShortTap;
        base.Disable();
    }

    #endregion::初期化
    void OnShortTap(Vector2 pos)
    {
        UpdateAttackState(pos);

    }

    public void UpdateAttackState(Vector2 position)
    {
        if (isNowAction == true) return;
        if (CameraChange.Instance.nowCameraMode != CameraMode.FromBack && CameraChange.Instance.nowCameraMode != CameraMode.FromFront) return;
        SetTarget(position);
    }

    #region::ターゲット選択
    //ターゲットの決定
    public void SetTarget(Vector2 touchPosition)
    {
        //ターゲットの検索
        var target = GetOpponentCharacterFromTouch(touchPosition);

        //ターゲットが存在しないマスをタップ
        if (target == null) return;

        //攻撃範囲内
        //if (Mathf.Abs(target.positionArray.x - character.positionArray.x) + Mathf.Abs(target.positionArray.y - character.positionArray.y) > character.characterParameter.attackRange) return;
        if (IsInAttackRange(target.positionArray) == false) return;

        //複数攻撃
        if (selectAttackParameter.isMultiAttack == true)
        {
            //未選択からの選択
            if (attackTarget.Contains(target) == false)
            {
                //タイル変更

                attackTarget.Add(target);
            }
            //選択からの解除
            else
            {
                //タイル変更
                attackTarget.Remove(target);
                return;
            }

        }
        else
        {
            //他のキャラが既に選択されていた場合は除く
            if (attackTarget.Count != 0)
            {
                //タイル変更
                //除く
                attackTarget = new List<Character>();
            }
            //再設定
            attackTarget.Add(target);

        }
        //ターゲットのタイル変更
        //BattleStage.Instance.UpdateTileColors(target, TileState.Attack);


        SetAttackMode(true);
        //PlayerOwner.Instance.commandState = CommandState.Attack;

    }

    bool IsInAttackRange(IntVect2D targetPositionArray)
    {
        return selectAttackParameter.attackRange.Any(x => x.IsEqual(IntVect2D.Sub(targetPositionArray, character.positionArray)));
    }
    //ターゲット選択ボタンを選択した時
    void SetAttackMode(bool isSet)
    {
        isSetTarget = isSet;
        ActionSelect.Instance.EnableAttackButton();

    }

    #endregion::ターゲット選択


    //攻撃モーション時間
    //モーション時間＋猶予時間の案もありか
    public float resetInterval = 3f;
    void OnCompleteAnimation()
    {
        isNowAction = false;
        character.EndActiveCharacterAction();
    }


    //一度のタップのパラメータ


    //タップで攻撃
    public float changeTimeTapMode = 1f;
    //public GameObject attackMakerPrefab;
    bool isTapDetect = false;
    float startTime;
    float tapLeftTime;
    GameObject nowAttackMaker;
    TapActionParameter nowTapAction;
    public GameObject attackEffectPrefab;
    Vector3 popupPositionInScreen;
    public IEnumerator AttackWithTap()
    {
        if (attackTarget.Count == 0) yield return null;

        //カメラ移動
        cameraMove.MoveToTapAttack(this, attackTarget[0].transform.position, changeTimeTapMode);
        yield return new WaitForSeconds(changeTimeTapMode);

        //攻撃アニメーション
        animator.SetTrigger("TapAttack");
        isNowAction = true;

        popupPositionInScreen = Camera.main.WorldToScreenPoint(new Vector3(attackTarget[0].transform.position.x, attackTarget[0].transform.position.y + 1f, attackTarget[0].transform.position.z));


        foreach(var action in tapActionParamaters){

            nowTapAction = action;
            //startInterval待ってからマーカー縮小
            yield return new WaitForSeconds(action.startInterval);
            //マーカー表示
            nowAttackMaker = Instantiate(action.attackMakerPrefab, popupPositionInScreen, Quaternion.identity) as GameObject;
            nowAttackMaker.transform.SetParent(GameObject.FindGameObjectWithTag("EffectCanvas").transform);
            

            //マーカー縮小始まり
            iTween.ScaleTo(nowAttackMaker, iTween.Hash("scale", new Vector3(0.1f, 0.1f, 1.0f), "time", action.judgeTime));


            //タップ判定
            startTime = Time.time;
            //タップできなかったら最大時間
            tapLeftTime = 0;
            IT_Gesture.onShortTapE += OnTapForAttack;
            yield return new WaitForSeconds(action.judgeTime);
            IT_Gesture.onShortTapE -= OnTapForAttack;
            isTapDetect = false;
            if (nowAttackMaker != null)
            {
                Destroy(nowAttackMaker);

            }
            //攻撃
            foreach (var target in attackTarget)
            {
                //target.Damage(character.characterParameter.power);
                //攻撃力に関わらず秒数
                target.Damage((int)(tapLeftTime * 1000));

            }


        }

        foreach (var target in attackTarget)
        {
            target.CheckDestroy();
        }
       

        attackTarget = null;

        Invoke("OnCompleteAnimation", resetInterval);
        //攻撃時にUI非表示
        ActionSelect.Instance.EndActiveAction();
        yield return null;

    }
    //タイミングを合わせたタップ
    void OnTapForAttack(Vector2 pos)
    {
        if (isTapDetect == true) return;
        isTapDetect = true;
        //残り時間
        var nowTime = Time.time - startTime;
        tapLeftTime = Mathf.Clamp(nowTime, 0, nowTapAction.judgeTime);
        Destroy(nowAttackMaker);
        nowAttackMaker = null;
        var attackEffect = Instantiate(attackEffectPrefab, popupPositionInScreen, Quaternion.identity) as GameObject;
        attackEffect.transform.SetParent(GameObject.FindGameObjectWithTag("EffectCanvas").transform);
    }



}
