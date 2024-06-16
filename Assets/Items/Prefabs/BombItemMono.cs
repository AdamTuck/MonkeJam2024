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
        GameObject animationObj = Instantiate(animationPrefab, transform.position, transform.rotation);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bombable"))
        {
            Debug.Log("Bomb hit destroyable Object");
            Destroy(collision.gameObject);
        }else
        {
            //Potential box cast to see if any enemies are around and killing them?
        }
        Destroy(this.gameObject);
    }
}
