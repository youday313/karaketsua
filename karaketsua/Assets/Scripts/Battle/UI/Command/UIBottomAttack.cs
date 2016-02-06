using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIBottomAttack : UIBottomBase
{
    Button button;
    public UIBottomCommandParent commandParent;
    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void UpdateUI()
    {
        button.interactable = false;
        //キャラクター状態取得
        var chara = CharacterManager.Instance.GetNowActiveCharacter();
        if (chara == null) return;
        if (chara.isAttacked == false) return;

        button.interactable = true;
        
    }

    //ボタンクリック
    public void OnClick()
    {
        commandParent.CreateWazaSelect();
    }
}
