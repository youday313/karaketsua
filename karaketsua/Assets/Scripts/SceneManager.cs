using UnityEngine;

public class SceneManager : DontDestroySingleton<SceneManager> 
{

    //public
    public void SetNextScene(string nextScene)
    {
        //FadeManager.Instance.LoadLevel(nextScene, 0.5f);
        Application.LoadLevel(nextScene);
    }
}
