using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFrisbee : MonoBehaviour
{
    public Vector3 offset;
    public bool followXAxis;
    public bool followYAxis;
    public bool followZAxis;

    [Range(0.0f, 0.2f)]
    public float lerpTime = 0.1f;

    public bool useLerp;

    private GameObject frisbeeObject;

    // Start is called before the first frame update
    void Start()
    {
        frisbeeObject = GameObject.FindGameObjectWithTag("Frisbee");
        transform.position = frisbeeObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x = followXAxis ? useLerp ? Vector3.Lerp(transform.position, frisbeeObject.transform.position, lerpTime).x : frisbeeObject.transform.position.x : transform.position.x;
        float y = followYAxis ? useLerp ? Vector3.Lerp(transform.position, frisbeeObject.transform.position, lerpTime).y : frisbeeObject.transform.position.y : transform.position.y;
        float z = followZAxis ? useLerp ? Vector3.Lerp(transform.position, frisbeeObject.transform.position, lerpTime).z : frisbeeObject.transform.position.z : transform.position.z;
        transform.position = new Vector3(x, y, z) + offset;
    }
}
