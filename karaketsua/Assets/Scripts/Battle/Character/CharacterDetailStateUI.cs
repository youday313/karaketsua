using UnityEngine;

using BattleScene;
/// <summary>
/// uGUIで3D空間のオブジェクト上に追従するHUD制御用コンポーネント
/// </summary>
public class CharacterDetailStateUI : MonoBehaviour
{
    //public Vector3 offset = Vector3.zero;

    //RectTransform myRectTrans;

    void Awake()
    {
        //myRectTrans = GetComponent<RectTransform>();
    }
    public void Init(Vector2 touchPosition,CharacterMasterParameter param)
    {
     
        //親の設定
        var parent = GameObject.FindGameObjectWithTag("CharacterDetailStateUIParent").transform;
        transform.SetParent(parent);
        //ターゲットの設定
        //UpdateUiLocalPosFromTargetPos(touchPosition);
        SetParam(param);
    }
    void SetParam(CharacterMasterParameter param)
    {

    }


    ////表示位置修正
    ////画面から出ないように修正しないといけない
    //void UpdateUiLocalPosFromTargetPos(Vector2 touchPosition)
    //{
    //    //var screenPos = Camera.main.WorldToScreenPoint(touchPosition);
    //    var screenPos = touchPosition;
    //    screenPos +=new Vector2(0,-myRectTrans.sizeDelta.y / 2);
    //    myRectTrans.position = screenPos;
    //    //RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTrans, screenPos, uiCamera, out localPos);
    //    //myRectTrans.localPosition = localPos;
        
    //}
}