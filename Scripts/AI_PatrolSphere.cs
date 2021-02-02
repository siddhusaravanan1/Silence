using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_PatrolSphere : MonoBehaviour
{
    public bool playerSpotted;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            playerSpotted = true;

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            playerSpotted = false;
    }
}
