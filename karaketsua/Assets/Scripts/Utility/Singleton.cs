using UnityEngine;
using System.Collections;

//シーン上で一つしか存在しないオブジェクトに適用
//継承させた後、Hoge.Instanse.メソッド()で使用
//シーン変更時に破棄、次シーンにも残したい場合は下のDontDestroySingletonを使用
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    /**
    Returns the instance of this singleton.
    */
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    Debug.Log("An instance of " + typeof(T) +
                    " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }
    //シングルトンオブジェクトはAwakeでonAwakeを呼ぶ
    //継承先でAwakeを使う場合はAwake内でonAwakeを呼ぶ
    void Awake()
    {
        onAwake();
    }
    protected void onAwake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        //DontDestroyOnLoad(this.gameObject);

    }
}
public class DontDestroySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    /**
    Returns the instance of this singleton.
    */
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    Debug.Log("An instance of " + typeof(T) +
                    " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }
    //シングルトンオブジェクトはAwakeでonAwakeを呼ぶ
    void Awake()
    {
        onAwake();
    }
    protected void onAwake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
