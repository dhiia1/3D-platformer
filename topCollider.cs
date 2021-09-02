using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class topCollider : MonoBehaviour
{
    public bool fatalCollusion = false;
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            fatalCollusion = true;
        }
    }
}
