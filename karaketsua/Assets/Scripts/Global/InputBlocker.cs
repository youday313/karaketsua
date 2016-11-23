using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// 画面入力を停止する
public class InputBlocker : SingletonMonoBehaviour<InputBlocker> {

    // EventSystemやInputTouches
    [SerializeField] List<GameObject> stopObjects = new List<GameObject>();

    void Start()
    {
        Instance.Release();
    }

    public void Block()
    {
        gameObject.SetActive(true);
        stopObjects.ForEach(o => o.SetActive(false));
    }

    public void Release()
    {
        stopObjects.ForEach(o => o.SetActive(true));
        gameObject.SetActive(false);
    }
}
