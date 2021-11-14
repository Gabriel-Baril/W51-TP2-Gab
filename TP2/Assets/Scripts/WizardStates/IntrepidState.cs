using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntrepidState : IWizardState
{
    private GameObject target;
    private GameObject closestTower;

    private float timeSinceLastShot = 0;

    private const float ATTACK_SPEED = 0.5f; // 2 tirs / seconde
    private const float MOVEMENT_SPEED = 4.0f;
    private const int REGENERATION_PER_SECONDS = 2;
    private const float MIN_DAMAGE = 7.0f;
    private const float MAX_DAMAGE = 20.0f;

    private const float MIN_TARGET_RADIUS = 1.5f;
    private const float MAX_TARGET_RADIUS = 3.0f;

    private new void Awake()
    {
        base.Awake();
        SetSpeed(MOVEMENT_SPEED);
        SetRegenerationPerSeconds(REGENERATION_PER_SECONDS);
        targetRadius = Random.Range(MIN_TARGET_RADIUS, MAX_TARGET_RADIUS);
        GetComponent<CircleCollider2D>().radius = targetRadius;
    }

    private void Update()
    {
        Regen();
        ManageStateChange();

        // TODO: refactor duplication
        if (target != null && IsWizard(target))
        {
            WizardManager targetedWizardManager = target.GetComponent<WizardManager>();
            if (targetedWizardManager.IsHiddenInForest())
            {
                target = null;
            }
        }

        // Si la cible est détruite, on passe à la prochaine
        if (target != null && !target.activeSelf)
        {
            target = null;
        }

        if (target != null && target.activeSelf && timeSinceLastShot >= ATTACK_SPEED)
        {
            Shoot();
        }


        if(target == null)
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

    private int RandomDamageRange()
    {
        return (int)Random.Range(MIN_DAMAGE, MAX_DAMAGE);
    }

    public override void Move()
    {
        closestTower = GameManager.Instance.FindClosestTower(transform.position, wizardManager.GetOpponentTeam());
        if (Vector3.Distance(gameObject.transform.position, closestTower.transform.position) > MIN_TARGET_RADIUS) MoveTo(closestTower);
    }

    /// <summary>
    /// Changements possibles : Inactif
    /// </summary>
    public override void ManageStateChange()
    {
        if (!wizardManager.IsAlive())
        {
            if (wizardManager.PrintStates())
            {
                print("Intrépide -> Inactif");
            }

            wizardManager.ChangeWizardState(WizardState.INACTIVE);
            gameObject.SetActive(false);
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        // Si un ennemi l'attque, le magicien l'attaque en retour
        if(collision.gameObject.CompareTag(wizardManager.GetOpponentProjectileTag()) && IsEnemyTargetable(collision))
        {
            target = collision.gameObject;
            LookAt(collision.gameObject);
        }

        // Attaque la tour lorsqu'elle rentre dans sa portée.
        if (collision.gameObject.CompareTag(wizardManager.GetOpponentTowerTag()))
        {
            target = collision.gameObject;
            LookAt(collision.gameObject);
        }
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        // Arrête d'attaquer le magicien ennemi s'il s'enfuit
        if (target == collision.gameObject)
        {
            target = null;
        }
    }
}
