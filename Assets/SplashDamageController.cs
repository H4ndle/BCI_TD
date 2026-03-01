using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDamageController : MonoBehaviour
{
    [SerializeField] float radius = 5;
    public int damage = 1;
    [SerializeField] float cleanupDelay = .5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one * radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<EnemyController>().ReceiveDamage(damage);
        
        Invoke("Cleanup", cleanupDelay);
    }

    void Cleanup()
    {
        //GetComponent<Collider>().enabled = false;
        Destroy(gameObject);
    }
}
