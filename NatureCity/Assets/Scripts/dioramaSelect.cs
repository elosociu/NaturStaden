using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dioramaSelect : MonoBehaviour
{
    [SerializeField] Button Diorama1Button;

    [SerializeField] Button Diorama2Button;

    [SerializeField] Button Diorama3Button;

    [SerializeField] public GameObject SelectedDiorama;

    [SerializeField] public GameObject SelectedDioramaPosition;

    

    [SerializeField] GameObject Diorama1;

    [SerializeField] GameObject Diorama2;

    [SerializeField] GameObject Diorama3;

    [SerializeField] GameObject Diorama1SelectedPosition;
                                        
    [SerializeField] GameObject Diorama2SelectedPosition;
                                        
    [SerializeField] GameObject Diorama3SelectedPosition;


    public bool dioramaMovement = false;

    float timePassed = 1f;



    // Start is called before the first frame update
    void Start()
    {
        Button button1 = Diorama1Button;
        button1.onClick.AddListener(diorama1Select);

        Button button2 = Diorama2Button;
        button2.onClick.AddListener(diorama2Select);

        Button button3 = Diorama3Button;
        button3.onClick.AddListener(diorama3Select);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void diorama1Select ()
    {
        Diorama2.GetComponent<dioramaController>().enabled = false;
        Diorama3.GetComponent<dioramaController>().enabled = false;
        Diorama2.SetActive(false);
        Diorama3.SetActive(false);

        //DioramaMover.transform.position = Vector3.Lerp(currentPosition, new Vector3(0, 0, 0), 5);
        Diorama1.GetComponent<dioramaController>().enabled = true;
        Diorama1.SetActive(true);

        SelectedDioramaPosition = Diorama1SelectedPosition;
        SelectedDiorama = Diorama1;

        dioramaMovement = true;
    }

    void diorama2Select()
    {
        Diorama1.GetComponent<dioramaController>().enabled = false;
        Diorama3.GetComponent<dioramaController>().enabled = false;
        Diorama1.SetActive(false);
        Diorama3.SetActive(false);

        //DioramaMover.transform.position = Vector3.Lerp(currentPosition, new Vector3(-150, 0, 0), 5);
        Diorama2.GetComponent<dioramaController>().enabled = true;
        Diorama2.SetActive(true);

        SelectedDioramaPosition = Diorama2SelectedPosition;
        SelectedDiorama = Diorama2;

        dioramaMovement = true;
    }

    void diorama3Select()
    {
        Diorama1.GetComponent<dioramaController>().enabled = false;
        Diorama2.GetComponent<dioramaController>().enabled = false;
        Diorama1.SetActive(false);
        Diorama2.SetActive(false);
        //DioramaMover.transform.position = Vector3.Lerp(currentPosition, new Vector3(-300, 0, 0), 5);
        Diorama3.GetComponent<dioramaController>().enabled = true;
        Diorama3.SetActive(true);

        SelectedDioramaPosition = Diorama3SelectedPosition;
        SelectedDiorama = Diorama3;

        dioramaMovement = true;
    }

}
