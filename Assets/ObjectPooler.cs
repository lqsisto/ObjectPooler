using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private static GameObject objectHolder;
    
    //prefab array
    [SerializeField] private Transform[] powerUpPrefabs;
    
    //object array
    [SerializeField] private PowerUp [] powerUpObjects;
    
    [SerializeField] private int poolSize;
    
    //Dictionary where both the key and the queue type are the same powerup prefab
    private static Dictionary<Transform, Queue<Transform>> PoolDict = new Dictionary<Transform, Queue<Transform>>();
    
    private static ObjectPooler instance;
    
    private PowerUp powerUp;

    public bool poolInitialized = false;
    
    private void Awake ()
    {
        objectHolder = GameObject.Find ("ObjectHolder");
        for(int i = 0; i < powerUpObjects.Length; i++)
        {
            powerUp = new PowerUp ();
            PoolDict.Add (powerUpPrefabs[i], powerUp.CreatePool (poolSize, powerUpPrefabs[i]));
        }

        print ($"Pools created: {PoolDict.Keys.Count}");
        poolInitialized = true;
    }

    public Queue<Transform> TryGetPowerUp ()
    {
        var queueToReturn = Randomize (powerUpPrefabs);
        
        if (queueToReturn.Count == 0)
        {
            print ("QUEUE IS NULL, FETCHING OTHER ONE");
            TryGetPowerUp ();
        }
        print ("RETURNING QUEUE!");
        return queueToReturn;
    }

    public static void ReturnPowerUp (Queue<Transform> powerUpQueue, Transform powerUpObject)
    {
        powerUpObject.parent = objectHolder.transform;
        powerUpQueue.Enqueue (powerUpObject);
    }

    private Queue<Transform> Randomize (IReadOnlyList<Transform> objects)
    {
        var random = Random.Range (0, objects.Count);
        return PoolDict[objects[random]];
    }

    [System.Serializable]
    public class PowerUp
    {
        public string name;
        public Queue<Transform> pool;
        
        public Queue<Transform> CreatePool (int poolSize, Transform poolableObject)
        {
            pool = new Queue<Transform> ();
            for (var i = 0; i < poolSize; i++)
            {
                var pu = Instantiate (poolableObject,objectHolder.transform);
                pu.transform.position = Vector3.zero;
                pu.gameObject.SetActive (false);
                pool.Enqueue (pu);
            }
            return pool;
        }
    }
}
