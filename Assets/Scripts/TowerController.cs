using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerController : MonoBehaviour
{
    [Header ("Parameters")]
    [SerializeField] float range = 15;   
    [SerializeField] float fireRate = 3;
    [SerializeField] float firingTimer;
    [SerializeField] int damage = 1;
    [SerializeField] bool splashDamage;
    public int cost = 50;
    public int saleValue = 50;

    [Header("References")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject damagePrefab;
    [SerializeField] GameObject splashPrefab;
    [SerializeField] Transform radiusObject;
    [SerializeField] EnemyController currentTarget;
    [SerializeField] GameObject upgradeUI;

    public List<EnemyController> potentialTargets = new List<EnemyController>();

    // Start is called before the first frame update
    void Start()
    {
        CloseUpgradeUI();
        SetRadius(range);
    }

    public void SetRadius(float radius)
    {
        range = radius;
        radiusObject.localScale = new Vector3(range, .2f, range);
    }

    // Update is called once per frame
    void Update()
    {
        if (firingTimer > 0)
        {
            firingTimer -= Time.deltaTime;
        }
        else
        {
            if (currentTarget)
            {
                ProjectileController bullet = Instantiate(projectilePrefab,transform.position, Quaternion.identity).GetComponent<ProjectileController>();
                bullet.target = currentTarget.transform;
                bullet.damage = damage;
                bullet.splashDamage = splashDamage;
                //currentTarget.ReceiveDamage(damage);
                firingTimer = fireRate;
            }
        }

        if (!currentTarget && potentialTargets.Count > 0)
        {
            if (potentialTargets[0] == null)
            {
                potentialTargets.RemoveAt(0);
            }
            else
            {
                currentTarget = potentialTargets[0];
            }              
        }

        if (GameManager.instance.waveInProgress)
        {
            CloseUpgradeUI();
        }
    }

    private void OnMouseUpAsButton()
    {
        if (GameManager.instance.waveInProgress)
        {

        }
        else if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (upgradeUI.activeInHierarchy)
            {
                CloseUpgradeUI();                
            }
            else
            {
                transform.parent.BroadcastMessage("CloseUpgradeUI");
                SummonUpgradeUI();
            }
        }
    }

    //NOTE: Towers only interact with the enemy physics layer, so we don't need to check tags here
    //and can assume they have the EnemyController component.
    private void OnTriggerEnter(Collider other)
    {
        potentialTargets.Add(other.GetComponent<EnemyController>());

    }

    private void OnTriggerExit(Collider other)
    {
        potentialTargets.Remove(other.GetComponent<EnemyController>());
        if (other.GetComponent<EnemyController>() == currentTarget)
        {
            currentTarget = null;
        }
    }

    public void SummonUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
    }

    public void SellTower()
    {
        GameManager.instance.ModifyGold(saleValue);
        Destroy(gameObject);
    }

    public void UpgradeDamage(int amount)
    {
        damage += amount;
        CloseUpgradeUI();
    }

    public void UpgradeRange(float amount)
    {
        range += amount;
        SetRadius(range);
        CloseUpgradeUI();
    }

    public void UpgradeSplash()
    {
        fireRate = 1.7f;
        damage = 2;
        splashDamage = true;
        CloseUpgradeUI();
    }

    public void UpgradeSpeed()
    {
        fireRate = fireRate * .75f;
        CloseUpgradeUI();
    }
}
