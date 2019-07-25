using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public ObjectPooler pooler;

    private Queue<Transform> powerUpQueue;

    private Transform powerUp;
    // Start is called before the first frame update
   private  void Start()
   {
       pooler = FindObjectOfType<ObjectPooler> ();
        StartCoroutine (SetPowerUpActive ());
    }

    private IEnumerator SetPowerUpActive ()
    {

        while (!pooler.poolInitialized)
        {
            print ("POOLER NOT INITIALIZED");
            yield return null;
        }
        print ("POOLER INITIALIZED");
        
        powerUpQueue = pooler.TryGetPowerUp ();
        powerUp = powerUpQueue.Dequeue ();

        var transform1 = powerUp.transform;
        
        transform1.parent = gameObject.transform;
        transform1.localPosition = Vector3.zero;
        powerUp.gameObject.SetActive (true);
    }
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.Space))
        {
            powerUp.gameObject.SetActive (false);
            powerUp.transform.parent = null;
            ObjectPooler.ReturnPowerUp (powerUpQueue, powerUp);
        }
    }
}
