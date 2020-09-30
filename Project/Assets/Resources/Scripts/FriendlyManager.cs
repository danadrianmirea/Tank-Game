using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyManager : MonoBehaviour
{

    private float InstantiationTimer = 2.5f;
    private Vector3 spawnPos;
    private Vector3 distPos;
    private Vector3 dirPos;

    public GameObject _friendlyPrefab;
    public Turret target;

    protected List<Friendly> mFriendly = new List<Friendly>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SpawnFriendly()
    {
        InstantiationTimer -= Time.deltaTime;

        if (InstantiationTimer <= 0)
        {
            // Set position
            distPos = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Debug.Log("distpos is");
            Debug.Log(distPos);
            // Set rotation
            dirPos = distPos - target.transform.position;
            dirPos.Normalize();
            dirPos *= 12;
            Debug.Log(dirPos);


            Instantiate(_friendlyPrefab, dirPos, Quaternion.LookRotation(dirPos));
            InstantiationTimer = 2.5f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        SpawnFriendly();
    }
}
