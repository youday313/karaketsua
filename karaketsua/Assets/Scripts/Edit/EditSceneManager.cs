using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using BattleScene;

namespace EditScene
{
    /// <summary>
    /// バトル前編成画面
    /// </summary>
    public class EditSceneManager: MonoBehaviour
    {

        [SerializeField]
        private ECharacterIcon characterIconNode;

        void Start()
        {
            // キャラクターデータ読み込み
            loadPlayerCharacters();
            // エネミーデータ読み込み
            loadEnemyCharacters();
        }

        /// <summary>
        /// プレイヤーキャラ生成
        /// </summary>
        private void loadPlayerCharacters()
        {
            // 味方取得
            var allPlayers = MasterDataLoader.Instance.LoadPlayerCharacters();
            var selectPlayerIds = PlayerGameData.Instance.selectPlayerChatacterIds;
            var playerPositions = PlayerGameData.Instance.battlePlayerPosition;
            // IDが存在するキャラのみ生成
            foreach(var chara in allPlayers) {
                var icon = Instantiate(characterIconNode) as ECharacterIcon;
                var prefabName = "Character/ATB/" + chara.charaName;
                var image = Resources.Load<Sprite>(prefabName);
                icon.GetComponent<Image>().sprite = image;
                icon.Initialize(playerPositions[chara.id]);
            }
        }

        /// <summary>
        /// 敵キャラ生成
        /// </summary>
        private void loadEnemyCharacters()
        {
            // 敵取得
            var allEnemys = MasterDataLoader.Instance.LoadEnemyCharacters();
            var selectEnemyIds = PlayerGameData.Instance.selectEnemyCharacterIds;
            var enemyPositions = PlayerGameData.Instance.battleEnemyPosition;
            // 敵生成
            var count = 0;
            foreach(var chara in allEnemys.Where(x => selectEnemyIds.Contains(x.id))) {
                var icon = Instantiate(characterIconNode) as ECharacterIcon;
                var prefabName = "Character/ATB/" + chara.charaName;
                var image = Resources.Load<Sprite>(prefabName);
                icon.GetComponent<Image>().sprite = image;
                // TODO:敵の出現の仕方
                // 今は奥から左詰め
                var position = new IntVect2D(-BBattleStage.stageSizeX + count, -BBattleStage.stageSizeY);
                icon.Initialize(position);
                count++;
            }
        }
    }
}