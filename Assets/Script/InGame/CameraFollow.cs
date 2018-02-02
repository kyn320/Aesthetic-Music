using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    public Vector3 distance;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + distance,Time.fixedDeltaTime);
    }


}
