using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class UIActionSelect : UIBase {

    //子
    public Button attackButton;
    public Button deffenceButton;
    public Text nameText;




    //シーン開始時
    void Awake()
    {
        base.OnAwake();
        ActionSelect.Instance.OnStartWaveE += DisableUI;
        ActionSelect.Instance.OnActiveE += OnActiveCharacter;
        ActionSelect.Instance.OnSelectAttackE += OffUI;
        ActionSelect.Instance.OnSelectDeffenceE += OffUI;
        ActionSelect.Instance.OnCancelE += OnCancel;
        ActionSelect.Instance.OnCameraMoveE += OnCameraMove;
        ActionSelect.Instance.OnCharacterMoveE += OnCharacterMove;
        
    }


    //Event
    //全てオフ
    public override void OffUI()
    {
        base.OffUI();
        attackButton.interactable = false;
        deffenceButton.interactable = false;
        nameText.text = "";
    }
    //2つが半透明
    public void DisableUI()
    {
        parent.SetActive(true);
        attackButton.interactable = false;
        deffenceButton.interactable = false;
        nameText.text = "";
    }

    

    //キャラクター選択時
    public void OnActiveCharacter()
    {
        parent.SetActive(true);
        attackButton.interactable = true;
        
        //変動
        //deffenceButton.interactable = false;

        nameText.text=ActionSelect.Instance.activeCharacter.characterParameter.charaName;

    }

    public void OnCameraMove()
    {

    }

    public void OnCameraReset()
    {

    }

    public void OnCancel()
    {
        

    }
    public void OnCharacterMove()
    {

    }



}
