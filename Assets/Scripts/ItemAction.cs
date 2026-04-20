using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void OnTriggerStay(Collider other)
    {
        //  Debug.Log("Triggering with: " + other.name);
        if (other.gameObject.CompareTag("Player"))
        {
            transform.position = Vector3.Lerp(transform.position, other.gameObject.transform.position, 0.01f);
        }
    }
}
