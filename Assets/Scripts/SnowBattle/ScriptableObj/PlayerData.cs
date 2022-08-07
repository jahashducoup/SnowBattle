using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public Sprite                   mySprite;
    public float                    stunAfterHitCd         = 1f;
    public float                    speed                  = 2.5f;
    public float                    speedWhenCrouching     = 1f;
    public float                    shotCd                 = .8f;
    public float                    outOfAmmoBoxCd         = 3f;
    public float                    reloadingDuration      = 1f;
    public int                      hp                     = 10;
    public int                      ammoMax                = 6;
    public KeyCode                  shootKey               = KeyCode.Q;
    public KeyCode                  reloadKey              = KeyCode.E;
    public KeyCode                  crouchingKey           = KeyCode.C;
    public int                      playerNumber;
    public Sprite                   myHead;
    public RuntimeAnimatorController  animator;
}
