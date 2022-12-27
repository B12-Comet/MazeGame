using System.Collections;

using System.Collections.Generic;

using System.Text;

using System.Threading.Tasks;

using UnityEngine;

using Random = UnityEngine.Random;

public class Wall //内部类Wall

{

    public Vector2Int pos; //位置向量

    public Vector2Int relative; //相对格子向量

    public Wall(Vector2Int pos, Vector2Int anc)
    {

        this.pos = pos;

        ancToRelative(anc);

    }

    public void ancToRelative(Vector2Int ancestor)
    { //输入一个父格子，求出其相对格子并赋值

        this.relative = pos * 2 - ancestor; //int不能左乘Vector2Int（因为这里的*是Vector2Int的运算符重载）

    }

}

public class Maze //内部类Maze

{

    private Vector2Int[] e = { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1), }; //四方向

    private int width;

    private int height; //迷宫矩阵的宽和高 默认[宽，高]，Maze位置的游标在<[0..width],[0..height]>

    private Vector2Int origin; //起点

    public List<Wall> M; //list

    public List<Wall> W; //list

    public List<Vector2Int> maze; //最终的迷宫列表

    public Maze(int width, int height, Vector2Int origin)
    {

        this.width = width;

        this.height = height;

        this.origin = origin;

        this.M = new List<Wall>();

        this.W = new List<Wall>();

        this.maze = new List<Vector2Int>();

        this.maze.Add(origin);

    }

    bool findMaze(Vector2Int pos)
    { //只判断此位置是否已移除墙

        if (this.maze.Contains(pos))
        {

            return true;

        }

        return false;

    }

    bool borderExam(Vector2Int pos)
    { //边界检查

        if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
        {

            return true;

        }

        return false;

    }

    void createWall(Vector2Int anc)
    { //传入移除墙的格子，创建相邻墙

        foreach (Vector2Int i in this.e)
        {

            Vector2Int index = anc + i; //上下左右格子的位置

            if (borderExam(index) && !findMaze(index))
            { //检查确定这个位置确实是墙，并且没有越界

                this.M.Add(new Wall(index, anc)); //将墙加入M中

            }

        }

    }

    void funcM()
    { //随机取出M中的墙，执行步骤

        int index = Random.Range(0, this.M.Count);

        Wall wall = this.M[index];

        if (findMaze(wall.pos))
        { //之前已经移除了此墙

            this.M.RemoveAt(index);

            return;

        }

        if (findMaze(wall.relative) || !borderExam(wall.relative))
        { //相对位置已移除墙，或者此墙的相对位置无效

            this.M.RemoveAt(index); //此墙移除

        }

        else
        {

            this.M.RemoveAt(index);

            this.maze.Add(wall.pos);

            this.maze.Add(wall.relative); //后面填什么无所谓，给相对位置挖墙

            createWall(wall.relative); //加入新的墙

        }

    }

    public void exec()
    { //入口？

        createWall(this.origin); //起点

        while (M.Count != 0)
        {

            funcM();

        }

    }

}

//上略

public class MazeController : MonoBehaviour

{

    public GameObject wall; //墙体预制体

    public Transform destArea; //终点区域的位置

    const int Width = 10;

    const int Height = 10;

    // Start is called before the first frame update

    void Start()

    {

        Vector3 correct1 = new Vector3(0.5f, 0, 0.5f); //墙体中心修正

        Vector3 correct2 = new Vector3(-5, 0, -5); //平面偏移修正

        Vector2Int dest = new Vector2Int((int)(destArea.position.x + 4.5f), (int)(destArea.position.y + 4.5f)); //目的区域位置修正

        Maze maze = new Maze(Width, Height, dest); //按照平台的大小生成迷宫

        maze.exec();

        //可惜C#里没有Range

        for (int x = 0; x < Width; x++)
        {

            for (int y = 0; y < Height; y++)
            {

                if (!maze.maze.Contains(new Vector2Int(x, y)))
                { //本区域不为空

                    Instantiate(wall, new Vector3(x, 2, y) + correct1 + correct2, Quaternion.identity); //Quaternion.identity是0角度

                }

            }

        }

    }

    // Update is called once per frame

    void Update()

    {

    }

}
