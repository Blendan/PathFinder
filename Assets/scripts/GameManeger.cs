using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManeger : MonoBehaviour
{
    private static int difficulty = 1;
    private static bool increment = true;

    public Animator transitionAnim;

    public Text display;

    private void Start()
    {
        SetDisplay();
    }

    public int Difficulty { get => difficulty; set => difficulty = value; }

    public void LoadLevel()
    {
        if (transitionAnim != null)
        {
            StartCoroutine(LoadLevelTrans());
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }

    IEnumerator LoadLevelTrans()
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("Game");
    }

    public void LoadMenue()
    {
        StartCoroutine(LoadMenueTrans());
    }

    IEnumerator LoadMenueTrans()
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(0.4f);
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

        LoadLevel();
    }

    public void AddDifficulty(int i)
    {
        difficulty += i;

        if(difficulty<1)
        {
            difficulty = 1;
        }
        else if(difficulty>50)
        {
            difficulty = 50;
        }

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
        else
        {
            Debug.LogError("display missing");
        }
    }
}
