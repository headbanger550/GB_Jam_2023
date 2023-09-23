using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

//Wave Spawner "ThatOneUnityDev" on youtube 

[System.Serializable]
public class EnemyObject
{
    public GameObject enemyPrefab;
    public int cost;
}

public class WaveSystem : MonoBehaviour
{
    [SerializeField] List<EnemyObject> enemies = new List<EnemyObject>();
    [SerializeField] List<GameObject> enemiesToSpawn = new List<GameObject>();

    [Space]

    [SerializeField] int currWave;
    [SerializeField] int waveValue;

    [Space]

    [SerializeField] Transform spawnPos;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    [HideInInspector] public int enemyCount;
    [HideInInspector] public int endedWaves;

    private int waveDuration;
    private float waveTimer;
    
    private float spawnInterval;
    private float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        GenerateWave();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(spawnTimer <= 0)
        {
            if(enemiesToSpawn.Count > 0)
            {
                GameObject obj = Instantiate(enemiesToSpawn[0], spawnPos.position, Quaternion.identity);
                spawnedEnemies.Add(obj);
                enemiesToSpawn.RemoveAt(0);
                spawnTimer = spawnInterval;
            }
            else
            {
                waveTimer = 0;
            }
        }
        else
        {
            spawnTimer -= 0.01f;
            waveTimer -= 0.01f;
        }

        if(enemyCount == spawnedEnemies.Count)
        {
            //The wave ends
            endedWaves++;
        }
    }

    public void GenerateWave()
    {
        waveValue = currWave * 10;
        GenerateEnemies();

        spawnInterval = waveDuration/enemiesToSpawn.Count;
        waveTimer = waveDuration;
    }

    void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while(waveValue > 0)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if(waveValue-randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if(waveValue <= 0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }
}
