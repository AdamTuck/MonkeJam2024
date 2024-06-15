using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombItemMono : MonoBehaviour
{
    public GameObject animationPrefab;
    public void Start()
    {
        Destroy(this.gameObject, 3);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bombable"))
        {
            Debug.Log("Bomb hit destroyable Object");
            GameObject animationObj = Instantiate(animationPrefab, transform.position, transform.rotation);
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
