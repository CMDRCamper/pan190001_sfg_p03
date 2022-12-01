using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private string UInput;
    [SerializeField] int bufferTime;
    private Queue<string> inputBuffer = new Queue<string>();
    private string[] searchBuffer;
    private bool canTakeInputs = true;
    private bool downFound = false;
    private bool downRightFound = false;
    private bool rightFound = false;
    private bool canAct = true;
    private bool QCFExecuted = false;
    private Animator playerAnim;
    [SerializeField] AudioSource fireballAudio;
    [SerializeField] AudioClip attackClip;
    [SerializeField] AudioClip fireballClip;
    AudioSource playerAudio;
    [SerializeField] private float moveSpeed;
    Rigidbody playerRB;
    [SerializeField] GameObject fireball;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        searchBuffer = new string[bufferTime];
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if (canAct)
        {
            playerRB.velocity = new Vector3(x * moveSpeed * Time.deltaTime, 0, 0);
        }
        if (!canAct)
        {
            playerRB.velocity = new Vector3(0, 0, 0);
        }
        Debug.Log(UInput);
        if(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D))
        {
            UInput = "Left";
            
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(UInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A))
        {
            UInput = "Right";
        
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(UInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            UInput = "Up";
          
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(UInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A))
        {
            UInput = "Down";
           
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(UInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D))
        {
            UInput = "Down-left";
           
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(UInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            UInput = "Up-left";
           
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(UInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A))
        {
            UInput = "Down-right";
            
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(UInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A))
        {
            UInput = "Up-right";
            
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(UInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (!Input.anyKey)
        {
            UInput = "Neutral";
            
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(UInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {

            //When player presses a button has a motion input been performed?
            QCFExecuted = checkForQCF();
            //Kill input that is done too early during inactionable state
            StartCoroutine(fireballDecay());
            //perform action depending on whether or not a motion input has been performed.
            if(QCFExecuted && canAct)
            {
                UInput = "HADOKEN!!!!";
                throwFireball();
            }
            else if(!QCFExecuted && canAct)
            {
                UInput = "Attack";
                Attack();
            }
                
            
            //Record input in buffer
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(UInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if(inputBuffer.Count >= bufferTime)
        {
            inputBuffer.Dequeue();
        }
        
        
        
    }

    private bool checkForQCF()
    {
        bool execution = false; ;
        searchBuffer = inputBuffer.ToArray();
        for (int i = 0; i < searchBuffer.Length; i++)
        {
            if (searchBuffer[i] == "Down" && !downFound && !downRightFound && !rightFound)
            {
                downFound = true;
            }
            if (searchBuffer[i] == "Down-right" && downFound && !downRightFound && !rightFound)
            {
                downRightFound = true;
            }
            if (searchBuffer[i] == "Right" && downFound && downRightFound && !rightFound)
            {
                rightFound = true;
            }
        }

        if(downFound && downRightFound && rightFound)
        {
            execution = true;
        }

        return execution;
    }

    private void Attack()
    {
        playerAnim.SetBool("Attacking", true);
        canAct = false;
        playerAudio.clip = attackClip;
        playerAudio.Play();
        StartCoroutine(makeActionable(.5f));
    }

    private void throwFireball()
    {
        downFound = false;
        downRightFound = false;
        rightFound = false;
        canAct = false;
        playerAudio.clip = fireballClip;
        playerAudio.Play();
        playerAnim.SetBool("Hadoukening", true);
        StartCoroutine(fireballPosChange());
        StartCoroutine(makeActionable(.5f));
    }

    private IEnumerator makeActionable(float time)
    {
        yield return new WaitForSeconds(time);
        playerAnim.SetBool("Attacking", false);
        playerAnim.SetBool("Hadoukening", false);
        canAct = true;
    }
    private IEnumerator ResetInput(float resetTime = .032f)
    {
        yield return new WaitForSeconds(resetTime);
        canTakeInputs = true;
    }

    private IEnumerator fireballPosChange()
    {
        yield return new WaitForSeconds(.4f);
        fireball.transform.position = gameObject.transform.position;
        fireballAudio.Play();
    }
    private IEnumerator fireballDecay()
    {
        yield return new WaitForSeconds(.26f);
        QCFExecuted = false;
    }
}
