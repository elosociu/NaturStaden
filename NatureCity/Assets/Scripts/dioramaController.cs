using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    TextMeshPro text;

    [SerializeField]
    GameObject textBackground;

    public bool isFocused = false;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (isFocused == false)
            {
                rotationPoint.transform.Rotate(new Vector3(0, 0, Input.GetAxis("Mouse X")) * Time.deltaTime * 500);
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (isFocused == false)
            {
                rotationPoint.transform.Translate(new Vector3(Input.GetAxis("Mouse X"), 0, 0), Space.World);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                Debug.Log("hit");
                if (hit.collider.gameObject.tag == "PoI")
                {
                    isFocused = true;

                    if (focusTarget == null)
                    {
                        focusTarget = hit.collider.gameObject;
                        focusTarget.GetComponent<poiManager>().focus();
                        text.text = focusTarget.GetComponent<poiManager>().text;
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
                    isFocused = false;
                }
            }

        }

        //Zoom out
        if (Input.mouseScrollDelta.y > 0 && rotationPoint.transform.position.z < 80)
        {
            transform.position += new Vector3(0, 0, 2);
        }

        //Zoom in
        if (Input.mouseScrollDelta.y < 0 && rotationPoint.transform.position.z > 10)
        {
            transform.position += new Vector3(0, 0, -2);
        }

        if (isFocused == true)
        {
            textBackground.SetActive(true);
        }
        else
        {
            textBackground.SetActive(false);
        }
    }
}
