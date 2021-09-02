using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;


public class AnimationAndMovementController : MonoBehaviour
{
    PlayerInput playerinput;
    CharacterController characterController;
    Animator animator;
    //variables to store optimized setter/getter parametre IDs
    int isWalkingHash;
    int isRunningHash;

    //variables to store player input
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;

    bool isRunPressed;
    float rotationFactorPerFrame = 15f;

    //gravity variables
    float groundedGravity = -.05f;
    public float gravity = -9.8f;

    //jumping variables
    bool isJumpPressed =  false;
    public float intialJumpeVelovity;
    public float maxJumpHight = 2.0f;
    public float maxJumpTime = 1f;
    public bool isJumping = false;
    int isJumpingHash;
    int jumpCountHash;
    bool isJumpingAnimating = false;
    int jumpCount = 0;
    Dictionary<int,float> intialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int,float> JumpGravities = new Dictionary<int, float>();
    Coroutine currentJumpResetRoutine = null;

    void Awake()
    {
        playerinput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");

        //when the action starts , okay unity listen when the player starts using this action
        playerinput.CharacterControlls.Move.started += onMovementInput;
        playerinput.CharacterControlls.Move.canceled += onMovementInput;
        playerinput.CharacterControlls.Move.performed += onMovementInput;
        playerinput.CharacterControlls.run.started += onRun;
        playerinput.CharacterControlls.run.canceled += onRun;
        playerinput.CharacterControlls.run.performed += onRun;
        playerinput.CharacterControlls.Jump.started += onJump;
        playerinput.CharacterControlls.Jump.canceled +=onJump;

        setUpJumpVariables();
    }
    void setUpJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = ((-2 * maxJumpHight)/ Mathf.Pow(timeToApex,2));
        intialJumpeVelovity = 2 * ((2 * maxJumpHight) / timeToApex);
        float secondJumpGravity = (-2 * (2 + maxJumpHight))/ Mathf.Pow((timeToApex * 1.25f),2);
        float secondJumpInitialVelovity = 2 * (2* (2 +  maxJumpHight)) / (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * (3 + maxJumpHight))/ Mathf.Pow((timeToApex * 1.5f),2);
        float thirdJumpInitialVelovity = (2* (3 +  maxJumpHight) / timeToApex * 1.5f);


        intialJumpVelocities.Add(1,intialJumpeVelovity);
        intialJumpVelocities.Add(2,secondJumpInitialVelovity);
        intialJumpVelocities.Add(3,thirdJumpInitialVelovity);

        JumpGravities.Add(0,gravity);
        JumpGravities.Add(1,gravity);
        JumpGravities.Add(2,secondJumpGravity);
        JumpGravities.Add(3,thirdJumpGravity);
        

    }
    void handleJump()
    {
        if(!isJumping && characterController.isGrounded && isJumpPressed)
        {
            if(jumpCount < 3 && currentJumpResetRoutine != null)
            {
                StopCoroutine(currentJumpResetRoutine);
            }
            animator.SetBool(isJumpingHash,true);
            isJumpingAnimating = true;
            isJumping = true;
            jumpCount +=1;
            animator.SetInteger(jumpCountHash,jumpCount);
            currentMovement.y = intialJumpVelocities[jumpCount] * 0.5f;
            currentRunMovement.y = intialJumpVelocities[jumpCount] * 0.5f;
        }else if(!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
    }
    IEnumerator jumpResetRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        jumpCount = 0;
    }
    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        //Debug.Log(isJumpPressed);
    }

    private void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void onMovementInput(InputAction.CallbackContext context) // this function is made to not repeat the process of getting the inputs from the 3
    {                                                         // actions modes (start,perform,cancel) so we made this function and we subscribed them to it
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * 2f;
        currentMovement.z = currentMovementInput.y * 2f;
        currentRunMovement.x = currentMovementInput.x *5.0f;
        currentRunMovement.z = currentMovementInput.y *5.0f;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }
    void handleRotation()
    {
        Vector3 positionToLookAt;
        //the position to look at is the currrent position of the player only the y component is set to 0;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z  = currentMovement.z;
        //the current rotation of the player
        Quaternion currentRotation = transform.rotation;

        if(isMovementPressed)
        {
            //creates a new rotation based on where the player is pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation,targetRotation,rotationFactorPerFrame * Time.deltaTime);
        }       

        
    }
    void handleAnimation()
    {
        //get parametres values from the animator (improved)
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        //starts walking if there is movement and not already walking
        if(isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash,true);
        }
        //stops walking if there's no movement and already walking
        else if(!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash,false);
        }

        if((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash,true);
        }
        else if((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash,false);
        }
    }

    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMulitiplier =2.0f;
        if(characterController.isGrounded)
        {
            if(isJumpingAnimating)
            {
                animator.SetBool(isJumpingHash,false);
                isJumpingAnimating = false;
                currentJumpResetRoutine = StartCoroutine(jumpResetRoutine()); 
                if(jumpCount == 3)
                {
                    jumpCount = 0;
                    animator.SetInteger(jumpCountHash,jumpCount);
                }
            }
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else if(isFalling)
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (JumpGravities[jumpCount] * fallMulitiplier * Time.deltaTime); 
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y = newYVelocity;
            currentRunMovement.y =newYVelocity; 
        }
        else{   
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (JumpGravities[jumpCount] * Time.deltaTime); 
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y = newYVelocity;
            currentRunMovement.y = newYVelocity;
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        handleAnimation();
        handleRotation();
        if(isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }else{
            characterController.Move(currentMovement * Time.deltaTime);
        }
        handleGravity();
        handleJump();
    }
    void OnEnable()
    {
        //enable CharacterControlls ActionMap
        playerinput.CharacterControlls.Enable();
    }
    void OnDisable()
    {
        //Disable CharacterControlls ActionMap
        playerinput.CharacterControlls.Disable();
    }
}
