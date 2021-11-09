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

    private const float WIZARD_SPAWN_OFFSET_Y = -0.75f;

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
        ManageSpawn();
        timeSinceLastSpawn += Time.deltaTime;
    }

    private void ManageSpawn()
    {
        if (timeSinceLastSpawn >= timeBetweenSpawns)
        {
            timeSinceLastSpawn = 0;
            ManageWizardSpawn(Team.GREEN, greenWizards);
            ManageWizardSpawn(Team.BLUE, blueWizards);
        }
    }
    private void ManageWizardSpawn(Team team, GameObject[] wizardPool)
    {
        List<GameObject> filteredTowers = GameManager.Instance.GetFilteredTowers(team);

        if (filteredTowers.Count <= 0)
            return;

        for (int i = 0; i < maxNumberOfWizardsPerTeam; i++)
        {
            if (!wizardPool[i].activeSelf)
            {
                wizardPool[i].SetActive(true);
                wizardPool[i].transform.right = Vector3.right;

                // La position du magicien est d�termin�e al�atoirement parmi les tours actives.
                wizardPool[i].transform.position = filteredTowers[Random.Range(0, filteredTowers.Count)].transform.position;

                // On applique un offset � la position du magicien.
                Vector3 spawnPoint = wizardPool[i].transform.position;
                spawnPoint.y += WIZARD_SPAWN_OFFSET_Y;
                wizardPool[i].transform.position = spawnPoint;

                GameManager.Instance.AddWizardCount(team);
                break;
            }
        }
    }
}
