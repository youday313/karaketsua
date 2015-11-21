using UnityEngine;
using System.Collections;

public class SkillTile : MonoBehaviour {

	bool isStartTile=false;
    bool isEndTile = false;
    //ひとつ前のタイル
    SkillTile preSkillTile;

    SkillTileWave skillTileWave;

    [System.NonSerialized]
    public bool isInputed = false;

	IntVect2D arrayPosition = new IntVect2D (IntVect2D.nullNumber, IntVect2D.nullNumber);//配列番号

	// Use this for initialization
	void Start () {
	
	}
	public void Init(SkillTileWave _skillTileWave,IntVect2D _arrayPosition,bool _isStart,bool _isEnd,SkillTile _preSkillTile){
        skillTileWave = _skillTileWave;
        arrayPosition = _arrayPosition;
        isStartTile = _isStart;
        isEndTile = _isEnd;
        preSkillTile = _preSkillTile;
        
        UpdatePosition ();
        RegistTouchEvent();

	}
	void UpdatePosition(){
		var realX = transform.localScale.x * (arrayPosition.x - BattleStage.stageSizeX);
		var realZ = transform.localScale.z * (arrayPosition.y - BattleStage.stageSizeY);

		transform.position = new Vector3 (realX,0,realZ);

	}

	// Update is called once per frame
	void Update () {
	
	}
    void RegistTouchEvent()
    {
        if (isStartTile == true)
        {
            IT_Gesture.onShortTapE += OnShortTap;
        }
        if (isStartTile != false && isEndTile != false)
        {
            IT_Gesture.onDraggingE += OnDragging;
        }

        if (isEndTile == true)
        {
            IT_Gesture.onChargeEndE += OnChargeEnd;
        }
    }
    void OnDisable()
    {

    }

    //開始
    void OnShortTap(Vector2 pos)
    {
        var tile = GetSkillTileFromRay(pos);
        if (tile == null) return;
        if (tile != this) return;
        isInputed = true;

    }
    //移動中
    void OnDragging(DragInfo dragInfo)
    {
        //判定が終了していない
        if (isInputed == true) return;
        //ひとつ前の判定が終了している
        if (preSkillTile.isInputed == false) return;

        var tile = GetSkillTileFromRay(dragInfo.pos);
        //別のところを押している
        if (tile == null || (tile != preSkillTile && tile!=this))
        {
            skillTileWave.MissDrag();
        }

        isInputed = true;

    }
    //終了
    void OnChargeEnd(ChargedInfo cInfo)
    {
        //判定が終了していない
        if (isInputed == true) return;
        //ひとつ前の判定が終了している
        if (preSkillTile.isInputed == false) return;

        var tile = GetSkillTileFromRay(cInfo.pos);
        if (tile == null) return;
        if (tile != this) return;
        isInputed = true;
    }
    #region::Utility
    //クリックしたタイル位置を取得
    IntVect2D GetArrayFromRay(Vector2 touchPosition)
    {
        RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス 
        Ray ray;  // 光線クラス

        // スクリーン座標に対してマウスの位置の光線を取得
        ray = Camera.main.ScreenPointToRay(touchPosition);
        // マウスの光線の先にオブジェクトが存在していたら hit に入る 
        //Tileのlayer番号は8
        var layerMask = 1 << 8;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.tag == "SkillTile")
            {
                // 当たったオブジェクトのTileBaseクラスを取得
                return hit.collider.GetComponent<SkillTile>().arrayPosition;

            }
        }
        return null;
    }
    SkillTile GetSkillTileFromRay(Vector2 touchPosition)
    {
        RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス 
        Ray ray;  // 光線クラス

        // スクリーン座標に対してマウスの位置の光線を取得
        ray = Camera.main.ScreenPointToRay(touchPosition);
        // マウスの光線の先にオブジェクトが存在していたら hit に入る 
        //Tileのlayer番号は8
        var layerMask = 1 << 8;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.tag == "SkillTile")
            {
                // 当たったオブジェクトのTileBaseクラスを取得
                return hit.collider.GetComponent<SkillTile>();

            }
        }
        return null;
    }
    #endregion:Utility
}
