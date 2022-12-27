using UnityEngine;
using System.Collections;

public class FlowPlayerMove : MonoBehaviour
{
	//public Vector3 juli = new Vector3(0f, 2.5f, -2f);
	public Vector3 juli;
	public GameObject player;
	public float smoothing = 6;
	public Vector3 playerPos;
	// Use this for initialization
	void Start()
	{
		//player = GameObject.FindGameObjectWithTag("player");
		juli = this.transform.position - player.transform.position;
		playerPos = player.transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		Debug.Log(player.transform.position);
		//Vector3 pos = player.transform.position + juli;//相机移动的目标位置
		Vector3 pos = new Vector3(playerPos.x, playerPos.y, playerPos.z + 5f);
		transform.position = Vector3.Lerp(transform.position, pos, smoothing * Time.deltaTime);
	}
}