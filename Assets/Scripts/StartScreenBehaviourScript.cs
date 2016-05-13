using UnityEngine;
using System.Collections;

public class StartScreenBehaviourScript : MonoBehaviour {

    public void StartGame()
    {
        UnityEngine.
          SceneManagement.
          SceneManager.LoadScene(1);
    }
	
}
