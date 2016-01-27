using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//オブジェクトの表示のみを行う
//処理は行わない
[System.Serializable]
public class UIBottomController : UIBase {


    //子
    public Button cancelButton;
    public Button menuButton;
    public Button autoButton;
    public Button cameraButton;

    //シーン開始時
    void Awake()
    {
        base.OnAwake();
        ActionSelect.Instance.OnAwakeE += OffUI;
        ActionSelect.Instance.OnStartWaveE += OnStartWave;
        ActionSelect.Instance.OnActiveE += OnActiveCharacter;
        ActionSelect.Instance.OnSelectAttackE += OnSelectAttack;

        ActionSelect.Instance.OnCameraMoveE += OnCameraMove;
        ActionSelect.Instance.OnCancelE += OnCancel;
        
    }
    void OnAwake()
    {

    }

    //全てオフ
    public override void OffUI()
    {

        base.OffUI();
        cancelButton.interactable = false;
        menuButton.interactable = true;
        autoButton.interactable = false;
        cameraButton.interactable = false;
    }

    //戦闘のゲーム部が開始した時
    public void OnStartWave()
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

    public void OnSelectAttack()
    {
        cancelButton.interactable = true;
        menuButton.interactable = true;
        autoButton.interactable = true;
        cameraButton.interactable = false;
    }

    //攻撃開始
    public void OnExecuteAttack()
    {

    }

    //キャラクター行動終了時
    public void OnEndActive()
    {
        parent.SetActive(true);
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

    public void OnCancel()
    {

    }


}
