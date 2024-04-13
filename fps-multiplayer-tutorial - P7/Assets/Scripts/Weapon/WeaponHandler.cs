using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unity.Burst.Intrinsics;

public class WeaponHandler : NetworkBehaviour
{
    [Header("Prefabs")]
    public GrenadeHandler grenadePrefab;
    public RocketHandler rocketPrefab;
    public NetworkObject teleportAura;

    [Header("Effects")]
    public ParticleSystem fireParticleSystem;

    [Header("Aim")]
    public Transform aimPoint;

    [Header("Collision")]
    public LayerMask collisionLayers;
    public LayerMask objLayer;


    [Networked(OnChanged = nameof(OnFireChanged))]
    public bool isFiring { get; set; }

    private int kills;
    float lastTimeFired = 0;
    private NetworkObject playerAura;

    private Vector3 telePortLocation;
    private bool transportSet = false;
    [SerializeField] private int startEnergy = 100;
    [SerializeField] private int teleportEnergy = 25;
    private int energy;

    [SerializeField] private GameObject pauseMenu; //Yes should be in a different script but I am lazy right now.
    private bool isPaused = false;

    //Timing
    TickTimer grenadeFireDelay = TickTimer.None;
    TickTimer rocketFireDelay = TickTimer.None;

    //Other components
    HPHandler hpHandler;
    NetworkPlayer networkPlayer;
    NetworkObject networkObject;
    NetworkInGameMessages networkInGameMessages;

    private void Awake()
    {
        hpHandler = GetComponent<HPHandler>();
        networkPlayer = GetBehaviour<NetworkPlayer>();
        networkObject = GetComponent<NetworkObject>();
        networkInGameMessages = GetComponent<NetworkInGameMessages>();

    }

    // Start is called before the first frame update
    void Start()
    {
        kills = 0;
        energy = startEnergy;
        networkInGameMessages.SetSlider(energy);
        pauseMenu.SetActive(false);
    }

    public override void FixedUpdateNetwork()
    {
        if (hpHandler.isDead)
        {
            energy = startEnergy;
            networkInGameMessages.SetSlider(energy);
            return;
        }
            

        //Get the input from the network
        if (GetInput(out NetworkInputData networkInputData))
        {
            if (networkInputData.isFireButtonPressed)
                Fire(networkInputData.aimForwardVector);

            if (networkInputData.isGrenadeFireButtonPressed)
                FireGrenade(networkInputData.aimForwardVector);

            if (networkInputData.isRocketLauncherFireButtonPressed)
                FireRocket(networkInputData.aimForwardVector);

            if (networkInputData.isSetTransPortPressed)
                setTransportLocation();
            if (networkInputData.isTransportPressed)
                teleport();

            if (networkInputData.isPushPressed)
                PushOBJ(networkInputData.aimForwardVector);

            if (networkInputData.isPullPressed)
                PullOBJ(networkInputData.aimForwardVector);

            if (networkInputData.isPausedPressed)
                pausePressed();
        }
    }

    void PushOBJ(Vector3 aimForwardVector)
    {
        print("Pushed");
        Runner.LagCompensation.Raycast(aimPoint.position, aimForwardVector, 100, Object.InputAuthority, out var hitinfo, collisionLayers, HitOptions.IgnoreInputAuthority);
        if (hitinfo.Hitbox != null)
        {
            print("Push Player");

            if (Object.HasStateAuthority)
            {
                hitinfo.Hitbox.transform.root.GetComponent<NetworkCharacterControllerPrototypeCustom>().pushPlayer(aimForwardVector);

                
            }

            

        }

    }

    void PullOBJ(Vector3 aimForwardVector)
    {
        Runner.LagCompensation.Raycast(aimPoint.position, aimForwardVector, 100, Object.InputAuthority, out var hitinfo, collisionLayers, HitOptions.IgnoreInputAuthority);
        if (hitinfo.Hitbox != null)
        {
            print("Push Player");

            if (Object.HasStateAuthority)
            {
                hitinfo.Hitbox.transform.root.GetComponent<NetworkCharacterControllerPrototypeCustom>().pullPlayer(aimForwardVector);


            }



        }
    }

    void Fire(Vector3 aimForwardVector)
    {
        //Limit fire rate
        if (Time.time - lastTimeFired < 0.15f)
            return;

        StartCoroutine(FireEffectCO());

        Runner.LagCompensation.Raycast(aimPoint.position, aimForwardVector, 100, Object.InputAuthority, out var hitinfo, collisionLayers, HitOptions.IgnoreInputAuthority);

        float hitDistance = 100;
        bool isHitOtherPlayer = false;

        if (hitinfo.Distance > 0)
            hitDistance = hitinfo.Distance;

        if (hitinfo.Hitbox != null)
        {
            Debug.Log($"{Time.time} {transform.name} hit hitbox {hitinfo.Hitbox.transform.root.name}");

            if (Object.HasStateAuthority) {
                hitinfo.Hitbox.transform.root.GetComponent<HPHandler>().OnTakeDamage(networkPlayer.nickName.ToString(), 1);

                if (hitinfo.Hitbox.transform.root.GetComponent<HPHandler>().getDeath())
                {
                    networkInGameMessages.SendKillRPCMessage();
                }
            }

            isHitOtherPlayer = true;

        }
        else if (hitinfo.Collider != null)
        {
            Debug.Log($"{Time.time} {transform.name} hit PhysX collider {hitinfo.Collider.transform.name}");
        }

        //Debug
        if (isHitOtherPlayer)
            Debug.DrawRay(aimPoint.position, aimForwardVector * hitDistance, Color.red, 1);
        else Debug.DrawRay(aimPoint.position, aimForwardVector * hitDistance, Color.green, 1);

        lastTimeFired = Time.time;
    }

    void FireGrenade(Vector3 aimForwardVector)
    {
        //Check that we have not recently fired a grenade. 
        if (grenadeFireDelay.ExpiredOrNotRunning(Runner))
        {
            Runner.Spawn(grenadePrefab, aimPoint.position + aimForwardVector * 1.5f, Quaternion.LookRotation(aimForwardVector), Object.InputAuthority, (runner, spawnedGrenade) =>
            {
                spawnedGrenade.GetComponent<GrenadeHandler>().Throw(aimForwardVector * 15, Object.InputAuthority, networkPlayer.nickName.ToString());
            });

            //Start a new timer to avoid grenade spamming
            grenadeFireDelay = TickTimer.CreateFromSeconds(Runner, 1.0f);
        }
    }

    void FireRocket(Vector3 aimForwardVector)
    {
        //Check that we have not recently fired a grenade. 
        if (rocketFireDelay.ExpiredOrNotRunning(Runner))
        {
            Runner.Spawn(rocketPrefab, aimPoint.position + aimForwardVector * 1.5f, Quaternion.LookRotation(aimForwardVector), Object.InputAuthority, (runner, spawnedRocket) =>
            {
                spawnedRocket.GetComponent<RocketHandler>().Fire(Object.InputAuthority, networkObject, networkPlayer.nickName.ToString());
            });

            //Start a new timer to avoid grenade spamming
            rocketFireDelay = TickTimer.CreateFromSeconds(Runner, 3.0f);
        }
    }

    IEnumerator FireEffectCO()
    {
        isFiring = true;

        fireParticleSystem.Play();

        yield return new WaitForSeconds(0.09f);

        isFiring = false;
    }


    static void OnFireChanged(Changed<WeaponHandler> changed)
    {
        //Debug.Log($"{Time.time} OnFireChanged value {changed.Behaviour.isFiring}");

        bool isFiringCurrent = changed.Behaviour.isFiring;

        //Load the old value
        changed.LoadOld();

        bool isFiringOld = changed.Behaviour.isFiring;

        if (isFiringCurrent && !isFiringOld)
            changed.Behaviour.OnFireRemote();

    }

    void setTransportLocation()
    {
        if (Object.HasStateAuthority)
        {
            if (playerAura != null) Runner.Despawn(playerAura);
            telePortLocation = Object.transform.position;
            transportSet = true;
            playerAura = Runner.Spawn(teleportAura, telePortLocation);
            RPC_SetVisibility(playerAura);
            RPC_MakeVisable(playerAura,telePortLocation);
            //Try sending rpc to self and rest of clients
        }
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    void RPC_SetVisibility(NetworkObject arg, RpcInfo info = default)
    {        
            arg.gameObject.SetActive(false);           
            
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.InputAuthority)]
    void RPC_MakeVisable(NetworkObject arg,Vector3 pos, RpcInfo info = default)
    {
        arg.gameObject.SetActive(true);
        arg.gameObject.transform.position = pos;

    }

    void teleport()
    {
        if (Object.HasStateAuthority && transportSet && energy > 0) {
            Object.transform.position = telePortLocation;
            transportSet = false;
            energy -= teleportEnergy;
            networkInGameMessages.SetSlider(energy);
            Runner.Despawn(playerAura);
        }
    }

    void OnFireRemote()
    {
        if (!Object.HasInputAuthority)
            fireParticleSystem.Play();
    }

    void pausePressed()
    {
        if (isPaused)
        {
            pauseMenu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
        }
        else
        {
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            isPaused = true;
        }
    }
}
