using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStandState : IWizardState
{
    private const int REGENERATION_PER_SECONDS = 2;
    private const float ATTACK_SPEED = 0.25f; // 4 tirs / seconde
    private const float MIN_TARGET_RADIUS = 2.5f;
    private const float MAX_TARGET_RADIUS = 4.0f;
    private const float MIN_DAMAGE = 3.0f;
    private const float MAX_DAMAGE = 5.0f;

    private GameObject target;
    private float timeSinceLastShot = 0;

    private new void Awake()
    {
        base.Awake();
        SetRegenerationPerSeconds(REGENERATION_PER_SECONDS);
        targetRadius = Random.Range(MIN_TARGET_RADIUS, MAX_TARGET_RADIUS);
        GetComponent<CircleCollider2D>().radius = targetRadius;
    }

    private int RandomDamageRange()
    {
        return (int)Random.Range(MIN_DAMAGE, MAX_DAMAGE);
    }

    private void Update()
    {
        Regen();
        ManageStateChange();

        if (target != null && IsWizard(target))
        {
            WizardManager targetedWizardManager = target.GetComponent<WizardManager>();
            if (targetedWizardManager.IsHiddenInForest())
            {
                target = null;
            }
        }

        // Si la cible est d�truite, on passe � la prochaine
        if (target != null && !target.activeSelf)
        {
            target = null;
        }

        if (target != null && target.activeSelf && timeSinceLastShot >= ATTACK_SPEED)
        {
            Shoot();
        }

        if (target == null)
        {
            Move();
        }

        timeSinceLastShot += Time.deltaTime;
    }

    public override void Shoot()
    {
        ProjectileRecycler.Instance.SpawnProjectile(wizardManager, RandomDamageRange(), GetDirectionVector(target));
        timeSinceLastShot = 0;
    }

    private Vector3 GetDirectionVector(GameObject target)
    {
        return (target.transform.position - transform.position).normalized;
    }

    public override void Move(){}

    public override void ManageStateChange()
    {
        if(EnemyAroundCount() <= 0)
        {
            if (wizardManager.ShouldPrintStates())
            {
                print("Last Stand -> Normal");
            }

            wizardManager.ChangeWizardState(WizardState.NORMAL);
        }
        else if (!wizardManager.IsAlive())
        {
            if (wizardManager.ShouldPrintStates())
            {
                print("Last Stand -> Inactif");
            }

            wizardManager.ChangeWizardState(WizardState.INACTIVE);
            gameObject.SetActive(false);
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (IsEnemyTargetable(collision))
        {
            target = collision.gameObject;
            LookAt(target);
        }
    }
    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (IsEnemyTargetable(collision))
        {
            target = null;
        }
    }
}
