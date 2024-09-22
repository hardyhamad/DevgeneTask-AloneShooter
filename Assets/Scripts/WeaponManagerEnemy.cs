using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManagerEnemy : MonoBehaviour
{
    public int Bullets;
    public EnemyController enemyController;
    void Start()
    {

    }
    private void OnEnable()
    {
        enemyController.WeaponsBullets = Bullets;
    }
}
