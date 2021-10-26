using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewObstructionCheck : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask affectedLayers;

    private void Update()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(transform.position, cameraTransform.position - transform.position, Vector3.Distance(transform.position, cameraTransform.position), affectedLayers);
        //hits = Physics.BoxCastAll(transform.position, , cameraTransform.position - transform.position, Vector3.Distance(transform.position, cameraTransform.position), affectedLayers);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            hit.transform.gameObject.GetComponent<ObstructiveObject>().hiddenTimer = 0.1f;
        }
 
    }

    
}
