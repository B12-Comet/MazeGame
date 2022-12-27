using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//这个版本√
public class MazeGenerator : MonoBehaviour
{
    public int row;
    public int col;
    public int wallWidth;
    public Transform wall, road,Current,Exit,Entry,player;
    public GameObject gameover;
    public GameObject playtime;
    uint NORTH = 0b_0000_0001;
    uint EAST = 0b_0000_0010;
    uint SOUTH = 0b_0000_0100;
    uint WEST = 0b_0000_1000;
    uint VISITED = 0b_0001_0000;

    private uint[] maze;
    // Visited cells as well as the visit history to be recorded
    private int visitedCells;
    private Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();
    private List<Transform> previousCurrent = new List<Transform>();
    System.Random rnd = new System.Random();


    public float horizontalinput;//水平参数
    public float verticalinput;//垂直参数
    float speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameover.SetActive(false);
        playtime.SetActive(false);
        maze = new uint[row * col];
        // We start from a random position and set up whatever needed
        int x = rnd.Next(row); int y = rnd.Next(col);
        stack.Push(Tuple.Create(x, y));
        maze[y * row + x] = VISITED;
        visitedCells = 1;
        InitializeMaze();//生成初始状态的封闭方格
        SetExitAndEntry();
        CreatePlayer();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (RB_Algorithm())
        {
            CreateMaze();
            playtime.SetActive(true);
        }
       
        PlayerMove();
        
    }
    void InitializeMaze()
    {
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                // 基本单元
                for (int py = 0; py < wallWidth; py++)
                {
                    for (int px = 0; px < wallWidth; px++)
                    {
                        Vector3 v3 = new Vector3(x * (wallWidth + 1) + px, 0, y * (wallWidth + 1) + py);
                        Instantiate(road, v3, Quaternion.identity);
                        Vector3 v3_1 = new Vector3(x * (wallWidth + 1) + px, 0, y * (wallWidth + 1) + wallWidth);
                        Vector3 v3_2 = new Vector3(x * (wallWidth + 1) + wallWidth, 0, y * (wallWidth + 1) + px);
                        Instantiate(wall, v3_1, Quaternion.identity);
                        Instantiate(wall, v3_2, Quaternion.identity);
                        
                    }

                }
                // 右上角
                Instantiate(wall, new Vector3((x + 1) * (wallWidth + 1) - 1, 0, (y + 1) * (wallWidth + 1) - 1), Quaternion.identity);
            }
        }
        // Fill in rest of the cells
        
        for (int i = 0; i < row * (wallWidth + 1); i++)//最下面一行
        {
            Instantiate(wall, new Vector3(i, 0, (col - col) * (wallWidth + 1) - 1), Quaternion.identity);
        }
        for (int j = -1; j < col * (wallWidth + 1); j++)//最左侧一列
        {
            Instantiate(wall, new Vector3((row - row) * (wallWidth + 1) - 1, 0, j), Quaternion.identity);
        }
        

    }
    //回溯法生成迷宫
    bool RB_Algorithm()
    {
        List<int> neighbours = new List<int>();
        Func<int, int, uint> lookAt = (px, py) => (uint)((stack.Peek().Item1 + px) + (stack.Peek().Item2 + py) * row);

        if (visitedCells < row * col)
        {
            // North neighbour
            if (stack.Peek().Item2 > 0 && (maze[lookAt(0, -1)] & VISITED) == 0)
                neighbours.Add(0); // meaning the north neighbour exists and unvisited
            // East neighbour
            if (stack.Peek().Item1 < row - 1 && (maze[lookAt(+1, 0)] & VISITED) == 0)
                neighbours.Add(1);
            // South neighbour
            if (stack.Peek().Item2 < col - 1 && (maze[lookAt(0, +1)] & VISITED) == 0)
                neighbours.Add(2);
            // West neighbour
            if (stack.Peek().Item1 > 0 && (maze[lookAt(-1, 0)] & VISITED) == 0)
                neighbours.Add(3);

            // Are there any neighbours available?
            if (neighbours.Count > 0)
            {
                // Choose one available neighbour at random
                int nextCellDir = neighbours[rnd.Next(neighbours.Count)];

                // Create a path between the neighbour and the current cell
                switch (nextCellDir)
                {
                    case 0: // North
                        maze[lookAt(0, -1)] |= VISITED | SOUTH;
                        maze[lookAt(0, 0)] |= NORTH;
                        stack.Push(Tuple.Create((stack.Peek().Item1 + 0), (stack.Peek().Item2 - 1)));
                        break;

                    case 1: // East
                        maze[lookAt(+1, 0)] |= VISITED | WEST;
                        maze[lookAt(0, 0)] |= EAST;
                        stack.Push(Tuple.Create((stack.Peek().Item1 + 1), (stack.Peek().Item2 + 0)));
                        break;

                    case 2: // South
                        maze[lookAt(0, +1)] |= VISITED | NORTH;
                        maze[lookAt(0, 0)] |= SOUTH;
                        stack.Push(Tuple.Create((stack.Peek().Item1 + 0), (stack.Peek().Item2 + 1)));
                        break;

                    case 3: // West
                        maze[lookAt(-1, 0)] |= VISITED | EAST;
                        maze[lookAt(0, 0)] |= WEST;
                        stack.Push(Tuple.Create((stack.Peek().Item1 - 1), (stack.Peek().Item2 + 0)));
                        break;

                }
                visitedCells++; // Update the visited cells' number
            }
            else
            {
                // No available neighbours so backtrack!
                stack.Pop();
            }
            //DrawEverything();
            return false;
        }
        return true;
    }

    //绘制迷宫
    void CreateMaze()
    {
        // Draw Maze
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                // Knock down the wall pieces we put before
                for (int p = 0; p < wallWidth; p++)
                {
                    Vector3 v3 = new Vector3(x * (wallWidth + 1) + p, 0, y * (wallWidth + 1) + wallWidth);
                    Vector3 v3_2 = new Vector3(x * (wallWidth + 1) + wallWidth, 0, y * (wallWidth + 1) + p);
                    if (((maze[y * row + x] & SOUTH) == 4) && checkIfPosEmpty(v3))
                        Instantiate(road, v3, Quaternion.identity);
                    if (((maze[y * row + x] & EAST) == 2) && checkIfPosEmpty(v3_2))
                        Instantiate(road, v3_2, Quaternion.identity);
                }
            }
        }

        // Get rid of the previous stack peek first
        foreach (Transform t in previousCurrent)
        {
            if (t != null) // because it can be destroyed already
                Instantiate(road, t.position, Quaternion.identity);
        }

        // Then we draw the new stack peek
        for (int py = 0; py < wallWidth; py++)
        {
            for (int px = 0; px < wallWidth; px++)
            {
                Vector3 v3 = new Vector3(stack.Peek().Item1 * (wallWidth + 1) + px, 0, stack.Peek().Item2 * (wallWidth + 1) + py);
                if (checkIfPosEmpty(v3))
                    previousCurrent.Add(Instantiate(Current, v3, Quaternion.identity));
            }
        }
    }

    bool checkIfPosEmpty(Vector3 targetPos)
    {
        GameObject[] allTilings = GameObject.FindGameObjectsWithTag("obj");
        foreach (GameObject t in allTilings)
        {
            if (t.transform.position == targetPos)//不为空
            {
                Destroy(t);
            }
        }
        return true;
    }
    void SetExitAndEntry()
    {
        /*
        List<Transform> corner = new List<Transform>();
        for (int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                if ((i == 0 & j == 0) || (i == 0 & j == col - 1) || (i == row - 1 & j == 0) || (i == col - 1 & j == row - 1))
                {
                    for (int p = 0; p < wallWidth; p++)
                    {
                        GameObject[] allTilings = GameObject.FindGameObjectsWithTag("obj");
                        Vector3 corPos = new Vector3(i, 0, j);
                        foreach (GameObject t in allTilings)
                        {
                            if (t.transform.position == corPos)
                            {
                                Destroy(t);
                                Instantiate(Entry, corPos, Quaternion.identity);
                            }
                        }
                    }
                    //corner.Add(new Vector3(i, 0, j));
                }
            }
        }
        */
        int x1,x2;
        int y1,y2;
        int index = rnd.Next(4);
        Debug.Log(index);
        
        GameObject[] allTilings = GameObject.FindGameObjectsWithTag("obj");
        switch (index)
        {
            case 0:
                x1 = 0; y1 = 0;//起点
                x2 = row - 1;y2 = col - 1;//终点
                for (int py = 0; py < wallWidth; py++)
                {
                    for (int px = 0; px < wallWidth; px++)
                    {
                        Vector3 v1 = new Vector3(x1 * (wallWidth + 1) + px, 0, y1 * (wallWidth + 1) + py);//起点
                        Vector3 v2 = new Vector3(x2 * (wallWidth + 1) + px, 0, y2 * (wallWidth + 1) + py);//终点

                        foreach (GameObject t in allTilings)
                        {
                            if (t.transform.position == v1)
                            {
                                Destroy(t);
                                Instantiate(Entry, v1, Quaternion.identity);
                            }
                            if (t.transform.position == v2)
                            {
                                Destroy(t);
                                Instantiate(Exit, v2, Quaternion.identity);
                            }
                        }
                    }
                }
                break;
            case 1:
                x1 = 0; y1 = col-1;//起点
                x2 = row - 1; y2 = 0;//终点
                for (int py = 0; py < wallWidth; py++)
                {
                    for (int px = 0; px < wallWidth; px++)
                    {
                        Vector3 v1 = new Vector3(x1 * (wallWidth + 1) + px, 0, y1 * (wallWidth + 1) + py);//起点
                        Vector3 v2 = new Vector3(x2 * (wallWidth + 1) + px, 0, y2 * (wallWidth + 1) + py);//终点

                        foreach (GameObject t in allTilings)
                        {
                            if (t.transform.position == v1)
                            {
                                Destroy(t);
                                Instantiate(Entry, v1, Quaternion.identity);
                            }
                            if (t.transform.position == v2)
                            {
                                Destroy(t);
                                Instantiate(Exit, v2, Quaternion.identity);
                            }
                        }
                    }
                }
                break;
            case 2:
                x1 = row-1; y1 = 0;//起点
                x2 = 0; y2 = col - 1;//终点
                for (int py = 0; py < wallWidth; py++)
                {
                    for (int px = 0; px < wallWidth; px++)
                    {
                        Vector3 v1 = new Vector3(x1 * (wallWidth + 1) + px, 0, y1 * (wallWidth + 1) + py);//起点
                        Vector3 v2 = new Vector3(x2 * (wallWidth + 1) + px, 0, y2 * (wallWidth + 1) + py);//终点

                        foreach (GameObject t in allTilings)
                        {
                            if (t.transform.position == v1)
                            {
                                Destroy(t);
                                Instantiate(Entry, v1, Quaternion.identity);
                            }
                            if (t.transform.position == v2)
                            {
                                Destroy(t);
                                Instantiate(Exit, v2, Quaternion.identity);
                            }
                        }
                    }
                }
                break;
            case 3:
                x1 = row - 1; y1 = col-1;//起点
                x2 = 0; y2 = 0;//终点
                for (int py = 0; py < wallWidth; py++)
                {
                    for (int px = 0; px < wallWidth; px++)
                    {
                        Vector3 v1 = new Vector3(x1 * (wallWidth + 1) + px, 0, y1 * (wallWidth + 1) + py);//起点
                        Vector3 v2 = new Vector3(x2 * (wallWidth + 1) + px, 0, y2 * (wallWidth + 1) + py);//终点

                        foreach (GameObject t in allTilings)
                        {
                            if (t.transform.position == v1)
                            {
                                Destroy(t);
                                Instantiate(Entry, v1, Quaternion.identity);
                            }
                            if (t.transform.position == v2)
                            {
                                Destroy(t);
                                Instantiate(Exit, v2, Quaternion.identity);
                            }
                        }
                    }
                }
                break;

        }
            

        

    }



    void CreatePlayer()
    {
        GameObject enter = GameObject.FindWithTag("entry");//找到入口
        Vector3 temp = enter.transform.position;
        Debug.Log(temp);
        Vector3 startpos = new Vector3(temp.x, (float)(temp.y+0.5), temp.z);
        Instantiate(player, startpos, Quaternion.identity);

    }

    void PlayerMove()
    {
        GameObject player1 = GameObject.FindWithTag("player");//找到玩家
       
        horizontalinput = Input.GetAxis("Horizontal");
        verticalinput = Input.GetAxis("Vertical");
        player1.transform.Translate(Vector3.right * horizontalinput * Time.deltaTime * speed);
        player1.transform.Translate(Vector3.forward * verticalinput * Time.deltaTime * speed);

    }


    
}
