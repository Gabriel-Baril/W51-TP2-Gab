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
        if (lastTargetEnemy != null && timeSinceLastShot > shotCooldown)
        {
            Shoot();
        }

        Move();
        ManageStateChange();

        timeSinceLastShot += Time.deltaTime;
    }

    private float RandomDamageRange()
    {
        return Random.Range(MIN_DAMAGE, MAX_DAMAGE);
    }

    public override void Shoot()
    {
        ProjectileRecycler.Instance.SpawnProjectile(wizardManager, RandomDamageRange(), lastTargetEnemy.transform.position - transform.position);
        timeSinceLastShot = 0;
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

    private void MoveTo(GameObject target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        LookAt(target);
    }

    private void LookAt(GameObject target)
    {
        transform.up = target.transform.position - transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if(collision.gameObject.CompareTag())
        // {
        // 
        // }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if(other.gameObject.CompareTag(wizardManager.GetOpponentTag()))
        {
            lastTargetEnemy = other.gameObject;
            LookAt(lastTargetEnemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.gameObject.CompareTag(wizardManager.GetOpponentTag()))
        {
            lastTargetEnemy = null;
        }
    }
}
