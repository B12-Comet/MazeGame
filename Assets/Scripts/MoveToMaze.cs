using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToMaze : MonoBehaviour
{
  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeScene()
    {
        Debug.Log("click");
        SceneManager.LoadScene("Maze");
    }

    public void ExitGame()
    {
        //预处理
        #if UNITY_EDITOR    //在编辑器模式下
        EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
