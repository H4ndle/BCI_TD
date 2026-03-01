using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;


public class EnemyController : MonoBehaviour
{
    SplineContainer path;
    SplineAnimate splineAnimator;

    [SerializeField] int damageAmount = 5;

    [SerializeField] int maxHealth = 3;
    [SerializeField] int currentHealth;
    [SerializeField] LifebarController healthReadout;
    [SerializeField] GameObject deathEffect;

    [SerializeField] int goldValue = 1;


    // Start is called before the first frame update
    void Start()
    {
        path = GameObject.FindGameObjectWithTag("Path").GetComponent<SplineContainer>();
        splineAnimator = GetComponent<SplineAnimate>();
        splineAnimator.Container = path;
        splineAnimator.Play();

        healthReadout.gameObject.SetActive(false);
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keep"))
        {
            GameManager.instance.ReceiveDamage(damageAmount);
            Destroy(gameObject);
        }
    }

    public void ReceiveDamage(int amount)
    {
        healthReadout.gameObject.SetActive(true);
        currentHealth -= amount;

        healthReadout.UpdateReadout(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            GameManager.instance.ModifyGold(goldValue);
            deathEffect.transform.parent = null;
            deathEffect.GetComponentInChildren<TextMeshPro>().text = "+" + goldValue.ToString();
            deathEffect.SetActive(true);
            Destroy(gameObject);
        }
    }
}
