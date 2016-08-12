using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;
using System;

namespace BattleScene
{
    [Serializable]
    public class CharacterMasterParameter
    {
        //キャラ名
        public string charaName;
        //体力
        public int hp;
        //行動力
        public int activeSpeed;
        //攻撃力
        public int power;
        //防御力
        public int deffence;
        //精神力,MP
        public int skillPoint;

        //攻撃技関連
        public List<SingleAttackParameter> singleAttackParameters = new List<SingleAttackParameter>();

        //移動攻撃
        public MoveAttackParameter moveAttackParameter = null;

        //自動攻撃
        public AutoAttackParameter autoAttackParameter = new AutoAttackParameter();


    }

    //一つの攻撃のパラメーター
    [System.Serializable]
    public class AttackParameter
    {
        //技名
        public string wazaName;

        //使用MP
        public int needSkillPoint;

        //攻撃倍率
        public float powerMagnification;


    }
    [System.Serializable]
    public class SingleAttackParameter : AttackParameter
    {
        //攻撃範囲
        public List<IntVect2D> attackRanges;

        //攻撃種類
        public int attackDistance;

        //範囲内の全ての敵にダメージ
        public bool isMultiAttack;

        //タップアクション
        //リスト数＝タップ回数
        public List<SingleActionParameter> actionParameters = null;

        public AttackDistance GetAttackDistance()
        {
            return (AttackDistance)attackDistance;
        }

    }

    [System.Serializable]
    public class SingleActionParameter
    {
        public float judgeTime;
        public float startInterval;
        public GameObject attackMakerPrefab;
    }



    [System.Serializable]
    public class AutoAttackParameter : AttackParameter
    {
        public List<IntVect2D> attackRanges;
        public int attackDistance;

        public AttackDistance GetAttackDistance()
        {
            return (AttackDistance)attackDistance;
        }
    }

    [System.Serializable]
    public class MoveAttackParameter:AttackParameter
    {
        //移動可能距離
        public int moveRange;
        public int needMovePoint;
    }

    public enum AttackDistance { 近, 中, 遠 }

    public enum EnemyState {Wait, Active,MoveStart,Moving,Moved,AttackStart,Attacking,Attacked,End}
}
