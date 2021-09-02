using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanScript : MonoBehaviour
{
    public int rotationSpeed;
    public player player;

    // Update is called once per frame
    void Update()
    {
        if(this.tag == "horizontal")
        {
            transform.Rotate(new Vector3(0f,rotationSpeed * Time.deltaTime , 0f));
        }
        else if(this.tag == "vertical")
        {
           transform.Rotate(new Vector3(rotationSpeed * Time.deltaTime ,0f , 0f)); 
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            Debug.Log("should take dmg");
            player.takeDamage(5);
        }
    }
}
