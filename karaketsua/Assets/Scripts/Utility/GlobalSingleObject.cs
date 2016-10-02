using UnityEngine;
using System.Collections;

// シングルトンとともに使うことでゲーム内でのオブジェクトにする
public class GlobalSingleObject : MonoBehaviour {

    private static GlobalSingleObject instance;

    void Awake()
    {
        if(instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
