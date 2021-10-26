using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class dioramaController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public GameObject focusTarget;

    [SerializeField] GameObject lastFocusTarget;

    [SerializeField] public GameObject currentDiorama;

    [SerializeField] GameObject CurrentSelectedDiorama;

    [SerializeField] GameObject obstructingObject;

    [SerializeField] Canvas Canvas;

    [SerializeField] Camera mainCamera;

    [SerializeField] float maxZoom;

    [SerializeField] float minZoom;

    [SerializeField] TextMeshProUGUI text;

    [SerializeField] GameObject textBackground;

    [SerializeField] Image image;

    [SerializeField] GameObject DioramaMover;

    public bool isFocused = false;

    Vector3 focusDir;
    Quaternion focusRot;

    Vector3 lastFocusDir;
    Quaternion lastFocusRot;

    Vector3 unFocusDir;
    Quaternion unFocusRot;

    float timePassedCamera = 1f;

    float cameraMoveDur = 0.5f;

    float timePassedDiorama = 0f;

    float dioramaMoveDur = 1f;

    void Start()
    {
        mainCamera.transform.LookAt(currentDiorama.transform);
        CurrentSelectedDiorama = currentDiorama;
        lastFocusTarget = currentDiorama;
    }

    // Update is called once per frame
    void Update()
    {
        //Would have been used to raycast from camera to focustarget, disabeling meshrenderers inbetween
        //Vector3 CameraRayDebug = transform.TransformDirection(Vector3.forward) * 10;
        //Debug.DrawRay(Camera.main.transform.position, CameraRayDebug, Color.green, 900f, false);

        //RaycastHit obstructed;
        //Ray obstructedRay = Camera.main.ScreenPointToRay(currentDiorama.transform.position);
        //if (Physics.Raycast(obstructedRay, out obstructed, Vector3.Distance(Camera.main.transform.position, currentDiorama.transform.position)))
        //{

        //    if (obstructed.collider.gameObject.tag == "Environment")
        //    {
        //        Debug.Log("hit environment");
        //        obstructingObject = obstructed.collider.gameObject;
        //        obstructingObject.GetComponent<MeshRenderer>().enabled = false;
        //    }
        //}
        //else
        //{
        //    Debug.Log("Reenabled renderer");
        //    if (obstructingObject != null)
        //    {
        //        obstructingObject.GetComponent<MeshRenderer>().enabled = true;

        //    }
        //}


        //raycast to rotate diorama
        if (Input.GetMouseButton(0))
        {
            if (isFocused == false)
            {
                currentDiorama.transform.Rotate(new Vector3(0, -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * 500);
            }
        }

        //raycast to check PoI
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                Debug.Log("hit");
                if (hit.collider.gameObject.tag == "PoI")
                {
                    if (isFocused == false)
                    {
                        timePassedCamera = 0;
                    }

                    isFocused = true;

                    //if raycast hits PoI and no previous focus target exists
                    if (focusTarget == null)
                    {
                        Debug.Log("Focused target");
                        focusTarget = hit.collider.gameObject;
                        lastFocusTarget = focusTarget;
                        focusTarget.GetComponent<poiManager>().focus();

                        text.text = focusTarget.GetComponent<poiManager>().text;
                        image.sprite = focusTarget.GetComponent<poiManager>().sprite;
                    }
                    //if raycast hits PoI and a previous focus target exists. Replace current and set previous
                    else
                    {
                        Debug.Log("Replaced lastFocusTarget");

                        lastFocusTarget = focusTarget;
                        lastFocusTarget.GetComponent<poiManager>().unfocused();
                       
                        focusTarget = hit.collider.gameObject;
                        focusTarget.GetComponent<poiManager>().focus();
                        
                        if (focusTarget != lastFocusTarget)
                        {
                            FocusShift();
                        }

                        text.text = focusTarget.GetComponent<poiManager>().text;
                        image.sprite = focusTarget.GetComponent<poiManager>().sprite;
                    }
                }
                else if (hit.collider.gameObject.tag != "PoI")
                {
                    if (focusTarget != null)
                        focusTarget.GetComponent<poiManager>().unfocused();
                    if (isFocused == true)
                    {
                        timePassedCamera = 0;
                    }
                    focusTarget = null;
                    isFocused = false;
                }
            }

        }

        //Zoom out
        if (Input.mouseScrollDelta.y > 0 && mainCamera.transform.localPosition.z > minZoom)
        {
            mainCamera.transform.Translate(new Vector3(0, 0, -2), Space.Self);
            Debug.Log("zoom out");
        }

        //Zoom in
        if (Input.mouseScrollDelta.y < 0 && mainCamera.transform.localPosition.z < maxZoom)
        {
            mainCamera.transform.Translate(new Vector3(0, 0, 2), Space.Self);
            Debug.Log("zoom in");
        }


        if (isFocused == true)
        {

            if (timePassedCamera < cameraMoveDur)
            {
                focusDir = focusTarget.transform.position - mainCamera.transform.position;
                focusRot = Quaternion.LookRotation(focusDir);
                mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, focusRot, 8 * Time.deltaTime);

                timePassedCamera += Time.deltaTime;
            }
            else
            {
                mainCamera.transform.LookAt(focusTarget.transform);
                textBackground.SetActive(true);
            }
        }
        else
        {
            if (timePassedCamera < cameraMoveDur)
            {
                unFocusDir = currentDiorama.transform.position - mainCamera.transform.position;
                unFocusRot = Quaternion.LookRotation(unFocusDir);
                mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, unFocusRot, 8 * Time.deltaTime);

                timePassedCamera += Time.deltaTime;
            }
            else
            {
                mainCamera.transform.LookAt(currentDiorama.transform);
            }
            textBackground.SetActive(false);
        }

        if (DioramaMover.GetComponent<dioramaSelect>().dioramaMovement == true)
        {
            Canvas.enabled = false;
            if (timePassedDiorama < dioramaMoveDur)
            {
                DioramaMover.GetComponent<dioramaSelect>().enabled = false;
                Debug.Log("Lerping");
                DioramaMover.transform.position = Vector3.Lerp(DioramaMover.transform.position, DioramaMover.GetComponent<dioramaSelect>().SelectedDioramaPosition.transform.position, 5 * Time.deltaTime);
                timePassedDiorama += Time.deltaTime;
            }
            else
            {
                Debug.Log("Lerpd");
                DioramaMover.transform.position = DioramaMover.GetComponent<dioramaSelect>().SelectedDioramaPosition.transform.position;
                timePassedDiorama = 0f;
                DioramaMover.GetComponent<dioramaSelect>().dioramaMovement = false;

                currentDiorama = DioramaMover.GetComponent<dioramaSelect>().SelectedDiorama;
                DioramaMover.GetComponent<dioramaSelect>().enabled = true;
                Canvas.enabled = true;
            }
            
            CurrentSelectedDiorama = DioramaMover.GetComponent<dioramaSelect>().SelectedDioramaPosition;
        }        
     }

    public void FocusShift()
    {
        timePassedCamera = 0;
        if (timePassedCamera < cameraMoveDur)
        {
            textBackground.SetActive(false);

            lastFocusDir = focusTarget.transform.position - lastFocusTarget.transform.position;
            lastFocusRot = Quaternion.LookRotation(lastFocusDir);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, lastFocusRot, 0.1f * Time.deltaTime);

            timePassedCamera += Time.deltaTime;
        }
        else
        {
            mainCamera.transform.LookAt(focusTarget.transform);
            textBackground.SetActive(true);
        }
    }
}
