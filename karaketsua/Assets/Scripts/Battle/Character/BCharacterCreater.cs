using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace BattleScene
{
    public class BCharacterCreater: MonoBehaviour
    {

        public List<BCharacterBase> CreateBattleCharacters()
        {
            var characters = new List<BCharacterBase>();

            // 味方取得
            var allPlayers = MasterDataLoader.Instance.LoadPlayerCharacters();
            var selectPlayerIds = PlayerGameData.Instance.selectPlayerChatacterIds;
            var playerPositions = PlayerGameData.Instance.battlePlayerPosition;
            // IDが存在するキャラのみ生成
            foreach(var chara in allPlayers.Where(x => selectPlayerIds.Contains(x.id))) {
                var prefabName = "Character/Playable/" + chara.charaName;
                var resources = Resources.Load<BCharacterPlayer>(prefabName);
                var cha = Instantiate(resources) as BCharacterPlayer;
                cha.Initialize(playerPositions[chara.id]);
                characters.Add(cha);
            }

            // 敵取得
            var allEnemys = MasterDataLoader.Instance.LoadEnemyCharacters();
            var selectEnemyIds = PlayerGameData.Instance.selectEnemyCharacterIds;
            var enemyPositions = PlayerGameData.Instance.battleEnemyPosition;

            foreach(var chara in allEnemys.Where(x => selectEnemyIds.Contains(x.id))) {
                var prefabName = "Character/Enemy/" + chara.charaName;
                var resources = Resources.Load<BCharacterEnemy>(prefabName);
                var cha = Instantiate(resources) as BCharacterEnemy;
                cha.Initialize(enemyPositions[chara.id]);
                characters.Add(cha);
            }

            //共通初期化
            foreach(var chara in characters) {
                chara.transform.SetParent(transform);
            }

            return characters;
        }

    }

}