using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerStateManager : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer[] renderes;
    [SerializeField] private Collider2D colider;
    public void Enable(){
        EnableClientRpc();
    }
    [ClientRpc]
    private void EnableClientRpc(){
        colider.enabled=true;
        foreach (SpriteRenderer renderer in renderes) {
            Color color = renderer.color;
            color.a = 1; // alpha opaque
            renderer.color=color;
        }
    }
    public void Disable(){
        DisableClientRpc();
    }
    [ClientRpc]
    private void DisableClientRpc(){
        colider.enabled=false;
        foreach (SpriteRenderer renderer in renderes) {
            Color color = renderer.color;
            color.a = 0.2f; // alpha opaque
            renderer.color=color;
        }
    }
}
