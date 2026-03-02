using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifebarController : MonoBehaviour
{
    [SerializeField] Image healthBar;

    public void UpdateReadout(int currentHealth, int maxHealth)
    {
        float fillValue = (float)currentHealth / (float)maxHealth;
        healthBar.fillAmount = fillValue;
    }
}
