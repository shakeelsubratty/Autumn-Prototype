//Very Basic Code to test things

using UnityEngine;
using System;
using System.Collections;

public class col_Spawn : MonoBehaviour {

    public GameObject leafToSpawn; // Need a complet leaf as a copy anywhere in the map
    public GameObject spawnCenterTree; // Need a tree to spawn at 
    public float timeLeft = 30.0f;
    public int minX = 0;
    public int maxX = 5;
    public int minZ = 0;
    public int maxZ = 5;
    private float defaultTimeLeft = 0f;
    Vector3 currentPos;

    System.Random rnd = new System.Random();
    void Start()
    {
        defaultTimeLeft = timeLeft;
        currentPos = spawnCenterTree.gameObject.transform.position;
    }
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            SpawnNewLeave();
        }
    }
    private void SpawnNewLeave()
    {
        Instantiate(GameObject.Find(leafToSpawn.name), GetRandomSpawn(), transform.rotation);

        timeLeft = defaultTimeLeft;
    }
    public Vector3 GetRandomSpawn()
    {
        Vector3 newSpawn = new Vector3(currentPos.x + rnd.Next(minX, maxX), 0.3f,currentPos.z + rnd.Next(minZ, maxZ));
        return newSpawn;
    }

}
