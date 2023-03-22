using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterVillageHandler : MonoBehaviour
{
    public List<GameObject> objectsToDestroy;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            DestroyObjects();
        }
    }

    public void DestroyObjects()
    {
        foreach (var item in objectsToDestroy)
        {
            Destroy(item);
        }
    }
}
