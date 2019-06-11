using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManeger : MonoBehaviour
{
    private static int difficulty = 0;
    private static bool increment = true;

    public int Difficulty { get => difficulty; set => difficulty = value; }

    public void NextLevel()
    {
        if(increment)
        {
            difficulty++;
        }

        SceneManager.LoadScene("Game");
    }
}
