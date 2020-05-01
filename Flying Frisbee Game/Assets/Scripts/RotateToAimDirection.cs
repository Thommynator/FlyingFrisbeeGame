using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToAimDirection : MonoBehaviour
{

    private Quaternion newRotation;
    private DragAim dragAim;

    private float rotationSpeed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        newRotation = transform.rotation;
        dragAim = GameObject.Find("AimPlane").GetComponent<DragAim>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 throwDistanceVector = dragAim.GetThrowDistanceVector();
        float angle = Vector3.SignedAngle(Vector3.forward, throwDistanceVector, Vector3.up) * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed);
    }
}
