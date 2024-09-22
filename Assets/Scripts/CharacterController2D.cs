using System.Collections;
using System.Collections.Generic; // For List
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public FixedJoystick joyStick;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sensitivity = 1f;

    [Header("References")]
    public Rigidbody2D rb;
    public Camera mainCamera;
    [Header("Bullet")]
    public BulletPool bulletPool;
    public Transform firePoint;
    [Header("Weapons")]
    public GameObject[] Weapons;
    public int DefaultWeapon;
    private Vector2 moveInput;
    private Vector2 moveVelocity;

    // List to hold all enemies in the scene
    public List<EnemyController> enemies; 

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f; // Speed of rotation from its point to the point of enemy who is near

    void Start()
    {
        Weapons[DefaultWeapon].SetActive(true);
    }
    void Update()
    {
        moveInput.x = joyStick.Horizontal;
        moveInput.y = joyStick.Vertical;

        moveInput *= sensitivity;

        moveVelocity = moveInput.normalized * moveSpeed;

        LookAtNearestEnemy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null && !enemies.Contains(enemy)) // Check if it's already in the list
            {
                enemies.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
           // enemies.Remove(collision.GetComponent<EnemyController>());
        }
    }

    void FixedUpdate()
    {
        // Move the character
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    private void LookAtNearestEnemy()
    {
        if (enemies.Count == 0) return; 

        EnemyController nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (EnemyController enemy in enemies)
        {
            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }

        }

                                    // if nearest enemy is found look at it
        if (nearestEnemy != null)
        {
            Vector2 direction = (nearestEnemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Adjust for 2D rotation
            rb.rotation = Mathf.LerpAngle(rb.rotation, targetRotation.eulerAngles.z, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // No enemies, look in the direction of movement
            LookInMovementDirection();
        }
    }
    private void LookInMovementDirection()
    {
        if (moveVelocity.sqrMagnitude > 0.01f) // Ensure there's some movement
        {
            float moveAngle = Mathf.Atan2(moveVelocity.y, moveVelocity.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, moveAngle - 90); // Adjust for 2D rotation
            rb.rotation = Mathf.LerpAngle(rb.rotation, targetRotation.eulerAngles.z, rotationSpeed * Time.deltaTime);
        }
    }
    public void WeaponsSelection(int WeaponIndex)
    {
        StartCoroutine(Weapon_Selection(WeaponIndex));
    }
    IEnumerator Weapon_Selection(int WeaponIndex)
    {
        for (int i = 0; i < Weapons.Length; i++)
        {
            Weapons[i].SetActive(false);
        }
        yield return new WaitForSeconds(.25f);
        Weapons[WeaponIndex].SetActive(true);


    }
    public void FireBullet()
    {
        FireBullet(GameManager.WeaponsBullets, 10f); // this will fire 5 bullets with a spread of 10 degrees each bullet

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
                Debug.Log("Bullet activated: " + bullet.name);
            }
            else
            {
                Debug.LogWarning("No available bullet from pool."); 
            }
        }
    }

}
