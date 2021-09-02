using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public player player;
    // Start is called before the first frame update
    void Start()
    {
        player.setUp(100);
    }
    private void Update() {
        //Debug.Log(player.getHealth());
    }
}
