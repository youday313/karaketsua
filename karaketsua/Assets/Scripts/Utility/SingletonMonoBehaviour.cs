using UnityEngine;
using System.Collections;

//シーン上で一つしか存在しないオブジェクトに適用
//継承させた後、Hoge.Instanse.メソッド()で使用
//シーン変更時に破棄、次シーンにも残したい場合は下のDontDestroySingletonを使用
public class SingletonMonoBehaviour<T>: MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    /**
    Returns the instance of this singleton.
    */
    public static T Instance {
        get {
            if(instance == null) {
                instance = (T)FindObjectOfType(typeof(T));
                if(instance == null) {
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
        if(this != Instance) {
            Destroy(this.gameObject);
            return;
        }
        //DontDestroyOnLoad(this.gameObject);

    }
}
public class DontDestroySingleton<T>: MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    /**
    Returns the instance of this singleton.
    */
    public static T Instance {
        get {
            if(instance == null) {
                instance = (T)FindObjectOfType(typeof(T));

                if(instance == null) {
                    //Debug.LogError( "Instance [ " + typeof(T) + " ] is none " );
                    //GameObject go = new GameObject(typeof(T).Name);
                    GameObject go = new GameObject();
                    instance = go.AddComponent<T>();
                    go.name = instance.GetType().ToString();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }
}

/// <summary>
/// シングルトン構造
/// </summary>
public class SingletonBase<T> where T : new()
{
    private static T instance;
    public static T Instance {
        get {
            if(instance == null) {
                instance = new T();
            }
            return instance;
        }
        set {
            instance = value;
        }
    }
}