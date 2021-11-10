using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeState : WizardState
{
    private bool inTower = false;
    private bool inForest = false;
    private GameObject escapeTarget;
    private new void Awake()
    {
        base.Awake();
        speed = 6.0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentTarget();
        Move();
        ManageStateChange();
    }

    public override void Shoot()
    {
        // Ne tir pas quand il est en fuite
    }

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
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.SAFE);
        }
        else if(inForest)
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.HIDDEN);
        }
        else if (wizardManager.GetLifePercentage() <= 0.0f)
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.DEAD);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(Tags.FOREST))
        {
            inForest = true;
        }
        else if (collision.gameObject.CompareTag(wizardManager.GetTeamTowerTag()))
        {
            inTower = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.FOREST))
        {
            inForest = false;
        }
        else if(collision.gameObject.CompareTag(wizardManager.GetTeamTowerTag()))
        {
            inTower = false;
        }
    }
}
