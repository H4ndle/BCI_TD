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
    private int upgradeTier = 0;

    [Header("References")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject damagePrefab;
    [SerializeField] GameObject splashPrefab;
    [SerializeField] GameObject tier2SplashPrefab;
    [SerializeField] GameObject tier2DamagePrefab;
    [SerializeField] Transform modelSocket;
    [SerializeField] Transform radiusObject;
    [SerializeField] EnemyController currentTarget;
    [SerializeField] GameObject upgradeUI;

    public List<EnemyController> potentialTargets = new List<EnemyController>();

    public enum TowerType {Normal, Splash, Damage}
    public TowerType towerType = TowerType.Normal;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        CloseUpgradeUI();
        SetRadius(range);

        anim = GetComponentInChildren<Animator>();
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
                BCITDHelper.instance.DisableStimGroup(BCITDHelper.StimGroup.Upgrades);
                BCITDHelper.instance.ActivateStimGroup(BCITDHelper.StimGroup.Towers);
            }
            else
            {
                transform.parent.BroadcastMessage("CloseUpgradeUI");
                SummonUpgradeUI();
                BCITDHelper.instance.DisableStimGroup(BCITDHelper.StimGroup.Towers);
                BCITDHelper.instance.ActivateStimGroup(BCITDHelper.StimGroup.Upgrades);
            }
        }
    }
    public void RemoteMouseUp()
    {
        OnMouseUpAsButton();
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
        
        if(upgradeTier == 0)
            towerType = TowerType.Damage; //damage type chosen

        //Swap Model
        SwapModel();

        upgradeTier++;
        CloseUpgradeUI();
    }

    public void UpgradeRange(float amount)
    {
        range += amount;
        SetRadius(range);

        //Swap Model
        SwapModel();

        upgradeTier++;
        CloseUpgradeUI();
    }

    public void UpgradeSplash()
    {
        fireRate = 1.7f;
        damage = 2;
        splashDamage = true;
        if (upgradeTier == 0)
            towerType = TowerType.Splash;//damage type chosen

        //Swap Model
        SwapModel();

        upgradeTier++;
        CloseUpgradeUI();
    }

    public void UpgradeSpeed()
    {
        fireRate = fireRate * .75f;

        //Swap Model
        SwapModel();

        upgradeTier++;
        CloseUpgradeUI();
    }
    private void CleanSocket()
    {
        if(modelSocket.childCount > 0)
        {
            for(int i = 0; i < modelSocket.childCount; i++)
            {
                Destroy(modelSocket.GetChild(i).gameObject);
            }
        }
    }
    private void SwapModel()
    {
        anim.SetTrigger("Upgrade");
        CleanSocket();

        if(upgradeTier == 0)
        {
            if(towerType == TowerType.Splash)
                Instantiate(splashPrefab, modelSocket);
            else
                Instantiate(damagePrefab, modelSocket);
        }
        else
        {
            if(towerType == TowerType.Splash)
                Instantiate(tier2SplashPrefab, modelSocket);
            else
                Instantiate(tier2DamagePrefab, modelSocket);
        }
    }
}
