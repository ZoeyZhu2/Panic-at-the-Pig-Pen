using UnityEngine;
using System;

public class PigSpawnScript : MonoBehaviour
{
    [SerializeField] private GameObject PinkPigPrefab;
    [SerializeField] private GameObject GreenPigPrefab;
    [SerializeField] UIScript uiScript;
    private System.Random r = new System.Random();
    private double pigType;

    private float timer = 0f;
    private float timeToWait = 0.75f;

    private bool gameInProgress = false;
     

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pigType = r.NextDouble();
    }

    // Update is called once per frame
    void Update()
    {
        gameInProgress = uiScript.getGameInProgress();
        if (gameInProgress)
        {
             timer += Time.deltaTime;
            if (timer >= timeToWait)
            {
                timer = 0f;
                SpawnPig();
            }
        }
    }

    //determine visibility later
    private void SpawnPig()
    {
        pigType = r.NextDouble();
        //double spawnX = r.NextDouble() * 6 - 3;
        double spawnX = r.NextDouble() * Camera.main.orthographicSize * 2 * Camera.main.aspect - (Camera.main.orthographicSize * 2 * Camera.main.aspect) / 2; 
        //width is Camera.main.orthographicSize * 2 * Camera.main.aspect
        //half of width is Camera.main.orthographicSize * Camera.main.aspect
        Vector3 spawnPosition = new Vector3((float)spawnX, 6, 0);
        if (pigType < 0.5)
        {
            Instantiate(PinkPigPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Instantiate(GreenPigPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
