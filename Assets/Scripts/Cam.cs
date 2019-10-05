using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    Camera mainCam;
    GameObject player;
    Transform camTarget;
    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                camTarget = player.transform;
                vcam.LookAt = camTarget;
                vcam.Follow = camTarget;
            }
        }
    }
}
