using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//オブジェクトの表示のみを行う
//処理は行わない
[System.Serializable]
public class UIBottomController : MonoBehaviour {

    public GameObject parent;

    //子
    public Button cancelButton;
    public Button menuButton;
    public Button autoButton;
    public Button cameraButton;

    //シーン開始時
    void Awake()
    {
        OffUI();
    }

    //全てオフ
    void OffUI()
    {
        parent.SetActive(false);
        cancelButton.interactable = false;
        menuButton.interactable = true;
        autoButton.interactable = false;
        cameraButton.interactable = false;
    }

    //戦闘のゲーム部が開始した時
    public void OnStartBattleScene()
    {
        parent.SetActive(true);
        cancelButton.interactable = false;
        menuButton.interactable = true;
        autoButton.interactable = true;
        cameraButton.interactable = false;
    }

    //キャラクター選択時
    public void OnActiveCharacter()
    {
        cancelButton.interactable = false;
        menuButton.interactable = true;
        autoButton.interactable = true;
        cameraButton.interactable = false;
    }

    //キャラクター行動終了時
    public void UnSetActive()
    {
        cancelButton.interactable = false;
        menuButton.interactable = true;
        autoButton.interactable = true;
        cameraButton.interactable = false;
    }

    public void OnCameraMove()
    {
        cameraButton.interactable = true;
    }

    //行動を選択した時
    public void OnActionSelect()
    {
        cancelButton.interactable = true;
    }
}
