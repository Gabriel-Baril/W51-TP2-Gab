using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSpawner : MonoBehaviour
{
    [SerializeField] private float timeBetweenSpawns = 5f;
    [SerializeField] private int maxNumberOfWizardsPerTeam = 10;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject blueWizardObject;
    [SerializeField] private GameObject greenWizardObject;

    private GameObject[] greenWizards; 
    private GameObject[] blueWizards;

    private const float WIZARD_SPAWN_OFFSET = -0.75f;

    // Donc, un magicien apparaîtera 1 seconde après le début du jeu.
    private float timeSinceLastSpawn = 4f;

    private void Awake()
    {
        greenWizards = new GameObject[maxNumberOfWizardsPerTeam];
        blueWizards = new GameObject[maxNumberOfWizardsPerTeam];

        // Une seule boucle, il y aura toujours la même quantité de magiciens dans chaque équipe.
        for (int i = 0; i < greenWizards.Length; i++)
        {
            greenWizards[i] = Instantiate(greenWizardObject);
            greenWizards[i].SetActive(false);

            blueWizards[i] = Instantiate(blueWizardObject);
            blueWizards[i].SetActive(false);
        }
    }

    private void Update()
    {
        ManageWizardSpawn();
    }

    private void ManageWizardSpawn()
    {
        if(timeSinceLastSpawn >= timeBetweenSpawns)
        {
            timeSinceLastSpawn = 0;

            // On détermine l'état des tours.
            GameObject[] greenTowers = gameManager.GetGreenTowers();
            List<GameObject> filteredGreenTowers = new List<GameObject>();

            GameObject[] blueTowers = gameManager.GetBlueTowers();
            List<GameObject> filteredBlueTowers = new List<GameObject>();

            // Une seule boucle, il y aura toujours la même quantité de tours dans chaque équipe.
            for (int i = 0; i < greenTowers.Length; i++)
            {
                if (greenTowers[i].GetComponent<TowerBehavior>().IsAlive())
                {
                    filteredGreenTowers.Add(greenTowers[i]);
                }

                if (blueTowers[i].GetComponent<TowerBehavior>().IsAlive())
                {
                    filteredBlueTowers.Add(blueTowers[i]);
                }
            }

            // Spawning des magiciens
            // Magicien vert
            for (int i = 0; i < maxNumberOfWizardsPerTeam; i++)
            {
                if (!greenWizards[i].activeSelf)
                {
                    greenWizards[i].SetActive(true);

                    // La position du magicien est déterminée aléatoirement parmi les tours actives.
                    greenWizards[i].transform.position = filteredGreenTowers[Random.Range(0, filteredGreenTowers.Count)].transform.position;
                    
                    // On applique un offset à la position du magicien.
                    Vector3 spawnPoint = greenWizards[i].transform.position;
                    spawnPoint.y += WIZARD_SPAWN_OFFSET;
                    greenWizards[i].transform.position = spawnPoint;

                    break;
                }
            }

            // Magicien bleu
            for (int i = 0; i < maxNumberOfWizardsPerTeam; i++)
            {
                if (!blueWizards[i].activeSelf)
                {
                    blueWizards[i].SetActive(true);

                    // La position du magicien est déterminée aléatoirement parmi les tours actives.
                    blueWizards[i].transform.position = filteredBlueTowers[Random.Range(0, filteredBlueTowers.Count)].transform.position;

                    // On applique un offset à la position du magicien.
                    Vector3 spawnPoint = blueWizards[i].transform.position;
                    spawnPoint.y += WIZARD_SPAWN_OFFSET;
                    blueWizards[i].transform.position = spawnPoint;

                    break;
                }
            }
        }

        timeSinceLastSpawn += Time.deltaTime;
    }
}
