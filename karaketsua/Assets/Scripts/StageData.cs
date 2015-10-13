using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageData : DontDestroySingleton<StageData> {

    public List<CharacterData> playerCharacters;
    public List<CharacterData> enemyCharacters;

    [System.Serializable]
    public class CharacterData
    {
        public Character prefab;
        public Vector2 position;
    }
}
