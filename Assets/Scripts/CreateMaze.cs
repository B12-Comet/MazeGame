using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMaze : MonoBehaviour
{
    public int row;//�Թ�����
    public int col;//�Թ�����
    public GameObject wall;//ǽ��prefab
    public GameObject road;//��·prefab
    private int[,] _maze;//�߼���ͼ
    private GameObject[,] _map;//ʵ�ʵ�ͼ
    private Transform tran;
    private List<Vector2> _moves = new List<Vector2>();//����Ŀ���

    // Start is called before the first frame update
    void Start()
    {
        InitTerrain();//��ʼ������
        MazeGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void InitTerrain()
    {
        //��ʼ���߼���ͼ
        _maze = new int[row, col];
        //��ʼ��ʵ�ʵ�ͼ
        _map = new GameObject[row, col];
        //tran = _map.GetComponent<Transform>();
        //�ԣ�0��0��Ϊ��׼�㿪ʼ����Ŀ��������Թ�
        /*
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                Instantiate(wall, new Vector3(i, 0,j), Quaternion.identity);
            }
        }
        */
        QueryRoad(0, 0);
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                Debug.Log(_maze[i, j]);
            }
        }

    }
    void QueryRoad(int r,int c)
    {
        string dirs = "";
        if ((r - 2 >= 0) && (_maze[r - 2, c] == 0)) dirs += "N";
        if ((c - 2 >= 0) && (_maze[r, c - 2] == 0)) dirs += "W";
        if ((r + 2 < row) && (_maze[r + 2, c] == 0)) dirs += "S";
        if ((c + 2 < c) && (_maze[r, c + 2] == 0)) dirs += "E";
        if (dirs == "")
        {
            _moves.RemoveAt(_moves.Count - 1);
            if (_moves.Count == 0)
            {
                //����б�Ϊ�գ�˵���Թ�������ϣ����Կ�ʼ�����Թ�
                DrawTerrain();
            }
            else
            {
                QueryRoad((int)_moves[_moves.Count - 1].x, (int)_moves[_moves.Count - 1].y);
            }
        }
        else
        {
            int ran = Random.Range(0, dirs.Length);
            char dir = dirs[ran];

            switch (dir)
            {
                case 'E':
                    _maze[r, c + 1] = 1;
                    c = c + 2;
                    break;
                case 'S':
                    //���м����������Ϊ�ѷ��ʵ�
                    _maze[r + 1, c] = 1;
                    r = r + 2;
                    break;
                case 'W':
                    //���м����������Ϊ�ѷ��ʵ�
                    _maze[r, c - 1] = 1;
                    c = c - 2;
                    break;
                case 'N':
                    //���м����������Ϊ�ѷ��ʵ�
                    _maze[r - 1, c] = 1;
                    r = r - 2;
                    break;
            }
            _maze[r, c] = 1;
            _moves.Add(new Vector2(r, c));

            QueryRoad(r, c);
        }
    }
    void DrawTerrain()
    {
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                switch (_maze[i, j])
                {
                    case 1:
                        if (_map[i, j] != null)
                        {
                            if (_map[i, j].tag == "Road")
                            {
                                continue;
                            }
                            else if (_map[i, j].tag == "Wall")
                            {
                                Destroy(_map[i, j]);
                                _map[i, j] = null;
                            }
                        }
                        _map[i, j] = Instantiate(road, new Vector3(i, 0, j), Quaternion.identity);
                        break;
                    case 0:
                        if (_map[i, j] != null)
                        {
                            if (_map[i, j].tag == "Wall")
                            {
                                continue;
                            }
                            else if (_map[i, j].tag == "Road")
                            {
                                Destroy(_map[i, j]);
                                _map[i, j] = null;
                            }
                        }
                        _map[i, j] = Instantiate(wall, new Vector3(i, 0, j), Quaternion.identity);
                        break;

                }
            }
        }
    }
    public void MazeGenerator()
    {

    }
}
