using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gemScoreSystem : MonoBehaviour
{
    public GameObject gemText;
    public int gems;
    //public AudioSource collectSound;

    private void OnTriggerEnter(Collider other) {
       // collectSound.Play();
        gems += 1;
        gemText.GetComponent<Text>().text = "Gems: " +  gems;
        Destroy(gameObject);
    }
}
