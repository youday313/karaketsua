using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class UIWaza : MonoBehaviour {

    public GameObject parent;

    void Awake()
    {
        OffUI();
    }

    //全てオフ
    void OffUI()
    {
        parent.SetActive(false);

    }
}
