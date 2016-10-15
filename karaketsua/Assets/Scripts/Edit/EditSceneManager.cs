using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

using BattleScene;

namespace EditScene
{
    // バトル前編成画面
    public class EditSceneManager: MonoBehaviour
    {
        [SerializeField]
        private ECharacterIcon characterIconNode;
        [SerializeField]
        private Transform iconParent;
        [SerializeField]
        private EBattleStartButton decideButton;

        void Start()
        {
            // タイル作成
            ETileManager.Instance.Initialize();
            // キャラクターデータ読み込み
            var icons = new List<ECharacterIcon>();
            var players = loadPlayerCharacters();
                icons.AddRange(players);
            // エネミーデータ読み込み
            var enemys = loadEnemyCharacters();
            icons.AddRange(enemys);

            decideButton.Initialize(players, () => {
                PlayerGameData.Instance.ResetPositionList();
                foreach(var icon in icons) {
                    icon.Decide();
                }
                SceneManager.Instance.LoadScene(Scene.Battle);
            });
        }

        // プレイヤーキャラ生成
        private List<ECharacterIcon> loadPlayerCharacters()
        {
            // 味方取得
            var allPlayers = MasterDataLoader.Instance.LoadPlayerCharacters();
            var selectPlayerIds = PlayerGameData.Instance.SelectPlayerChatacterIds;
            var playerPositions = PlayerGameData.Instance.BattlePlayerPosition;
            // IDが存在するキャラのみ生成
            var icons = new List<ECharacterIcon>();
            foreach(var chara in allPlayers) {
                var icon = Instantiate(characterIconNode) as ECharacterIcon;
                icons.Add(icon);
                icon.transform.SetParent(iconParent, worldPositionStays: false);
                icon.Initialize(chara, playerPositions[chara.id], isPlayer:true);
                icon.MoveOnTile();
            }
            return icons;
        }

        // 敵キャラ生成
        private List<ECharacterIcon> loadEnemyCharacters()
        {
            // 敵取得
            var allEnemys = MasterDataLoader.Instance.LoadEnemyCharacters();
            var selectEnemyIds = PlayerGameData.Instance.SelectEnemyCharacterIds;
            var enemyPositions = PlayerGameData.Instance.BattleEnemyPosition;
            // 敵生成
            var icons = new List<ECharacterIcon>();
            foreach(var chara in allEnemys.Where(x => selectEnemyIds.Contains(x.id))) {
                var icon = Instantiate(characterIconNode) as ECharacterIcon;
                icons.Add(icon);
                icon.transform.SetParent(iconParent, worldPositionStays: false);
                icon.Initialize(chara, enemyPositions[chara.id], isPlayer:false);
                icon.MoveOnTile();
            }
            return icons;
        }
    }
}