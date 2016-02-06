using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIBottomDeffence : UIBottomBase
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

        button.interactable = true;
    }

    public void OnClick()
    {
        commandParent.CreateExecuteDeffence();
    }
}
