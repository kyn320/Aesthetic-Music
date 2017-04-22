using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    void LateUpdate()
    {
        transform.position =  Vector3.Lerp(transform.position,target.position,Time.fixedDeltaTime);
    }


}
