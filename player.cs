using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private int health;
    private int maxHealth;
    public healthBar healthBar;
    Animator animator;
    public float invincibilityLength;
    public float invincibiltyCounter;
    public Component[] minimesh;
    public bool meshIsOn;
    public float flashCounter;
    public float flashLength;
    public bool isRespawning;
    private Vector3 respawnPoint;
    public float respawnLenght = 3f;
    int isDyingHash;
    public void setUp(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }
    public int getHealth()
    {
        return health;
    }
    public void takeDamage(int damageAmount)
    {
        if (invincibiltyCounter <= 0)
        {
            health -= damageAmount;
            if (health < 0)
            {
                respawn();
                health = 0;
            }
            else
            {
                invincibiltyCounter = invincibilityLength;
                meshOff();
                flashCounter = flashLength;
                healthBar.setHealth(health);
            }
        }

    }
    public void heal(int healAmount)
    {
        health += healAmount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthBar.setHealth(health);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            takeDamage(30);
        }
    }
    void meshOff()
    {
        foreach (SkinnedMeshRenderer mesh in minimesh)
        {
            mesh.enabled = false;
            meshIsOn = false;
        }
    }
    void meshOn()
    {
        foreach (SkinnedMeshRenderer mesh in minimesh)
        {
            mesh.enabled = true;
            meshIsOn = true;
        }
    }
    private void Update()
    {
        if (invincibiltyCounter > 0)
        {
            invincibiltyCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;
            if (flashCounter > 0)
            {
                if (meshIsOn)
                {
                    meshOff();
                }
                else
                {
                    meshOn();
                }
            }
            flashCounter = flashLength;
        }
        if (invincibiltyCounter <= 0)
        {
            meshOn();
        }
    }
    public void respawn()
    {
        /*CharacterController characterController = GetComponent<CharacterController>();
        characterController.enabled = false;
        transform.position = respawnPoint;
        characterController.enabled = true;
        health = maxHealth;*/
        if(! isRespawning)
        {
            StartCoroutine("respawnCo");
        }
    }
    public IEnumerator respawnCo()
    {
        animator.SetBool(isDyingHash,true);
        isRespawning = true;
        //meshOff();

        yield return new WaitForSeconds(respawnLenght);

        Debug.Log("post corotine check");
        isRespawning = false;
        animator.SetBool(isDyingHash,false);
        //meshOn();
        CharacterController characterController = GetComponent<CharacterController>();
        characterController.enabled = false;
        transform.position = respawnPoint;
        characterController.enabled = true;
        health = maxHealth;
        healthBar.setHealth(health);

    }
    private void Awake() {
        animator = GetComponent<Animator>();
        isDyingHash = Animator.StringToHash("isDying");
    }
    private void Start()
    {
        minimesh = GetComponentsInChildren<SkinnedMeshRenderer>();
        respawnPoint = transform.position;
    }
}