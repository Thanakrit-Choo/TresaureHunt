using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TressureBox : Interactable
{
    public GameObject panel; // im using m_Light name since 'light' is already a variable used by unity
    public GameObject endScene;
    //public bool isOn;

    private void Start()
    {
        panel.SetActive(false);
    }

    public override string GetDescription()
    {
        return "Press [E] to open the tressure Box.";
    }

    public override void Interact()
    {
        panel.SetActive(true);
        endScene.SetActive(true);
    }
}
