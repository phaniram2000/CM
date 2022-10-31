using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    [HideInInspector] public List<GameObject> ballPool;
    public GameObject objectToPool;
    public int amountToPool;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ballPool = new List<GameObject>();
        GameObject temp;

        for (int i = 0; i < amountToPool; i++)
        {
            temp = Instantiate(objectToPool);
            temp.SetActive(false);
            ballPool.Add(temp);
        }
    }

    public GameObject GetPooledObjects()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!ballPool[i].activeInHierarchy)
            {
                return ballPool[i];
            }
        }
        return null;
    }
}
