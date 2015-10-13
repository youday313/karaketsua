using UnityEngine;
using System.Collections;

public class SceneManager : DontDestroySingleton<SceneManager> {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //public
    public void SetNextScene(string nextScene)
    {
        //FadeManager.Instance.LoadLevel(nextScene, 0.5f);
        Application.LoadLevel(nextScene);
    }


}
