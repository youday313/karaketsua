using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using EditScene;

namespace EditScene
{

    public class ETileManager: SingletonMonoBehaviour<ETileManager>
    {

        [SerializeField]
        private ETile tilePrefab;

        public List<ETile> Tiles = new List<ETile>();

        public void Initialize()
        {
            for(int j = -BattleScene.BBattleStage.stageSizeY; j <= BattleScene.BBattleStage.stageSizeY; j++) {
                for(int i = -BattleScene.BBattleStage.stageSizeX; i <= BattleScene.BBattleStage.stageSizeX; i++) {
                    if(tilePrefab != null) {
                        // プレハブの複製 
                        var tile = Instantiate(tilePrefab) as ETile;
                        tile.Initialize(new IntVect2D(i, j), j > 0);

                        // 生成元の下に複製したプレハブをくっつける
                        tile.transform.SetParent(gameObject.transform, worldPositionStays: false);
                        tile.gameObject.SetActive(true);
                        Tiles.Add(tile);
                    }
                }
            }
        }

        // 指定点のタイルを取得
        public ETile GetTile(Vector2 pos)
        {
            return Tiles.Where(t => t.IsContain(pos)).FirstOrDefault();
        }
    }
}