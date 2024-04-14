using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;



public class UIButtons : NetworkBehaviour
{
    NetworkRunner networkRunner;
    NetworkPlayer networkPlayer;

    PlayerRef localPlayer;

    private void Awake()
    {
        networkPlayer = GetBehaviour<NetworkPlayer>();
        if(networkPlayer != null )
            localPlayer = networkRunner.LocalPlayer;
    }
    public void changeScene(int arg)
    {
        NetworkRunner networkRunnerInScene = FindObjectOfType<NetworkRunner>();

        //If we already have a network runner in the scene then we should not create another one but rahter use the existing one
        if (networkRunnerInScene != null)
            networkRunner = networkRunnerInScene;

        networkRunner.Disconnect(localPlayer);
        Debug.Log("Change Scenes");
        SceneManager.LoadScene(arg);
        
    }
}
