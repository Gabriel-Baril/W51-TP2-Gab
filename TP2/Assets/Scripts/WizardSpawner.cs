using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSpawner : MonoBehaviour
{
    private const float WIZARD_SPAWN_OFFSET_Y = -0.75f;

    [SerializeField] private float timeBetweenSpawns = 6f;
    [SerializeField] private int maxNumberOfWizardsPerTeam = 6;
    [SerializeField] private GameObject blueWizardObject;
    [SerializeField] private GameObject greenWizardObject;

    private GameObject[] greenWizards; 
    private GameObject[] blueWizards;

    private float timeSinceLastSpawn;

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

        // Donc, un magicien apparaîtera 1 seconde après le début du jeu.
        timeSinceLastSpawn = timeBetweenSpawns - 1f;
    }

    private void Update()
    {
        ManageSpawn();
        timeSinceLastSpawn += Time.deltaTime;
    }

    private void ManageSpawn()
    {
        if (!GameManager.Instance.GameOver() && timeSinceLastSpawn >= timeBetweenSpawns)
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

                // Le magicien commence à l'état normal.
                wizardPool[i].GetComponent<WizardManager>().ChangeWizardState(WizardState.NORMAL);

                // La position du magicien est déterminée aléatoirement parmi les tours actives.
                wizardPool[i].transform.position = filteredTowers[Random.Range(0, filteredTowers.Count)].transform.position;

                // On applique un offset à la position du magicien.
                Vector3 spawnPoint = wizardPool[i].transform.position;
                spawnPoint.y += WIZARD_SPAWN_OFFSET_Y;
                wizardPool[i].transform.position = spawnPoint;

                GameManager.Instance.AddWizardCount(team);
                break;
            }
        }
    }

    public void StopAllWizards()
    {
        for(int i = 0; i < maxNumberOfWizardsPerTeam; i++)
        {
            if (blueWizards[i].activeSelf)
            {
                blueWizards[i].GetComponent<WizardManager>().ChangeWizardState(WizardState.INACTIVE);
            }

            if (greenWizards[i].activeSelf)
            {
                greenWizards[i].GetComponent<WizardManager>().ChangeWizardState(WizardState.INACTIVE);
            }
        }
    }
}
