using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
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
        Debug.Log("TouchSphere triggered with: " + other.name); 
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collecting item, destroying parent...");
            PlayerShipController player = other.gameObject.GetComponent<PlayerShipController>();
            player.gameMode.itemsCollected += 1;
            Destroy(transform.parent.gameObject, 0.01f);
        }
    }
}
