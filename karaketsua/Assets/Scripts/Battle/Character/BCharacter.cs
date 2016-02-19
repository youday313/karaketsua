using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    #region::パラメーター
    //属性
    public enum ElementKind {なし,火,水,地,風,雷 }
    //状態異常
    public enum StateError { 炎焼,凍結,裂傷,感電,石化,毒,封印,混乱}
    public enum AttackDistance { 近,中,遠 }
    [System.Serializable]
    public class CharacterParameter
    {
        //キャラ名
        public string charaName;
        //体力
        public int HP;
        //攻撃技関連
        public List<AttackParameter> attackParameter=new List<AttackParameter>();
        //行動力
        public int activeSpeed;
        //攻撃力
        public int power;
        //属性攻撃力
        public List<int> elementPower=new List<int>();
        //防御力
        public int deffence;
        //属性防御力
        public List<int> elementDeffence=new List<int>(); 
        //精神力,MP
        public int skillPoint;
        //知力
        public int intellect;
        //気力,移動攻撃用
        public int movePoint;
        //状態異常
        public List<StateError> stateError=new List<StateError>();

    }

    #endregion::パラメーター

    [RequireComponent(typeof(BCharacterMove))]

    public class BCharacter : MonoBehaviour
    {

        BCharacterMove characterMove;
        BCharacterSingleAttack singleAttack;

        //現在のキャラクター位置配列
        [System.NonSerialized]
        public IntVect2D positionArray = new IntVect2D(0, 0);
        [System.NonSerialized]
        public bool isEnemy = false;

        ///インスペクタから編集
        public CharacterParameter characterParameter;

        // Use this for initialization
        void Start()
        {
            characterMove = GetComponent<BCharacterMove>();
        }





    }

}
