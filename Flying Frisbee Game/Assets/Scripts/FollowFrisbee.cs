using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFrisbee : MonoBehaviour
{
    public Vector3 offset;
    public bool followXAxis;
    public bool followYAxis;
    public bool followZAxis;
    public bool rotateAroundYAxis;
    private GameObject frisbee;

    // Start is called before the first frame update
    void Start()
    {
        frisbee = GameObject.FindGameObjectWithTag("Frisbee");
    }

    // Update is called once per frame
    void Update()
    {
        float x = followXAxis ? frisbee.transform.position.x : transform.position.x;
        float y = followYAxis ? frisbee.transform.position.y : transform.position.y;
        float z = followZAxis ? frisbee.transform.position.z : transform.position.z;
        transform.position = new Vector3(x, y, z) + offset;
    }
}
