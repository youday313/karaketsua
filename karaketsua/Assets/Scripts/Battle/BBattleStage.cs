//BattleStage
//作成日
//<summary>
//TileBass管理
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    public class BBattleStage : Singleton<BBattleStage>
    {
        public TileBase prefab;

        public static readonly int stageSizeX = 2;
        public static readonly int stageSizeY = 3;

        List<TileBase> tileBases = new List<TileBase>();
        // Use this for initialization
        void Start()
        {
            // 配置元のオブジェクト指定 
            var stageObject = GameObject.FindWithTag("Stage");
            // タイル配置
            for (int i = -stageSizeX; i <= stageSizeX; i++)
            {
                for (int j = -stageSizeY; j <= stageSizeY; j++)
                {

                    Vector3 tile_pos = new Vector3(
                        0 + prefab.transform.localScale.x * i,
                        0,
                        0 + prefab.transform.localScale.z * j

                      );

                    if (prefab != null)
                    {
                        // プレハブの複製 
                        var tile = Instantiate(prefab, tile_pos, Quaternion.identity) as TileBase;
                        tile.Init(new IntVect2D(i, j));

                        // 生成元の下に複製したプレハブをくっつける 
                        tile.transform.parent = stageObject.transform;

                        //リストに格納
                        tileBases.Add(tile);
                    }
                }
            }
            SetEvent();
        }
        void SetEvent()
        {
            BCharacterBase.OnActiveStaticE += OnActiveCharacter;
        }

        //アクティブ時
        public void OnActiveCharacter(BCharacterBase chara)
        {
            ResetAllTileColor();
        }

        public void OnMoverable(IntVect2D position)
        {
            //ChangeColor(position, TileState.Active, true);
            ChangeNeighborTilesColor(position, TileState.Move);
        }

        //攻撃選択
		public void OnSlectWaza(BCharacterBase chara,SingleAttackParameter selectWaza)
        {
            ResetAllTileColor();
            //攻撃範囲取得
            foreach(var range in selectWaza.attackRanges){
                var pos = IntVect2D.Clone(chara.positionArray);
                pos = IntVect2D.Add(pos, range);
                ChangeColor(pos, TileState.Attack);
            }
        }

        //ターゲット選択
        //移動攻撃も同様
        public void OnSelecSkillTarget(IntVect2D targetPosition)
        {
            ChangeColor(targetPosition, TileState.Skill);
        }
        //ターゲット解除
        public void OnCancelSkillTarget(IntVect2D targetPosition)
        {
            ChangeColor(targetPosition, TileState.Default);
        }
        
        


        //ゲット関連
        //タイルを返す
        public TileBase GetTile(IntVect2D position)
        {
            return tileBases.Where(t => position.IsEqual(t.positionArray)).FirstOrDefault();
        }
        //引数はスクリーン座標
        public TileBase GetTileFromScreenPostion(Vector2 touchPosition)
        {
            RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス 
            Ray ray;  // 光線クラス

            // スクリーン座標に対してマウスの位置の光線を取得
			var camera=FindObjectOfType<Camera>();
            //var camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            ray = camera.ScreenPointToRay(touchPosition);
            // マウスの光線の先にオブジェクトが存在していたら hit に入る
            //Tileのlayer番号は8
            var layerMask = 1 << 8;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.tag == "Tile")
                {
                    // 当たったオブジェクトのTileBaseクラスを取得
                    return hit.collider.GetComponent<TileBase>();
                }
            }
            return null;
        }

        //タイルのVect2Dを返す
        public IntVect2D GetTilePosition(IntVect2D position)
        {
            var tile = GetTile(position);
            if (tile == null) return null;
            return GetTile(position).positionArray;
        }
        //引数はスクリーン座標
        public IntVect2D GetTilePositionFromScreenPosition(Vector2 touchPosition)
        {
            var tile = GetTileFromScreenPostion(touchPosition);
            if (tile == null) return null;
            return tile.positionArray;
        }

        //実座標のxとzを返す
        public Vector2 GetTileXAndZPosition(IntVect2D position)
        {
            var tile = GetTile(position);
            return new Vector2(tile.transform.position.x, tile.transform.position.z);

        }

        public List<TileBase> GetVerticalHorizontalTiles(IntVect2D position)
        {

            return GetTilesFormDistance(position, 1f);
        }
        public List<TileBase> GetTilesFormDistance(IntVect2D position, float distance)
        {
            return tileBases.Where(t =>
            (Vector2.Distance(new Vector2(t.positionArray.x, t.positionArray.y), new Vector2(position.x, position.y)) <= distance)
            && !(position.IsEqual(t.positionArray))).ToList();
        }


        //色変更
        public void ResetAllTileColor()
        {
            foreach (var tile in tileBases)
            {
                tile.ChangeColor(TileState.Default);
            }
        }
        public void ChangeColor(IntVect2D position, TileState state, bool reset = false)
        {

            if (reset) ResetAllTileColor();
            var tile = GetTile(position);
            if (tile == null)
                return;
            tile.ChangeColor(state);


        }
        //上下左右のタイル色変更
        public void ChangeNeighborTilesColor(IntVect2D position, TileState toState, bool reset = false)
        {
            if (reset) ResetAllTileColor();
            foreach (var tile in GetVerticalHorizontalTiles(position))
            {
                tile.ChangeColor(toState);
            }

        }
        public void ChangeTilesColorFromDistance(IntVect2D position, TileState toState, float distance, bool reset = false)
        {
            if (reset) ResetAllTileColor();
            foreach (var tile in GetTilesFormDistance(position, distance))
            {
                tile.ChangeColor(toState);
            }
        }

    }
}