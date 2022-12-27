using System.Collections;

using System.Collections.Generic;

using System.Text;

using System.Threading.Tasks;

using UnityEngine;

using Random = UnityEngine.Random;

public class Wall //�ڲ���Wall

{

    public Vector2Int pos; //λ������

    public Vector2Int relative; //��Ը�������

    public Wall(Vector2Int pos, Vector2Int anc)
    {

        this.pos = pos;

        ancToRelative(anc);

    }

    public void ancToRelative(Vector2Int ancestor)
    { //����һ�������ӣ��������Ը��Ӳ���ֵ

        this.relative = pos * 2 - ancestor; //int�������Vector2Int����Ϊ�����*��Vector2Int����������أ�

    }

}

public class Maze //�ڲ���Maze

{

    private Vector2Int[] e = { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1), }; //�ķ���

    private int width;

    private int height; //�Թ�����Ŀ�͸� Ĭ��[����]��Mazeλ�õ��α���<[0..width],[0..height]>

    private Vector2Int origin; //���

    public List<Wall> M; //list

    public List<Wall> W; //list

    public List<Vector2Int> maze; //���յ��Թ��б�

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
    { //ֻ�жϴ�λ���Ƿ����Ƴ�ǽ

        if (this.maze.Contains(pos))
        {

            return true;

        }

        return false;

    }

    bool borderExam(Vector2Int pos)
    { //�߽���

        if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
        {

            return true;

        }

        return false;

    }

    void createWall(Vector2Int anc)
    { //�����Ƴ�ǽ�ĸ��ӣ���������ǽ

        foreach (Vector2Int i in this.e)
        {

            Vector2Int index = anc + i; //�������Ҹ��ӵ�λ��

            if (borderExam(index) && !findMaze(index))
            { //���ȷ�����λ��ȷʵ��ǽ������û��Խ��

                this.M.Add(new Wall(index, anc)); //��ǽ����M��

            }

        }

    }

    void funcM()
    { //���ȡ��M�е�ǽ��ִ�в���

        int index = Random.Range(0, this.M.Count);

        Wall wall = this.M[index];

        if (findMaze(wall.pos))
        { //֮ǰ�Ѿ��Ƴ��˴�ǽ

            this.M.RemoveAt(index);

            return;

        }

        if (findMaze(wall.relative) || !borderExam(wall.relative))
        { //���λ�����Ƴ�ǽ�����ߴ�ǽ�����λ����Ч

            this.M.RemoveAt(index); //��ǽ�Ƴ�

        }

        else
        {

            this.M.RemoveAt(index);

            this.maze.Add(wall.pos);

            this.maze.Add(wall.relative); //������ʲô����ν�������λ����ǽ

            createWall(wall.relative); //�����µ�ǽ

        }

    }

    public void exec()
    { //��ڣ�

        createWall(this.origin); //���

        while (M.Count != 0)
        {

            funcM();

        }

    }

}

//����

public class MazeController : MonoBehaviour

{

    public GameObject wall; //ǽ��Ԥ����

    public Transform destArea; //�յ������λ��

    const int Width = 10;

    const int Height = 10;

    // Start is called before the first frame update

    void Start()

    {

        Vector3 correct1 = new Vector3(0.5f, 0, 0.5f); //ǽ����������

        Vector3 correct2 = new Vector3(-5, 0, -5); //ƽ��ƫ������

        Vector2Int dest = new Vector2Int((int)(destArea.position.x + 4.5f), (int)(destArea.position.y + 4.5f)); //Ŀ������λ������

        Maze maze = new Maze(Width, Height, dest); //����ƽ̨�Ĵ�С�����Թ�

        maze.exec();

        //��ϧC#��û��Range

        for (int x = 0; x < Width; x++)
        {

            for (int y = 0; y < Height; y++)
            {

                if (!maze.maze.Contains(new Vector2Int(x, y)))
                { //������Ϊ��

                    Instantiate(wall, new Vector3(x, 2, y) + correct1 + correct2, Quaternion.identity); //Quaternion.identity��0�Ƕ�

                }

            }

        }

    }

    // Update is called once per frame

    void Update()

    {

    }

}
