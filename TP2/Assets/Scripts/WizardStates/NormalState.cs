using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : IWizardState
{
    private GameObject lastTargetEnemy;
    private GameObject closestTower;

    private float timeSinceLastShot = 0;

    private const float ATTACK_SPEED = 0.5f; // 2 tirs / seconde
    private const float MOVEMENT_SPEED = 3.0f;
    private const int REGENERATION_PER_SECONDS = 1;
    private const float MIN_TARGET_RADIUS = 1.5f;
    private const float MAX_TARGET_RADIUS = 3.0f;
    private const float MIN_DAMAGE = 3.0f;
    private const float MAX_DAMAGE = 10.0f;

    private const int INTREPID_STATE_KILL_THRESHOLD = 3;
    private const float ESCAPE_STATE_HEALTH_THRESHOLD = 0.25f; // 25% de vie

    private new void Awake()
    {
        base.Awake();
        SetSpeed(MOVEMENT_SPEED);
        SetRegenerationPerSeconds(REGENERATION_PER_SECONDS);
        targetRadius = Random.Range(MIN_TARGET_RADIUS, MAX_TARGET_RADIUS);
        GetComponent<CircleCollider2D>().radius = targetRadius;
        wizardManager.SetLastForestVisited(null);
    }

    private void Update()
    {
        Regen();
        ManageStateChange();

        // TODO: refactor duplication
        if(lastTargetEnemy != null && IsWizard(lastTargetEnemy))
        {
            WizardManager targetedWizardManager = lastTargetEnemy.GetComponent<WizardManager>();
            if(targetedWizardManager.IsHiddenInForest())
            {
                lastTargetEnemy = null;
            }
        }

        // Si la cible est détruite, on passe à la prochaine
        if (lastTargetEnemy != null && !lastTargetEnemy.activeSelf)
        {
            lastTargetEnemy = null;
        }

        if (lastTargetEnemy != null && lastTargetEnemy.activeSelf && timeSinceLastShot >= ATTACK_SPEED)
        {
            Shoot();
        }

        if (lastTargetEnemy == null)
        {
            Move();
        }

        timeSinceLastShot += Time.deltaTime;
    }

    private int RandomDamageRange()
    {
        return (int)Random.Range(MIN_DAMAGE, MAX_DAMAGE);
    }

    public override void Shoot()
    {
        ProjectileRecycler.Instance.SpawnProjectile(wizardManager, RandomDamageRange(), GetDirectionVector(lastTargetEnemy));
        timeSinceLastShot = 0;
    }

    private Vector3 GetDirectionVector(GameObject target)
    {
        return (target.transform.position - transform.position).normalized;
    }

    public override void Move()
    {
        closestTower = GameManager.Instance.FindClosestTower(transform.position, wizardManager.GetOpponentTeam());
        if (Vector3.Distance(gameObject.transform.position, closestTower.transform.position) >= MIN_TARGET_RADIUS - 1) MoveTo(closestTower);
    }

    /// <summary>
    /// Changements possibles : Intrepide, Fuite, Incactif
    /// </summary>
    public override void ManageStateChange()
    {
        if (wizardManager.GetNumberbOfKills() >= INTREPID_STATE_KILL_THRESHOLD)
        {
            if (wizardManager.ShouldPrintStates())
            {
                print("Normal -> Intrépide");
            }

            wizardManager.ChangeWizardState(WizardState.INTREPID);
        } 
        else if (wizardManager.GetLifePercentage() <= ESCAPE_STATE_HEALTH_THRESHOLD)
        {
            if (wizardManager.ShouldPrintStates())
            {
                print("Normal -> Fuite");
            }

            wizardManager.ChangeWizardState(WizardState.ESCAPE);
        } 
        else if(!wizardManager.IsAlive())
        {
            if (wizardManager.ShouldPrintStates())
            {
                print("Normal -> Inactif");
            }

            wizardManager.ChangeWizardState(WizardState.INACTIVE);
            gameObject.SetActive(false);
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if(IsEnemyTargetable(collision) || collision.gameObject.CompareTag(wizardManager.GetOpponentTowerTag()))
        {
            lastTargetEnemy = collision.gameObject;
            LookAt(lastTargetEnemy);
        }
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (IsEnemyTargetable(collision))
        {
            lastTargetEnemy = null;
        }
    }
}
