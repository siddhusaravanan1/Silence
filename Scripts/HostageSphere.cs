using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostageSphere : MonoBehaviour
{
    public Text commentText;
    public GameObject hostage;
    public GameObject target;
    Vector3 startPos;
    Quaternion startRot;
    GameObject player;
    bool isFollowing;
    Animator animator;
    float MaximumDistance = 7f;
    private void Start()
    {
        animator = hostage.GetComponent<Animator>();
        startPos = hostage.transform.position;
        startRot = hostage.transform.rotation;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isFollowing)
        {
            if (other.tag == "Player")
            {
                commentText.text = "Press E to rescue Hostage";
            }
        }
        else
            commentText.text = "";
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isFollowing)
        {
            if (other.tag == "Player")
            {
                player = other.gameObject;
                commentText.text = "Press E to rescue Hostage";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    other.GetComponent<PlayerController>().isHostageRescued = true;
                    animator.SetTrigger("stand");
                    commentText.text = "";
                    isFollowing = true;
                }
            }
        }
        else
            commentText.text = "";

    }

    private void OnTriggerExit(Collider other)
    {
        if (!isFollowing)
        {
            if (other.tag == "Player")
            {
                commentText.text = "";
            }
        }
        else
            commentText.text = "";
    }

    private void Update()
    {
        if (isFollowing)
        {
            hostage.transform.LookAt(player.transform);
            Vector3 Diff = transform.position - target.transform.position;
            if (player.GetComponent<Animator>().GetBool("walk") || player.GetComponent<Animator>().GetBool("back"))
            {
                if (Diff.magnitude > MaximumDistance)
                {
                    animator.SetBool("walk", true);
                    float mult = MaximumDistance / Diff.magnitude;
                    hostage.transform.position -= Diff;
                    hostage.transform.position += Diff * mult;
                }
            }
            else
                animator.SetBool("walk", false);
        }
    }

    public void doRespawn()
    {
        hostage.transform.position = startPos;
        hostage.transform.rotation = startRot;
        isFollowing = false;
        commentText.text = "";
        animator.SetTrigger("reset");
    }


}
