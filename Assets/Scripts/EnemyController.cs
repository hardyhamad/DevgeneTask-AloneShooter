using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 100f;
    [Header("Bullet")]
    public GameObject[] Weapons;
    public int WeaponsBullets;
    [Header("Bullet")]
    public BulletPool bulletPool; 
    public Transform firePoint;

    [Space]
    public bool isDead;
    void OnEnable()
    {
        if (!GameManager.Instance.isGameOver)
            player = FindAnyObjectByType<CharacterController2D>().gameObject.transform;
    }
    void Start()
    {
        LookAtPlayer();
        StartCoroutine(StartFiring());
    }

    void Update()
    {
        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    IEnumerator StartFiring()
    {
        yield return new WaitForSeconds(3); 

        while (true) 
        {
            FireBullet();
            yield return new WaitForSeconds(3);
        }
    }
    public void FireBullet()
    {
        FireBullet(WeaponsBullets, 10f); // this will fire 5 bullets with a spread of 10 degrees each bullet

    }
    public void FireBullet(int bulletCount, float spreadAngle)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = bulletPool.GetBullet(firePoint);
            if (bullet != null)
            {
                bullet.transform.position = firePoint.position; 

                float angle = (i - (bulletCount - 1) / 2f) * spreadAngle;
                bullet.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, angle); 

                bullet.SetActive(true); 
            }
            else
            {
                Debug.LogWarning("No available bullet from pool."); 
            }
        }
    }
}
