using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenState : WizardState
{
    private const float NORMAL_STATE_LIFE_THRESHOLD = 0.5f;
    private const float ESCAPE_STATE_LIFE_THRESHOLD = 0.25f;

    bool gotAttacked = false;

    private void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Shoot()
    {
    }

    public override void Move()
    {
    }

    public override void ManageStateChange()
    {
        float wizardLifePercentage = wizardManager.GetLifePercentage();
        if (EnemyAroundCount() > 0 || wizardLifePercentage >= NORMAL_STATE_LIFE_THRESHOLD)
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.NORMAL);
        }
        else if(gotAttacked && wizardLifePercentage <= ESCAPE_STATE_LIFE_THRESHOLD)
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.ESCAPE);
        }
        else if (wizardManager.GetLifePercentage() <= 0.0f)
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.DEAD);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(wizardManager.GetOpponentProjectileTag()))
        {
        }
    }
}
