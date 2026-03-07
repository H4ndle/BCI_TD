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
    public TextMeshProUGUI stateText;
    [SerializeField] Animator waveEndAnimator;
    [SerializeField] Animator gameOverAnimator;
    [SerializeField] Animator gameEndAnimator;
    private bool isGameOver = false;
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
        UpdateWaveText();
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

        if(currentHealth <= 0)
            InitiateGameOver();
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

    public void EndWave(bool finalWave)
    {
        waveInProgress = false;
        if (finalWave)
        {
            gameEndAnimator.SetTrigger("GameOver");
            BCITDHelper.instance.DisableAllStimGroups();
        }
        else
        {
            waveEndAnimator.SetTrigger("WaveCleared");
            UpdateWaveText();
            BCITDHelper.instance.DisableAllStimGroups();
            BCITDHelper.instance.ActivateStimGroup(BCITDHelper.StimGroup.Towers);
        }

    }

    public void InitiateGameOver()
    {
        if(!isGameOver) //if not initiated once, then set the trigger to initiate./
            gameOverAnimator.SetTrigger("GameOver");
        isGameOver = true;
    }

    public void UpdateWaveText()
    {
        if(waveInProgress)
            stateText.text = "Defend!";
        else
            stateText.text = "Build Phase";
    }

    public void Restart()
    {
        Application.LoadLevel(0);
    }
}
