using UnityEngine;
using System.Collections;

[System.Serializable]
public class UIActionExecute : MonoBehaviour {

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
