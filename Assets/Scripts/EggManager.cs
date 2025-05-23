using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class EggManager : NetworkBehaviour
{
    public static EggManager instance;
    [Header(" Elements ")]
    [SerializeField] private Egg eggPrefab;
    private void Awake(){
        if(instance==null)
            instance=this;
        else
            Destroy(gameObject);
    }
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
        eggInstance.transform.SetParent(this.transform);
    }
    public void ReuseEgg(){
        if(!IsServer)
            return;
        if(transform.childCount <=0)
            return;
        transform.GetChild(0).GetComponent<Egg>().Reuse();
    }
}
