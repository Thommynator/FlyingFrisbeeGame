using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAim : MonoBehaviour
{
    public float forceFactor = 0.1f;
    Vector3 startPosition;
    Vector3 endPosition;
    Vector3 velocity;
    private bool isAiming = false;

    private GameObject aimingHelper;

    private GameObject frisbee;

    // Start is called before the first frame update
    void Start()
    {
        isAiming = false;

        frisbee = GameObject.FindGameObjectWithTag("Frisbee");

        // configure aiming helper properties
        aimingHelper = new GameObject("Aiming Helper");
        aimingHelper.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = aimingHelper.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.4f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAiming)
        {
            // draw line to mouse position
            endPosition = GetMousePositionOnGroundAsWorldCoordinate();
            Debug.DrawLine(startPosition, endPosition);

            LineRenderer lineRenderer = aimingHelper.GetComponent<LineRenderer>();
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, startPosition + Vector3.up * 0.1f);
            lineRenderer.SetPosition(1, endPosition + Vector3.up * 0.1f);
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
            velocity = endPosition - startPosition;
            // throw frisbee only if mouse moved (draged) more than a threshold
            Debug.Log(velocity.magnitude);
            if (velocity.magnitude > 0.1)
            {
                Vector3 force = forceFactor * -velocity;
                frisbee.GetComponent<Frisbee>().DetachFromPlayer();
                frisbee.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            }
        }

        LineRenderer lineRenderer = aimingHelper.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
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
