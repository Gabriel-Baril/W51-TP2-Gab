using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : WizardState
{
    private const float MIN_TARGET_RADIUS = 1.5f;
    private const float MAX_TARGET_RADIUS = 3.0f;

    private const float MIN_DAMAGE = 3.0f;
    private const float MAX_DAMAGE = 10.0f;

    private GameObject closestTower;
    [SerializeField] private GameObject lastTargetEnemy;

    private float timeSinceLastShot = 0;
    private float shotCooldown = 0.5f; // 0.5 seconde entre chaque tir

    private void Awake()
    {
        base.Awake();
        targetRadius = Random.Range(MIN_TARGET_RADIUS, MAX_TARGET_RADIUS);
        GetComponent<CircleCollider2D>().radius = targetRadius;
    }

    void Update()
    {
        if (lastTargetEnemy != null && lastTargetEnemy.activeSelf && timeSinceLastShot > shotCooldown)
        {
            Shoot();
        }

        Move();
        ManageStateChange();

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
        if (lastTargetEnemy == null)
        {
            closestTower = GameManager.Instance.FindClosestTower(transform.position, wizardManager.GetOpponentTeam());
            MoveTo(closestTower);
        }
    }

    public override void ManageStateChange()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if(collision.gameObject.CompareTag(wizardManager.GetOpponentWizardTag()) || collision.gameObject.CompareTag(wizardManager.GetOpponentTowerTag()))
        {
            lastTargetEnemy = collision.gameObject;
            LookAt(lastTargetEnemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.gameObject.CompareTag(wizardManager.GetOpponentWizardTag()))
        {
            lastTargetEnemy = null;
        }
    }
}
