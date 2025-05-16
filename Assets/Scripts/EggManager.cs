using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class EggManager : NetworkBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Egg eggPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.onGameStateChanged += GameStateChangedCallback;
    }
    public override void OnDestroy(){
        base.OnDestroy();
        GameStateManager.onGameStateChanged -= GameStateChangedCallback;
    }
    private void GameStateChangedCallback(GameStateManager.State gameState){
        switch(gameState){
            case GameStateManager.State.Game:
                SpawnEgg();
                break;
        }
    }
    private void SpawnEgg(){
        if(!IsServer)
            return;
        Egg eggInstance = Instantiate(eggPrefab,Vector2.up * 5,Quaternion.identity);
        eggInstance.GetComponent<NetworkObject>().Spawn();
    }
}
