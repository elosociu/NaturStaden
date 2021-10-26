using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstructiveObject : MonoBehaviour
{
    public float hiddenTimer = 0;
    private bool hidden = false;
    private Renderer rend;

    private void Awake()
    {
        rend = gameObject.GetComponent<Renderer>();
    }
    private void Update()
    {
        if (hiddenTimer > 0)
        {
            if (!hidden)
            {
                hidden = true;
                Transparency(true);
            }
            hiddenTimer -= Time.deltaTime;
        }
        else if (hidden)
            Transparency(false);

    }

    private void Transparency(bool becomeTransparent)
    {
        switch (becomeTransparent)
        {
            case true: rend.enabled = false;
                break;
            case false: rend.enabled = true;
                hidden = false;
                break;
        }
    }
}
