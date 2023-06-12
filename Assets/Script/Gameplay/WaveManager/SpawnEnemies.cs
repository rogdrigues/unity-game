using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public static SpawnEnemies instance;
    private const float waitingTime = 2f;
    public GameObject spawnPointUp;
    public GameObject spawnPointDown;
    public GameObject[] enemiesUpWave;
    public int numberOfEnemiesUpWave;
    public GameObject[] enemiesDownWave;
    public int numberOfEnemiesDownWave;
    public int maxEnemiesOnWave;
    public int enemiesOnScreen;
    public int totalEnemies;
    public int enemiesPerSpawn;

    public GameObject HPBarragePrefab;
    public GameObject HPBarragePrefabLeft;
    public float hpBarUpperWaveVerticalOffset = 1.3f;
    public float hpBarDownWaveVerticalOffset = 1.22f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(SpawnWave(enemiesUpWave, numberOfEnemiesUpWave, spawnPointUp, true));
        StartCoroutine(SpawnWave(enemiesDownWave, numberOfEnemiesDownWave, spawnPointDown, false));
    }

    IEnumerator SpawnWave(GameObject[] enemies, int numberOfEnemies, GameObject spawnPoint, bool isUpperWave)
    {
        int spawnedEnemies = 0;
        GameObject enemiesParent = GameObject.Find("Canvas/Manager/Enemies");

        while (spawnedEnemies < numberOfEnemies)
        {
            yield return new WaitForSeconds(waitingTime);

            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (spawnedEnemies < numberOfEnemies && enemiesOnScreen < maxEnemiesOnWave)
                {
                    GameObject newEnemy = Instantiate(enemies[Random.Range(0, enemies.Length)] as GameObject);
                    newEnemy.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, 0f);
                    newEnemy.transform.SetParent(enemiesParent.transform);

                    GameObject newHPBarrage = Instantiate(HPBarragePrefab);
                    GameObject newHPBarrageL = Instantiate(HPBarragePrefabLeft);
                    newHPBarrageL.transform.SetParent(newEnemy.transform);
                    newHPBarrage.transform.SetParent(newEnemy.transform);

                    float verticalOffset = isUpperWave ? hpBarUpperWaveVerticalOffset : hpBarDownWaveVerticalOffset;
                    newHPBarrageL.transform.localPosition = new Vector3(0, verticalOffset, 0);
                    newHPBarrage.transform.localPosition = new Vector3(0, verticalOffset, 0.1f);

                    EnemyStatus enemyStatus = newEnemy.GetComponent<EnemyStatus>();
                    if (enemyStatus != null)
                    {
                        enemyStatus.HPBarrage = newHPBarrage;
                    }
                    enemiesOnScreen++;
                    spawnedEnemies++;
                }
            }

            if (spawnedEnemies >= numberOfEnemies || enemiesOnScreen >= totalEnemies)
            {
                break;
            }
        }
    }
}