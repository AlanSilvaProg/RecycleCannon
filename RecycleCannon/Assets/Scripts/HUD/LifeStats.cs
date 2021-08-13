using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStats : MonoBehaviour
{
    public GameObject empty;
    public GameObject full;

    public void UpdateState(bool state)
    {
        full.SetActive(state);
        empty.SetActive(!state);
    }
}
