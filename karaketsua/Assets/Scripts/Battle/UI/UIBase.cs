using UnityEngine;
using System.Collections;

public class UIBase : MonoBehaviour {

    public GameObject parent;

    public void OnAwake()
    {
        ActionSelect.Instance.OnAwakeE += OffUI;
    }

    public virtual void OffUI()
    {
        parent.SetActive(false);
    }
}
