using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.CompareTag("Player"))
        {
            PlayerInventory.instance.scrap += 10;
            gameObject.SetActive(false);
        }
    }
}
