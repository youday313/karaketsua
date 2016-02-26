using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{

    //状態異常
    public enum StateError { 炎焼, 凍結, 裂傷, 感電, 石化, 毒, 封印, 混乱 }
    [System.Serializable]
    public class CharacterParameter
    {
        //キャラ名
        public string charaName;
        //体力
        public int HP;
        //攻撃技関連
        public List<SingleAttackParameter> singleAttackParameter = new List<SingleAttackParameter>();
        //移動攻撃
        public MoveAttackParameter moveAttackParameter=null;
        //行動力
        public int activeSpeed;
        //攻撃力
        public int power;
        //属性攻撃力
        public List<int> elementPower = new List<int>();
        //防御力
        public int deffence;
        //属性防御力
        public List<int> elementDeffence = new List<int>();
        //精神力,MP
        public int skillPoint;
        //知力
        public int intellect;
        //気力,移動攻撃用
        public int movePoint;
        //状態異常
        public List<StateError> stateError = new List<StateError>();

    }

    //一つの攻撃のパラメーター
    [System.Serializable]
    public class AttackParameter
    {
        //技名
        public string wazaName;

        public ElementKind element;
        //範囲攻撃

        //使用MP
        public int needSkillPoint;

    }
    [System.Serializable]
    public class SingleAttackParameter : AttackParameter
    {
        //攻撃範囲
        public List<IntVect2D> attackRange;

        //攻撃種類
        public AttackDistance attackDistance;

        //範囲内の全ての敵にダメージ
        public bool isMultiAttack;

        //タップアクション
        //リスト数＝タップ回数
        public List<SingleActionParameter> actionParameter = null;

    }

    [System.Serializable]
    public class SingleActionParameter
    {
        public float judgeTime;
        public float startInterval;
        public GameObject attackMakerPrefab;
        //public AttackEffectKind attackEffectKind;
        public float power;
    }

    [System.Serializable]
    public class MoveAttackParameter:AttackParameter
    {
        //技の威力
        public int power;
        //移動可能距離
        public int moveRange;
    }

    public enum AttackDistance { 近, 中, 遠 }
    //属性
    public enum ElementKind { なし, 火, 水, 地, 風, 雷 }

}
