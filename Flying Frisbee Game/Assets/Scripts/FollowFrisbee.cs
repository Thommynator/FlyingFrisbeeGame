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

    private bool isFollowingEnabled;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onMovementManagerEnter += () => { isFollowingEnabled = false; };
        GameEvents.current.onMovementManagerExit += () => { isFollowingEnabled = true; };

        isFollowingEnabled = true;
        frisbeeObject = GameObject.FindGameObjectWithTag("Frisbee");
        transform.position = frisbeeObject.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowingEnabled)
        {
            float x = followXAxis ? useLerp ? Vector3.Lerp(transform.position, frisbeeObject.transform.position + offset, lerpTime).x : frisbeeObject.transform.position.x + offset.x : transform.position.x;
            float y = followYAxis ? useLerp ? Vector3.Lerp(transform.position, frisbeeObject.transform.position + offset, lerpTime).y : frisbeeObject.transform.position.y + offset.y : transform.position.y;
            float z = followZAxis ? useLerp ? Vector3.Lerp(transform.position, frisbeeObject.transform.position + offset, lerpTime).z : frisbeeObject.transform.position.z + offset.z : transform.position.z;
            transform.position = new Vector3(x, y, z);
        }
    }
}
