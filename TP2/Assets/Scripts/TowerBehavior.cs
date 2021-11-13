using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    private const int DEFAULT_TOWER_HEALTH = 200;

    [SerializeField] int towerHealth = DEFAULT_TOWER_HEALTH;
    [SerializeField] private Team towerTeam;
    [SerializeField] private HealthBarBehavior healthBar;

    private void Start()
    {
        healthBar.SetHealth(towerHealth, DEFAULT_TOWER_HEALTH);
    }

    private void Update()
    {
        if(towerHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public bool IsAlive()
    {
        return towerHealth > 0;
    }
    public Team GetTeam()
    {
        return towerTeam;
    }
    public string GetOpponentProjectileTag()
    {
        if (GetTeam() == Team.BLUE)
            return Tags.GREEN_PROJECTILE;
        return Tags.BLUE_PROJECTILE;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GetOpponentProjectileTag()))
        {
            collision.gameObject.SetActive(false);
            towerHealth -= collision.gameObject.GetComponent<ProjectileDamage>().GetDamage();
            healthBar.SetHealth(towerHealth, DEFAULT_TOWER_HEALTH);
        }
    }
}
