using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnSpecies : MonoBehaviour
{
    [Header("Species Prefabs")]
    public GameObject plant;
    public GameObject worm;
    public GameObject rabbit;
    public GameObject wolf;
    public GameObject lion;
    public int wormcount;
    public int plantcount;
    public int rabbitcount;
    public int wolfcount;
    public int lioncount;

    [Header("Spawner Objects")]
    public GameObject plantSpawner;
    public GameObject wormSpawner;
    public GameObject rabbitSpawner;
    public GameObject wolfSpawner;
    public GameObject lionSpawner;

    [Header("Action Points")]
    public GameObject actionmanager;

    [Header("Species Settings")]
    [SerializeField] public int speciesAmount = 0;
    [SerializeField] private TextMeshProUGUI spAmountText;

    // Store all spawned species
    [SerializeField]public List<SpeciesData> plants = new List<SpeciesData>();
    [SerializeField]public List<SpeciesData> worms = new List<SpeciesData>();
    [SerializeField] public List<SpeciesData> rabbits = new List<SpeciesData>();
    [SerializeField] public List<SpeciesData> wolves = new List<SpeciesData>();
    [SerializeField]public List<SpeciesData> lions = new List<SpeciesData>();

    [Header("Sound Effect")]
    [SerializeField] private AudioSource spawnsound;

    private void Update()
    {
        spAmountText.text = "Species Number : " + speciesAmount;
        lioncount = lions.Count;
        wormcount = worms.Count;
        plantcount = plants.Count;
        rabbitcount = rabbits.Count;
        wolfcount = wolves.Count;
        superPlantCount = plants.FindAll(p => p.isSuperUnit).Count;
        superWolfCount = wolves.FindAll(p => p.isSuperUnit).Count;
        superRabbitCount = rabbits.FindAll(p => p.isSuperUnit).Count;
    }

    #region Spawn Methods
    public void PlantSpawn() => SpawnSpeciesPrefab(plant, plantSpawner, plants, "Plant");
    public void WormSpawn() => SpawnSpeciesPrefab(worm, wormSpawner, worms, "Worm");
    public void RabbitSpawn() => SpawnSpeciesPrefab(rabbit, rabbitSpawner, rabbits, "Rabbit");
    public void WolfSpawn() => SpawnSpeciesPrefab(wolf, wolfSpawner, wolves, "Wolf");
    public void LionSpawn() => SpawnSpeciesPrefab(lion, lionSpawner, lions, "Lion");

    private void SpawnSpeciesPrefab(GameObject prefab, GameObject spawner, List<SpeciesData> list, string type)
    {
        if (actionmanager.GetComponent<LifePointsSystem>().actionpoints > 0)
        {
            Vector3 pos = GetRandomPosition(spawner);
            GameObject clone = Instantiate(prefab, pos, Quaternion.identity);

            ShowStats stats = clone.GetComponent<ShowStats>();
            if (stats == null) stats = clone.AddComponent<ShowStats>();
            stats.health = 100f;

            list.Add(new SpeciesData(clone, 100f, type));

            actionmanager.GetComponent<LifePointsSystem>().actionpoints -= 1;
            speciesAmount += 1;
            spawnsound.Play();
        }
    }
    #endregion

    #region Aging & Mechanics
    public void AgeAllSpecies()
    {
        int plantMin = 40, plantMax = 70;
        int wormMin = 20, wormMax = 30;
        int rabbitMin = 40, rabbitMax = 70;
        int wolfMin = 40, wolfMax = 70;
        int lionMin = 40, lionMax = 70;
        var dis = gameObject.GetComponent<DisasterManager>().dis;
        if (gameObject.GetComponent<DaySimulator>().israining)
        {
            AgeSpeciesList(plants, 10, 40);
            AgeSpeciesList(worms, 20, 30);
            AgeSpeciesList(rabbits, 10, 40);
            AgeSpeciesList(wolves, 40, 70);
            AgeSpeciesList(lions, 10, 40);
        }
        if (gameObject.GetComponent<DaySimulator>().isday)
        {
            AgeSpeciesList(plants, 40, 50);
            AgeSpeciesList(worms, 20, 30);
            AgeSpeciesList(rabbits, 40, 70);
            AgeSpeciesList(wolves, 40, 70);
            AgeSpeciesList(lions, 40, 50);
        }
        else if (dis == 11) // Worm disease
        {
            if (superPlantCount > 0)
            {
                wormMin = Mathf.Max(0, wormMin - 10 * superPlantCount);
                wormMax = Mathf.Max(1, wormMax - 50 * superPlantCount);
            }
            AgeSpeciesList(plants, plantMin, plantMax);
            AgeSpeciesList(worms, wormMin + 80, wormMax + 220);
            AgeSpeciesList(rabbits, rabbitMin, rabbitMax);
            AgeSpeciesList(wolves, wolfMin, wolfMax);
            AgeSpeciesList(lions, lionMin, lionMax);
        }

        else if (dis == 12) // Plant disease
        {
            if (superPlantCount > 0)
            {
                plantMin = Mathf.Max(0, plantMin - 10 * superPlantCount);
                plantMax = Mathf.Max(1, plantMax - 50 * superPlantCount);
            }
            AgeSpeciesList(plants, plantMin + 40, plantMax + 180);
            AgeSpeciesList(worms, wormMin, wormMax);
            AgeSpeciesList(rabbits, rabbitMin, rabbitMax);
            AgeSpeciesList(wolves, wolfMin, wolfMax);
            AgeSpeciesList(lions, lionMin, lionMax);
        }
        else if (dis == 13) // Rabbit disease
        {
            if (superPlantCount > 0)
            {
                rabbitMin = Mathf.Max(0, rabbitMin - 10 * superPlantCount);
                rabbitMax = Mathf.Max(1, rabbitMax - 50 * superPlantCount);
            }
            AgeSpeciesList(plants, plantMin, plantMax);
            AgeSpeciesList(worms, wormMin, wormMax);
            AgeSpeciesList(rabbits, rabbitMin + 40, rabbitMax + 180);
            AgeSpeciesList(wolves, wolfMin, wolfMax);
            AgeSpeciesList(lions, lionMin, lionMax);
        }
        else if (dis == 14) // Wolf disease
        {
            if (superPlantCount > 0)
            {
                wolfMin = Mathf.Max(0, wolfMin - 10 * superPlantCount);
                wolfMax = Mathf.Max(1, wolfMax - 50 * superPlantCount);
            }
            AgeSpeciesList(plants, plantMin, plantMax);
            AgeSpeciesList(worms, wormMin, wormMax);
            AgeSpeciesList(rabbits, rabbitMin, rabbitMax);
            AgeSpeciesList(wolves, wolfMin + 40, wolfMax + 180);
            AgeSpeciesList(lions, lionMin, lionMax);
        }
        else if (dis == 15) // Lion disease
        {
            if (superPlantCount > 0)
            {
                lionMin = Mathf.Max(0, lionMin - 10 * superPlantCount);
                lionMax = Mathf.Max(1, lionMax - 50 * superPlantCount);
            }
            AgeSpeciesList(plants, plantMin, plantMax);
            AgeSpeciesList(worms, wormMin, wormMax);
            AgeSpeciesList(rabbits, rabbitMin, rabbitMax);
            AgeSpeciesList(wolves, wolfMin, wolfMax);
            AgeSpeciesList(lions, lionMin + 40, lionMax + 180);
        }
        else if (dis >= 16) // Invasive species
        {
            if (lioncount <= 1)
            {
                AgeSpeciesList(plants, 100, 300);
                AgeSpeciesList(worms, 100, 300);
                AgeSpeciesList(rabbits, 100, 300);
                AgeSpeciesList(wolves, 100, 300);
            }
            else
            {
                AgeSpeciesList(plants, 40, 70);
                AgeSpeciesList(worms, 20, 30);
                AgeSpeciesList(rabbits, 40, 70);
                AgeSpeciesList(wolves, 40, 70);
                AgeSpeciesList(lions, 60, 70);
            }
        }

        else
        {
            AgeSpeciesList(plants, 40, 70);
            AgeSpeciesList(worms, 20, 30);
            AgeSpeciesList(rabbits, 40, 70);
            AgeSpeciesList(wolves, 40, 70);
            AgeSpeciesList(lions, 40, 70);
        }
    }

    private void AgeSpeciesList(List<SpeciesData> list, int minAge, int maxAge)
    {
        foreach (var data in list.ToArray())
        {
            if (data.instance == null)
            {
                list.Remove(data);
                continue;
            }

            // Skip heavy aging for super units
            if (data.isSuperUnit)
            {
                data.health -= Random.Range(minAge, maxAge); // slight aging only
            }
            else
            {
                int aging = Random.Range(minAge, maxAge);
                data.health -= aging;
            }

            // Sync with ShowStats
            ShowStats stats = data.instance.GetComponent<ShowStats>();
            if (stats != null)
                stats.health = data.health;

            if (data.health <= 0)
            {
                Destroy(data.instance);
                list.Remove(data);
                speciesAmount--;
            }
        }
    }

    public void EatSpecies()
    {
        EatFromList(plants, worms, 1f, 15, 50);
        EatFromList(rabbits, plants, 1f, 15, 50);
        EatFromList(wolves, rabbits, 1f, 15, 55);
        EatFromList(lions, wolves, 1f, 15, 50);
    }

    private void EatFromList(List<SpeciesData> predators, List<SpeciesData> preyList, float chance, float damage, float heal)
    {
        for (int i = predators.Count - 1; i >= 0; i--)
        {
            SpeciesData predator = predators[i];
            if (predator.instance == null) continue;

            if (Random.value <= chance && preyList.Count > 0)
            {
                int preyIndex = Random.Range(0, preyList.Count);
                SpeciesData prey = preyList[preyIndex];
                if (prey.instance == null)
                {
                    preyList.RemoveAt(preyIndex);
                    continue;
                }

                prey.health -= damage;
                predator.health += heal;

                ShowStats preyStats = prey.instance.GetComponent<ShowStats>();
                if (preyStats != null) preyStats.health = prey.health;

                ShowStats predStats = predator.instance.GetComponent<ShowStats>();
                if (predStats != null) predStats.health = predator.health;

                if (prey.health <= 0)
                {
                    Destroy(prey.instance);
                    preyList.RemoveAt(preyIndex);
                    speciesAmount--;
                }

                if (!predator.isSuperUnit)
                {
                    predator.health = Mathf.Min(predator.health, 100f);
                }
                else
                {
                    predator.health = Mathf.Min(predator.health, 500f);
                }
            }
        }
    }
    #endregion

    #region Helpers
    private Vector3 GetRandomPosition(GameObject spawner)
    {
        Vector3 scale = spawner.transform.localScale;
        Vector3 pos = spawner.transform.position;
        float x = Random.Range(pos.x - scale.x / 2, pos.x + scale.x / 2);
        float y = Random.Range(pos.y - scale.y / 2, pos.y + scale.y / 2);
        return new Vector3(x, y, 0);
    }
    #endregion
    public int superPlantCount;
    public int superRabbitCount;
    public int superWolfCount;
    [System.Serializable]
    
public class SpeciesData
{
    public GameObject instance;
    public float health;
    public bool isSuperUnit;
    public string speciesType;
    
    public SpeciesData(GameObject obj, float hp, string type, bool super = false)
    {
        instance = obj;
        health = hp;
        speciesType = type;
        isSuperUnit = super;
    }
}

    #region Merge/Super Units
    public void MergePlants() => MergeSpecies(plants, plant, "Plant");
    public void MergeWorms() => MergeSpecies(worms, worm, "Worm");
    public void MergeRabbits() => MergeSpecies(rabbits, rabbit, "Rabbit");
    public void MergeWolves() => MergeSpecies(wolves, wolf, "Wolf");
    public void MergeLions() => MergeSpecies(lions, lion, "Lion");

    private void MergeSpecies(List<SpeciesData> list, GameObject prefab, string speciesName)
    {
        List<SpeciesData> normalUnits = list.FindAll(s => !s.isSuperUnit);
        if (normalUnits.Count < 3)
        {
            Debug.Log($"Not enough {speciesName}s to merge (need 3 normal ones).");
            return;
        }

        GameObject spawner = GetSpawnerByName(speciesName);
        if (spawner == null) return;

        List<SpeciesData> toRemove = new List<SpeciesData>();
        for (int i = 0; i < 3; i++)
        {
            SpeciesData unit = normalUnits[i];
            if (unit.instance != null) Destroy(unit.instance);
            toRemove.Add(unit);
        }

        foreach (var unit in toRemove)
        {
            list.Remove(unit);
            speciesAmount--;
        }

        Vector3 pos = GetRandomPosition(spawner);
        var superUnit = Instantiate(prefab, pos, Quaternion.identity);
        list.Add(new SpeciesData(superUnit, 500f, speciesName, true));

        ShowStats stats = superUnit.GetComponent<ShowStats>();
        if (stats == null) stats = superUnit.AddComponent<ShowStats>();
        stats.health = 500f;
        if (speciesName == "Wolf")
        {
            superUnit.transform.localScale = new Vector3(1, 1, 1);
        }
        else if(speciesName == "Plant")
        {
            superUnit.transform.localScale = new Vector3(3,3,3);
        }
        else
        {
            superUnit.transform.localScale = new Vector3(2, 2, 2);
        }
        
        speciesAmount++;
        Debug.Log($"Merged 3 {speciesName}s into a Super {speciesName} (500 HP).");
    }

    private GameObject GetSpawnerByName(string name)
    {
        switch (name)
        {
            case "Plant": return plantSpawner;
            case "Worm": return wormSpawner;
            case "Rabbit": return rabbitSpawner;
            case "Wolf": return wolfSpawner;
            case "Lion": return lionSpawner;
            default: return null;
        }
    }
    #endregion
}