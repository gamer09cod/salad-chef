using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// Class to create orders and manage processed orders.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region PublicMembers
    public static Dictionary<int, string> vegetableWeightMapping = new Dictionary<int, string>()
    {
        {1,  "A" },
        {2,  "B" },
        {4,  "C" },
        {8,  "D" },
        {16, "E" },
        {32, "F" },
    };

    public static Dictionary<int, float> vegetableChopTimes = new Dictionary<int, float>()
    {
        {1,  2f},
        {2,  2.5f },
        {4,  6f },
        {8,  5.5f },
        {16, 4.5f },
        {32, 3.5f },
    };

    public List<CustomerOrder> custOrders = new List<CustomerOrder>();
    public List<CustomerOrder> completedOrders = new List<CustomerOrder>();
    public List<Player> players = new List<Player>();

    public static Action<Player> OnPlayerTimeOver;
    public static GameManager Instance;
    #endregion

    #region PrivateMembers
    private List<int> vegetableIds = new List<int>()
    {
        1,2,4,8,16,32
    };
    #endregion

    #region ResultScreen
    public GameObject resultScreen;
    public TextMeshProUGUI playerNameText;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        OnPlayerTimeOver += PlayerTimerOver;
        InitGame();
    }

    void InitGame()
    {
        UIManager.Instance.ClearOrders();
        CreateOrders();
    }

    /// <summary>
    /// Function to create orders for GameConfig.MAX_ORDERS
    /// </summary>
    void CreateOrders()
    {
        custOrders.Clear();
        completedOrders.Clear();

        for(int i = 0; i < GameConfig.MAX_ORDERS; i++)
        {
            CustomerOrder order = new CustomerOrder();

            for(int J = 0; J < GameConfig.MAX_VEGGIES_PER_ORDER; J++)
            {
                //Get random vegetable
                int key = vegetableIds[UnityEngine.Random.Range(0, 6)];

                order.requiredSum += key;
                order.sequence += vegetableWeightMapping[key];
            }

            //Compute random completion time
            order.time = UnityEngine.Random.Range(120,150);

            //Score
            order.score = 100;

            //Bonus
            order.bonus = CustomerBonus.Score;

            custOrders.Add(order);
        }

        UIManager.Instance.InitializeOrders(custOrders, OnOrderProcessed);
    }

    public void ProcessOrder(int sum, Player player)
    {
        CustomerOrder order = custOrders.Find(o => o.requiredSum == sum);//Check is order exists.

        if(order != null)
        {
            player.UpdateScore(order.score);//Give player some score points of that order.
            player.OnOrderProcessed();
            UIManager.Instance.UpdateOrder(order.requiredSum);//Update order UI.
            custOrders.Remove(order);//Remove order from available orders.
        }
    }

    void OnOrderProcessed(OrderUIItem ord)
    {
        CustomerOrder order = custOrders.Find(x=> x.sequence == ord.sequence.text);
        if (order != null) 
        {
            custOrders.Remove(order);
            Destroy(ord.gameObject);
        }
    }

    void PlayerTimerOver(Player player)
    {
        Debug.Log("---PlayerTimerOver---");
        playerNameText.text = player.name + " " + player.playerID ;//Winner name
        GameOver();
    }

    void GameOver()
    {
        resultScreen.SetActive(true);
    }

    public void RestartGame()
    {
        foreach(Player p in players)
        {
            p.InitPlayer();
        }

        resultScreen.SetActive(false);

        InitGame();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }

    private void OnDisable()
    {
        OnPlayerTimeOver -= PlayerTimerOver;
    }
}

public enum CustomerBonus
{
    Speed,
    Time,
    Score
}