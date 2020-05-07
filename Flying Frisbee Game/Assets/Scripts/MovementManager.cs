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

    private float lerpSpeed = 0.1f;

    private GameObject frisbee;

    private GameObject minimapImage;

    private IEnumerator currentMoveCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onMovementManagerEnter += StartMoveCameraToTopView;

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

            if (isInManagerView)
            {
                GameEvents.current.MovementManagerEnter();
            }
            else
            {
                StartMoveCameraToPlayView();
            }
        }
    }

    /// Triggers the coroutine to move the camera to the top (manager) view
    private void StartMoveCameraToTopView()
    {
        minimapImage.GetComponent<RawImage>().enabled = false;

        if (currentMoveCoroutine != null)
        {
            StopCoroutine(currentMoveCoroutine);
        }

        currentMoveCoroutine = MoveCameraToTopView();
        StartCoroutine(currentMoveCoroutine);
    }
    private IEnumerator MoveCameraToTopView()
    {
        while (Vector3.Distance(mainCameraRig.transform.position, targetTopViewPosition) > 0.001f)
        {
            mainCameraRig.transform.position = Vector3.Lerp(mainCameraRig.transform.position, targetTopViewPosition, lerpSpeed);
            mainCameraRig.transform.rotation = Quaternion.Lerp(mainCameraRig.transform.rotation, targetTopViewRotation, lerpSpeed);
            yield return null;
        }
        Debug.Log("Moved camera to Movement Manager view");
    }

    /// Triggers the coroutine to move the camera to the player (normal) view
    private void StartMoveCameraToPlayView()
    {
        minimapImage.GetComponent<RawImage>().enabled = true;

        if (currentMoveCoroutine != null)
        {
            StopCoroutine(currentMoveCoroutine);
        }

        currentMoveCoroutine = MoveCameraToPlayView();
        StartCoroutine(currentMoveCoroutine);
    }
    private IEnumerator MoveCameraToPlayView()
    {
        Vector3 targetPosition = frisbee.transform.position + offsetFrisbeeToCameraRigPosition;
        while (Vector3.Distance(mainCameraRig.transform.position, targetPosition) > 0.001f)
        {
            mainCameraRig.transform.position = Vector3.Lerp(mainCameraRig.transform.position, targetPosition, lerpSpeed);
            mainCameraRig.transform.rotation = Quaternion.Lerp(mainCameraRig.transform.rotation, initialCameraRigRotation, lerpSpeed);
            yield return null;
        }
        Debug.Log("Moved camera to play view");
        GameEvents.current.MovementManagerExit();
    }
}
