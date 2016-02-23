using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{

    [System.Serializable]
    public class SingleActionParameter
    {
        public float judgeTime;
        public float startInterval;
        public GameObject attackMakerPrefab;
        //public AttackEffectKind attackEffectKind;
    }
    //一つの攻撃のパラメーター
    [System.Serializable]
    public class AttackParameter
    {
        //攻撃範囲
        public List<IntVect2D> attackRange;

        //攻撃種類
        public AttackDistance attackDistance;

        public ElementKind element;
        //範囲攻撃
        //範囲内の全ての敵にダメージ
        public bool isMultiAttack;

        //技の威力
        public int power;

    }
    //タップによる単体攻撃
    public class BCharacterSingleAttack : BCharacterAttacker
    {
        public List<SingleActionParameter> singleActionParamaters = new List<SingleActionParameter>();

        //一度のタップのパラメータ

        //タップで攻撃
        public float changeTimeSingleMode = 1f;
        bool isTapDetect = false;
        float startTime,leftTime;
        
        GameObject nowAttackMaker;
        SingleActionParameter nowSingleAction;
        public GameObject attackEffectPrefab;
        Vector3 popupPositionInScreen;
        //選択した攻撃方法
        [System.NonSerialized]
        public AttackParameter selectAttackParameter = null;
        public int selectActionNumber=0;

        Transform effectCanvas;

        #region::初期化
        public override void Enable()
        {
            base.Enable();
            IT_Gesture.onShortTapE += OnShortTap;
            selectAttackParameter = character.characterParameter.attackParameter[selectActionNumber];
            
            //BattleStage.Instance.ChangeTileColorsToAttack(selectAttackParameter.attackRange, this.character);
        }
        public override void Disable()
        {
            IT_Gesture.onShortTapE -= OnShortTap;
            isTapDetect = false;
            
            //CameraChange.Instance.
            base.Disable();
        }
        public override void Init()
        {
            base.Init();
            effectCanvas = GameObject.FindGameObjectWithTag("EffectCanvas").transform;
        }

        #endregion::初期化

        void OnShortTap(Vector2 pos)
        {
            UpdateAttackState(pos);
        }

        public void UpdateAttackState(Vector2 position)
        {
            if (isNowAction == true) return;
            //if (CameraChange.Instance.nowCameraMode != CameraMode.FromBack && CameraChange.Instance.nowCameraMode != CameraMode.FromFront) return;
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
                    attackTarget = new List<BCharacterPlayer>();
                }
                //再設定
                attackTarget.Add(target);
            }
            //ターゲットのタイル変更
            //BattleStage.Instance.UpdateTileColors(target, TileState.Attack);

            

            SetAttackMode();
            //PlayerOwner.Instance.commandState = CommandState.Attack;

        }

        bool IsInAttackRange(IntVect2D targetPositionArray)
        {
            return selectAttackParameter.attackRange.Any(x => x.IsEqual(IntVect2D.Sub(targetPositionArray, character.positionArray)));
        }
        //ターゲット選択ボタンを選択した時
        void SetAttackMode()
        {
            if (attackTarget.Count == 0)
            {
                isSetTarget = false;
                //UIの変更
                UIBottomCommandParent.Instance.CreateExecuteAttack();
                //攻撃決定ボタンの取り消し
            }
            else
            {
                isSetTarget = true;
                //UIの変更
                UIBottomCommandParent.Instance.CreateExecuteAttack();
                //攻撃決定ボタンの有効化
                //ActionSelect.Instance.EnableAttackButton();
            }
        }

        #endregion::ターゲット選択


        //攻撃モーション時間

        //UIボタンから押す
        public void ExecuteAttack()
        {
            StartCoroutine("AttackWithTap");
            IT_Gesture.onShortTapE -= OnShortTap;
        }

        IEnumerator AttackWithTap()
        {
            if (attackTarget.Count == 0) yield return null;


            //カメラ切り替え
            BCameraChange.Instance.ActiveLeanMode();
            BCameraMove.Instance.MoveToTapAttack(this, attackTarget[0].transform.position, changeTimeSingleMode);
            yield return new WaitForSeconds(changeTimeSingleMode);


            //攻撃アニメーション
            animator.SetSingleAttack(selectActionNumber);
            
            isNowAction = true;

            popupPositionInScreen = Camera.main.WorldToScreenPoint(new Vector3(attackTarget[0].transform.position.x, attackTarget[0].transform.position.y + 1f, attackTarget[0].transform.position.z));

            var totalDamage = 0;
            foreach (var action in singleActionParamaters)
            {

                nowSingleAction = action;
                //startInterval待ってからマーカー縮小
                yield return new WaitForSeconds(action.startInterval);
                //マーカー表示
                nowAttackMaker = Instantiate(action.attackMakerPrefab, popupPositionInScreen, Quaternion.identity) as GameObject;
                nowAttackMaker.transform.SetParent(effectCanvas);


                //マーカー縮小始まり
                iTween.ScaleTo(nowAttackMaker, iTween.Hash("scale", new Vector3(0.1f, 0.1f, 1.0f), "time", action.judgeTime));


                //タップ判定
                startTime = Time.time;
                //タップできなかったら最大時間
                leftTime = 0;
                IT_Gesture.onShortTapE += OnTapForAttack;
                yield return new WaitForSeconds(action.judgeTime);
                IT_Gesture.onShortTapE -= OnTapForAttack;
                isTapDetect = false;
                if (nowAttackMaker != null)
                {
                    Destroy(nowAttackMaker);

                }
                totalDamage += CalcDamageFromLeftTime(leftTime);
            }

            //攻撃
            foreach (var target in attackTarget)
            {

                //target.Damage(character.characterParameter.power);
                //攻撃力に関わらず秒数
                target.Life.Damage(totalDamage);
            }

            foreach (var target in attackTarget)
            {
                target.Life.CheckDestroy();
            }


            attackTarget = null;
            character.IsAttacked = true;
            Invoke("OnCompleteAnimation", resetInterval);
            //攻撃時にUI非表示
            //ActionSelect.Instance.EndActiveAction();
            yield return null;

        }
        //タイミングを合わせたタップ
        void OnTapForAttack(Vector2 pos)
        {
            if (isTapDetect == true) return;
            isTapDetect = true;
            //残り時間
            var nowTime = Time.time - startTime;
            leftTime = Mathf.Clamp(nowTime, 0, nowSingleAction.judgeTime);
            Destroy(nowAttackMaker);
            nowAttackMaker = null;
            var attackEffect = Instantiate(attackEffectPrefab, popupPositionInScreen, Quaternion.identity) as GameObject;
            attackEffect.transform.SetParent(effectCanvas);
        }
        //ダメージ量計算
        int CalcDamageFromLeftTime(float _leftTime)
        {
            return (int)(_leftTime * 1000);
        }
        //モーション時間＋猶予時間の案もありか
        public float resetInterval = 3f;
        void OnCompleteAnimation()
        {
            isNowAction = false;
            //行動終了

            character.EndActiveCharacterAction();
            
        }
    }
}
