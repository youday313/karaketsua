using UnityEngine;
using System.Collections;

//シーン上で一つしか存在しないオブジェクトに適用
//継承させた後、Hoge.Instanse.メソッド()で使用
//シーン変更時に破棄、次シーンにも残したい場合は下のDontDestroySingletonを使用
[RequireComponent(typeof(GlobalSingleObject))]
public class SingletonMonoBehaviour<T>: MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    public static T Instance {
        get {
            if(instance == null) {
                instance = (T)FindObjectOfType(typeof(T));
                if(instance == null) {
                    GameObject go = new GameObject();
                    instance = go.AddComponent<T>();
                    go.name = instance.GetType().ToString();
                    Debug.Log("Create:" + go.name);
                }
                instance.GetComponent<SingletonMonoBehaviour<T>>().create();
            }
            return instance;
        }
    }
    // 継承先でのコンストラクタ
    protected virtual void create()
    {
    }
}

[RequireComponent(typeof(GlobalSingleObject))]
public class DontDestroySingleton<T>: MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    public static T Instance {
        get {
            if(instance == null) {
                instance = (T)FindObjectOfType(typeof(T));
                if(instance == null) {
                    GameObject go = new GameObject();
                    instance = go.AddComponent<T>();
                    go.name = instance.GetType().ToString();
                    Debug.Log("Create:" + go.name);
                }
                instance.GetComponent<DontDestroySingleton<T>>().create();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    // 継承先でのコンストラクタ
    protected virtual void create()
    {
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