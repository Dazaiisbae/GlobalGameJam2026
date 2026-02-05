using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera firstPersonCam;
    public Camera thirdPersonCam;

    private bool isFirstPerson = true;

    void Start()
    {
        SetCamera(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) // press V to switch
        {
            isFirstPerson = !isFirstPerson;
            SetCamera(isFirstPerson);
        }
    }

    void SetCamera(bool firstPerson)
    {
        firstPersonCam.enabled = firstPerson;
        thirdPersonCam.enabled = !firstPerson;

        // Make Camera.main point to the active camera
        firstPersonCam.tag = firstPerson ? "MainCamera" : "Untagged";
        thirdPersonCam.tag = firstPerson ? "Untagged" : "MainCamera";
    }
}
