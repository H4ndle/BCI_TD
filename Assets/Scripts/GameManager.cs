using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    [SerializeField] LifebarController lifebarController;

    [Header("Gold")]
    public TextMeshProUGUI goldReadout;
    public float startingGold = 100;
    public float currentGold;

    [Header("State")] //We could enumify this if we need more states.
    public bool waveInProgress = false;
    [SerializeField] Animator waveEndAnimator;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        ResetResources();
        UpdateReadout();
    }

    void ResetResources()
    {
        currentHealth = maxHealth;
        currentGold = startingGold;
    }

    public void ReceiveDamage(int amount)
    {
        currentHealth -= amount;
        lifebarController.UpdateReadout(currentHealth, maxHealth);
    }

    public void ModifyGold(int amount)
    {
        currentGold += amount;
        UpdateReadout();
    }

    void UpdateReadout()
    {
        goldReadout.text = currentGold.ToString();
    }

    public void EndWave()
    {
        waveInProgress = false;
        waveEndAnimator.SetTrigger("WaveCleared");
    }
}
