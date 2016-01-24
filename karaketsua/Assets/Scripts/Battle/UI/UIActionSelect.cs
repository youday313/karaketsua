using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class UIActionSelect : MonoBehaviour {

    //親
    public GameObject parent;
    //子
    public Button attackButton;
    public Button deffenceButton;
    public Text nameText;

    //シーン開始時
    void Awake()
    {
        OffUI();
    }

    //全てオフ
    void OffUI()
    {
        parent.SetActive(false);
        attackButton.interactable = false;
        deffenceButton.interactable = false;
        nameText.text = "";
    }

    //戦闘のゲーム部が開始した時
    public void OnStartBattleScene()
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
        deffenceButton.interactable = false;
        nameText.text = "";
    }


}
