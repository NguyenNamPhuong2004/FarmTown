using System;
using UnityEngine;

[Serializable]
public class MissionData 
{
    public MissionType missionType;
    public string targetItem;         
    public int requiredAmount;        
    public int coinReward;            
    public string description;
    public Sprite itemSprite;
}