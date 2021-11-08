using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardManager : MonoBehaviour
{
    [SerializeField] private Team wizardTeam;
    [SerializeField] private Team wizardOpponentTeam;
    [SerializeField] private HealthBarBehavior healthBar;

    private WizardState wizardState;
    private int healtPoints;
    private int maxHealthPoints;

    private const int MIN_HEALTH_POINTS = 50;
    private const int MAX_HEALTH_POINTS = 100;

    public enum WizardStateToSwitch { DEAD, ESCAPE, HIDDEN, INTREPID, NORMAL, SAFE }

    private void Awake()
    {
        // ChangeWizardState(WizardStateToSwitch.NORMAL);
        wizardState = GetComponent<WizardState>();
    }

    /// <summary>
    ///  D�termine al�atoirement (entre 2 valeurs fixes) la vie d'un magicien lorsqu'il est activ�.
    /// </summary>
    private void OnEnable()
    {
        // Maximum est exclusif, donc on fait + 1.
        maxHealthPoints = Random.Range(MIN_HEALTH_POINTS, MAX_HEALTH_POINTS + 1);
        healtPoints = maxHealthPoints;

        healthBar.SetHeatlh(50, 100);
    }

    private void TakeDamage(int damage)
    {
        healtPoints -= damage;
        healthBar.SetHeatlh(healtPoints, maxHealthPoints);

        if(healtPoints <= 0)
        {
            // Magicien est mort.
            gameObject.SetActive(false);
            GameManager.Instance.RemoveWizardCount(wizardTeam);

            // A FAIRE:
            // AJOUTER UN KILL AU MAGICIEN QUI A ATTAQU�
        }
    }

    public void ChangeWizardState(WizardStateToSwitch nextState)
    {
        Destroy(wizardState);

        switch (nextState)
        {
            case WizardStateToSwitch.DEAD:
                {
                    wizardState = gameObject.AddComponent<DeadState>() as DeadState;
                    break;
                }
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
        }
    }

    public Team GetTeam()
    {
        return wizardTeam;
    }

    public Team GetOpponentTeam()
    {
        return wizardOpponentTeam;
    }

    public string GetOpponentTag()
    {
        if (GetTeam() == Team.BLUE)
            return Tags.GREEN_WIZARD;
        return Tags.BLUE_WIZARD;
    }
}
