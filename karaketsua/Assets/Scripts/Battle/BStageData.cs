using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BattleScene;

namespace BattleScene
{
    public class BStageData : DontDestroySingleton<BStageData>
    {

        public List<CharacterData> playerCharacters;
        public List<CharacterData> enemyCharacters;

        [System.Serializable]
        public class CharacterData
        {
            public BCharacterPlayer prefab;
            public Vector2 position;
        }
    }

}