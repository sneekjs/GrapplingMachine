using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public int amountOfBalls;
    public GameObject[] balls;

	void Start () {
        SpawnRandomBalls();
    }
	
	void Update () {
        TimeHacks();
	}
#region HackFunctions
    public void SpawnRandomBalls()
    {
        for (int i = 0; i < amountOfBalls; i++)
        {
            Instantiate(balls[Random.Range(0,balls.Length)], new Vector3(0f, 2.2f, -0.75f), Quaternion.identity);
        }
    }

    public void TimeHacks()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            Time.timeScale = Time.timeScale / 2;
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            Time.timeScale = Time.timeScale * 2;
        }
    }
#endregion
}
