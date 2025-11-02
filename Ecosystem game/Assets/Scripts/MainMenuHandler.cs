using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    private float chunguschance;
    public GameObject chungus;
    // Start is called before the first frame update
    void Start()
    {
        chunguschance = Random.Range(1, 100);
        chungus.SetActive(false);
        if (chunguschance == 2)
        {
            chungus.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
