using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAim : MonoBehaviour
{
    Vector3 startPosition;
    Vector3 endPosition;
    Vector3 deltaDistanceVector;
    private bool isAiming = false;

    private GameObject aimingHelper;

    private GameObject frisbee;

    private LineRenderer lineRenderer;

    private GameObject throwDistanceIndicator;

    /// Multiplies the "drag"-distance, e.g. 10m dragged = 20m thrown 
    private float forceFactor = 2.0f;

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
            // draw line to mouse position
            endPosition = GetMousePositionOnGroundAsWorldCoordinate();
            Debug.DrawLine(startPosition, endPosition);

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, startPosition + Vector3.up * 0.5f);
            lineRenderer.SetPosition(1, endPosition + Vector3.up * 0.5f);

            throwDistanceIndicator.GetComponent<MeshRenderer>().enabled = true;
            float newZ = frisbee.transform.position.z - GetThrowDistanceVector().z;
            throwDistanceIndicator.transform.position = new Vector3(throwDistanceIndicator.transform.position.x, throwDistanceIndicator.transform.position.y, newZ);
        }
    }

    void OnMouseDown()
    {
        isAiming = true;
        startPosition = GetMousePositionOnGroundAsWorldCoordinate();
    }

    void OnMouseUp()
    {
        isAiming = false;
        if (startPosition != Vector3.zero && endPosition != Vector3.zero)
        {
            deltaDistanceVector = GetThrowDistanceVector();
            // throw frisbee only if mouse moved (dragged) more than a threshold
            if (deltaDistanceVector.magnitude > 0.1)
            {
                float angleToWorldX = Vector3.SignedAngle(deltaDistanceVector, Vector3.right, Vector3.up);

                float v0 = GetThrowVelocity(deltaDistanceVector.magnitude);
                float throwAngleRad = frisbee.GetComponent<Frisbee>().throwAngleDegree * Mathf.Deg2Rad;
                float vx = -v0 * Mathf.Cos(throwAngleRad) * Mathf.Cos(angleToWorldX * Mathf.Deg2Rad);
                float vy = v0 * Mathf.Sin(throwAngleRad);
                float vz = -v0 * Mathf.Cos(throwAngleRad) * Mathf.Sin(angleToWorldX * Mathf.Deg2Rad);
                Vector3 velocity = new Vector3(vx, vy, vz);
                frisbee.GetComponent<Frisbee>().DetachFromPlayer();
                frisbee.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
            }
        }

        lineRenderer.enabled = false;
        throwDistanceIndicator.GetComponent<MeshRenderer>().enabled = false;
    }

    private Vector3 GetThrowDistanceVector()
    {
        return forceFactor * (endPosition - startPosition);
    }

    private float GetThrowVelocity(float distance)
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

    private Vector3 GetMousePositionOnGroundAsWorldCoordinate()
    {
        Plane plane = new Plane(Vector3.up, 0);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}
