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

    private void Update()
    {
        Regen();
        UpdateCurrentTarget();
        Move();
        ManageStateChange();
    }

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
            if (wizardManager.PrintStates())
            {
                print("Fuite -> Sécurité");
            }

            wizardManager.ChangeWizardState(WizardState.SAFE);
        }
        else if(inForest)
        {
            if (wizardManager.PrintStates())
            {
                print("Fuite -> Caché");
            }

            wizardManager.ChangeWizardState(WizardState.HIDDEN);
        }
        else if (!wizardManager.IsAlive())
        {
            if (wizardManager.PrintStates())
            {
                print("Fuite -> Inactif");
            }

            wizardManager.ChangeWizardState(WizardState.INACTIVE);
            gameObject.SetActive(false);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(wizardManager.InsideForest(collision))
        {
            inForest = true;
        }
        else if (wizardManager.InsideTower(collision))
        {
            inTower = true;
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (!wizardManager.InsideForest(collision))
        {
            inForest = false;
        }
        else if(!wizardManager.InsideTower(collision))
        {
            inTower = false;
        }
    }
}
