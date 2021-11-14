using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeState : IWizardState
{
    private const float NORMAL_STATE_LIFE_THRESHOLD = 1.0f;

    private const int REGENERATION_PER_SECONDS = 2;

    bool gotAttacked = false;

    private new void Awake()
    {
        base.Awake();
        SetRegenerationPerSeconds(REGENERATION_PER_SECONDS);
    }

    public override void Shoot(){}

    public override void Move(){}

    /// <summary>
    /// Changements possbiles : Normal, Last Stand, Inactif
    /// </summary>
    public override void ManageStateChange()
    {
        if (wizardManager.GetLifePercentage() >= NORMAL_STATE_LIFE_THRESHOLD)
        {
            wizardManager.ChangeWizardState(WizardState.NORMAL);
        }
        else if (gotAttacked)
        {
            wizardManager.ChangeWizardState(WizardState.LAST_STAND);
        }
        else if (!wizardManager.IsAlive())
        {
            wizardManager.ChangeWizardState(WizardState.INACTIVE);
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
}
