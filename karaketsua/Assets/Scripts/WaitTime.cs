//WaitTime
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaitTime : MonoBehaviour
{
	//public

	//private

    RectTransform rect;
    public float waitSpeed;
	void Start ()
	{
		rect=GetComponent<RectTransform>();
        StartCoroutine("Move");
	}
	
	void Update ()
	{
        
    }
    IEnumerator Move()
    {
        while (true)
        {
            rect.position = new Vector3(Mathf.Clamp(rect.position.x + waitSpeed, -220f, 220f), rect.position.y, rect.position.z);
            //rect.position = Mathf.Clamp(rect.position, -220f, 220f);
            yield return new WaitForEndOfFrame();
        }
    }
	
}