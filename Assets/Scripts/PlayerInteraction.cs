using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    public float interactionDistance;

    public TMPro.TextMeshProUGUI interactionText;
    //public GameObject interactionHoldGO; // the ui parent to disable when not interacting
    //public UnityEngine.UI.Image interactionHoldProgress; // the progress bar for hold interaction type

    Camera cam;
    PlayerController playerController;
    void Start()
    {
        cam = Camera.main;
        TryGetComponent<PlayerController>(out playerController);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        //Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        Vector3 origin = new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z);
        bool successfulHit = false;

        if (Physics.Raycast(origin, transform.forward, out hit, 0.5f))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                HandleInteraction(interactable);
                interactionText.text = interactable.GetDescription();
                successfulHit = true;

                //interactionHoldGO.SetActive(interactable.interactionType == Interactable.InteractionType.Hold);
            }
        }

        // if we miss, hide the UI
        if (!successfulHit)
        {
            interactionText.text = "";
            //interactionHoldGO.SetActive(false);
            playerController.InteractDisable();
            playerController.animator.SetBool("interaction", PlayerController.isInteraction);
        }
    }

    void HandleInteraction(Interactable interactable)
    {
        //KeyCode key = KeyCode.E;
        playerController.InteractEnable();
        switch (interactable.interactionType)
        {
            case Interactable.InteractionType.Click:
                // interaction type is click and we clicked the button -> interact
                if (PlayerController.isInteraction)
                {
                    interactable.Interact();
                    playerController.OnDisable();
                }
                PlayerController.isInteraction = false;
                break;
            case Interactable.InteractionType.Hold:
                if (PlayerController.isInteraction)
                {
                    // we are holding the key, increase the timer until we reach 1f
                    interactable.IncreaseHoldTime();
                    if (interactable.GetHoldTime() > 1f) {
                        interactable.Interact();
                        interactable.ResetHoldTime();
                    }
                }
                else
                {
                    interactable.ResetHoldTime();
                }
                //interactionHoldProgress.fillAmount = interactable.GetHoldTime();
                break;
            // here is started code for your custom interaction :)
            case Interactable.InteractionType.Minigame:
                // here you make ur minigame appear
                break;

            // helpful error for us in the future
            default:
                throw new System.Exception("Unsupported type of interactable.");
        }
    }
}