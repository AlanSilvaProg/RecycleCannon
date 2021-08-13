using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public LifeStats[] lifeStats;

    [HideInInspector]public int playerLife;

    public void ChangeLife(int value)
    {
        if (value == -1)
        {
            lifeStats[playerLife - 1].UpdateState(false);
            playerLife--;
            if (playerLife == 0) GameManager.Instance.EndGame(false);
        }
        else
        {
            lifeStats[playerLife]?.UpdateState(true);
            playerLife++;
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        InitializeLifeState();
    }

    public void InitializeLifeState()
    {
        foreach (LifeStats life in lifeStats) life.UpdateState(true);
        playerLife = lifeStats.Length;
    }
}
