using System;
using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public EnemyProperties[] objectsToSpawn; 
    public Transform[] spawnPoints;    
    private float initialSpawnInterval = 2f;
    private void Start()
    {
        StartCoroutine(SpawnObjectsCoroutine());
    }
    void Update()
    {
        initialSpawnInterval = GameManager.Instance.SpawnInterval;
    }
    private IEnumerator SpawnObjectsCoroutine()
    {
        while (true)
        {
            SpawnRandomObject();
            yield return new WaitForSeconds(initialSpawnInterval); // Wait for the defined interval before spawning the next object
        }
    }

    private void SpawnRandomObject()
    {
        if (objectsToSpawn.Length == 0 || spawnPoints.Length == 0)
            return;

        int randomObjectIndex = UnityEngine.Random.Range(0, objectsToSpawn.Length);
        int randomSpawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length);

        GameObject spawnedObject = Instantiate(objectsToSpawn[randomObjectIndex].Object, spawnPoints[randomSpawnPointIndex].position, Quaternion.identity);

        spawnedObject.GetComponent<EnemyController>().Weapons[GetGunIndex(objectsToSpawn[randomObjectIndex].Weapon)].SetActive(true);
    }

    private int GetGunIndex(EnemyGuns gun)
    {
        return (int)gun; // converting enum to int
    }
}
[Serializable]
public class EnemyProperties
{
    public GameObject Object;
    public EnemyGuns Weapon;
}
[Serializable]

public enum EnemyGuns
{
    ShotGun,
    Pistol,
}