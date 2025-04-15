using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AppleTree : MonoBehaviour
{
    public int Id { get; private set; }
    private TreeData treeData;
    private SpriteRenderer spriteRenderer;
    public Sprite spriteGrowing;
    public Sprite spriteWithFruit; 

    public string TreeName => treeData.treeName;
    public TreeState State => treeData.state;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        CheckGrowthState();
    }

    public void Initialize(TreeData data)
    {
        treeData = data;
        Id = data.id;
        transform.position = data.position;
        UpdateSprite();
    }

    private void CheckGrowthState()
    {
        if (treeData.state == TreeState.Growing && !string.IsNullOrEmpty(treeData.endTime))
        {
            DateTime endTime;
            if (DateTime.TryParse(treeData.endTime, out endTime))
            {
                TimeSpan remainingTime = endTime - DateTime.Now;
                if (remainingTime <= TimeSpan.Zero)
                {
                    treeData.state = TreeState.HasFruit;
                    treeData.endTime = "";
                    UpdateSprite();
                    DataPlayer.UpdateTreeData(Id, treeData.state, treeData.endTime, transform.position);
                    Debug.Log($"{treeData.treeName} đã có quả!");
                }
            }
        }
    }

    public void ResetToGrowing()
    {
        treeData.state = TreeState.Growing;
        DateTime endTime = DateTime.Now.AddSeconds(420); 
        treeData.endTime = endTime.ToString("yyyy-MM-ddTHH:mm:ss");
        UpdateSprite();
        DataPlayer.UpdateTreeData(Id, treeData.state, treeData.endTime, transform.position);
    }

    private void UpdateSprite()
    {
        spriteRenderer.sprite = treeData.state == TreeState.Growing ? spriteGrowing : spriteWithFruit;
    }

    public TimeSpan GetRemainingTime()
    {
        if (treeData.state != TreeState.Growing || string.IsNullOrEmpty(treeData.endTime))
        {
            return TimeSpan.Zero;
        }

        DateTime endTime;
        if (DateTime.TryParse(treeData.endTime, out endTime))
        {
            TimeSpan remaining = endTime - DateTime.Now;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }
        return TimeSpan.Zero;
    }

    public string GetRemainingTimeFormatted()
    {
        if (treeData.state == TreeState.HasFruit)
        {
            return "Đã có quả!";
        }
        TimeSpan remaining = GetRemainingTime();
        if (remaining == TimeSpan.Zero)
        {
            return "00:00:00";
        }
        int hours = (int)remaining.TotalHours;
        int minutes = remaining.Minutes;
        int seconds = remaining.Seconds;
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    public void UpdateId(int newId)
    {
        Id = newId;
        treeData.id = newId;
    }
}