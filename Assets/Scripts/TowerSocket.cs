using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSocket : MonoBehaviour
{
    [SerializeField] GameObject towerPrefab;
    [SerializeField] GameObject currentTower;

    private void OnMouseDown()
    {
        if (!GameManager.instance.waveInProgress)
        {
            SpawnTower();
        }        
    }

    public void SpawnTower()
    {
        if (!currentTower && GameManager.instance.currentGold >= towerPrefab.GetComponent<TowerController>().cost)
        {
            currentTower = Instantiate(towerPrefab, transform.position, Quaternion.identity,transform.parent);
            GameManager.instance.ModifyGold(-currentTower.GetComponent<TowerController>().cost);
        }
    }
}
