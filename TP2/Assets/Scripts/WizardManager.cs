using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardManager : MonoBehaviour
{
    [SerializeField] private Team wizardTeam;
    [SerializeField] private Team wizardOpponentTeam;
    [SerializeField] private HealthBarBehavior healthBar;

    private IWizardState wizardState;
    private int healthPoints;
    private int maxHealthPoints;
    private int nbOfKills = 0;
    private bool isHiddenInForest = false;

    private const int MIN_HEALTH_POINTS = 50;
    private const int MAX_HEALTH_POINTS = 100;

    private void Awake()
    {
        // ChangeWizardState(WizardState.NORMAL);
        wizardState = GetComponent<IWizardState>();
    }

    /// <summary>
    ///  Determine aleatoirement (entre 2 valeurs fixes) la vie d'un magicien lorsqu'il est active.
    /// </summary>
    private void OnEnable()
    {
        // Maximum est exclusif, donc on fait + 1.
        maxHealthPoints = Random.Range(MIN_HEALTH_POINTS, MAX_HEALTH_POINTS + 1);
        healthPoints = maxHealthPoints;

        healthBar.SetHealth(healthPoints, maxHealthPoints);
    }

    private void TakeDamage(int damage, WizardManager attacker)
    {
        healthPoints -= damage;
        healthBar.SetHealth(healthPoints, maxHealthPoints);

        if (healthPoints <= 0)
        {
            // Magicien est mort.
            ChangeWizardState(WizardState.INACTIVE);
            gameObject.SetActive(false);
            GameManager.Instance.RemoveWizardCount(wizardTeam);
            attacker.AddKill();
        }
    }

    public void RegenHP(int healAmount)
    {
        healthPoints += healAmount;

        if (healthPoints > maxHealthPoints)
        {
            healthPoints = maxHealthPoints;
        }

        healthBar.SetHealth(healthPoints, maxHealthPoints);
    }

    private void AddKill()
    {
        nbOfKills++;
    }

    public void ChangeWizardState(WizardState nextState)
    {
        Destroy(wizardState);

        switch (nextState)
        {
            case WizardState.ESCAPE:
                {
                    wizardState = gameObject.AddComponent<EscapeState>() as EscapeState;
                    break;
                }
            case WizardState.HIDDEN:
                {
                    wizardState = gameObject.AddComponent<HiddenState>() as HiddenState;
                    break;
                }
            case WizardState.INTREPID:
                {
                    wizardState = gameObject.AddComponent<IntrepidState>() as IntrepidState;
                    break;
                }
            case WizardState.NORMAL:
                {
                    wizardState = gameObject.AddComponent<NormalState>() as NormalState;
                    break;
                }
            case WizardState.SAFE:
                {
                    wizardState = gameObject.AddComponent<SafeState>() as SafeState;
                    break;
                }
            case WizardState.LAST_STAND:
                {
                    wizardState = gameObject.AddComponent<LastStandState>() as LastStandState;
                    break;
                }
            case WizardState.INACTIVE:
                {
                    wizardState = gameObject.AddComponent<InactiveState>() as InactiveState;
                    break;
                }
        }
    }

    public void SetHiddenInForest(bool isHidden)
    {
        isHiddenInForest = isHidden;
    }

    public bool IsHiddenInForest()
    {
        return isHiddenInForest;
    }

    public bool IsAlive()
    {
        return GetLifePercentage() > 0.0f;
    }

    public float GetLifePercentage()
    {
        return (float)healthPoints / maxHealthPoints;
    }
    public int GetCurrentHealthPoints()
    {
        return healthPoints;
    }

    public int GetMaxHealthPoints()
    {
        return maxHealthPoints;
    }

    public Team GetTeam()
    {
        return wizardTeam;
    }

    public Team GetOpponentTeam()
    {
        return wizardOpponentTeam;
    }

    public string GetOpponentWizardTag()
    {
        if (GetTeam() == Team.BLUE)
            return Tags.GREEN_WIZARD;
        return Tags.BLUE_WIZARD;
    }

    public string GetOpponentProjectileTag()
    {
        if (GetTeam() == Team.BLUE)
            return Tags.GREEN_PROJECTILE;
        return Tags.BLUE_PROJECTILE;
    }
    public string GetOpponentTowerTag()
    {
        if (GetTeam() == Team.BLUE)
            return Tags.GREEN_TOWER;
        return Tags.BLUE_TOWER;
    }

    public string GetTeamTowerTag()
    {
        if (GetTeam() == Team.BLUE)
            return Tags.BLUE_TOWER;
        return Tags.GREEN_TOWER;
    }

    public int GetNumberbOfKills()
    {
        return nbOfKills;
    }

    public float DistanceFromGameobject(GameObject sourceObject)
    {
        return Vector3.Distance(gameObject.transform.position, sourceObject.gameObject.transform.position);
    }

    public bool IsGettingTouched(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(GetOpponentProjectileTag())) return false;
        return DistanceFromGameobject(collision.gameObject) < 0.5f; // TODO: Refactor 0.32f
    }

    public bool InsideForest(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(Tags.FOREST)) return false;
        return DistanceFromGameobject(collision.gameObject) <= collision.gameObject.GetComponent<BoxCollider2D>().size.x / 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsGettingTouched(collision))
        {
            collision.gameObject.SetActive(false);
            ProjectileDamage projectileDamage = collision.gameObject.GetComponent<ProjectileDamage>();
            TakeDamage(projectileDamage.GetDamage(), projectileDamage.GetSource());
        }
        else if (InsideForest(collision))
        {
            SetHiddenInForest(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!InsideForest(collision))
        {
            SetHiddenInForest(false);
        }
    }
}
