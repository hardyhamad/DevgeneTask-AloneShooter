using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 20;

    public List<GameObject> bulletPool;

    void Start()
    {
        bulletPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++) // bullet pool is initializing here

        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false); 
            bulletPool.Add(bullet);
        }
    }

    public GameObject GetBullet(Transform parent)
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                //bullet.transform.SetParent(parent);
                return bullet;
            }
        }
        GameObject newBullet = Instantiate(bulletPrefab);
        bulletPool.Add(newBullet); 
        return newBullet;
    }

    private void OnDisable()
    {
        foreach (GameObject bullet in bulletPool)
        {
            Destroy(bullet);
        }
        bulletPool.Clear(); 
    }
}
