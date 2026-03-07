using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class TowerSocket : MonoBehaviour
{
    [SerializeField] GameObject towerPrefab;
    [SerializeField] GameObject currentTower;
    [SerializeField] GameObject uiParent;
    [SerializeField] TowerController towerController;

    private void OnMouseDown()
    {
        if (!GameManager.instance.waveInProgress)
        {
            SpawnTower();
        }        
    }

    public void SpawnTower()
    {
        if (!GameManager.instance.waveInProgress && !currentTower && GameManager.instance.currentGold >= towerPrefab.GetComponent<TowerController>().cost)
        {
            currentTower = Instantiate(towerPrefab, transform.position, Quaternion.identity,transform.parent);
            GameManager.instance.ModifyGold(-currentTower.GetComponent<TowerController>().cost);
            uiParent.SetActive(false);
            towerController = currentTower.GetComponent<TowerController>();
        }
    }

    public void SocketInteract()
    {
        if(!currentTower)
            SpawnTower();
        else
        {
            towerController.RemoteMouseUp();
        }
    }
}
