using UnityEngine;
using System.Collections;

public class UIBottomExecuteAttack : UIBottomBase {

    public UIBottomAllParent allParent;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClick()
    {
        allParent.Off();
    }
}
