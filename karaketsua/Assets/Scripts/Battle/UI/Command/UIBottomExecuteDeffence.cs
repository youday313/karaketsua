using UnityEngine;
using System.Collections;

public class UIBottomExecuteDeffence : UIBottomBase
{

    public UIBottomCommandParent commandParent;
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
        commandParent.CreateAction();
    }
}
