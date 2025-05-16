using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class GameStateManager : NetworkBehaviour
{
	public static GameStateManager instance;
	public enum State { Menu, Game, Win, Lose };
	[SerializeField] private State gameState;
	private int connectedPlayers;
	[Header(" Events ")]
	public static Action<State> onGameStateChanged;

	public void Awake(){
		if(instance==null)
			instance=this;
		else
			Destroy(gameObject);
	}
	public override void OnNetworkSpawn(){
		base.OnNetworkSpawn();
		NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
	}
	private void NetworkManager_OnServerStarted(){
		if(!IsServer)
			return;
		connectedPlayers++;
		NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
	}
	private void Singleton_OnClientConnectedCallback(ulong obj){
		connectedPlayers++;
		if(connectedPlayers>=2)
			StartGame();
	}
	private void StartGame(){
		StartGameClientRpc();
	}
	[ClientRpc]
	private void StartGameClientRpc(){
		gameState=State.Game;
		onGameStateChanged?.Invoke(gameState);
	}
	void Start(){
		gameState=State.Menu;
	}
	public override void OnDestroy(){
		base.OnDestroy();
		NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
		NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
	}
}
