using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

using BattleScene;



namespace BattleScene
{
    public class BStageData : DontDestroySingleton<BStageData>
    {


        public List<PlayerCharacterData> playerCharacters;
		public List<EnemyCharacterData> enemyCharacters;

        [System.Serializable]
        public class CharacterData
        {
            //public BCharacterBase prefab;
            public Vector2 position;
			public string charaName;

        }
        [System.Serializable]
        public class PlayerCharacterData:CharacterData
        {
            public BCharacterPlayer prefab;
        }

        [System.Serializable]
        public class EnemyCharacterData:CharacterData
        {
            public BCharacterEnemy prefab;
        }

		public void AddCharacter(){

		}

		public void ResetEnemy(){
			enemyCharacters = new List<EnemyCharacterData> ();

		}



    }

	public class BStageEnemyData{
		public List<BEnemySingleData> enemys = new List<BEnemySingleData> ();




	}
	public class BEnemySingleData{
		[XmlElement("X")]
		public int x;
		[XmlElement("Y")]
		public int y;
		[XmlElement("Name")]
		public string charaName;
	}


}
