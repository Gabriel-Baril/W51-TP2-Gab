using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenState : IWizardState
{
    private const int REGENERATION_PER_SECONDS = 3;
    private const float NORMAL_STATE_LIFE_THRESHOLD = 0.5f;
    private const float ESCAPE_STATE_LIFE_THRESHOLD = 0.25f;

    bool gotAttacked = false;

    private new void Awake()
    {
        base.Awake();
        SetRegenerationPerSeconds(REGENERATION_PER_SECONDS);
        wizardManager.SetHiddenInForest(true);
    }

    private void Update()
    {
        Regen();
        ManageStateChange();
    }

    public override void Shoot(){}

    public override void Move(){}

    /// <summary>
    /// Changement possibles : Normal, Fuite
    /// </summary>
    public override void ManageStateChange()
    {
        float wizardLifePercentage = wizardManager.GetLifePercentage();
        if (wizardLifePercentage >= 1.0f || (wizardLifePercentage >= NORMAL_STATE_LIFE_THRESHOLD && EnemyAroundCount() > 0))
        {
            if (wizardManager.ShouldPrintStates())
            {
                print("Caché -> Normal");
            }

            wizardManager.SetHiddenInForest(false);
            wizardManager.ChangeWizardState(WizardState.NORMAL);
        }
        else if (gotAttacked && wizardLifePercentage <= ESCAPE_STATE_LIFE_THRESHOLD)
        {
            if (wizardManager.ShouldPrintStates())
            {
                print("Caché -> Fuite");
            }

            wizardManager.SetHiddenInForest(false);
            wizardManager.ChangeWizardState(WizardState.ESCAPE);
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (wizardManager.IsGettingTouched(collision))
        {
            gotAttacked = true;
        }
    }
    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}
