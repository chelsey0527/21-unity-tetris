using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    public gameManager gameManager;
    private float previousTime;
    public float fallTime = 0.8f;
    public static int height = 20;
    public static int width = 10;
    private static Transform[,] grid = new Transform[width, height];

    internal bool OverHeight()
    {
        throw new NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(1, 0, 0);
        }
        else if ( Input.GetKeyDown(KeyCode.UpArrow))
        {
            // ROTATE //
            // RotateAround(Vector3 point, Vector3 axis, float angle);
            transform.RotateAround(transform.TransformPoint(rotationPoint),Vector3.forward, 90);
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotationPoint),Vector3.forward, -90);
        }

        // if KeyCode.DownArrow == true, then made fallTime/10 the new fallTime 
        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                //when the tetromino touch the ground, add it into the grid
                AddToGrid();
                CheckForLines();
                CheckEndGame();
                this.enabled = false;
                //if (OverHeight())
                //{
                //    gameManager.GameOver();
                //}
                //else {
                    FindObjectOfType<SpawnerTetromino>().NewTetromino();
                //}
            }

           
            previousTime = Time.time;
        }
    }

    //delete the line that is full
    void CheckForLines()
    {
        for( int i = height-1; i >= 0 ; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }

        }
    }

    //check if there is line or not
    bool HasLine( int i )
    {
        for(int j =0; j<width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }

    //delete  the line which is full
    void DeleteLine( int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    //make the upper row get down
    void RowDown (int i)
    {
        for ( int y = i; y< height; y++)
        {
            for ( int j=0; j<width; j++)
            {
                if (grid[j,y] != null)
                {
                    grid[j, y - 1] = grid[j, y]; //格值向下遞移
                    grid[j, y] = null; //原格清空
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0); //將遞移之格之物件實際轉換位子
                }
            }
        }
    }


    //add the stopped tetromino into grid to make it stacked
    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            // ROUNDTOINT //
            // Returns f rounded to the nearest integer.
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;
        }
    }

    bool ValidMove() //超過畫面就是錯誤的移動
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if(roundedX < 0 || roundedX >= width || roundedY < 0)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
                return false;

        }
        return true;
    }

    void CheckEndGame()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (OverHeight(i))
            {
                gameManager.GameOver();
            }

        }

    }


    //check if there is line or not
    public bool OverHeight(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }


}
