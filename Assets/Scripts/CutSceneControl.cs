using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneControl : MonoBehaviour
{
    public PlayerController player;

    private void Start()
    {
        player.OnDisable();
    }

    public void FinishScene()
    {
        player.OnEnable();
        this.gameObject.SetActive(false);
    }

    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
}
