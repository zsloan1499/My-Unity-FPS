using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{

    public Text HUDText;

    private PlayerMovement playerMovement;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        HUDText.text = "State: " + playerMovement.GetCurrentState().ToString();
    }

    // Update is called once per frame
    void Update()
    {

        HUDText.text = "State: " + playerMovement.GetCurrentState().ToString();

    }
}
