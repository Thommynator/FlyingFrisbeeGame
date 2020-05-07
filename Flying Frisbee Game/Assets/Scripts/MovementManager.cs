using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementManager : MonoBehaviour
{

    public GameObject mainCameraRig;
    private Vector3 offsetFrisbeeToCameraRigPosition;
    private Quaternion initialCameraRigRotation;

    private Vector3 targetTopViewPosition;
    private Quaternion targetTopViewRotation;

    private bool isInManagerView;

    private float lerpSpeed = 0.15f;

    private GameObject frisbee;

    private GameObject minimapImage;

    private IEnumerator currentMoveCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onMovementManagerEnter += StartMoveCameraToTopView;
        GameEvents.current.onMovementManagerExit += StartMoveCameraToPlayView;

        frisbee = GameObject.FindGameObjectWithTag("Frisbee");
        minimapImage = GameObject.Find("MinimapImage");

        isInManagerView = false;

        offsetFrisbeeToCameraRigPosition = frisbee.transform.position - mainCameraRig.transform.position;
        initialCameraRigRotation = mainCameraRig.transform.rotation;

        targetTopViewPosition = new Vector3(0, 30, -1);
        targetTopViewRotation = Quaternion.Euler(70, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isInManagerView = !isInManagerView;
            Debug.Log(isInManagerView);

            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
            }

            if (isInManagerView)
            {
                GameEvents.current.MovementManagerEnter();
            }
            else
            {
                GameEvents.current.MovementManagerExit();
            }
        }
    }

    /// Triggers the coroutine to move the camera to the player (normal) view
    private void StartMoveCameraToPlayView()
    {
        minimapImage.GetComponent<RawImage>().enabled = true;

        currentMoveCoroutine = MoveCameraToPlayView();
        StartCoroutine(currentMoveCoroutine);
    }
    private IEnumerator MoveCameraToPlayView()
    {
        while (Vector3.Distance(mainCameraRig.transform.position, offsetFrisbeeToCameraRigPosition) > 0.05f)
        {
            Vector3 targetPosition = frisbee.transform.position + offsetFrisbeeToCameraRigPosition;
            Debug.Log(Vector3.Distance(mainCameraRig.transform.position, targetPosition));
            mainCameraRig.transform.position = Vector3.Lerp(mainCameraRig.transform.position, targetPosition, lerpSpeed);
            mainCameraRig.transform.rotation = Quaternion.Lerp(mainCameraRig.transform.rotation, initialCameraRigRotation, lerpSpeed);
            yield return null;
        }
        Debug.Log("Moved camera to play view");
    }

    /// Triggers the coroutine to move the camera to the top (manager) view
    private void StartMoveCameraToTopView()
    {
        minimapImage.GetComponent<RawImage>().enabled = false;

        currentMoveCoroutine = MoveCameraToTopView();
        StartCoroutine(currentMoveCoroutine);
    }
    private IEnumerator MoveCameraToTopView()
    {
        while (mainCameraRig.transform.position != targetTopViewPosition)
        {
            Debug.Log(Vector3.Distance(mainCameraRig.transform.position, targetTopViewPosition));
            mainCameraRig.transform.position = Vector3.Lerp(mainCameraRig.transform.position, targetTopViewPosition, lerpSpeed);
            mainCameraRig.transform.rotation = Quaternion.Lerp(mainCameraRig.transform.rotation, targetTopViewRotation, lerpSpeed);
            yield return null;
        }
        Debug.Log("Moved camera to top view");
    }
}
