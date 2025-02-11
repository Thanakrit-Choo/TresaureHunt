using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameName;
    public GameObject playButton;
    public GameObject exitButton;
    public GameObject transition;
    public Animator cam;

    // Update is called once per frame

    public void PlayButton()
    {
        StartCoroutine(LoadScene("Darkruin"));

    }

    IEnumerator LoadScene(string name)
    {
        cam.SetBool("gameStart", true);
        playButton.SetActive(false);
        exitButton.SetActive(false);
        gameName.SetActive(false);
        yield return new WaitForSeconds(3f);
        transition.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(name);
    }
    public void ExitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
