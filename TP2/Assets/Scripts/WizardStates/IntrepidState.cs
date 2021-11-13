using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntrepidState : WizardState
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

    void Update()
    {
        ManageStateChange();

        if (target != null && target.activeSelf && timeSinceLastShot > ATTACK_SPEED)
        {
            Shoot();
        }

        Move();
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
        if (target == null)
        {
            closestTower = GameManager.Instance.FindClosestTower(transform.position, wizardManager.GetOpponentTeam());
            MoveTo(closestTower);
        }
    }

    /// <summary>
    /// Seul changement possible -> Inactif
    /// </summary>
    public override void ManageStateChange()
    {
        if (wizardManager.GetLifePercentage() <= 0.0f)
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.INACTIVE);
        }
    }

    private new void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if(other.gameObject.CompareTag(wizardManager.GetOpponentTowerTag()))
        {
            target = other.gameObject;
            LookAt(other.gameObject);
        }
    }

    private new void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.gameObject.CompareTag(wizardManager.GetOpponentWizardTag()))
        {
            target = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifie si un magicien ennemi l'attaque
        if (collision.gameObject.CompareTag(wizardManager.GetOpponentProjectileTag()))
        {
            target = collision.gameObject.GetComponent<ProjectileDamage>().GetSource().gameObject;
        }
    }
}
