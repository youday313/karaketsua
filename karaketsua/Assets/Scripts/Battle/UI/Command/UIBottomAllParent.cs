using UnityEngine;
using System.Collections;

//UIのオンオフ
public class UIBottomAllParent : UIBottomBase
{

    GameObject bottomParent;
    public UIBottomButtonParent bottomScript;
    GameObject commandParent;
    public UIBottomCommandParent commandScript;

	// Use this for initialization
	void Start () {
        bottomParent = bottomParent.gameObject;
        commandParent = commandParent.gameObject;
        //BSceneState.Instance.UpdateStateE += UpdateUI;
        Off();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void UpdateUI()
    {
    }
    public void On()
    {
        bottomParent.SetActive(true);
        commandParent.SetActive(true);

        bottomScript.UpdateUI();
        commandScript.UpdateUI();
    }

    public void Off()
    {
        bottomParent.SetActive(false);
        commandParent.SetActive(false);
    }
}
