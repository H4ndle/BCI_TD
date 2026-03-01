using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] float closeEnough = .01f;
    [SerializeField] GameObject splashDamagePrefab;
    public bool splashDamage;
    public int damage = 0;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            if (Vector3.Distance(transform.position, target.position) > closeEnough)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else
            {
                if (splashDamage)
                {
                    GameObject splash = Instantiate(splashDamagePrefab, target.position, Quaternion.identity);
                    splash.GetComponent<SplashDamageController>().damage = damage;
                }
                else
                {
                    target.GetComponent<EnemyController>().ReceiveDamage(damage);
                }
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
