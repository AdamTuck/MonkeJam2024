using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour {
    //Any DestroyEffect gameObject destroys itself 2 sec after being instantiated
    private void Start()
    {
        Destroy(this.gameObject, 2);
    }
}
