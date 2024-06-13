using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    
    public GameObject objectToPool;
    public int initialPoolSize = 20;

    private List<GameObject> pooledObjects;

    void Awake()
    {
        // Ensure there is only one instance of the ObjectPool
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize the pool
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    // Get an object from the pool
    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // If no inactive objects are available, create a new one
        GameObject newObj = Instantiate(objectToPool);
        newObj.SetActive(false);
        pooledObjects.Add(newObj);
        return newObj;
    }

    // Return an object to the pool
    public void ReturnPooledObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
