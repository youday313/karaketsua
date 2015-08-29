//CameraMove
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CameraMove : MonoBehaviour
{
	//public
    public Vector3[] positions;
    public Vector3[] rotations;
	//private
    public float changeTime=1f;
    public Transform cameraTransform;
    int cameraCount = 0;

	void Start ()
	{
        //StartCoroutine("CameraCorutine");
	}
	


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //var startPosition = cameraTransform.position;
            //cameraTransform.position = Vector3.Lerp(startPosition, positions[cameraCount%2], changeTime);
            //cameraTransform.eulerAngles = Vector3.Lerp(cameraTransform.eulerAngles, rotations[cameraCount%2], changeTime);
            iTween.MoveTo(cameraTransform.gameObject, iTween.Hash("x", positions[cameraCount % 2].x, "y", positions[cameraCount % 2].y, "z", positions[cameraCount % 2].z, "time", changeTime));
            iTween.RotateTo(cameraTransform.gameObject, iTween.Hash("x", rotations[cameraCount % 2].x, "y", rotations[cameraCount % 2].y, "z", rotations[cameraCount % 2].z, "time", changeTime, "islocal", true));
            //iTween.RotateTo(cameraTransform.gameObject, iTween.Hash("x", rotations[cameraCount % 2].x, "y", rotations[cameraCount % 2].y, "z", rotations[cameraCount % 2].z, "time", changeTime, "islocal", true));
            cameraCount++;
        }
    }

    public IEnumerator CameraCorutine()
    {
        while (true)
        {
            
            for (int i = 0; i < positions.Length; i++)
            {
                var startPosition=cameraTransform.position;
                cameraTransform.position = Vector3.Lerp(startPosition, positions[i], changeTime);
                cameraTransform.eulerAngles = Vector3.Lerp(cameraTransform.eulerAngles, rotations[i], changeTime);
                //cameraTransform.localEulerAngles = rotations[i];

                yield return new WaitForSeconds(changeTime);
            }
        }
    }
}