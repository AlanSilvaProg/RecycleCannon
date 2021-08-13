using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Interaction : MonoBehaviour
{

    TrashType currentType;
    bool freeSlot = true;
    public float distanceToInteractMachine = 2;

    List<GameObject> interactablesOnRange = new List<GameObject>();

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.playerInstance.playerInteraction = this;
    }

    public void InteractClick()
    {
        if (freeSlot)
            CheckInteractable()?.GetObject(this);
        else
            PutOnMachine();
    }

    public void PutOnMachine()
    {
        ClosestMachine closest = GameManager.Instance.GetClosestMachine();
        if(closest != null && closest.type == currentType)
        {
            GameManager.Instance.cannonInstance.RechargeBullet(currentType);
            freeSlot = true;
        }
    }

    public void SetNewType(TrashType type)
    {
        currentType = type;
        freeSlot = false;
    }

    public Interactable CheckInteractable()
    { 
        if(interactablesOnRange.Count == 0) return null;
        GameObject obj = interactablesOnRange[0];
        foreach(GameObject interactable in interactablesOnRange)
        {
            if (Vector3.Distance(interactable.transform.position, transform.position) < Vector3.Distance(obj.transform.position, transform.position)) obj = interactable;
        }
        return obj.GetComponent<Interactable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            foreach (GameObject interactable in interactablesOnRange)
            {
                if (interactable.gameObject.GetInstanceID().Equals(other.gameObject.GetInstanceID()))
                {
                    return;
                }
            }
            interactablesOnRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            foreach(GameObject interactable in interactablesOnRange)
            {
                if (interactable.gameObject.GetInstanceID().Equals(other.gameObject.GetInstanceID()))
                {
                    interactablesOnRange.Remove(interactable);
                    return;
                }
            }
        }
    }

}
