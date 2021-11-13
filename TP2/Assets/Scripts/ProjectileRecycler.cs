using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRecycler : MonoBehaviour
{
    public static ProjectileRecycler Instance;

    private const int PROJECTILE_POOL_SIZE = 50;

    private GameObject[] greenProjectilesPool;
    private GameObject[] blueProjectilesPool;
    [SerializeField] private GameObject greenProjectileObject;
    [SerializeField] private GameObject blueProjectileObject;

    private void Awake()
    {
        InitInstance();

        greenProjectilesPool = new GameObject[PROJECTILE_POOL_SIZE];
        blueProjectilesPool = new GameObject[PROJECTILE_POOL_SIZE];

        // Une seule boucle, il y aura toujours la même quantité de projectiles dans chaque équipe.
        for (int i = 0; i < PROJECTILE_POOL_SIZE; i++)
        {
            greenProjectilesPool[i] = Instantiate(greenProjectileObject);
            greenProjectilesPool[i].SetActive(false);

            blueProjectilesPool[i] = Instantiate(blueProjectileObject);
            blueProjectilesPool[i].SetActive(false);
        }
    }

    public void SpawnProjectile(WizardManager source, int damage, Vector2 direction)
    {
        GameObject projectile = null;
        
        if (source.GetTeam() == Team.BLUE)
        {
            projectile = FindFirstDeactivated(blueProjectilesPool);
        }
        else
        {
            projectile = FindFirstDeactivated(greenProjectilesPool);
        }

        if(projectile != null)
        {
            ProjectileDamage projDamage = projectile.GetComponent<ProjectileDamage>();
            projectile.SetActive(true);
            projectile.transform.position = source.transform.position + new Vector3(direction.x, direction.y, 0);
            projDamage.SetDirection(direction);
            projDamage.SetSource(source);
            projDamage.SetDamage(damage);
        }
    }

    private GameObject FindFirstDeactivated(GameObject[] projectilePool)
    {
        for (int i = 0; i < projectilePool.Length; i++)
        {
            if (!projectilePool[i].activeSelf)
                return projectilePool[i];
        }
        return null;
    }

    private void InitInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
