using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;
using System;

namespace BattleScene
{

    //タップによる単体攻撃
    public class BCharacterAttackerSingle : BCharacterAttackerBase
    {
        private GameObject attackMaker;   // タイミング判定表示マーカー
        private GameObject onTapEffect;         // タップ判定マーカー
        [SerializeField]
        private GameObject[] attackEffects;     // 攻撃演出

        //一度のタップのパラメータ

        //タップで攻撃
        public float changeTimeSingleMode = 1f;
        private bool isTapDetect = false;
        private float startTime;
        private float judgeRate;  //justRateは大きい程Just。MAX:1、Min:0

        private GameObject nowAttackMakerParent;
        SingleActionParameter nowSingleAction;
        Vector3 popupPositionInScreen;

        //選択した攻撃方法
        [System.NonSerialized]
        public SingleAttackParameter selectAttackParameter = null;

        public bool IsAttackable {
            get { return TargetList.Count != 0; }
        }

        //選択した技番号
        public int SelectWazaNumber;

        Transform effectCanvas;


        #region::初期化

        public override void Awake()
        {
            base.Awake();
            effectCanvas = GameObject.FindGameObjectWithTag("EffectCanvas").transform;
            attackMaker = Resources.Load<GameObject>(ResourcesPath.AttackMaker);
            onTapEffect = Resources.Load<GameObject>(ResourcesPath.TapEffect);
        }

        public override void Enable()
        {
            base.Enable();
            IT_Gesture.onShortTapE += OnShortTap;
            isTapDetect = false;

            //技のセット
            selectAttackParameter = character.characterParameter.singleAttackParameters[SelectWazaNumber];

            //攻撃範囲の表示
            BBattleStage.Instance.OnSlectWaza(character, selectAttackParameter);
        }

        public override void Disable()
        {
            IT_Gesture.onShortTapE -= OnShortTap;
            isTapDetect = false;
            SelectWazaNumber = 0;
            //タイル変更
            foreach(var tar in TargetList) {
                tar.SetTargeted(false);
            }
            base.Disable();
        }

        #endregion::初期化

        void OnShortTap(Vector2 pos)
        {
            if(IsNowAction == true)
                return;
            SetTarget(pos);
        }

        #region::ターゲット選択

        //ターゲットの決定
        public void SetTarget(Vector2 touchPosition)
        {
            //ターゲットの検索
            var target = BCharacterManager.Instance.GetOpponentCharacterOnTileFromTouch(touchPosition, character.IsEnemy);
            //ターゲットが存在しないマスをタップ
            if(target == null)
                return;
            //攻撃範囲内
            //if (Mathf.Abs(target.positionArray.x - character.positionArray.x) + Mathf.Abs(target.positionArray.y - character.positionArray.y) > character.characterParameter.attackRange) return;
            if(IsInAttackRange(target.PositionArray) == false)
                return;
            //複数攻撃
            if(selectAttackParameter.isMultiAttack == true) {
                //未選択からの選択
                if(TargetList.Contains(target) == false) {
                    //タイル変更
                    target.SetTargeted(true);
                    TargetList.Add(target);

                }
                //選択からの解除
                else {
                    //タイル変更
                    target.SetTargeted(false);
                    TargetList.Remove(target);

                    return;
                }

            } else {
                //他のキャラが既に選択されていた場合は除く
                if(TargetList.Count != 0) {
                    //タイル変更
                    foreach(var tar in TargetList)
                        tar.SetTargeted(false);
                    //除く
                    TargetList = new List<BCharacterBase>();
                } else {
                    //再設定
                    TargetList.Add(target);
                    target.SetTargeted(true);
                }
            }

            //UI更新
            UIBottomCommandParent.UICommandState = EUICommandState.ExecuteAttack;
            UIBottomCommandParent.Instance.UpdateUI();

        }

        bool IsInAttackRange(IntVect2D targetPositionArray)
        {
            return selectAttackParameter.attackRanges.Any(x => x.IsEqual(IntVect2D.Sub(targetPositionArray, character.PositionArray)));
        }

        #endregion::ターゲット選択


        //攻撃モーション時間

        //UIボタンから押す
        public void ExecuteAttack()
        {
            StartCoroutine(AttackWithTap());
            IT_Gesture.onShortTapE -= OnShortTap;
        }

        IEnumerator AttackWithTap()
        {
            if(TargetList.Count == 0) {
                yield return null;
            }

            IsNowAction = true;
            //カメラ切り替え
            BCameraManager.Instance.ActiveLeanMode();
            BCameraMove.Instance.MoveToTapAttack(this, TargetList[0].transform.position, changeTimeSingleMode);
            HideOtherCharacters();

            // 向く方向を変える
            FaceCharacter();

            // ちょっと待つ
            yield return new WaitForSeconds(changeTimeSingleMode);

            //攻撃アニメーション
            animator.SetSingleAttack(SelectWazaNumber);

            // 攻撃エフェクト
            StartAttackEffect(selectAttackParameter.isForceFace);

            popupPositionInScreen = Camera.main.WorldToScreenPoint(new Vector3(TargetList[0].transform.position.x, TargetList[0].transform.position.y + 1f, TargetList[0].transform.position.z));

            var attackList = selectAttackParameter.actionParameters;
            List<float> totalTapRatios = new List<float>();
            foreach(var action in attackList) {

                nowSingleAction = action;
                //startInterval待ってからマーカー縮小
                yield return new WaitForSeconds(action.judgeTime);
                //マーカー表示
                nowAttackMakerParent = Instantiate(attackMaker, popupPositionInScreen, Quaternion.identity) as GameObject;
                nowAttackMakerParent.transform.SetParent(effectCanvas);
                var maker = nowAttackMakerParent.transform.FindChild("Expand").gameObject;

                //マーカー縮小始まり
                iTween.ScaleTo(maker, iTween.Hash("scale", Vector3.one, "time", action.judgeTime));

                //タップ判定
                startTime = Time.time;
                //タップできなかったら最小倍率
                judgeRate = 0;
                IT_Gesture.onShortTapE += onTapForAttack;
                yield return new WaitForSeconds(action.judgeTime);
                IT_Gesture.onShortTapE -= onTapForAttack;
                isTapDetect = false;
                if(nowAttackMakerParent != null) {
                    // 親を削除
                    Destroy(nowAttackMakerParent);
                }
                totalTapRatios.Add(judgeRate);
            }

            //攻撃
            foreach(var target in TargetList) {
                var damageRate = calcTapDamageRate(totalTapRatios.ToArray());
                var characterPower = character.characterParameter.power;
                target.Life.Damage(characterPower, damageRate);
            }

            foreach(var target in TargetList) {
                target.Life.CheckDestroy();
            }

            foreach(var tar in TargetList.Where(x => x != null)) {
                tar.SetTargeted(false);
            }

            IsDone = true;
            StartCoroutine(WaitTimer.WaitSecond(() => onCompleteAnimation(), resetInterval));
            //攻撃時にUI非表示
            //ActionSelect.Instance.EndActiveAction();
            yield return null;

        }

        // タイミングを合わせたタップ
        // タップ入力から呼ばれる
        private void onTapForAttack(Vector2 pos)
        {
            if(isTapDetect) {
                return;
            }
            isTapDetect = true;
            // オーバーしていたら0、それ以外なら残り時間割合
            judgeRate = Time.time - startTime > nowSingleAction.judgeTime ? 0 : (Time.time - startTime) / nowSingleAction.judgeTime;
            Destroy(nowAttackMakerParent);
            nowAttackMakerParent = null;
            // 攻撃エフェクト　自動で削除
            var attackEffect = Instantiate(onTapEffect, popupPositionInScreen, Quaternion.identity) as GameObject;
            attackEffect.transform.SetParent(effectCanvas);
        }

        //モーション時間＋猶予時間の案もありか
        [SerializeField]
        private float resetInterval = 3f;

        // アニメーションを終了し行動を終了する
        private void onCompleteAnimation()
        {
            IsNowAction = false;
            //行動終了
            onCompleteAction();
            character.OnEndActive();
        }
            
        // 倍率の算出
        private readonly float[] TapRateRanges = { 0.9f, 0.51f };   // Just、Greatの判定割合
        private readonly float[] TapRateValues = { 1.3f, 1.1f, 1 }; // 攻撃倍率
        private float calcTapDamageRate(float[] timeRatios)
        {
            //タップ倍率の算出
            float tapRate = TapRateValues.Last();
            var ave = timeRatios.Average();
            for(var i = 0; i < TapRateRanges.Length; i++) {
                // 平均が高い→倍率も高い
                if(ave >= TapRateRanges[i]) {
                    tapRate = TapRateValues[i];
                    break;
                }
            }
            // ベース値*技倍率*タップ倍率
            return  calcBaseDamageRate() * selectAttackParameter.powerRate * tapRate;
        }
            
        // 攻撃エフェクト
        // エフェクトは再生終了後自動で削除される
        private void StartAttackEffect(bool isForceFace)
        {
            // 向き合う攻撃ならプレイヤー中心
            if(isForceFace) {
                var effect = Instantiate(attackEffects[SelectWazaNumber]);
                effect.transform.SetParent(transform);
                effect.transform.localPosition = Vector3.zero;
            }
            // 複数攻撃ならターゲット中心
            else {
                foreach(var target in TargetList) {
                    var effect = Instantiate(attackEffects[SelectWazaNumber]);
                    effect.transform.SetParent(target.transform);
                    effect.transform.localPosition = Vector3.zero;
                }
            }
        }

        void OnDestroy()
        {
            IT_Gesture.onShortTapE -= OnShortTap;
        }
    }
}