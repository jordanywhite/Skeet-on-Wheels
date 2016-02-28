using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour {

    public Button startBtn, exitBtn;

	// Use this for initialization
	void Start () {
        //Screen.SetResolution(800, 780, false);
        startBtn = startBtn.GetComponent<Button>();
        exitBtn = exitBtn.GetComponent<Button>();

        //startBtn.onClick.AddListener(() => { Play(); });
        //exitBtn.onClick.AddListener(() => { ExitGame(); });

        //PlayerPrefs.DeleteAll();
    }

    public void Play()
    {
        //SceneManager.LoadScene("Main");
        Application.LoadLevel(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
