using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DebugText : MonoBehaviour {

    public Text text;
    public string addText;
	// Use this for initialization
	void Start () {

        text = GameObject.FindGameObjectWithTag("Debug/Text").GetComponent<Text>();
        text.text = string.Empty;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddText(string str)
    {
        addText += str;
    }

    public void LateUpdate()
    {
        text.text = addText;
        addText=string.Empty;
    }
}
