using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public GameObject GameOverCanvas;

    // Start is called before the first frame update
    private void Start()
    {
        GameOverCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    public void GameOver()
    {
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

   
}
