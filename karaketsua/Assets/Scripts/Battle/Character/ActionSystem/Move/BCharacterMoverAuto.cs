using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using BattleScene;


namespace BattleScene
{
    public class BCharacterMoverAuto : BCharacterMoverBase
    {

        public event Action OnCompleteMove;
        AutoAttackParameter attackParameter;
        public override void Awake()
        {
            base.Awake();
            attackParameter = GetComponent<BCharacterEnemy>().characterParameter.autoAttackParameter;
        }

        //選択可能時
        public override void Enable()
        {
            if (isDone == true) return;
            base.Enable();
        }

        public override void Disable()
        {
            Reset();
        }

        public override void Reset()
        {
            base.Reset();
        }


        public IEnumerator StartAutoMove()
        {
            //２秒待つ
            yield return new WaitForSeconds(2f);
            //攻撃範囲に既にターゲット候補がいる
            if (CheckExistInAttackRange(character.PositionArray))
            {
                //移動しない
            }
                //移動するとターゲットとなる候補がいる
                //移動実行=true
            else if(TryMoveInAttackRange())
            {
                //移動した
            }
                //接近
            else
            {
                MoveForApproach();
            }
            //移動しない場合
            if (isDone == false)
            {
                isDone = true;
                if (OnCompleteMove != null) OnCompleteMove();
            }
            yield return null;
        }

        void RandamMove()
        {
            var toVect2D = new IntVect2D[] { 
                new IntVect2D(movableCount, 0),
                new IntVect2D(-movableCount, 0),
                new IntVect2D(0, movableCount),
                new IntVect2D(0, -movableCount)
            };
            //移動可能範囲を取得
            toVect2D = toVect2D.Shuffle();
            foreach (var toV in toVect2D)
            {
                RequestMoveFromVect2D(toV);
                if (isDone == true)
                {
                    break;
                }
            }
            if (isDone == false)
            {
                isDone = true;
                if (OnCompleteMove != null) OnCompleteMove();
            }
            
        }

        //攻撃範囲に敵がいる
        bool CheckExistInAttackRange(IntVect2D position)
        {
            return GetDistanceInAttackRange(position) == 0;
        }
        //攻撃範囲と敵キャラとの距離
        float GetDistanceInAttackRange(IntVect2D position)
        {
            //攻撃可能位置の設定
            var attackablePosition = attackParameter.attackRanges.Select(x => IntVect2D.Add(x, position)).ToList();
            //攻撃取得失敗
            if (attackablePosition == null) return -1;

            //攻撃可能位置にいるキャラクターとの距離,不可能=100
            float distance = 100;
            foreach (var pos in attackablePosition)
            {
                //攻撃位置と敵キャラとの距離比較
                //最小を設定
                var dis=BCharacterManager.Instance.GetOpponentCharacters(character.IsEnemy)
                    .Min(x => IntVect2D.Distance(x.PositionArray, pos));
                distance = Mathf.Min(distance, dis);
            }
            //一番近い位置がターゲット
            return distance;
        }

       //移動後攻撃範囲にいたら移動
        bool TryMoveInAttackRange()
        {
            var toVect2D = new IntVect2D[] { 
                new IntVect2D(movableCount, 0),
                new IntVect2D(-movableCount, 0),
                new IntVect2D(0, movableCount),
                new IntVect2D(0, -movableCount)
            };
            //移動可能範囲を取得
            toVect2D = toVect2D.Shuffle();
            foreach (var toV in toVect2D)
            {
                if (CheckExistInAttackRange(IntVect2D.Add(character.PositionArray,toV)))
                {
                    RequestMoveFromVect2D(toV);
                }
                if (isDone == true)return true;
            }
            return false;
        }


        //攻撃範囲に近づくように移動
        void MoveForApproach()
        {
            var toVect2D = new IntVect2D[] { 
                new IntVect2D(movableCount, 0),
                new IntVect2D(-movableCount, 0),
                new IntVect2D(0, movableCount),
                new IntVect2D(0, -movableCount)
            };

            toVect2D = toVect2D.Shuffle();
            List<VectAndDistance> vectAndDistance = new List<VectAndDistance>();
            //各移動後における攻撃範囲と敵キャラとの最小距離取得
            foreach (var toV in toVect2D)
            {
                vectAndDistance.Add(new VectAndDistance(GetDistanceInAttackRange(IntVect2D.Add(character.PositionArray, toV)), toV));
            }

            //距離が小さい順に移動試行
            foreach (var toV in vectAndDistance.OrderBy(v => v.distance).Select(v=>v.vect))
            {
                RequestMoveFromVect2D(toV);
                if (isDone == true)
                {
                    break;
                }

            }
        }
        class VectAndDistance
        {
            public float distance;
            public IntVect2D vect;
            public VectAndDistance(float dis, IntVect2D v)
            {
                distance = dis;
                vect = v;
            }
        }



        Hashtable GetMoveTable(Vector2 position)
        {
            Hashtable table = new Hashtable();
            table.Add("x", position.x);
            table.Add("z", position.y);
            //table.Add("time", 1.0f);
            table.Add("time", animator.moveTime);
            table.Add("easetype", iTween.EaseType.linear);
            table.Add("oncomplete", "CompleteMove");	// トゥイーン終了時にCompleteHandler()を呼ぶ
            return table;
        }
        void CompleteMove()
        {
            base.CompleteMove();
            if (OnCompleteMove != null) OnCompleteMove();
        }


    }
}