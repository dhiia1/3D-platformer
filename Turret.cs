using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;
    [Header("Attributes")]
    public float range = 27f;
    public float fireRate = 1f;
    public float fireCountdown = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Updatetarget", 0f ,0.5f);
    }
    void Updatetarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (fireCountdown <= 0f && distance <= range)
        {
            shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime * 30 ;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= range)
        {
            FaceTarget();
        }
    }

    void shoot()
    {
        Debug.Log("SHOOT !!");
        GameObject bulletGO = (GameObject) Instantiate(bulletPrefab , firePoint.position ,firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if(bullet != null)
        {
            bullet.seek(target);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
