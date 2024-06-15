using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shotgun", menuName = "ScriptableObjects / Useable Items / Shotgun", order = 2)]
public class ShotgunItem : IUseableItem
{
    public LayerMask enemyLayer;
    public override void Use()
    {
        //setCount(-1);
        count--;
        Debug.Log("Used Shotgun");
        Transform playerTransform = PlayerItemInteractor.instance.transform;

        //Shooting animation spawn
        Vector3 offsetFinal;
        Quaternion rotation = Quaternion.LookRotation(playerTransform.forward);
        offsetFinal = rotation * spawnOffset;
        GameObject created = Instantiate(spawnObject, playerTransform.position + offsetFinal, playerTransform.rotation);
        
        //Boxcast and delete hit object
        RaycastHit hit;
        if (Physics.BoxCast(playerTransform.position, new Vector3(5, 5, 5), playerTransform.forward, out hit, playerTransform.rotation, 10f, enemyLayer))
        {
            //Put die function of enemy here
            Destroy(hit.transform.gameObject);
        }
    }

    
}
