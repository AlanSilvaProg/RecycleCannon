using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ClosestMachine
{
    public GameObject machine;
    public TrashType type;
    public ClosestMachine(GameObject obj, TrashType t)
    {
        machine = obj;
        type = t;
    }
}

[System.Serializable]
public class CollectableInformations
{
    [SerializeField] private string name = "Material";
    public TrashType type;
    public Color typeColor;
}

public class GameManager : MonoBehaviour
{
    [Header("Instance Reference")]
    public static GameManager Instance;
    public Player playerInstance;
    public CannonController cannonInstance;
    public HordeManager hordeManager;
    public PoolingSystem poolingSystem;
    [Header("Game Reference")]
    public GameObject metalDrop, organicDrop, plasticDrop;
    public Transform playerInitialPoint;
    public GameObject endGameCanvas;
    public GameObject hudGameCanvas;
    [Header("Configurations")]
    public List<CollectableInformations> collectableInformations = new List<CollectableInformations>();
    public int lifePoints = 50; // vida da muralha
    public Image lifeBar;
    int initialLife;
    int life { get { return lifePoints; } set { lifePoints = value; if (life <= 0) EndGame(); UpdateLifeHUD(); } }
    public void RemoveLife() => life--;

    [HideInInspector] public int kills;
    [HideInInspector] public float time;

    private void Awake()
    {
        Instance = this;
        initialLife = lifePoints;
        poolingSystem.StartTrashQueue();
        poolingSystem.StartBulletQueue();
        poolingSystem.StartEnemyQueue();
        hordeManager.CallNewHordeBySystem();
        playerInstance.transform.position = playerInitialPoint.position;
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    public void UpdateLifeHUD()
    {
        lifeBar.fillAmount = (((float)lifePoints * 100) / (float)initialLife) / 100; 
    }

    public void RestartLife()
    {
        lifePoints = initialLife;
        playerInstance.lifeController.InitializeLifeState();
    }

    public void EndGame(bool win = false)
    {
        hudGameCanvas.gameObject.SetActive(false);
        endGameCanvas.gameObject.SetActive(true);
    }

    #region Trash Machine

    GameObject closest;
    TrashType type;
    public ClosestMachine GetClosestMachine()
    {
        closest = metalDrop;
        type = TrashType.METAL;
        if (Vector3.Distance(organicDrop.transform.position, playerInstance.transform.position) < Vector3.Distance(metalDrop.transform.position, playerInstance.transform.position))
        {
            closest = organicDrop;
            type = TrashType.ORGANIC;
        }

        if (Vector3.Distance(plasticDrop.transform.position, playerInstance.transform.position) < Vector3.Distance(closest.transform.position, playerInstance.transform.position))
        {
            closest = plasticDrop;
            type = TrashType.PLASTIC;
        }

        if (Vector3.Distance(closest.transform.position, playerInstance.transform.position) < playerInstance.playerInteraction.distanceToInteractMachine)
            return new ClosestMachine(closest, type);
        else
            return null;
    }

    #endregion

}
