using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public GameObject KillEffect;
    private Rigidbody2D rb;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
        StartCoroutine(BulletDisable());
    }
    IEnumerator BulletDisable()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);

    }
    private void Update()
    {
       Debug.DrawRay(transform.position, transform.up * 2, Color.red);
        rb.velocity = transform.up * speed;

    }

    private void OnDisable()
    {
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "BulletPlayer")
            gameObject.SetActive(false);

        if (collision.gameObject.tag == "Enemy" && collision.gameObject.GetComponent<EnemyController>().isDead ==false )
        {
            collision.gameObject.GetComponent<EnemyController>().isDead = true;
            Instantiate(KillEffect, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            GameManager.Instance.KillCount++;
            GameManager.Instance.UpdateText();
            if (GameManager.Instance.SpawnInterval >= 0.5f && GameManager.Instance.KillCount%10 == 0)
            {
                GameManager.Instance.SpawnInterval = GameManager.Instance.SpawnInterval - 0.25f;
            }
        }
        
            
    }
}
