using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HordeConfigure
{
    public int hordeCalls = 5;

    public int firstHordeQuantity = 10;
    public int increaseFirstQuantityPercentageByLevel = 10;
    public int increaseQuantityPercentageByHorde = 20;

    public List<Transform> spawnPoints = new List<Transform>();

    public HordeValues GetHordeValues(int currentHorde, int currentLevel)
    {
        int quantity = firstHordeQuantity + ((firstHordeQuantity * (increaseFirstQuantityPercentageByLevel * currentLevel)) / 100) + (currentHorde * increaseQuantityPercentageByHorde) / 100;
        return new HordeValues(quantity);
    }
}

public class HordeValues
{
    public int enemyQuantity;
    public int maxSpawnTime;
    public int minSpawnTime;
    public HordeValues(int quantity) { enemyQuantity = quantity; maxSpawnTime = 10; minSpawnTime = 5; }
}

public class HordeManager : MonoBehaviour
{

    public HordeConfigure hordeConfigure;
    public GameObject bossTemplate;
    public Transform bossSpawn;

    HordeValues currentHordeValues;
    int deaths { get { return default; } set { CheckDeathRemaining(); } }
    int currentHorde = 0;
    int enemysSpawned = 0;

    #region Enemy Horde System

    public void CallNewHordeBySystem()
    {
        currentHorde++;
        currentHordeValues = hordeConfigure.GetHordeValues(currentHorde, 1);
        GameManager.Instance.RestartLife();
        StartCoroutine(HordeCaller());
    }

    GameObject enemySpawned;
    IEnumerator HordeCaller()
    {
        yield return new WaitForSeconds(Random.Range(currentHorde < 2 ? currentHordeValues.minSpawnTime : currentHorde < 4 ? 3 : 0, currentHorde < 2 ? currentHordeValues.maxSpawnTime : currentHorde < 4 ? 8 : 6));
        enemySpawned = GameManager.Instance.poolingSystem.GetEnemyFromQueue();
        enemySpawned.transform.SetParent(null);
        enemySpawned.transform.position = hordeConfigure.spawnPoints[Random.Range(0, hordeConfigure.spawnPoints.Count)].position;
        enemySpawned.SetActive(true);
        enemysSpawned++;
        if (enemysSpawned < currentHordeValues.enemyQuantity) StartCoroutine(HordeCaller());
        else
        {
            bossTemplate.transform.position = new Vector3(bossSpawn.position.x, bossTemplate.transform.position.y, bossSpawn.position.z);
            bossTemplate.SetActive(true);
        }
    }

    public void NewDeathRegister() { deaths--; }

    public void CheckDeathRemaining()
    {
        if (deaths >= currentHordeValues.enemyQuantity)
        {
            if (currentHorde < hordeConfigure.hordeCalls) { if (!IsInvoking("CallNewHordeBySystem")) Invoke("CallNewHordeBySystem", 5); } else { GameManager.Instance.EndGame(true); }
        }
    }

    #endregion

}
