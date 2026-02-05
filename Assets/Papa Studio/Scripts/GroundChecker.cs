using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class GroundChecker : MonoBehaviour
{

    [Header("CineMachine Camera")]
    public CinemachineVirtualCamera vCam1;
    public CinemachineVirtualCamera vCam2;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground")) {
            //  gameObject.transform.parent.transform.GetComponent<SmoothFollow>().enabled = false;
            //    gameObject.transform.parent.transform.GetComponent<LookAt>().enabled = true;

            vCam2.Priority = 1;
            vCam1.Priority = 0;

        }
    }
}
