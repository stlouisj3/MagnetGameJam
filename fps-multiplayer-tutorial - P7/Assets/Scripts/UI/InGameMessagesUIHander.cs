using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;

public class InGameMessagesUIHander : MonoBehaviour
{
    public TextMeshProUGUI[] textMeshProUGUIs;

    public TextMeshProUGUI playerScoreTxt;
    public TextMeshProUGUI topScoreTxt;
    private int topScore;
    private int playerScore;

    public GameObject WinScreen;
    public TextMeshProUGUI gameCondTxt;
    public Slider energySlider;

    [SerializeField] private int killsToWin = 5;

    Queue messageQueue = new Queue();
    NetworkRunner networkRunner;

    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;
        topScore = 0;
        WinScreen.SetActive(false);
    }

    public void OnGameMessageReceived(string message)
    {
        //Debug.Log($"InGameMessagesUIHander {message}");

        messageQueue.Enqueue(message);

        if (messageQueue.Count > 3)
            messageQueue.Dequeue();

        int queueIndex = 0;
        foreach (string messageInQueue in messageQueue)
        {
            textMeshProUGUIs[queueIndex].text = messageInQueue;
            queueIndex++;
        }

    }

    public void UpdateOwnScore(int arg)
    {
        playerScoreTxt.text = arg.ToString();
        playerScore = arg;
    }



    public void UpdateTopScore(int arg)
    {
        if (arg > topScore)
        {
            topScoreTxt.text = arg.ToString();
            topScore = arg;
        }
        if(topScore >= killsToWin)
        {
            if (topScore == playerScore)
              gameOverScreen(true);
            else
                gameOverScreen(false);
        }

    }

    public void updateOwnEnergy(int arg)
    {
        energySlider.value = arg;
    }
    private void gameOverScreen(bool arg)
    {
        
        WinScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        if (arg)
        {
            gameCondTxt.text = "You Win!!";
        }
        else
        {
            gameCondTxt.text = "You Lose :(";
        }
        //yield return new WaitForSeconds(1f);
        //networkRunner = FindObjectOfType<NetworkRunner>();
        //if (networkRunner != null) { networkRunner.Shutdown(); }
    }
}
