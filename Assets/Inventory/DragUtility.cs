using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragUtility : Singleton<DragUtility>
{
    /// <summary>
    /// Minimum distance allowed that an item card will lock onto a grid. Else, the card will return to its original position.
    /// BUG: Not Working
    /// </summary>
    public float MinimumDragDistance = 1000000f;

    private GridContainer LastContainer = null;
    private int LastIndex = -1;

    [HideInInspector]
    public List<GridContainer> CurrentlyActiveContainers = new List<GridContainer>();

    public void StartDrag(GameObject card)
    {

        LastContainer = card.transform.GetComponentInParent<GridContainer>();
        //LastContainer = card.transform.parent.parent.parent.GetComponent<GridContainer>();
        LastIndex = LastContainer.GetCardIndex(card);
        card.transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
    }

    public void Dragging(GameObject card)
    {
        card.transform.position = Input.mousePosition;
        
    }

    public void EndDrag(GameObject card)
    {
        //Current lowest distance between grid and currently held card
        float lowest_distance = MinimumDragDistance;

        GridContainer container_final = null;

        int new_index = -1;
        //Check all active containers to detect where item should be placed
        foreach (GridContainer container in CurrentlyActiveContainers)
        {
            for (int i = 0; i < container.CardMax; ++i)
            {
                //print("iterating " + i);
                GameObject grid_space = container.GridDisplay.transform.GetChild(i).gameObject;
                float curr_lowest = Vector2.Distance(grid_space.transform.position, card.transform.position);
                if (curr_lowest < lowest_distance)
                {
                    lowest_distance = curr_lowest;
                    container_final = container;
                    new_index = i;
                }
            }
        }

        if (new_index != -1) //Switch
        {
            LastContainer.SwitchCards(LastIndex, card, container_final, new_index);
        }
        else // Do not switch
        {
            LastContainer.AddCard(card, LastIndex);
        }
    }
}
