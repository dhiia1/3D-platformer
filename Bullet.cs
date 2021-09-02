using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    public float speed = 15f;
    private player player;
    public void seek (Transform _target)
    {
        target = _target;
    }
    private void Awake() {
        player = FindObjectOfType<player>();
    }
    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 dir =target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            hitTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }
    void hitTarget()
    {
        Debug.Log("we hit somthing");
        player.takeDamage(5);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "barrier")
        {
            Debug.Log("that's a barrier, gotta die");
            Destroy(gameObject);
        }
    }
}
