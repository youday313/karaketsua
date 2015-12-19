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
    enum CameraState
    {
        Back,
        Lean,
        Attack,
        Moving
    }
    CameraState nowCameraState;
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
    //攻撃によってカメラ位置が変わる
    public List<CameraVector> attackCamera=new List<CameraVector>();

    Character activeCharacter;
    CameraMode nowCameraMode=CameraMode.FromBack;
	//private
    public float changeTime=1f;

    CameraVector resetPosition=new CameraVector();

    public Button cameraResetButton;
    public Button cameraChangeButton;

    #region::初期化
    void Start ()
	{
		MoveToLean ();
        SetResetCameraVector();
        activeCharacter = null;
	}

    void SetResetCameraVector()
    {
        resetPosition.position = transform.position;
        resetPosition.rotation = transform.eulerAngles;
    }


    void OnEnable()
    {
        //IT_Gesture.onDraggingE += OnDraggingInAttackMode;
        IT_Gesture.onDraggingStartE += OnDraggingStartInActionMode;
        IT_Gesture.onDraggingE += OnDraggingInActionModeForCameraMove;
    }

    void OnDisable()
    {
        //IT_Gesture.onDraggingE -= OnDraggingInAttackMode;
        IT_Gesture.onDraggingStartE -= OnDraggingStartInActionMode;
        IT_Gesture.onDraggingE -= OnDraggingInActionModeForCameraMove;
    }
    #endregion::初期化
    //攻撃時カメラ追従
    void OnDraggingInAttackMode(DragInfo dragInfo)
    {
        if (activeCharacter.IsNowAction() == false) return;
        //x方向
        var moveVect=GetMoveDirection(dragInfo.delta);
        CSTransform.SetX(transform,transform.position.x- moveVect.x);
        CSTransform.SetZ(transform, transform.position.z - moveVect.y);
        
    }


    public Vector2 GetMoveDirection(Vector2 delta)
    {
        //x方向
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            
            delta*=IT_Gesture.GetDPIFactor();
            return new Vector2(GetSignFromFrontMode()*delta.x,0);

        }
        //y方向
        else
        {
            delta *= IT_Gesture.GetDPIFactor();
            return new Vector3(0,GetSignFromFrontMode()*delta.y);
        }
    }

    #region::行動選択時カメラ移動
    void OnDraggingStartInActionMode(DragInfo dragInfo)
    {
        //SetResetCameraVector();
    }

    //ドラッグでカメラ移動
    void OnDraggingInActionModeForCameraMove(DragInfo dragInfo)
    {
        if (nowCameraState != CameraState.Back) return;
        if (activeCharacter.IsNowOnFinger()== true) return;
        var delta = dragInfo.delta *IT_Gesture.GetDPIFactor();
        transform.position -= new Vector3(GetSignFromFrontMode() * delta.x, 0, GetSignFromFrontMode() * delta.y);
        ClampOnFromFront();
    }

    public void ResetCamera()
    {
        if (nowCameraState != CameraState.Back && nowCameraState != CameraState.Lean) return;
        transform.position = resetPosition.position;
        transform.eulerAngles = resetPosition.rotation;
    }
    void ClampOnFromFront()
    {
        if (nowCameraMode == CameraMode.FromBack)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3, 3), transform.position.y, Mathf.Clamp(transform.position.z, -14, -2));
        }
        else if (nowCameraMode == CameraMode.FromFront)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3, 3), transform.position.y, Mathf.Clamp(transform.position.z, 2, 14));
        }
    }

    #endregion::行動選択時カメラ移動


    public void SetActiveCharacter(Character _activeCharacter)
    {
        activeCharacter = _activeCharacter;
        MoveToBack();

    }
    public void DisableActiveCharacter()
    {
        activeCharacter = null;
        MoveToLean();
    }

	//キャラクター背面に移動
    public void MoveToBack()
    {
        Vector3 charaPosition = activeCharacter.transform.position;
        //SetResetCameraVector();
        iTween.MoveTo(gameObject, iTween.Hash("x", charaPosition.x + backCamera.position.x, "y", charaPosition.y + backCamera.position.y, "z", charaPosition.z + GetSignFromFrontMode() * backCamera.position.z,
            "time", changeTime, "oncomplete", "OnCompleteMove", "oncompletetarget", gameObject, "oncompleteparams", CameraState.Back));
        iTween.RotateTo(gameObject, iTween.Hash("x", backCamera.rotation.x, "y", GetInverseRotationFromFrontMode() + backCamera.rotation.y, "z", backCamera.rotation.z, "time", changeTime, "islocal", true));
        nowCameraState = CameraState.Moving;
        SetCameraButtonActivity(false);
        
	}
    

    //固定点
	public void MoveToLean(){
        iTween.MoveTo(gameObject, iTween.Hash("x", leanCamera.position.x, "y", leanCamera.position.y, "z", GetSignFromFrontMode() * leanCamera.position.z,
            "time", changeTime, "oncomplete", "OnCompleteMove", "oncompletetarget", gameObject, "oncompleteparams", CameraState.Lean));
        iTween.RotateTo(gameObject, iTween.Hash("x", leanCamera.rotation.x, "y", GetInverseRotationFromFrontMode() + leanCamera.rotation.y, "z", leanCamera.rotation.z, "time", changeTime, "islocal", true));
        //SetResetCameraVector();
        nowCameraState = CameraState.Moving;
        SetCameraButtonActivity(false);
	}

    //攻撃時
    public void MoveToAttack(Character attackCharacter,Vector3 targetPosition)
    {
        var centerPosition = (targetPosition - attackCharacter.transform.position) / 2;
        var newPosition = attackCharacter.transform.position + centerPosition;

        switch(attackCharacter.characterParameter.attackDistance){
            case AttackDistance.Near:
                StartCameraMoveUseiTween(newPosition,AttackDistance.Near);
                break;
            case AttackDistance.Middle:
                StartCameraMoveUseiTween(newPosition, AttackDistance.Middle);
                break;
            case AttackDistance.Far:
                StartCameraMoveUseiTween(new Vector3(0,0,0), AttackDistance.Far);
                break;
        }

        nowCameraState = CameraState.Moving;
        SetCameraButtonActivity(false);
    }

    void StartCameraMoveUseiTween(Vector3 pos,AttackDistance distance)
    {
        iTween.MoveTo(gameObject, iTween.Hash("x", pos.x + attackCamera[(int)distance].position.x, "y", pos.y+attackCamera[(int)distance].position.y, "z", pos.z+attackCamera[(int)distance].position.z,
           "time", changeTime, "oncomplete", "OnCompleteMove", "oncompletetarget", gameObject, "oncompleteparams", CameraState.Attack));
        iTween.RotateTo(gameObject, iTween.Hash("x", attackCamera[(int)distance].rotation.x, "y", GetInverseRotationFromFrontMode() + attackCamera[(int)distance].rotation.y, "z", attackCamera[(int)distance].rotation.z, "time", changeTime, "islocal", true));
    }
    void OnCompleteMove(CameraState state)
    {
        SetResetCameraVector();
        nowCameraState = state;
        if (nowCameraState == CameraState.Back)
        {
            SetCameraButtonActivity(true);
        }
        else
        {
            SetCameraButtonActivity(false);
        }
    }



    //移動アニメーション作成
    Hashtable SetMoveTable(Vector2 pos,float time)
    {
        Hashtable table = new Hashtable();
        table.Add("x", pos.x+transform.position.x);
        table.Add("z", pos.y + transform.position.z);
        table.Add("time", time);
        table.Add("easetype", iTween.EaseType.linear);
        table.Add( "oncomplete", "OnCompleteMove");
        table.Add("oncompletetarget", gameObject);
        table.Add("oncompleteparams", CameraState.Back);
        return table;

    }
    public void FollowCharacter(Vector2 pos,float time)
    {
        ResetCamera();
        var table=SetMoveTable(pos,time);
        iTween.MoveTo(gameObject, table);
        nowCameraState = CameraState.Moving;
        SetCameraButtonActivity(false);
    }

    //前からか後ろからか
    public void ChangeFrontMode(CameraMode _cameraMode)
    {
        nowCameraMode = _cameraMode;
        if (nowCameraState == CameraState.Back)
        {
            ResetCamera();
            transform.position = new Vector3(transform.position.x, transform.position.y, activeCharacter.transform.position.z+GetSignFromFrontMode()* backCamera.position.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
            SetResetCameraVector();

        }
        else if (nowCameraState == CameraState.Lean)
        {
            ResetCamera();
            transform.position = new Vector3(transform.position.x, transform.position.y, -transform.position.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
            SetResetCameraVector();
        }
    }
    void SetCameraButtonActivity(bool isEnable)
    {
        cameraChangeButton.interactable = isEnable;
        cameraResetButton.interactable = isEnable;
    }
    //カメラ手前からで1,カメラ奥からで-1
    int GetSignFromFrontMode(){
        return nowCameraMode == CameraMode.FromBack ? 1 : -1;
    }
    int GetInverseRotationFromFrontMode()
    {
        return nowCameraMode == CameraMode.FromBack ? 0 : 180;
    }


}