using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{

    public Text PlayerStateText;
    public Text ReloadText;

    private PlayerMovement playerMovement;
    private Glock glock;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        glock = FindObjectOfType<Glock>();
        PlayerStateText.text = "State: " + playerMovement.GetCurrentState().ToString();
        ReloadText.text = glock.isReloading.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        PlayerStateText.text = "State: " + playerMovement.GetCurrentState().ToString();
        ReloadText.text = glock.isReloading.ToString();

    }
}
