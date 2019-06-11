using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movment : MonoBehaviour
{
    private Placer placer;

    private bool paused = false;

    private GameObject[] pauseObjs;

    private int errors = 0;
    // Start is called before the first frame update
    void Start()
    {
        pauseObjs = GameObject.FindGameObjectsWithTag("pause");
        SetPauseActive(false);
        placer = GetComponent<Placer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (placer.ToFade < 0)
        {
            if (!paused)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    Move(0, 1);
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    Move(0, -1);
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    Move(-1, 0);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    Move(1, 0);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void SetPauseActive(bool b)
    {
        foreach(GameObject obj in pauseObjs)
        {
            obj.SetActive(b);
        }
    }

    public void Pause()
    {
        if(paused)
        {
            SetPauseActive(false);
        }
        else
        {
            SetPauseActive(true);
        }

        paused = !paused;
    }

    private void Move(int x, int y)
    {
        TileType active = placer.GetActive();

        int newX = active.X + x;
        int newY = active.Y + y;

        if (placer.GetAt(newX, newY) != null)
        {
            if (placer.GetAt(newX, newY).Type == "G")
            {
                GetComponent<GameManeger>().NextLevel();
            }
        }

        if(placer.CheckAt(newX,newY))
        {
            placer.SetActive(newX, newY);
        }
        else
        {
            placer.Show();
            placer.ToFade = 0.3f;
            errors++;
        }

    }
}
