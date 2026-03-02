using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Color normalColor;
    public Color powerColor;
    [SerializeField] float speed = 10;
    [SerializeField] float closeEnough = .01f;
    [SerializeField] GameObject splashDamagePrefab;
    Renderer[] renderers;
    public bool splashDamage;
    public int damage = 0;
    public Transform target;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        SetColor();
    }

    void SetColor()
    {
        
        foreach (Renderer renderer in renderers)
        {
            if (damage > 2) //Look, it's a game jam.
            {
                renderer.material.SetColor("_BaseColor", powerColor);
            }
            else
            {
                renderer.material.SetColor("_BaseColor", normalColor);
            }
        }
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
