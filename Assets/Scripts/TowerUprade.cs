using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerUprade : MonoBehaviour
{
    [SerializeField] TowerController tower;
    [SerializeField] int upgradeCost;
    [SerializeField] UnityEvent eventToFire;

    private void Start()
    {
        if (!tower)
        {
            tower = GetComponentInParent<TowerController>();
        }
    }
    public void ExecuteUpgrade()
    {
        if (GameManager.instance.currentGold >= upgradeCost)
        {
            GameManager.instance.ModifyGold(-upgradeCost);
            tower.saleValue += upgradeCost;
            eventToFire.Invoke();
        }
    }
}
