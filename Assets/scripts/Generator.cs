using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private int corners = 1;
    private int maxLenght = 6;

    public int StartX = -6, StartY = 0;

    private Placer placer;

    private int cornersMade = 0, lengthMade = 0;
    private int nowX, nowY, lastX, lastY;

    private bool neededForceRotate = false;

    private System.Random random;

    private struct KeyXY
    {
        public readonly int x;
        public readonly int y;
        public KeyXY(int p1, int p2)
        {
            x = p1;
            y = p2;
        }
    }

    private Dictionary<KeyXY, string> map;

    // Start is called before the first frame update
    void Start()
    {
        GameManeger gameManeger = GetComponent<GameManeger>();

        maxLenght = gameManeger.Difficulty + 5;

        corners = maxLenght / 4;
        random = new System.Random();
        placer = GetComponent<Placer>();

        placer.fade = corners*0.4f + 1f + 0.4f;

        Generate();
    }

    /*
     * soll sich vom start nach rechts bewegen      => beforzugt abigen richting rechts
     * nocht aus dem bilschrim kommen               => beforzugt beim abbigen reichting mitte
     * nur dei bestimmte anzual an ecken machen     => random + length / ecken
     */

    private void Generate()
    {
        placer.PlacePathHiddenAt(StartX, StartY);

        lastX = StartX;
        lastY = StartY;

        nowX = StartX;
        nowY = StartY;

        int dirX = 1, dirY = 0;

        for (lengthMade = 1; lengthMade <= maxLenght; lengthMade ++)
        {
            Debug.Log(lengthMade);
            if(ToMakeTurn()&&!neededForceRotate)
            {
                cornersMade ++;
                Debug.Log("turn");

                if (ToTurnLeft())
                {
                    if(dirX == 1)
                    {
                        dirY = 1;
                        dirX = 0;
                    }
                    else if(dirX == -1)
                    {
                        dirY = -1;
                        dirX = 0;
                    }
                    else if (dirY == 1)
                    {
                        dirY = 0;
                        dirX = -1;
                    }
                    else if (dirY == -1)
                    {
                        dirY = 0;
                        dirX = 1;
                    }
                }
                else
                {
                    if (dirX == 1)
                    {
                        dirY = -1;
                        dirX = 0;
                    }
                    else if (dirX == -1)
                    {
                        dirY = 1;
                        dirX = 0;
                    }
                    else if (dirY == 1)
                    {
                        dirY = 0;
                        dirX = 1;
                    }
                    else if (dirY == -1)
                    {
                        dirY = 0;
                        dirX = -1;
                    }
                }
            }

            if(neededForceRotate)
            {
                neededForceRotate = false;
            }

            lastX = nowX;
            lastY = nowY;

            nowX += dirX;
            nowY += dirY;

            if(!placer.CheckBound(nowX + dirX, nowY + dirY))
            {
                neededForceRotate = true;
                Debug.Log("FUCKFFFFFFFFFFFFFFFFFFFFF");
                if(lastY==nowY)
                {
                    if(nowX<0)
                    {
                        nowY += -1 - dirY;
                        dirY = -1;
                    }
                    else
                    {
                        nowY += 1 - dirY;
                        dirY = 1;
                    }

                    nowX += -dirX;
                    dirX = 0;
                }
                else
                {
                    if (placer.CheckBound(nowX + 2, nowY - dirY))
                    {
                        nowX += 1 - dirX;
                        dirX = 1;
                    }
                    else
                    {
                        nowX += -1 - dirX;
                        dirX = -1;
                    }
                    nowY -= dirY;
                    dirY = 0;
                }
            }

            placer.PlacePathHiddenAt(nowX, nowY);


        }

        placer.PalceTile(nowX, nowY, "G");
        placer.SetActive(StartX, StartY);

        placer.Show();
    }

    private bool ToMakeTurn()
    {
        if(cornersMade<corners)
        {
            int temp;
            if ((maxLenght/(corners+1))*(cornersMade+1)<=lengthMade)
            {
                Debug.Log("v: " + (maxLenght / (corners + 1)) * (cornersMade + 1));
                return true;
            }
            else if((temp=random.Next(10))==3)
            {
                Debug.Log("r: "+temp);
                return true;
            }
        }
        return false;
    }

    private bool ToTurnLeft()
    {
        if(lastX==nowX)
        {
            if(lastY>nowY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if(nowY == 0)
            {
                return random.Next(3) == 2;
            }
            else if(nowY<0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
