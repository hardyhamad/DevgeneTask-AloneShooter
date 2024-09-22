using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int WeaponsBullets;
    public bool isGameOver=false;
    public int KillCount=0;
    public TMP_Text KillCountTxt;
    public GameObject FailPanel;
    public float SpawnInterval=2;
    void Start()
    {
        isGameOver = false;
        Instance = this;
    }
    public void ResetLevel()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(0);
    }
    public void UpdateText()
    {
        KillCountTxt.text= "Kills: "+ KillCount.ToString();
    }
    public void GameOver()
    {
        isGameOver = true;

        StartCoroutine(GameOverPanel());

    }
    IEnumerator GameOverPanel ()
    {
        yield return new WaitForSeconds(1);
        GameManager.Instance.FailPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
