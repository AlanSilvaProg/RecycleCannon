using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PoolingSystem : MonoBehaviour
{
    public List<GameObject> startTrashPooling;
    public GameObject trashTemplate;
    public List<GameObject> startBulletPooling;
    public GameObject bulletTemplate;
    public List<GameObject> startEnemyPooling;
    public GameObject enemyTemplate;

    #region queue interactables

    public Queue<GameObject> interactables = new Queue<GameObject>();

    public void StartTrashQueue() => startTrashPooling?.ForEach(pre => PutTrashOnQueue(pre));
    public void PutTrashOnQueue(GameObject interactable) { interactables.Enqueue(interactable); interactable.SetActive(false); }
    public GameObject GetTrashFromQueue()
    {
        if (interactables.Count > 0) return interactables.Dequeue();
        else
        {
            return Instantiate(trashTemplate, null);
        }
    }
    public GameObject DropTrashByType(TrashType type, Vector3 position)
    {
        GameObject trash = GetTrashFromQueue();
        trash.GetComponent<Interactable>().InitializeObj(position, GameManager.Instance.collectableInformations.First(pre => pre.type == type));
        return trash;
    }

    #endregion

    #region queue bullets

    public Queue<GameObject> bullets = new Queue<GameObject>();

    public void StartBulletQueue() => startBulletPooling?.ForEach(pre => PutBulletOnQueue(pre));
    public void PutBulletOnQueue(GameObject bullet) { bullets.Enqueue(bullet); bullet.SetActive(false); }
    public GameObject GetBulletFromQueue()
    {
        if (bullets.Count > 0) return bullets.Dequeue();
        else
        {
            return Instantiate(bulletTemplate, null);
        }
    }

    #endregion

    #region queue enemys

    public Queue<GameObject> enemys = new Queue<GameObject>();

    public void StartEnemyQueue() => startEnemyPooling?.ForEach(pre => PutEnemyOnQueue(pre));
    public void PutEnemyOnQueue(GameObject enemy) { enemys.Enqueue(enemy); enemy.SetActive(false); }
    public GameObject GetEnemyFromQueue()
    {
        if (enemys.Count > 0) return enemys.Dequeue();
        else
        {
            return Instantiate(enemyTemplate, null);
        }
    }

    #endregion
}
