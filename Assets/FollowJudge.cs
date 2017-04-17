using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowJudge : MonoBehaviour {

    public Transform target;

    void FixedUpdate() {
        transform.position = target.position;
    }
}
