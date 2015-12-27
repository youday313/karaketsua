using UnityEngine;
using System.Collections;

//指定時間後に削除
public class DestroyedAfterTime : MonoBehaviour {

    [Tooltip("指定時間後に削除")]
    public float destroyTime;
	// Use this for initialization
	void Start () {

        Destroy(gameObject, destroyTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
