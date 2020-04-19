using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAim : MonoBehaviour
{

    public float throwAngleDegree = 30f;
    Vector3 startPosition;
    Vector3 endPosition;
    Vector3 deltaDistance;
    private bool isAiming = false;

    private GameObject aimingHelper;

    private GameObject frisbee;

    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        isAiming = false;

        frisbee = GameObject.FindGameObjectWithTag("Frisbee");
        lineRenderer = GetComponent<LineRenderer>();
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
            deltaDistance = endPosition - startPosition;
            // throw frisbee only if mouse moved (dragged) more than a threshold
            if (deltaDistance.magnitude > 0.1)
            {
                float angleToWorldX = Vector3.SignedAngle(deltaDistance, Vector3.right, Vector3.up);

                float v0 = getThrowVelocity(deltaDistance.magnitude);
                float throwAngleRad = throwAngleDegree * Mathf.Deg2Rad;
                float vx = -(v0 * Mathf.Cos(throwAngleRad)) * Mathf.Cos(angleToWorldX * Mathf.Deg2Rad);
                float vy = v0 * Mathf.Sin(throwAngleRad);
                float vz = -(v0 * Mathf.Cos(throwAngleRad)) * Mathf.Sin(angleToWorldX * Mathf.Deg2Rad);
                Vector3 velocity = new Vector3(vx, vy, vz);

                frisbee.GetComponent<Frisbee>().DetachFromPlayer();
                frisbee.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
            }
        }

        lineRenderer.enabled = false;
    }

    private float getThrowVelocity(float distance)
    {
        // return Mathf.Sqrt(sqrDistance * -Physics.gravity.y / Mathf.Sin(2 * throwAngleDegree * Mathf.Deg2Rad));
        float h0 = 0.5f;
        float throwAngleRad = throwAngleDegree * Mathf.Deg2Rad;
        float denominator = -2 * (-h0 - distance * Mathf.Tan(throwAngleRad)) * Mathf.Cos(throwAngleRad) * Mathf.Cos(throwAngleRad);

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
