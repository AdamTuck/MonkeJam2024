using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform playerPos;
    [SerializeField] bool mapRotates;

    private void LateUpdate()
    {
        Vector3 newPosition = playerPos.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        if (mapRotates)
            transform.rotation = Quaternion.Euler(90f, playerPos.eulerAngles.y, 0);
    }
}
