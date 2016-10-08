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
                OnCompleteAction();
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


            IsNowAction = true;

            //ダメージ
            foreach(var target in TargetList) {
                var damageMagnification = calcDamageMagnification();
                var characterPower = character.characterParameter.power;
                target.Life.Damage(characterPower,damageMagnification);
            }
            //死亡
            foreach(var target in TargetList) {
                target.Life.CheckDestroy();
            }

            //攻撃終了
            Invoke("onCompleteAnimation",attackTime);
            IsDone = true;
            yield return null;
        }

        private void onCompleteAnimation()
        {
            IsNowAction = false;
            //行動終了
            OnCompleteAction();
            //character.OnEndActive();

        }
        //倍率の算出
        private float calcDamageMagnification()
        {
            //会心＊振れ幅＊技倍率＊タップ倍率
            return 1 * 1 * selectAttackParameter.powerMagnification * 1;
        }





    }
}