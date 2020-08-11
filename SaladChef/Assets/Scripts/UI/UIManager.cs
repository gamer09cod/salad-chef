using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to manage dynamic UI items creation.
/// </summary>
public class UIManager : MonoBehaviour
{
    public Transform parent;
    public GameObject orderItemPrefab;

    private List<OrderUIItem> orderItems = new List<OrderUIItem>();

    public static UIManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

        }else if(Instance != this)
        {
            Destroy(this);
        }
    }
    public void InitializeOrders(List<CustomerOrder> orders, Action<OrderUIItem> onOrderProcessed)
    {
        for(int i = 0; i < orders.Count; i++)
        {
            //Create orders(UI)
            GameObject obj = Instantiate(orderItemPrefab, parent);

            OrderUIItem item = obj.GetComponent<OrderUIItem>();          
            item.sequence.text = orders[i].sequence;
            item.startTime = orders[i].time;
            item.onOrderProcessed = onOrderProcessed;
            item.scoreID = orders[i].requiredSum;

            orderItems.Add(item);
        }
    }

    public void UpdateOrder(int scoreID)
    {
        foreach(OrderUIItem item in orderItems)
        {
            if(item.scoreID == scoreID)
            {
                item.onOrderProcessed.Invoke(item);
            }
        }
    }

    public void ClearOrders()
    {
        foreach (OrderUIItem item in orderItems)
        {
            if(item != null)
                Destroy(item.gameObject);
        }

        orderItems.Clear();
    }
}
