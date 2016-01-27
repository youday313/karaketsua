using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class UIWaza : UIBase {

    public List<GameObject> wazaObjects;
    public Button specialAttackButton;
    public Text leftCountText;

    void Awake()
    {
        base.OnAwake();
        ActionSelect.Instance.OnSelectAttackE += OnSelectAttack;
        ActionSelect.Instance.OnSelectWazaE+=OnSelectWaza;
        ActionSelect.Instance.OnSelectSpecialAttackE += OnSelectWaza;

        ActionSelect.Instance.OnCancelE += OnCancel;
    }

    public virtual void OffUI()
    {
        base.OffUI();
        
    }

    


    public void OnSelectAttack()
    {
        parent.SetActive(true);

        //技テキストのアップデート


        //スペシャルのアップデート

    }
    public void OnSelectWaza()
    {
        OffUI();
    }
    public void OnSelectSpecialAttack()
    {
        OffUI();
    }

    public void OnCancel()
    {

    }
}
