using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
//using UnityEditor.VersionControl; 

public class NetworkInGameMessages : NetworkBehaviour
{
    InGameMessagesUIHander inGameMessagesUIHander;

    private int kills = 0;
    private int topKills = 0;
    private int lives = 5;
    private bool setTopScore = false;

    

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SendInGameRPCMessage(string userNickName, string message)
    {
        RPC_InGameMessage($"<b>{userNickName}</b> {message}");
    }

    public void SendKillRPCMessage()
    {
        ++kills;
        RPC_UpdateOwnKills(kills);
        Debug.Log($"Own Kill is {kills} Top Kill is {topKills}");
        if(kills > topKills)
        {
            Debug.Log("Change Top");
            RPC_UpdateKills(kills);
        }
            
    }

    public void SendDeathMessage(int arg)
    {
        
       
        RPC_UpdateOwnKills(arg);
        if(lives < 0)
        {

        }
    }

    public void SetSlider(int arg)
    {
        RPC_setOwnSlider(arg);
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    void RPC_setOwnSlider(int arg)
    {
        inGameMessagesUIHander.updateOwnEnergy(arg);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_InGameMessage(string message, RpcInfo info = default)
    {
        Debug.Log($"[RPC] InGameMessage {message}");

        if (inGameMessagesUIHander == null)
            inGameMessagesUIHander = NetworkPlayer.Local.localCameraHandler.GetComponentInChildren<InGameMessagesUIHander>();

        if (inGameMessagesUIHander != null)
            inGameMessagesUIHander.OnGameMessageReceived(message);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    void RPC_UpdateOwnKills(int arg)
    {
        //kills = kills + 1;
        
        /*if (kills >= topKills)
        {            
            RPC_UpdateKills(kills);
        } */
            
            
            if (inGameMessagesUIHander == null)
                inGameMessagesUIHander = NetworkPlayer.Local.localCameraHandler.GetComponentInChildren<InGameMessagesUIHander>();

            if (inGameMessagesUIHander != null)
                inGameMessagesUIHander.UpdateOwnScore(arg);
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_UpdateKills(int arg)
    {
        
        if(arg >= topKills) {
            print($"print State topKill {topKills}, arg = {arg}");
            topKills = arg;
            if (inGameMessagesUIHander == null)
                inGameMessagesUIHander = NetworkPlayer.Local.localCameraHandler.GetComponentInChildren<InGameMessagesUIHander>();

            if (inGameMessagesUIHander != null)
                inGameMessagesUIHander.UpdateTopScore(arg);
        }
    }
}
