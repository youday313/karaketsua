//CameraMove
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CameraMove : Singleton<CameraMove>
{
	//public
	[System.Serializable]
	public class CameraVector{

		public Vector3 position;
		public Vector3 rotation;
	}
	public CameraVector backCamera;
	public CameraVector leanCamera;
	//private
    public float changeTime=1f;
    bool isTurn = false;

	void Start ()
	{
		MoveToLean ();
	}
	


    void Update()
    {
        //if (isTurn)
        //{
        //    TurnAroundForWaiting();
        //}
    }

	//背面に移動
	public void MoveToBack(Character chara){

        iTween.MoveTo(gameObject, iTween.Hash("x", chara.transform.position.x+backCamera.position.x, "y", chara.transform.position.y + backCamera.position.y, "z", chara.transform.position.z + backCamera.position.z, "time", changeTime));
		iTween.RotateTo(gameObject, iTween.Hash("x", backCamera.rotation.x, "y", backCamera.rotation.y, "z", backCamera.rotation.z, "time", changeTime, "islocal", true));
        
        isTurn = true;
	}


	public void MoveToLean(){

        iTween.MoveTo(gameObject, iTween.Hash("x", leanCamera.position.x, "y", leanCamera.position.y, "z", leanCamera.position.z, "time", changeTime));
        iTween.RotateTo(gameObject, iTween.Hash("x", leanCamera.rotation.x, "y", leanCamera.rotation.y, "z", leanCamera.rotation.z, "time", changeTime, "islocal", true));
        isTurn = false;
	}
    //移動アニメーション作成
    Hashtable SetMoveTable()
    {
        Hashtable table = new Hashtable();
        table.Add("x", leanCamera.position.x);
        table.Add("z", leanCamera.position.z);
        table.Add("time", changeTime);
        table.Add("easetype", iTween.EaseType.linear);
        table.Add("oncomplete", "CompleteMove");	// トゥイーン終了時にCompleteHandler()を呼ぶ
        return table;

    }
    //回転
    //待機画面。アクティブタイム増加中
    public void TurnAroundForWaiting()
    {
        transform.LookAt(new Vector3(0,0,0));
        transform.Rotate(new Vector3(0, 0, 0), Time.deltaTime);
    }

}