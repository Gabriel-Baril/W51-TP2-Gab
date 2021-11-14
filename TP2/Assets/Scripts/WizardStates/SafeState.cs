using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeState : IWizardState
{
    private GameObject tower;
    private bool towerAttacked = false;
    private float towerHealth;

    private const int REGENERATION_PER_SECONDS = 5;
    private const float NORMAL_STATE_LIFE_THRESHOLD = 1.0f;

    private new void Awake()
    {
        base.Awake();
        SetRegenerationPerSeconds(REGENERATION_PER_SECONDS);

        tower = GameManager.Instance.FindClosestTower(transform.position, wizardManager.GetTeam());

        towerHealth = tower.GetComponent<TowerBehavior>().GetTowerHealth();
    }

    private void Update()
    {
        Regen();

        if(towerHealth != tower.GetComponent<TowerBehavior>().GetTowerHealth())
        {
            // La vie de la tour baisse, donc elle se fait attaquer.
            towerAttacked = true;
        }

        ManageStateChange();
    }

    public override void Shoot() { }

    public override void Move() { }

    /// <summary>
    /// Changements possbiles : Normal, Last Stand
    /// </summary>
    public override void ManageStateChange()
    {
        if (wizardManager.GetLifePercentage() >= NORMAL_STATE_LIFE_THRESHOLD)
        {
            if (wizardManager.ShouldPrintStates())
            {
                print("Sécurité -> Normal");
            }

            wizardManager.ChangeWizardState(WizardState.NORMAL);
        }
        else if (towerAttacked)
        {
            if (wizardManager.ShouldPrintStates())
            {
                print("Sécurité -> Last Stand");
            }

            wizardManager.ChangeWizardState(WizardState.LAST_STAND);
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}
