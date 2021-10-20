using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class dioramaController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject rotationPoint;

    [SerializeField]
    GameObject focusTarget;

    [SerializeField]
    GameObject lastFocusTarget;

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    float maxZoom;

    [SerializeField]
    float minZoom;

    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    GameObject textBackground;

    [SerializeField]
    Image image;


    public bool isFocused = false;

    public Vector3 dioramaUnfocusedPos = new Vector3(0, 30, -10);

    public Vector3 dioramaFocusedPos = new Vector3(-4.5f, 30, -10);

    float timePassed = 1f;

    float dioramaMoveDur = 0.3f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //raycast to check focus
        if (Input.GetMouseButton(0))
        {
            if (isFocused == false)
            {
                rotationPoint.transform.Rotate(new Vector3(0, -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * 500);
            }
        }

        //if (Input.GetMouseButton(1))
        //{
        //    if (isFocused == false)
        //    {
        //        rotationPoint.transform.Translate(new Vector3(Input.GetAxis("Mouse X"), 0, 0), Space.World);
        //    }
        //}

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
                        timePassed = 0;
                    }

                    isFocused = true;

                    if (focusTarget == null)
                    {
                        focusTarget = hit.collider.gameObject;
                        focusTarget.GetComponent<poiManager>().focus();
                        text.text = focusTarget.GetComponent<poiManager>().text;
                        image.sprite = focusTarget.GetComponent<poiManager>().sprite;
                    }
                    else
                    {
                        Debug.Log("Replaced lastFocusTarget");
                        lastFocusTarget = focusTarget;
                        lastFocusTarget.GetComponent<poiManager>().unfocused();
                        focusTarget = hit.collider.gameObject;
                        focusTarget.GetComponent<poiManager>().focus();
                        text.text = focusTarget.GetComponent<poiManager>().text;
                    }
                }
                else if (hit.collider.gameObject.tag != "PoI")
                {
                    focusTarget.GetComponent<poiManager>().unfocused();
                    if (isFocused == true)
                    {
                        timePassed = 0;
                    }
                    isFocused = false;
                }
            }

        }

        //Zoom out
        if (Input.mouseScrollDelta.y > 0 && mainCamera.transform.localPosition.z > minZoom)
        {
            mainCamera.transform.Translate(new Vector3(0, 0, -2), Space.Self);
            dioramaFocusedPos.z = mainCamera.transform.localPosition.z;
            dioramaFocusedPos.y = mainCamera.transform.localPosition.y;
            dioramaUnfocusedPos.z = mainCamera.transform.localPosition.z;
            dioramaUnfocusedPos.y = mainCamera.transform.localPosition.y;
            Debug.Log("zoom out");
        }

        //Zoom in
        if (Input.mouseScrollDelta.y < 0 && mainCamera.transform.localPosition.z < maxZoom)
        {
            mainCamera.transform.Translate(new Vector3(0, 0, 2), Space.Self);
            dioramaFocusedPos.z = mainCamera.transform.localPosition.z;
            dioramaFocusedPos.y = mainCamera.transform.localPosition.y;
            dioramaUnfocusedPos.z = mainCamera.transform.localPosition.z;
            dioramaUnfocusedPos.y = mainCamera.transform.localPosition.y;
            Debug.Log("zoom in");
        }


        //Pans camera to the right 
        if (isFocused == true)
        {
            if(timePassed < dioramaMoveDur)
            {
                mainCamera.transform.position = Vector3.Lerp(dioramaUnfocusedPos, dioramaFocusedPos, timePassed / dioramaMoveDur);
                timePassed += Time.deltaTime;
            }
            else
            {
                mainCamera.transform.position = dioramaFocusedPos;
                textBackground.SetActive(true);
            }
            
        }
        //Pans camera to the left 
        else
        {
            if (timePassed < dioramaMoveDur)
            {
                mainCamera.transform.position = Vector3.Lerp(dioramaFocusedPos, dioramaUnfocusedPos, timePassed / dioramaMoveDur);
                timePassed += Time.deltaTime;
            }
            else
            {
                /*mainCamera.transform.position = dioramaUnfocusedPos*/;
            }
            textBackground.SetActive(false);
        }
    }
}
