using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// 画面入力を停止する
public class InputBlocker : DontDestroySingleton<InputBlocker> {

    [SerializeField] private Button block;
    // EventSystemやInputTouches
    [SerializeField] List<GameObject> stopObjects;

    public void Block()
    {
        block.interactable = true;
        stopObjects.ForEach(o => o.SetActive(false));
    }

    public void Release()
    {
        block.interactable = false;
        stopObjects.ForEach(o => o.SetActive(true));
    }
}
