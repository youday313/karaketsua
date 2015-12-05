//CameraMove
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
	//public
	[System.Serializable]
	public class CameraVector{

		public Vector3 position;
		public Vector3 rotation;
	}
    [Tooltip("キャラクター選択時")]
	public CameraVector backCamera;
    [Tooltip("ActiveTime稼働時")]
	public CameraVector leanCamera;
    [Tooltip("攻撃時")]
    public CameraVector attackCamera;


	//private
    public float changeTime=1f;
    bool isTurn = false;
    bool isAttack = false;

    CameraVector resetPosition=new CameraVector();
    [SerializeField]
    Button resetButton;

	void Start ()
	{
		MoveToLean ();
        SetCameraVector();
	}

    void SetCameraVector()
    {
        resetPosition.position = transform.position;
        resetPosition.rotation = transform.eulerAngles;
    }


    void Update()
    {
    }
    void OnEnable()
    {
        IT_Gesture.onDraggingE += OnDraggingInAttackMode;

        IT_Gesture.onDraggingStartE += OnDraggingStartInActionMode;
        IT_Gesture.onDraggingE += OnDraggingInActionModeForCameraMove;
    }

    void OnDisable()
    {
        IT_Gesture.onDraggingE -= OnDraggingInAttackMode;
        IT_Gesture.onDraggingStartE -= OnDraggingStartInActionMode;
        IT_Gesture.onDraggingE -= OnDraggingInActionModeForCameraMove;
    }
    //攻撃時カメラ追従
    void OnDraggingInAttackMode(DragInfo dragInfo)
    {
        if (isAttack == false) return;
        //x方向
        var moveVect=GetMoveDirection(dragInfo.delta);
        CSTransform.SetX(transform,transform.position.x+ moveVect.x);
        CSTransform.SetZ(transform, transform.position.z + moveVect.z);
        
    }


    #region::行動選択時カメラ移動
    void OnDraggingStartInActionMode(DragInfo dragInfo)
    {
        resetPosition.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        resetPosition.rotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
    }

    //ドラッグでカメラ移動
    void OnDraggingInActionModeForCameraMove(DragInfo dragInfo)
    {
        var delta = dragInfo.delta *IT_Gesture.GetDPIFactor();
        transform.position += new Vector3(delta.x, 0, delta.y);
    }

    public void ResetCamera()
    {
        transform.position = resetPosition.position;
        transform.rotation = Quaternion.Euler(resetPosition.rotation);
    }

    #endregion
    Vector3 GetMoveDirection(Vector2 delta)
    {
        //x方向
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            
            delta*=IT_Gesture.GetDPIFactor();
            return new Vector3(delta.x,0, 0);

        }
        //y方向
        else
        {
            delta *= IT_Gesture.GetDPIFactor();
            return new Vector3(0,0,delta.y);
        }
    }
	//キャラクター背面に移動
    public void MoveToBack(Vector3 charaPosition)
    {

        iTween.MoveTo(gameObject, iTween.Hash("x", charaPosition.x + backCamera.position.x, "y", charaPosition.y + backCamera.position.y, "z", charaPosition.z + backCamera.position.z, "time", changeTime));
		iTween.RotateTo(gameObject, iTween.Hash("x", backCamera.rotation.x, "y", backCamera.rotation.y, "z", backCamera.rotation.z, "time", changeTime, "islocal", true));
        
        isTurn = true;
        SetCameraVector();
	}

    //固定点
	public void MoveToLean(){
        iTween.MoveTo(gameObject, iTween.Hash("x", leanCamera.position.x, "y", leanCamera.position.y, "z", leanCamera.position.z, "time", changeTime));
        iTween.RotateTo(gameObject, iTween.Hash("x", leanCamera.rotation.x, "y", leanCamera.rotation.y, "z", leanCamera.rotation.z, "time", changeTime, "islocal", true));
        isTurn = false;
        SetCameraVector();
	}

    //攻撃時
    public void MoveToAttack(Vector3 attackerPosition,Vector3 targetPosition)
    {
        var centerPosition = (targetPosition - attackerPosition) / 2;
        var newPosition = attackerPosition + centerPosition;
        iTween.MoveTo(gameObject, iTween.Hash("x", newPosition.x + attackCamera.position.x, "y", newPosition.y + attackCamera.position.y, "z", newPosition.z + attackCamera.position.z, "time", changeTime));
        iTween.RotateTo(gameObject, iTween.Hash("x", attackCamera.rotation.x, "y", attackCamera.rotation.y, "z", attackCamera.rotation.z, "time", changeTime, "islocal", true));
        isTurn = false;
    }
    //移動アニメーション作成
    Hashtable SetMoveTable(Vector2 pos,float time)
    {
        Hashtable table = new Hashtable();
        table.Add("x", pos.x+transform.position.x);
        table.Add("z", pos.y + transform.position.z);
        table.Add("time", time);
        table.Add("easetype", iTween.EaseType.linear);
        return table;

    }
    public void FollowCharacter(Vector2 pos,float time)
    {
        var table=SetMoveTable(pos,time);
        iTween.MoveTo(gameObject, table);
    }

    public void SetAttackMoveMode(bool _isAttack)
    {
        isAttack = _isAttack;
    }
    //回転
    //待機画面。アクティブタイム増加中
    public void TurnAroundForWaiting()
    {
        //transform.parent.Rotate(0,1 , 0);
    }

}