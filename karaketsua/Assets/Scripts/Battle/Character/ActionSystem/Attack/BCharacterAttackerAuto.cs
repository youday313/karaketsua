using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using BattleScene;


namespace BattleScene
{
    public class BCharacterAttackerAuto: BCharacterAttackerBase
    {
        [SerializeField]
        private float cameraInterval = 1f;
        [SerializeField]
        private float attackTime = 2f;
        [SerializeField]
        private GameObject attackEffect;


        private Transform effectCanvas;
        private AutoAttackParameter selectAttackParameter;

        public override void Awake()
        {
            base.Awake();
            effectCanvas = GameObject.FindGameObjectWithTag("EffectCanvas").transform;
            selectAttackParameter = GetComponent<BCharacterEnemy>().characterParameter.autoAttackParameter;
        }

        public override void Enable()
        {
            base.Enable();

            //技のセット
            //selectAttackParameter = character.characterParameter.attackParameter[selectActionNumber];

            //BattleStage.Instance.ChangeTileColorsToAttack(selectAttackParameter.attackRange, this.character);
        }
        public override void Disable()
        {


            //CameraChange.Instance.
            base.Disable();
        }


        public void StartAutoAttack()
        {
            //ターゲットの検索と設定
            var target = setTarget();
            if(target == false) {
                onCompleteAction();
                return;
            }
            //攻撃の実行
            StartCoroutine(executeAttack());
        }

        private bool setTarget()
        {
            //攻撃可能位置の設定
            var attackablePosition = selectAttackParameter.attackRanges.Select(x => IntVect2D.Add(x,character.PositionArray)).ToList();
            if(attackablePosition == null) return false;
            //デバッグ出力
            //攻撃可能位置にいるキャラクター
            var opponentCharacters = new List<BCharacterBase>();
            foreach(var pos in attackablePosition) {
                var chara = BCharacterManager.Instance.GetOpponentCharacterOnTileFormVect2D(pos,character.IsEnemy);
                if(chara != null) {
                    opponentCharacters.Add(chara);
                }
            }
            if(opponentCharacters.Count == 0) return false;

            //一番近い位置がターゲット
            TargetList.Add(opponentCharacters.OrderBy(c => IntVect2D.Distance(c.PositionArray,character.PositionArray)).First());
            return true;

        }

        private IEnumerator executeAttack()
        {
            //横を向く

            //UIOff
            UIBottomCommandParent.UICommandState = EUICommandState.None;
            UIBottomAllManager.Instance.UpdateUI();
            //カメラ移動
            BCameraManager.Instance.ActiveLeanMode();
            BCameraMove.Instance.MoveToAutoAttack(this,TargetList[0].transform.position);
            HideOtherCharacters();
            FaceCharacter();
            yield return new WaitForSeconds(cameraInterval);

            //攻撃アニメーション
            animator.SetAutoAttack();
            // 攻撃エフェクト
            StartAttackEffect(selectAttackParameter.isForceFace);

            IsNowAction = true;

            //ダメージ
            foreach(var target in TargetList) {
                var damageRate = calcAutoDamageRate();
                var characterPower = character.characterParameter.power;
                target.Life.Damage(characterPower,damageRate);
            }
            //死亡
            foreach(var target in TargetList) {
                target.Life.CheckDestroy();
            }

            //攻撃終了
            StartCoroutine(WaitTimer.WaitSecond(() => onCompleteAnimation(), attackTime));
            IsDone = true;
            yield return null;
        }

        private void onCompleteAnimation()
        {
            IsNowAction = false;
            //行動終了
            onCompleteAction();
            //character.OnEndActive();
        }

        private float calcAutoDamageRate()
        {
            return calcBaseDamageRate() * selectAttackParameter.powerRate;
        }

        // 攻撃エフェクト
        // エフェクトは再生終了後自動で削除される
        private void StartAttackEffect(bool isForceFace)
        {
            // 向き合う攻撃ならプレイヤー中心
            if(isForceFace) {
                var effect = Instantiate(attackEffect);
                effect.transform.SetParent(transform);
                effect.transform.localPosition = Vector3.zero;
            }
            // 複数攻撃ならターゲット中心
            else {
                foreach(var target in TargetList) {
                    var effect = Instantiate(attackEffect);
                    effect.transform.SetParent(target.transform);
                    effect.transform.localPosition = Vector3.zero;
                }
            }
        }





    }
}