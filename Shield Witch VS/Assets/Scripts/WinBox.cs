using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinBox : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			Debug.Log("you won");
			//Destroy(col.gameObject);
			SceneManager.LoadScene("_Win");
		}
	} 
}