using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAim : MonoBehaviour
{
    Vector3 startPosition;
    Vector3 endPosition;
    Vector3 deltaDistanceVector;
    private bool isAiming = false;

    private GameObject frisbee;

    private LineRenderer lineRenderer;

    private GameObject throwDistanceIndicator;

    private float minimumThrowDistanceThreshold = 1;

    /// Multiplies the "drag"-distance, e.g. 10m dragged = 20m thrown 
    public float forceFactor = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        isAiming = false;

        frisbee = GameObject.FindGameObjectWithTag("Frisbee");
        lineRenderer = GetComponent<LineRenderer>();
        throwDistanceIndicator = GameObject.Find("ThrowDistanceIndicator");
        throwDistanceIndicator.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (isAiming)
        {
            AdjustThrowAngle();
            endPosition = GetMousePositionOnPlaneAsWorldCoordinate();

            if (GetThrowDistanceVector().magnitude > minimumThrowDistanceThreshold)
            {
                // Drag-line for aiming
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, startPosition + Vector3.up * 0.5f);
                lineRenderer.SetPosition(1, endPosition + Vector3.up * 0.5f);

                // horizontal distance indicator on the ground
                throwDistanceIndicator.GetComponent<MeshRenderer>().enabled = true;
                float newZ = frisbee.transform.position.z + GetThrowDistanceVector().z;
                throwDistanceIndicator.transform.position = new Vector3(throwDistanceIndicator.transform.position.x, throwDistanceIndicator.transform.position.y, newZ);

                // curve indicator
                Vector3 targetInWorld = GetThrowDistanceVector();
                float v0 = GetThrowVelocityScalar(targetInWorld.magnitude);
                DrawThrowCurve(v0);
            }
            else
            {
                lineRenderer.enabled = false;
                throwDistanceIndicator.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    void OnMouseDown()
    {
        isAiming = true;
        startPosition = GetMousePositionOnPlaneAsWorldCoordinate();
    }

    void OnMouseUp()
    {
        isAiming = false;
        if (startPosition != Vector3.zero && endPosition != Vector3.zero)
        {
            deltaDistanceVector = GetThrowDistanceVector();
            // throw frisbee only if mouse moved (dragged) more than a threshold
            if (deltaDistanceVector.magnitude > minimumThrowDistanceThreshold)
            {
                Vector3 velocity = GetThrowVelocityVector(deltaDistanceVector);
                frisbee.GetComponent<Frisbee>().DetachFromPlayer();
                frisbee.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
            }
        }

        lineRenderer.enabled = false;
        throwDistanceIndicator.GetComponent<MeshRenderer>().enabled = false;
    }

    public Vector3 GetThrowDistanceVector()
    {
        return forceFactor * (endPosition - startPosition);
    }

    /// Computes v0 (scalar) that is needed to reach the distance with the given throw angle.
    private float GetThrowVelocityScalar(float distance)
    {
        float h0 = frisbee.transform.position.y;
        float throwAngleRad = frisbee.GetComponent<Frisbee>().throwAngleDegree * Mathf.Deg2Rad;
        float denominator = 2 * (h0 + (distance * Mathf.Tan(throwAngleRad))) * Mathf.Cos(throwAngleRad) * Mathf.Cos(throwAngleRad);

        if (Mathf.Approximately(distance, 0.0f) || Mathf.Approximately(denominator, 0.0f))
        {
            return 0.0f;
        }

        return Mathf.Sqrt(distance * distance * -Physics.gravity.y / denominator);
    }

    private Vector3 GetThrowVelocityVector(Vector3 distanceVector)
    {
        float angleToWorldX = Vector3.SignedAngle(distanceVector, Vector3.right, Vector3.up);
        float v0 = GetThrowVelocityScalar(distanceVector.magnitude);
        float throwAngleRad = frisbee.GetComponent<Frisbee>().throwAngleDegree * Mathf.Deg2Rad;
        float vx = v0 * Mathf.Cos(throwAngleRad) * Mathf.Cos(angleToWorldX * Mathf.Deg2Rad);
        float vy = v0 * Mathf.Sin(throwAngleRad);
        float vz = v0 * Mathf.Cos(throwAngleRad) * Mathf.Sin(angleToWorldX * Mathf.Deg2Rad);
        return new Vector3(vx, vy, vz);
    }

    private void DrawThrowCurve(float v0)
    {

        GameObject curveIndicator = new GameObject("CurveIndicator");
        for (float t = 0; t < 2; t += 0.01f)
        {
            float horizontalPos = v0 * Mathf.Cos(frisbee.GetComponent<Frisbee>().throwAngleDegree * Mathf.Deg2Rad) * t;
            float verticalPos = v0 * Mathf.Sin(frisbee.GetComponent<Frisbee>().throwAngleDegree * Mathf.Deg2Rad) * t - 0.5f * -Physics.gravity.y * t * t;

            // dont' draw curve indicator line if it's below the ground plane
            if (verticalPos < -frisbee.transform.position.y)
            {
                break;
            }

            GameObject spehre = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spehre.transform.SetParent(curveIndicator.transform);
            Destroy(spehre.GetComponent<SphereCollider>());
            spehre.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            spehre.transform.localPosition = new Vector3(0, verticalPos, horizontalPos);
        }

        Vector3 distanceVector = GetThrowDistanceVector();
        curveIndicator.transform.position = frisbee.transform.position;
        curveIndicator.transform.LookAt(distanceVector + frisbee.transform.position, Vector3.up);
        Destroy(curveIndicator, 0.05f);
    }

    // void OnDrawGizmos()
    // {
    //     // Draw a yellow sphere at the target position
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawSphere(GetThrowDistanceVector() + frisbee.transform.position, 1);
    // }

    /// Uses the mousewheel scrolling to adjust the throw angle of the frisbee
    private void AdjustThrowAngle()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            frisbee.GetComponent<Frisbee>().DecreaseThrowAngle();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            frisbee.GetComponent<Frisbee>().IncreaseThrowAngle();
        }
    }

    private Vector3 GetMousePositionOnPlaneAsWorldCoordinate()
    {
        Plane plane = new Plane(Vector3.up, 0.0f);

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
}
