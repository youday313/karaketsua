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
        private GameObject attackMakerPrefab;
        [SerializeField]
        private GameObject[] attackEffects;

        //一度のタップのパラメータ

        //タップで攻撃
        public float changeTimeSingleMode = 1f;
        bool isTapDetect = false;
        float startTime, leftTime;

        GameObject nowAttackMaker;
        SingleActionParameter nowSingleAction;
        public GameObject onTapEffect;
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
            // NOTE:キャラごとに変えるならキャラマスタからプレハブ名を指定させる
            attackMakerPrefab = Resources.Load("AttackMaker") as GameObject;
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
            foreach(var tar in TargetList)
                tar.SetTargeted(false);
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
                nowAttackMaker = Instantiate(attackMakerPrefab, popupPositionInScreen, Quaternion.identity) as GameObject;
                nowAttackMaker.transform.SetParent(effectCanvas);


                //マーカー縮小始まり
                iTween.ScaleTo(nowAttackMaker, iTween.Hash("scale", new Vector3(0.1f, 0.1f, 1.0f), "time", action.judgeTime));


                //タップ判定
                startTime = Time.time;
                //タップできなかったら最大時間
                //leftTimheは大きい程よい。leftTime=judgeTimeがパーフェクト
                leftTime = 0;
                IT_Gesture.onShortTapE += OnTapForAttack;
                yield return new WaitForSeconds(action.judgeTime);
                IT_Gesture.onShortTapE -= OnTapForAttack;
                isTapDetect = false;
                if(nowAttackMaker != null) {
                    Destroy(nowAttackMaker);

                }
                totalTapRatios.Add(leftTime / action.judgeTime);
            }

            //攻撃
            foreach(var target in TargetList) {
                var damageMagnification = CalcDamageMagnification(CalcDamageFromTapMagnificationRation(totalTapRatios));
                var characterPower = character.characterParameter.power;
                target.Life.Damage(characterPower, damageMagnification);
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
        //タイミングを合わせたタップ
        void OnTapForAttack(Vector2 pos)
        {
            if(isTapDetect == true)
                return;
            isTapDetect = true;
            //残り時間
            var nowTime = Time.time - startTime;
            leftTime = Mathf.Clamp(nowTime, 0, nowSingleAction.judgeTime);
            Destroy(nowAttackMaker);
            nowAttackMaker = null;
            var attackEffect = Instantiate(onTapEffect, popupPositionInScreen, Quaternion.identity) as GameObject;
            attackEffect.transform.SetParent(effectCanvas);
        }
        //ダメージ量計算
        int CalcDamageFromLeftTime(float _leftTime)
        {
            return (int)(_leftTime * 1000);
        }
        //モーション時間＋猶予時間の案もありか
        public float resetInterval = 3f;

        // アニメーションを終了し行動を終了する
        private void onCompleteAnimation()
        {
            IsNowAction = false;
            //行動終了
            onCompleteAction();
            character.OnEndActive();
        }

        //会心（5%）
        public static float criticalMagnification = 1.5f;
        public static float criticalProbability = 5f;

        float CalcCriticalDamage()
        {
            return UnityEngine.Random.Range(0, 100) < criticalProbability ? criticalMagnification : 1.0f;
        }
        //ダメージ振れ幅
        public static float minRandamAmplitude = 0.9f;
        public static float maxRandamAmplitude = 0.9f;

        float CalcRandamAmplitudeDamage()
        {
            return UnityEngine.Random.Range(minRandamAmplitude, maxRandamAmplitude);
        }
        //倍率の算出
        float CalcDamageMagnification(float tapMagnification)
        {
            //会心＊振れ幅＊技倍率＊タップ倍率
            return CalcCriticalDamage() * CalcRandamAmplitudeDamage() * selectAttackParameter.powerMagnification * tapMagnification;
        }
        //タップ倍率の算出
        public static List<float> judgeRange = new List<float>(){ 0.9f, 0.51f };
        public static List<float> judgeAmplitude = new List<float>() { 1.3f, 1.1f, 1 };

        float CalcDamageFromTapMagnificationRation(List<float> timeRatios)
        {
            var ave = timeRatios.Average();
            for(var i = 0; i < judgeRange.Count; i++) {
                if(ave >= judgeRange[i])
                    return judgeAmplitude[i];
            }
            return judgeAmplitude.Last();
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
    }
}