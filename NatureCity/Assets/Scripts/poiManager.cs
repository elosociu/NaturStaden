using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class poiManager : MonoBehaviour
{

    public string text;
    public Sprite sprite;
    [EventRef, SerializeField] string focusAudio;
    [EventRef, SerializeField] string unfocusAudio;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void focus()
    {
        FMODUnity.RuntimeManager.PlayOneShot(focusAudio);

    }

    public void unfocused()
    {
        FMODUnity.RuntimeManager.PlayOneShot(unfocusAudio);

    }
}
