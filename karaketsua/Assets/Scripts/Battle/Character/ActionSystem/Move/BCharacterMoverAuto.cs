using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    public class BCharacterMoverAuto : BCharacterMoverBase
    {


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


        public void StartEnemyMove()
        {
            RandamMove();
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
                if (isDone == true) break;
            }
            isDone = true;
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
        }


    }
}