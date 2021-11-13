using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardManager : MonoBehaviour
{
    [SerializeField] private Team wizardTeam;
    [SerializeField] private Team wizardOpponentTeam;
    [SerializeField] private HealthBarBehavior healthBar;

    private WizardState wizardState;
    private int healthPoints;
    private int maxHealthPoints;
    private int nbOfKills = 0;

    private const int MIN_HEALTH_POINTS = 50;
    private const int MAX_HEALTH_POINTS = 100;

    public enum WizardStateToSwitch { ESCAPE, HIDDEN, INTREPID, NORMAL, SAFE, INACTIVE }

    private void Awake()
    {
        // ChangeWizardState(WizardStateToSwitch.NORMAL);
        wizardState = GetComponent<WizardState>();
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
            ChangeWizardState(WizardStateToSwitch.INACTIVE);
            gameObject.SetActive(false);
            GameManager.Instance.RemoveWizardCount(wizardTeam);
            attacker.AddKill();
        }
    }

    public void RegenHP(int healAmount)
    {
        healthPoints += healAmount;

        if(healthPoints > maxHealthPoints)
        {
            healthPoints = maxHealthPoints;
        }

        healthBar.SetHealth(healthPoints, maxHealthPoints);
    }

    private void AddKill()
    {
        nbOfKills++;
    }

    public void ChangeWizardState(WizardStateToSwitch nextState)
    {
        Destroy(wizardState);

        switch (nextState)
        {
            case WizardStateToSwitch.ESCAPE:
                {
                    wizardState = gameObject.AddComponent<EscapeState>() as EscapeState;
                    break;
                }
            case WizardStateToSwitch.HIDDEN:
                {
                    wizardState = gameObject.AddComponent<HiddenState>() as HiddenState;
                    break;
                }
            case WizardStateToSwitch.INTREPID:
                {
                    wizardState = gameObject.AddComponent<IntrepidState>() as IntrepidState;
                    break;
                }
            case WizardStateToSwitch.NORMAL:
                {
                    wizardState = gameObject.AddComponent<NormalState>() as NormalState;
                    break;
                }
            case WizardStateToSwitch.SAFE:
                {
                    wizardState = gameObject.AddComponent<SafeState>() as SafeState;
                    break;
                }
            case WizardStateToSwitch.INACTIVE:
                {
                    wizardState = gameObject.AddComponent<InactiveState>() as InactiveState;
                    break;
                }
        }
    }

    public float GetLifePercentage()
    {
        return (float)healthPoints / maxHealthPoints;
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

        public int GetCurrentHealthPoints()
    {
        return healthPoints;
    }

    public int GetMaxHealthPoints()
    {
        return maxHealthPoints;
    }

    public int GetNumberbOfKills()
    {
        return nbOfKills;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GetOpponentProjectileTag()))
        {
            collision.gameObject.SetActive(false);
            TakeDamage(collision.gameObject.GetComponent<ProjectileDamage>().GetDamage(), collision.gameObject.GetComponent<ProjectileDamage>().GetSource());
        }
    }
}
