using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRecycler : MonoBehaviour
{
    private const int PROJECTILE_SPEED = 20;
    private const int PROJECTILE_POOL_SIZE = 20;

    private GameObject[] greenProjectilesPool;
    private GameObject[] blueProjectilesPool;
    [SerializeField] private GameObject greenProjectileObject;
    [SerializeField] private GameObject blueProjectileObject;


    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
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

    void SpawnProjectile(WizardManager source)
    {
        GameObject projectile = null;
        if (source.GetTeam() == Team.BLUE)
        {
            projectile = FindFirstDeactivated(blueProjectilesPool);
        }
        else if(source.GetTeam() == Team.GREEN)
        {
            projectile = FindFirstDeactivated(greenProjectilesPool);
        }

        if(projectile != null)
        {
            projectile.SetActive(true);
            projectile.GetComponent<ProjectileDamage>().SetSource(source);
        }
    }

    GameObject FindFirstDeactivated(GameObject[] projectilePool)
    {
        for (int i = 0; i < projectilePool.Length; i++)
        {
            if (!projectilePool[i].activeSelf)
                return projectilePool[i];
        }
        return null;
    }
}
