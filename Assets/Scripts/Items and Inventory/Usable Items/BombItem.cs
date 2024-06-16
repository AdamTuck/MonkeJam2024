using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bomb", menuName = "ScriptableObjects / Useable Items / Bomb", order = 1)]
public class BombItem : IUseableItem
{
    public override void Use()
    {
        count--;
        playerTransform = PlayerItemInteractor.instance.itemPos;
        //Bomb spawn with offset
        Vector3 offsetFinal;
        Quaternion rotation = Quaternion.LookRotation(playerTransform.forward);
        offsetFinal = rotation * spawnOffset;
        GameObject created = Instantiate(spawnObject, playerTransform.position + offsetFinal, playerTransform.rotation);

        created.GetComponent<Rigidbody>().AddForce(PlayerMovementBehaviour.instance.transform.forward * PlayerMovementBehaviour.instance.currentSpeed * Time.deltaTime, ForceMode.Impulse);
        //Throw created
        created.GetComponent<Rigidbody>().AddForce((created.transform.forward * 2 + created.transform.up) * 7, ForceMode.Impulse);
    }
}
