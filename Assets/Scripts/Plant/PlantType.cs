using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantType : MonoBehaviour
{
    /* public ScrollRect scrollRect;
     public RectTransform contentPanel;
     public RectTransform plantSelected;

     public HorizontalLayoutGroup HLG;

     bool isPlanted;
     public float plantForce;
     float plantSpeed;

     private void Start()
     {
         isPlanted = false;
     }
     private void Update()
     {
         int curentItem = Mathf.RoundToInt(0 - (contentPanel.localPosition.x/(plantSelected.rect.width + HLG.spacing)));

         if (scrollRect.velocity.magnitude < 50 && !isPlanted)
         {
             scrollRect.velocity = Vector2.zero;
             plantSpeed = plantForce * Time.deltaTime;
             contentPanel.localPosition = new Vector3(Mathf.MoveTowards(contentPanel.localPosition.x, 0 - (curentItem * (plantSelected.rect.width + HLG.spacing)), plantSpeed),
                 contentPanel.localPosition.y,
                 contentPanel.localPosition.z);
             if(contentPanel.localPosition.x == 0 - (curentItem * (plantSelected.rect.width + HLG.spacing)))
             isPlanted = true;
         }
         if (scrollRect.velocity.magnitude > 50)
         { 
             isPlanted = false;
             plantSpeed = 0;
         }
     }*/
    public ScrollRect scrollRect;
    public RectTransform viewPortTransform;
    public RectTransform contentPanel;
    public RectTransform plantSelected;
    public HorizontalLayoutGroup HLG;

    public RectTransform[] ItemList;

    private bool isPlanted;
    private bool isUpdatingPosition;
    public float plantForce;
    private float plantSpeed;
    private float itemWidth;
    private Vector2 previousVelocity;

    private void Start()
    {
        isPlanted = false;
        isUpdatingPosition = false;
        previousVelocity = Vector2.zero;

        
        itemWidth = plantSelected.rect.width + HLG.spacing;

        
        int itemToAdd = Mathf.CeilToInt(viewPortTransform.rect.width - 100 / itemWidth);
        for (int i = 0; i < itemToAdd; i++)
        {
            RectTransform RT = Instantiate(ItemList[i % ItemList.Length], contentPanel);
            RT.SetAsLastSibling();
        }
        for (int i = 0; i < itemToAdd; i++)
        {
            int num = ItemList.Length - i - 1;
            while (num < 0) num += ItemList.Length;
            RectTransform RT = Instantiate(ItemList[num], contentPanel);
            RT.SetAsFirstSibling();
        }


        contentPanel.localPosition = new Vector3(
            -(itemWidth * itemToAdd),
            contentPanel.localPosition.y,
            contentPanel.localPosition.z
        );
    }

    private void Update()
    {
        int currentItem = Mathf.RoundToInt(-contentPanel.localPosition.x / itemWidth);

        if (!isUpdatingPosition && scrollRect.velocity.magnitude < 50 && !isPlanted)
        {
            
            scrollRect.velocity = Vector2.zero;
            plantSpeed = plantForce * Time.deltaTime;

            float targetPositionX = -currentItem * itemWidth;
            contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(contentPanel.localPosition.x, targetPositionX, plantSpeed),
                contentPanel.localPosition.y,
                contentPanel.localPosition.z
            );


            if (Mathf.Approximately(contentPanel.localPosition.x, targetPositionX))
                isPlanted = true;
        }

        
        if (scrollRect.velocity.magnitude > 50)
        {
            isPlanted = false;
            plantSpeed = 0;
        }

       
        if (contentPanel.localPosition.x > - 100)
        {
            Canvas.ForceUpdateCanvases();
            previousVelocity = scrollRect.velocity;
            contentPanel.localPosition -= new Vector3(ItemList.Length * itemWidth, 0, 0);
            isUpdatingPosition = true;
        }
        else if (contentPanel.localPosition.x < - ItemList.Length * itemWidth - 100)
        {
            Canvas.ForceUpdateCanvases();
            previousVelocity = scrollRect.velocity;
            contentPanel.localPosition += new Vector3(ItemList.Length * itemWidth, 0, 0);
            isUpdatingPosition = true;
        }

        if (isUpdatingPosition)
        {
            isUpdatingPosition = false;
            scrollRect.velocity = previousVelocity;
        }
    }
}
