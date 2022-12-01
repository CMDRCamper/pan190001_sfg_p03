using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private string UInput;
    [SerializeField] float bufferTime;
    private Queue<InputClass> inputBuffer = new Queue<InputClass>();
    private InputClass[] searchBuffer = new InputClass[16];
    private ArrayList fireballArray = new ArrayList();
    private bool canTakeInputs = true;
    private bool downFound = false;
    private bool downRightFound = false;
    private bool rightFound = false;
    private bool canAct = true;
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
        InputClass motionInput = new InputClass("Down", bufferTime);
        fireballArray.Add(motionInput);
        motionInput = new InputClass("Down-right", bufferTime);
        fireballArray.Add(motionInput);
        motionInput = new InputClass("Right", bufferTime);
        fireballArray.Add(motionInput);
        motionInput = new InputClass("Attack", bufferTime);
        fireballArray.Add(motionInput);
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
            InputClass motionInput = new InputClass(UInput, bufferTime);
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(motionInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A))
        {
            UInput = "Right";
            InputClass motionInput = new InputClass(UInput, bufferTime);
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(motionInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            UInput = "Up";
            InputClass motionInput = new InputClass(UInput, bufferTime);
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(motionInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A))
        {
            UInput = "Down";
            InputClass motionInput = new InputClass(UInput, bufferTime);
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(motionInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D))
        {
            UInput = "Down-left";
            InputClass motionInput = new InputClass(UInput, bufferTime);
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(motionInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            UInput = "Up-left";
            InputClass motionInput = new InputClass(UInput, bufferTime);
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(motionInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A))
        {
            UInput = "Down-right";
            InputClass motionInput = new InputClass(UInput, bufferTime);
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(motionInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A))
        {
            UInput = "Up-right";
            InputClass motionInput = new InputClass(UInput, bufferTime);
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(motionInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (!Input.anyKey)
        {
            UInput = "Neutral";
            InputClass motionInput = new InputClass(UInput, bufferTime);
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(motionInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {

            searchBuffer = inputBuffer.ToArray();
            for (int i = 0; i < searchBuffer.Length; i++)
            {
                
                if (searchBuffer[i].getInputDir() == "Down" && !downFound && !downRightFound && !rightFound)
                {
                    
                    downFound = true;
                    
                }
                if (searchBuffer[i].getInputDir() == "Down-right" && downFound && !downRightFound && !rightFound)
                {
                    
                    downRightFound = true;

                }
                if (searchBuffer[i].getInputDir() == "Right" && downFound && downRightFound && !rightFound)
                {
                    
                    rightFound = true;

                }


            }

            StartCoroutine(fireballDecay());
            
            if(downFound && downRightFound && rightFound && canAct)
            {
                UInput = "HADOKEN!!!!";
                throwFireball();
            }
            else if(!downFound && canAct || !downRightFound && canAct || !rightFound && canAct)
            {
                UInput = "Attack";
                Attack();
                
            }
                
            
            InputClass motionInput = new InputClass(UInput, bufferTime);
            if (canTakeInputs)
            {
                inputBuffer.Enqueue(motionInput);
                canTakeInputs = false;
                StartCoroutine(ResetInput());
            }
        }
        if(inputBuffer.Count >= 16)
        {
            inputBuffer.Dequeue();
        }
        
        
        
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
        downFound = false;
        downRightFound = false;
        rightFound = false;
    }
}
