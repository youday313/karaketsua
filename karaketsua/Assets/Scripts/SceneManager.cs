using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using System;

public class SceneManager : DontDestroySingleton<SceneManager> 
{
    private Scene currentScene = Scene.None;

    // 起動時現在のシーンを取得
    protected override void create()
    {
        currentScene = (Scene)Enum.Parse(typeof(Scene), UnitySceneManager.GetActiveScene().name);
    }

    public void LoadNextScene(Scene nextScene)
    {
        //FadeManager.Instance.LoadLevel(nextScene, 0.5f);
        UnitySceneManager.LoadScene(nextScene.ToString(), UnityEngine.SceneManagement.LoadSceneMode.Single);
        currentScene = nextScene;
    }
}

public enum Scene
{
    None,
    Title,
    Field,
    Edit,
    Battle,
    Result,
}


