using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;
    bool isFireButtonPressed = false;
    bool isGrenadeFireButtonPressed = false;
    bool isRocketLauncherFireButtonPressed = false;
    bool isSetTransPortPressed = false;
    bool isTransportPressed = false;
    bool isPausedPressed = false;

    //Other components
    LocalCameraHandler localCameraHandler;
    CharacterMovementHandler characterMovementHandler;

    private void Awake()
    {
        localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
        characterMovementHandler = GetComponent<CharacterMovementHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!characterMovementHandler.Object.HasInputAuthority)
            return;

        //View input
        viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y") * -1; //Invert the mouse look

        //Move input
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");

        //Jump
        if (Input.GetButtonDown("Jump"))
            isJumpButtonPressed = true;

        //Fire
        if (Input.GetButtonDown("Fire1"))
            isFireButtonPressed = true;

        //Fire
        if (Input.GetButtonDown("Fire2"))
            isRocketLauncherFireButtonPressed = true;

        //Throw grenade
        if (Input.GetKeyDown(KeyCode.G))
            isGrenadeFireButtonPressed = true;

        if(Input.GetKeyDown(KeyCode.Q))
            isSetTransPortPressed = true;
        if(Input.GetKeyDown(KeyCode.E))
            isTransportPressed = true;

        if(Input.GetKeyDown(KeyCode.Z))
            isPausedPressed = true;

        //Set view
        localCameraHandler.SetViewInputVector(viewInputVector);

    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        //Aim data
        networkInputData.aimForwardVector = localCameraHandler.transform.forward;

        //Move data
        networkInputData.movementInput = moveInputVector;

        //Jump data
        networkInputData.isJumpPressed = isJumpButtonPressed;

        //Fire data
        networkInputData.isFireButtonPressed = isFireButtonPressed;

        //Rocket data
        networkInputData.isRocketLauncherFireButtonPressed = isRocketLauncherFireButtonPressed;

        //Grenade fire data
        networkInputData.isGrenadeFireButtonPressed = isGrenadeFireButtonPressed;

        //Set Transportation Data
        networkInputData.isSetTransPortPressed = isSetTransPortPressed;
        networkInputData.isTransportPressed = isTransportPressed;

        networkInputData.isPausedPressed = isPausedPressed;

        //Reset variables now that we have read their states
        isJumpButtonPressed = false;
        isFireButtonPressed = false;
        isGrenadeFireButtonPressed = false;
        isRocketLauncherFireButtonPressed = false;
        isSetTransPortPressed = false;
        isTransportPressed = false;
        isPausedPressed = false;

        return networkInputData;
    }
}
