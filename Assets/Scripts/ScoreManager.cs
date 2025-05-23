using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using TMPro;

public class ScoreManager : NetworkBehaviour
{
	[Header(" Elements ")]
	private int hostScore;
	private int clientScore;
	[SerializeField] private TextMeshProUGUI scoreText;

	public override void OnNetworkSpawn(){
		base.OnNetworkSpawn();
		NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
	}
	private void NetworkManager_OnServerStarted(){
		if(!IsServer)
			return;
		Egg.onFellInWater += EggFellinWaterCallback;
		GameStateManager.onGameStateChanged += GameStateChangedCallback;
		Debug.Log("Event Subscribed");
	}
	public override void OnDestroy(){
		base.OnDestroy();
		NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
		Egg.onFellInWater -= EggFellinWaterCallback;
		GameStateManager.onGameStateChanged -= GameStateChangedCallback;
	}
	private void GameStateChangedCallback(GameStateManager.State gameState){
		switch(gameState){
			case GameStateManager.State.Game:
				ResetScores();
				break;
		}
	}
	private void ResetScores(){
		hostScore=0;
		clientScore=0;
		UpdateScoreClientRpc(hostScore,clientScore);
		UpdateScoreText();
	}
	private void EggFellinWaterCallback(){
		if (PlayerSelector.instance.IsHostTurn())
			clientScore++;
		else
			hostScore++;
		UpdateScoreClientRpc(hostScore,clientScore);
		UpdateScoreText();
		CheckForEndGame();
	}
	private void CheckForEndGame(){
		if(hostScore>=3){
			// Host wins
		} else if (clientScore>=3){
			// Client wins
		}else{
			EggManager.instance.ReuseEgg();
		}
	}
	[ClientRpc]
	private void UpdateScoreClientRpc(int hostScore,int clientScore){
		this.hostScore=hostScore;
		this.clientScore=clientScore;
	}
	private void UpdateScoreText(){
		UpdateScoreTextClientRpc();
	}
	[ClientRpc]
	private void UpdateScoreTextClientRpc(){
		scoreText.text = hostScore + " - <color=#FF0000>"+clientScore+"</color>";
	}
	void Start(){
		UpdateScoreText();
	}
}
