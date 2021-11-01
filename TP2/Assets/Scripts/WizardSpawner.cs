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

    // Donc, un magicien appara�tera 1 seconde apr�s le d�but du jeu.
    private float timeSinceLastSpawn = 4f;

    private void Awake()
    {
        greenWizards = new GameObject[maxNumberOfWizardsPerTeam];
        blueWizards = new GameObject[maxNumberOfWizardsPerTeam];

        // Une seule boucle, il y aura toujours la m�me quantit� de magiciens dans chaque �quipe.
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

            // On d�termine l'�tat des tours.
            GameObject[] greenTowers = gameManager.GetGreenTowers();
            List<GameObject> filteredGreenTowers = new List<GameObject>();

            GameObject[] blueTowers = gameManager.GetBlueTowers();
            List<GameObject> filteredBlueTowers = new List<GameObject>();

            // Une seule boucle, il y aura toujours la m�me quantit� de tours dans chaque �quipe.
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
            // Une seule boucle, il y aura toujours la m�me quantit� de magiciens dans chaque �quipe.
            for (int i = 0; i < maxNumberOfWizardsPerTeam; i++)
            {
                // Magicien vert
                if (!greenWizards[i].activeSelf)
                {
                    greenWizards[i].SetActive(true);

                    // La position du magicien est d�termin�e al�atoirement parmi les tours actives.
                    greenWizards[i].transform.position = filteredGreenTowers[Random.Range(0, filteredGreenTowers.Count)].transform.position;
                }

                // Magicien bleu
                if (!blueWizards[i].activeSelf)
                {
                    blueWizards[i].SetActive(true);

                    blueWizards[i].transform.position = filteredBlueTowers[Random.Range(0, filteredBlueTowers.Count)].transform.position;
                }
            }
        }

        timeSinceLastSpawn += Time.deltaTime;
    }
}
