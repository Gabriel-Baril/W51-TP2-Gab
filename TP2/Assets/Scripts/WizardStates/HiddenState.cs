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
    }

    public override void Shoot()
    {
    }

    public override void Move()
    {
    }

    /// <summary>
    /// Changement possibles : Normal, Fuite, Inactif
    /// </summary>
    public override void ManageStateChange()
    {
        float wizardLifePercentage = wizardManager.GetLifePercentage();
        if (EnemyAroundCount() > 0 || wizardLifePercentage >= NORMAL_STATE_LIFE_THRESHOLD)
        {
            wizardManager.ChangeWizardState(WizardState.NORMAL);
        }
        else if(gotAttacked && wizardLifePercentage <= ESCAPE_STATE_LIFE_THRESHOLD)
        {
            wizardManager.ChangeWizardState(WizardState.ESCAPE);
        }
        else if (!wizardManager.IsAlive())
        {
            wizardManager.ChangeWizardState(WizardState.INACTIVE);
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag(wizardManager.GetOpponentProjectileTag()))
        {
            gotAttacked = true;
        }
    }
}
