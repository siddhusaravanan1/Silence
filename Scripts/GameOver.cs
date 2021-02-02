using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject playerPos;
    public GameObject hostagePos;
    public Image img;
    
    public GameObject player;
    public GameObject hostage;
    public GameObject hostageSphere;
    public GameObject playerCamera;

    public GameObject danceEnemy;
    public GameObject goText, rescueText;

    private bool gameOver;

    private void Start()
    {
        goText.SetActive(false);
        rescueText.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.GetComponent<Animator>().SetTrigger("reset");
            hostage.GetComponent<Animator>().SetTrigger("idle");
            StartCoroutine(FadeImage(false));

            playerCamera.GetComponent<AudioSource>().Stop();
            goText.SetActive(true);
            rescueText.SetActive(true);
            player.GetComponent<PlayerController>().enabled = false;

            GetComponent<AudioSource>().Play();
            StartCoroutine(doDance());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PlayerController>().isHostageRescued)
            {
                StartCoroutine(FadeImage(false));
                hostageSphere.GetComponent<HostageSphere>().enabled = false;
                playerCamera.GetComponent<AudioSource>().Stop();
                goText.SetActive(true);
                rescueText.SetActive(true);
                player.GetComponent<PlayerController>().enabled = false;

                GetComponent<AudioSource>().Play();
                StartCoroutine(doDance());
            }

        }
    }


    IEnumerator FadeImage(bool fadeAway)
    {
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                img.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }

        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                img.color = new Color(0, 0, 0, i);
                yield return null;
            }

            yield return new WaitForSeconds(2f);
            player.GetComponent<Animator>().SetTrigger("dance");
            hostage.GetComponent<Animator>().SetTrigger("dance");

            player.transform.position = playerPos.transform.position;
            player.transform.rotation = playerPos.transform.rotation;
            hostage.transform.position = hostagePos.transform.position;
            hostage.transform.rotation = hostagePos.transform.rotation;

            

        }
    }

    IEnumerator doDance()
    {
        yield return new WaitForSeconds(2f);
        playerCamera.GetComponent<Animator>().SetTrigger("dance");
        yield return new WaitForSeconds(1.25f);
        StartCoroutine(FadeImage(true));
        danceEnemy.SetActive(true);
        Animator[] go = danceEnemy.GetComponentsInChildren<Animator>();
        foreach (var item in go)
        {
            item.SetTrigger("dance");
        }
    }
}
