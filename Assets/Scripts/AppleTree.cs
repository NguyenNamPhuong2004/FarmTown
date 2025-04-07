using UnityEngine;

public class AppleTree : MonoBehaviour 
{
    private TreeData treeData;
    private SpriteRenderer spriteRenderer;
    public Sprite spriteNoFruit; 
    public Sprite spriteWithFruit; 

    public string TreeName => treeData.treeName;
    public bool HasFruit => treeData.hasFruit;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!treeData.hasFruit)
        {
            treeData.remainingTime -= Time.deltaTime;
            if (treeData.remainingTime <= 0)
            {
                treeData.hasFruit = true;
                spriteRenderer.sprite = spriteWithFruit;
            }
        }
    }

    public void Initialize(TreeData data)
    {
        treeData = data;
        spriteRenderer.sprite = spriteNoFruit; 
        transform.position = data.position;
    }

    public void ResetToGrowing()
    {
        treeData.hasFruit = false;
        treeData.remainingTime = 1200; 
        spriteRenderer.sprite = spriteNoFruit;
    }

    public string GetRemainingTimeFormatted()
    {
        if (treeData.hasFruit) return "?ã có qu?!";
        int totalSeconds = Mathf.CeilToInt(treeData.remainingTime);
        int days = totalSeconds / (24 * 3600);
        int hours = (totalSeconds % (24 * 3600)) / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;
        return string.Format("{0:00}:{1:00}:{2:00}:{3:00}", days, hours, minutes, seconds);
    }

   
}