using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject hostageSphere;
    public Image img;
    public float speed = 18;
    public GameObject bullet;
    public GameObject gunPoint;
    public GameObject playerCamera;
    public GameObject crossHair;
    public float health;
    public float damage;
    public GameObject right, left;
    public bool isHostageRescued;

    private bool isAiming;
    private Rigidbody rig;
    bool canRotate;
    bool readyToShoot;
    bool canMove;
    float t;
    bool isInRecovery;
    bool isRespawning;
    bool isRunning;
    bool isCrouching;
    bool isCrouchAim;

    Vector3 startPos;
    Quaternion startRot;
    float currSpeed;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        startPos = transform.position;
        startRot = transform.rotation;
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(0, 0, vAxis) * speed * Time.deltaTime;

        if (hAxis > 0 && !isAiming)
        {
            GetComponent<Transform>().Rotate(new Vector3(0, 1, 0), 0.5f);
            GetComponent<Animator>().SetBool("right", true);
        }
        else if (hAxis < 0 && !isAiming)
        {
            GetComponent<Transform>().Rotate(new Vector3(0, -1, 0), 0.5f);
            GetComponent<Animator>().SetBool("left", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("right", false);
            GetComponent<Animator>().SetBool("left", false);
        }

        if (vAxis > 0 && !readyToShoot)
        {
            if (!isAiming)
                transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed * 2);
            GetComponent<Animator>().SetBool("right", false);
            GetComponent<Animator>().SetBool("left", false);
            if (isRunning)
            {
                GetComponent<Animator>().SetBool("run", true);
                //GetComponent<Animator>().SetBool("walk", false);
                GetComponent<Animator>().SetBool("crouchWalk", false);
            }
            else if (isCrouching)
            {
                GetComponent<Animator>().SetBool("crouchWalk", true);
                GetComponent<Animator>().SetBool("run", false);
                GetComponent<Animator>().SetBool("walk", false);
            }
            else
            {
                GetComponent<Animator>().SetBool("walk", true);
                GetComponent<Animator>().SetBool("run", false);
                GetComponent<Animator>().SetBool("crouchWalk", false);
            }
            rig.MovePosition(transform.position + transform.forward * (0.2f * currSpeed));
            canRotate = true;
        }
        else if (vAxis < 0 && !readyToShoot)
        {
            if (!isAiming)
                transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed * 2);
            GetComponent<Animator>().SetBool("right", false);
            GetComponent<Animator>().SetBool("left", false);
            GetComponent<Animator>().SetBool("back", true);
            rig.MovePosition(transform.position + transform.forward * (-0.1f * currSpeed));
            canRotate = true;
        }
        else
        {
            GetComponent<Animator>().SetBool("walk", false);
            GetComponent<Animator>().SetBool("run", false);
            GetComponent<Animator>().SetBool("back", false);
            canRotate = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //if (!isCrouching)
            //{
                readyToShoot = true;
                GetComponent<Animator>().SetBool("aim", true);
                playerCamera.GetComponent<Animator>().SetBool("camZoom", true);
                crossHair.SetActive(true);
                isAiming = true;
                GetComponent<Rigidbody>().isKinematic = false;
            //}
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            readyToShoot = false;
            GetComponent<Animator>().SetBool("aim", false);
            playerCamera.GetComponent<Animator>().SetBool("camZoom", false);
            crossHair.SetActive(false);
            isAiming = false;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
            GetComponent<Rigidbody>().isKinematic = false;
        }


        if (Input.GetKeyDown(KeyCode.Mouse0) && readyToShoot && !isInRecovery)
        {

            GetComponent<Animator>().SetTrigger("shoot");
            Instantiate(bullet, gunPoint.transform.position, transform.rotation);
            GetComponent<AudioSource>().Play();
            isInRecovery = true;
            t = 0;
        }

        if (isAiming)
        {
            float angle = transform.eulerAngles.x;
            angle = (angle > 180) ? angle - 360 : angle;
            if (angle >= -15 && angle <= 15)
            {
                if (angle + (-Input.GetAxis("Mouse Y")) > -14 && angle + (-Input.GetAxis("Mouse Y")) < 14)
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x + (-Input.GetAxis("Mouse Y")), transform.eulerAngles.y, transform.eulerAngles.z);
            }

            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed * 2);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }


        if (isInRecovery)
        {
            t = t + Time.deltaTime;
            if (t > 0.8f)
                isInRecovery = false;
        }


        if (health <= 0)
        {
            if (!isRespawning)
                StartCoroutine(FadeImage(false, 0.5f));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            isRunning = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isRunning = false;
        if (Input.GetKeyDown(KeyCode.LeftControl))
            isCrouching = true;
        if (Input.GetKeyUp(KeyCode.LeftControl))
            isCrouching = false;

        if (isRunning)
        {
            isCrouching = false;
            currSpeed = 2f;
        }
        else if (!isCrouching && !isRunning)
        {
            currSpeed = 1f;
        }
        if (isCrouching)
        {
            isRunning = false;
            currSpeed = 0.5f;
            if (vAxis == 0)
            {
                GetComponent<Animator>().SetBool("crouchWalk", false);
                GetComponent<Animator>().SetBool("crouch", true);
            }
            else
            {
                GetComponent<Animator>().SetBool("crouchWalk", true);
                GetComponent<Animator>().SetBool("crouch", false);
            }
        }
        else if (!isRunning && !isCrouching)
        {
            currSpeed = 1f;
            GetComponent<Animator>().SetBool("crouch", false);
            GetComponent<Animator>().SetBool("crouchWalk", false);
        }
    }

    public void doDamage(float damage)
    {
        health = health - damage;
    }

    void doRespawn()
    {
        health = 100f;
        transform.position = startPos;
        transform.rotation = startRot;
        playerCamera.GetComponent<Animator>().SetTrigger("reset");
        StartCoroutine(FadeImage(true, 1f));
        hostageSphere.GetComponent<HostageSphere>().doRespawn();
    }

    IEnumerator FadeImage(bool fadeAway, float t)
    {
        isRespawning = true;
        if (fadeAway)
        {
            yield return new WaitForSeconds(2f);
            for (float i = 1; i >= 0; i -= Time.deltaTime * t)
            {
                img.color = new Color(0, 0, 0, i);
                yield return null;
            }

            isRespawning = false;

        }

        else
        {
            GetComponent<Animator>().SetTrigger("die");
            yield return new WaitForSeconds(2f);

            for (float i = 0; i <= 1; i += Time.deltaTime * t)
            {
                img.color = new Color(0, 0, 0, i);
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            doRespawn();

            GetComponent<Animator>().SetTrigger("reset");
        }
    }
}
