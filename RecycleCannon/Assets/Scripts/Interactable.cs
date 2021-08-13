using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TrashType { METAL, PLASTIC, ORGANIC }

public class Interactable : MonoBehaviour
{

    public TrashType type;
    public Renderer myRenderer;
    Material myMaterial;

    private void OnEnable()
    {
        if(myMaterial == null) myMaterial = new Material(myRenderer.material);
        if (!myRenderer.material.Equals(myMaterial)) myRenderer.material = myMaterial;
    }

    public void InitializeObj(Vector3 spawnPoint, CollectableInformations newType)
    {
        transform.SetParent(null);
        transform.position = spawnPoint;
        type = newType.type;
        gameObject.SetActive(true);
        myMaterial.color = newType.typeColor;
    }

    public void GetObject(Interaction player)
    {
        player.SetNewType(type);
        GameManager.Instance.poolingSystem.PutTrashOnQueue(gameObject);
    }
}
