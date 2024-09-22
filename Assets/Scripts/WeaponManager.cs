using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    public int Bullets;
    void Start()
    {

    }
    private void OnEnable()
    {
        GameManager.WeaponsBullets = Bullets;
    }
}