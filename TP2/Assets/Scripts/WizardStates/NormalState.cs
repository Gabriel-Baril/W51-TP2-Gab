using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : WizardState
{
    private const float MIN_TARGET_RADIUS = 1.5f;
    private const float MAX_TARGET_RADIUS = 4.0f;

    private GameObject closestTower;
    private GameManager manager;
    [SerializeField] private GameObject lastTargetEnemy;

    private void Awake()
    {
        base.Awake();
        targetRadius = Random.Range(MIN_TARGET_RADIUS, MAX_TARGET_RADIUS);
        GetComponent<CircleCollider2D>().radius = targetRadius;
    }

    void Start()
    {
        manager = GameManager.Instance;
    }

    void Update()
    {
        closestTower = manager.FindClosestTower(transform.position, wizardManager.GetOpponentTeam());
        Move();
        ManageStateChange();
    }

    public override void Shoot()
    {

    }
    public override void Move()
    {
        if (lastTargetEnemy != null)
        {
            MoveTo(lastTargetEnemy);
            transform.up = lastTargetEnemy.transform.position - transform.position;
        }
        else
        {
            MoveTo(closestTower);
            transform.up = closestTower.transform.position - transform.position;
        }
    }

    public override void ManageStateChange()
    {
    }

    private void MoveTo(GameObject target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        // float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, target.transform.eulerAngles, speed * Time.deltaTime);
        // transform.eulerAngles = new Vector3(0, angle, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if(other.gameObject.CompareTag(wizardManager.GetOpponentTag()))
        {
            lastTargetEnemy = other.gameObject;
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
