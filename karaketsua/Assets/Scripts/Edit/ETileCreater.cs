using UnityEngine;
using System.Collections;

using EditScene;

namespace EditScene
{

    public class ETileCreater: MonoBehaviour
    {

        [SerializeField]
        private ETile tilePrefab;

        void Start()
        {

            for(int j = -BattleScene.BBattleStage.stageSizeY; j <= BattleScene.BBattleStage.stageSizeY; j++) {
                for(int i = -BattleScene.BBattleStage.stageSizeX; i <= BattleScene.BBattleStage.stageSizeX; i++) {
                    if(tilePrefab != null) {
                        // プレハブの複製 
                        var tile = Instantiate(tilePrefab) as ETile;
                        tile.Init(new IntVect2D(i, j), j > 0, this);

                        // 生成元の下に複製したプレハブをくっつける
                        tile.transform.SetParent(gameObject.transform, false);
                        tile.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}