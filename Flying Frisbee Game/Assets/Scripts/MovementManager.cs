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
        GameEvents.current.onMovementManagerEnter += StartMovementManager;

        frisbee = GameObject.FindGameObjectWithTag("Frisbee");
        minimapImage = GameObject.Find("MinimapImage");

        isInManagerView = false;

        offsetFrisbeeToCameraRigPosition = frisbee.transform.position - mainCameraRig.transform.position;
        initialCameraRigRotation = mainCameraRig.transform.rotation;

        targetTopViewPosition = new Vector3(1, 15, 0);
        targetTopViewRotation = Quaternion.Euler(70, -90, 0);

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
                ExitMovementManager();
            }
        }
    }

    /// Triggers the coroutine to move the camera to the top (manager) view
    private void StartMovementManager()
    {
        minimapImage.GetComponent<RawImage>().enabled = false;

        // stop physics temporarily
        Time.timeScale = 0;

        if (currentMoveCoroutine != null)
        {
            StopCoroutine(currentMoveCoroutine);
        }

        currentMoveCoroutine = MoveCameraToTopView();
        StartCoroutine(currentMoveCoroutine);
    }
    private IEnumerator MoveCameraToTopView()
    {
        while ((mainCameraRig.transform.position - targetTopViewPosition).sqrMagnitude > 0.001f)
        {
            mainCameraRig.transform.position = Vector3.Lerp(mainCameraRig.transform.position, targetTopViewPosition, lerpSpeed);
            mainCameraRig.transform.rotation = Quaternion.Lerp(mainCameraRig.transform.rotation, targetTopViewRotation, lerpSpeed);
            yield return null;
        }
        Debug.Log("Moved camera to Movement Manager view");
    }

    /// Triggers the coroutine to move the camera to the player (normal) view
    private void ExitMovementManager()
    {
        minimapImage.GetComponent<RawImage>().enabled = true;

        // continue physics
        Time.timeScale = 1;

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
        while ((mainCameraRig.transform.position - targetPosition).sqrMagnitude > 1f)
        {
            mainCameraRig.transform.position = Vector3.Lerp(mainCameraRig.transform.position, targetPosition, lerpSpeed);
            mainCameraRig.transform.rotation = Quaternion.Lerp(mainCameraRig.transform.rotation, initialCameraRigRotation, lerpSpeed);
            yield return null;
        }
        Debug.Log("Moved camera to play view");
        GameEvents.current.MovementManagerExit();
    }
}
