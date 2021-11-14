using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeState : WizardState
{
    private const float NORMAL_STATE_LIFE_THRESHOLD = 1.0f;

    private const int REGENERATION_PER_SECONDS = 2;

    bool gotAttacked = false;

    private new void Awake()
    {
        base.Awake();
        SetRegenerationPerSeconds(REGENERATION_PER_SECONDS);
    }

    // Not shooting 
    public override void Shoot(){}

    // 
    public override void Move(){}

    /// <summary>
    /// Changements possbiles : Normal, Last Stand, Inactif
    /// </summary>
    public override void ManageStateChange()
    {
        if (wizardManager.GetLifePercentage() >= NORMAL_STATE_LIFE_THRESHOLD)
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.NORMAL);
        }
        else if (gotAttacked)
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.LAST_STAND);
        }
        else if (!wizardManager.IsAlive())
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.INACTIVE);
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
