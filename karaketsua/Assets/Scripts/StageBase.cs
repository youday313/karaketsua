//StageBase
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StageBase : MonoBehaviour
{
	//public

	//private
    public Character selectCharacter;

	void Start ()
	{
		
	}

    void Update()
    {
        UpdateInput();
        UpdateRay();
    }

    void UpdateInput()
    {
#if UNITY_EDITOR
        // mimic the touch event on Desktop platform
        if (Input.GetMouseButtonDown(0))
        {
            var f = new Vect2D<float>();
            f.x = Input.mousePosition.x;
            f.y = Input.mousePosition.y;

            HandleTouchBegan(f);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            var f = new Vect2D<float>();
            f.x = Input.mousePosition.x;
            f.y = Input.mousePosition.y;

            HandleTouchEnded(f);
        }
        else if (Input.GetMouseButton(0))
        {
            var f = new Vect2D<float>();
            f.x = Input.mousePosition.x;
            f.y = Input.mousePosition.y;

            HandleTouchMoved(f);
        }
#elif UNITY_IPHONE || UNITY_ANDROID
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);
			
			var f = new Vect2D<float>();
			f.x = touch.position.x;
			f.y = touch.position.y;
			
			if (touch.phase == TouchPhase.Began)
				HandleTouchBegan(f);
			if (touch.phase == TouchPhase.Moved)
				HandleTouchMoved(f);
			if (touch.phase == TouchPhase.Ended)
				HandleTouchEnded(f);
			if (touch.phase == TouchPhase.Canceled)
				HandleTouchCanceled(f);
		}
#else
		// mimic the touch event on Desktop platform
		if (Input.GetMouseButtonDown(0)) {
			var f = new Vect2D<float>();
			f.x = Input.mousePosition.x;
			f.y = Input.mousePosition.y;
			
			HandleTouchBegan(f);
		} else if (Input.GetMouseButtonUp(0)) {
			var f = new Vect2D<float>();
			f.x = Input.mousePosition.x;
			f.y = Input.mousePosition.y;
			
			HandleTouchEnded(f);
		} else if (Input.GetMouseButton(0)) {
			var f = new Vect2D<float>();
			f.x = Input.mousePosition.x;
			f.y = Input.mousePosition.y;
			
			HandleTouchMoved(f);
		}
#endif
    }
    #region ::Touch Handlers::
    public void HandleTouchBegan(Vect2D<float> f)
    {
        RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス 
        Ray ray;  // 光線クラス

        // スクリーン座標に対してマウスの位置の光線を取得
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // マウスの光線の先にオブジェクトが存在していたら hit に入る 
        //Tileのlayer番号は8
        var layerMask = 1 << 8;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {

            if (hit.collider.tag == "Tile")
            {
                // 当たったオブジェクトのTileBaseクラスを取得
                TileBase tile_base = hit.collider.GetComponent<TileBase>();
                //タイルの配列番号取得
                var tileArray = tile_base.positionArray;
                //タイル上のキャラクター取得
                var select=GameObject.FindGameObjectsWithTag("PlayerCharacter")
                    .Where(c => c.GetComponent<Character>().positionArray.x == tileArray.x && c.GetComponent<Character>().positionArray.y == tileArray.y).FirstOrDefault(); 
                if (select != null)
                {
                    selectCharacter = select.GetComponent<Character>();
                    selectCharacter.isSelect = true;
                    //移動可能距離を示す
                    //絶対値の和がcountより小さいところが移動可能
                    var movableTiles = GameObject.FindGameObjectsWithTag("Tile")
                        .Where(c => Mathf.Abs(c.GetComponent<TileBase>().positionArray.x - tileArray.x) + Mathf.Abs(c.GetComponent<TileBase>().positionArray.y - tileArray.y) <= selectCharacter.movableCount)
                        .Select(c => c.GetComponent<TileBase>());
                    foreach (var tile in movableTiles)
                    {
                        tile.isMovableState = true;
                    }

                }
            }

        }
    }

    public void HandleTouchMoved(Vect2D<float> f)
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(f.x, f.y));
        //var moveVect = BlockPlaceManager.ChangeToIndex(touchPosition);
        //動かした
        //if (touchBlockVect.x != moveVect.x || touchBlockVect.y != moveVect.y)
        //{
        //    isTouchMove = true;
        //}
        ////動かした後元に戻した時用
        //else if (isTouchMove == true)
        //{
        //    isTouchMove = false;
        //}

    }

    public void HandleTouchEnded(Vect2D<float> f)
    {
        RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス 
        Ray ray;  // 光線クラス

        // スクリーン座標に対してマウスの位置の光線を取得
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // マウスの光線の先にオブジェクトが存在していたら hit に入る 
        //Tileのlayer番号は8
        var layerMask = 1 << 8;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {

            if (hit.collider.tag == "Tile")
            {
                // 当たったオブジェクトのTileBaseクラスを取得
                TileBase tile_base = hit.collider.GetComponent<TileBase>();
                //タイルの配列番号取得
                var tileArray = tile_base.positionArray;

                //キャラクターを選択していた場合

                if (selectCharacter != null)
                {
                    //移動可能な場所
                    if (Mathf.Abs(selectCharacter.positionArray.x - tileArray.x) + Mathf.Abs(selectCharacter.positionArray.y - tileArray.y) <= selectCharacter.movableCount)
                    {
                        //移動処理
                        //座標移動
                        selectCharacter.Move(tile_base.transform.position);
                        
                        
                        //selectCharacter.transform.position = new Vector3(tile_base.transform.position.x, selectCharacter.transform.position.y, tile_base.transform.position.z);
                        //配列番号変更
                        selectCharacter.positionArray = new Vect2D<int>(tileArray.x, tileArray.y);
                        
                    }
                    selectCharacter.isSelect = false;
                    foreach (var tile in GameObject.FindGameObjectsWithTag("Tile").Select(c=>c.GetComponent<TileBase>()))
                    {
                        tile.isMovableState = false;
                    }
                    selectCharacter = null;
                }

            }

        }
    }

    public void HandleTouchCanceled(Vect2D<float> f)
    {
    }
    #endregion

    void UpdateRay()
    {
        RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス 
        Ray ray;  // 光線クラス

        // スクリーン座標に対してマウスの位置の光線を取得
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // マウスの光線の先にオブジェクトが存在していたら hit に入る 
        //Tileのlayer番号は8
        var layerMask = 1 << 8;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {

            if (hit.collider.tag == "Tile")
            {
                // 当たったオブジェクトのTileBaseクラスを取得
                TileBase tile_base = hit.collider.GetComponent<TileBase>();
                tile_base.bColorState = true;
            }

        }
    }
}