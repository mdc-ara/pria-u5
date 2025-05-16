using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{
    [Header(" Panels ")]
    [SerializeField] private GameObject connectionPannel;
    [SerializeField] private GameObject waitingPannel;
    [SerializeField] private GameObject gamePannel;
    // Start is called before the first frame update
    void Start()
    {
        ShowConnectionPannel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowConnectionPannel(){
        connectionPannel.SetActive(true);
        waitingPannel.SetActive(false);
        gamePannel.SetActive(false);
    }
    private void ShowWaitingPannel(){
        connectionPannel.SetActive(false);
        waitingPannel.SetActive(true);
        gamePannel.SetActive(false);
    }
    private void ShowGamePannel(){
        connectionPannel.SetActive(false);
        waitingPannel.SetActive(false);
        gamePannel.SetActive(true);
    }
    public void HostButtonCallback(){
        NetworkManager.Singleton.StartHost();
        ShowWaitingPannel();
    }
    public void ClientButtonCallback(){
        NetworkManager.Singleton.StartClient();
        ShowWaitingPannel();
    }
}
