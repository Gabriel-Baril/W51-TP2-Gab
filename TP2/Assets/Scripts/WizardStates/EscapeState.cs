using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeState : IWizardState
{
    private const float MOVEMENT_SPEED = 6.0f;

    private const int REGENERATION_PER_SECONDS = 1;

    private bool inTower = false;
    private bool inForest = false;
    private GameObject escapeTarget;
    private new void Awake()
    {
        base.Awake();
        SetSpeed(MOVEMENT_SPEED);
        SetRegenerationPerSeconds(REGENERATION_PER_SECONDS);
    }

    void Update()
    {
        UpdateCurrentTarget();
        Move();
        ManageStateChange();
    }

    // Ne tir pas quand il est en fuite 
    public override void Shoot() {}

    public override void Move()
    {
        if (escapeTarget != null)
        {
            MoveTo(escapeTarget);
        }
    }

    public override void ManageStateChange()
    {
        // Se cache dans la forêt ou la tour la plus proche
        if (inTower)
        {
            wizardManager.ChangeWizardState(WizardState.SAFE);
        }
        else if(inForest)
        {
            wizardManager.ChangeWizardState(WizardState.HIDDEN);
        }
        else if (!wizardManager.IsAlive())
        {
            wizardManager.ChangeWizardState(WizardState.INACTIVE);
        }
    }

    private void UpdateCurrentTarget()
    {
        GameObject closestForest = GameManager.Instance.FindClosestForest(transform.position);
        GameObject closestTower = GameManager.Instance.FindClosestTower(transform.position, wizardManager.GetTeam());

        escapeTarget = closestForest;
        if (closestTower != null && Vector3.Distance(transform.position, closestTower.transform.position) < Vector3.Distance(transform.position, closestForest.transform.position))
        {
            escapeTarget = closestTower;
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if(wizardManager.InsideForest(collision))
        {
            inForest = true;
        }
        else if (collision.gameObject.CompareTag(wizardManager.GetTeamTowerTag()))
        {
            inTower = true;
        }
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        if (!wizardManager.InsideForest(collision))
        {
            inForest = false;
        }
        else if(collision.gameObject.CompareTag(wizardManager.GetTeamTowerTag()))
        {
            inTower = false;
        }
    }
}
