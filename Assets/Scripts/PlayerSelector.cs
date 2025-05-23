using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerSelector : NetworkBehaviour
{
	public static PlayerSelector instance;
	private bool isHostTurn;

	private void Awake(){
		if(instance==null)
			instance=this;
		else
			Destroy(gameObject);
	}
	public bool IsHostTurn(){
		return isHostTurn;
	}
	public override void OnNetworkSpawn(){
		base.OnNetworkSpawn();
		NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
	}
	private void NetworkManager_OnServerStarted(){
		if(!IsServer)
			return;
		GameStateManager.onGameStateChanged += GameStateChangedCallback;
		Egg.onHit += SwitchPlayers;
	}
	private void SwitchPlayers(){
		isHostTurn = !isHostTurn;
		InitializePlayers();
	}
	public override void OnDestroy(){
		base.OnDestroy();
		NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
		GameStateManager.onGameStateChanged -= GameStateChangedCallback;
		Egg.onHit -= SwitchPlayers;
	}
	private void GameStateChangedCallback(GameStateManager.State gameState){
		switch(gameState){
			case GameStateManager.State.Game:
				InitializePlayers();
				break;
		}
	}
	private void InitializePlayers(){
		PlayerStateManager[] playerStateManagers = FindObjectsOfType<PlayerStateManager>();
		for(int i =0;i<playerStateManagers.Length;i++){
			if(playerStateManagers[i].GetComponent<NetworkObject>().IsOwnedByServer){
				if(isHostTurn)
					playerStateManagers[i].Enable();
				else
					playerStateManagers[i].Disable();
			} else {
				if(isHostTurn)
					playerStateManagers[i].Disable();
				else
					playerStateManagers[i].Enable();
			}
		}
	}
}
