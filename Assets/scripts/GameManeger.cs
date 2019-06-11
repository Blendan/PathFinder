using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManeger : MonoBehaviour
{
    private static int difficulty = 1;
    private static bool increment = true;

    public Text display;

    private void Start()
    {
        SetDisplay();
    }

    public int Difficulty { get => difficulty; set => difficulty = value; }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMenue()
    {
        SceneManager.LoadScene("Menue");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void NextLevel()
    {
        if(increment)
        {
            difficulty++;
        }

        SceneManager.LoadScene("Game");
    }

    public void AddDifficulty(int i)
    {
        difficulty += i;
        SetDisplay();
    }

    private void SetDisplay()
    {
        if(display!=null)
        {
            string text = "Level: ";

            if(difficulty<10)
            {
                text += "0" + difficulty;
            }
            else
            {
                text += difficulty;
            }

            display.text = text;
        }
    }
}
