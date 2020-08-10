using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(PlayerUI))]
public class Player : Timer, IPlayerMechanics
{
    public int playerID;

    public List<Vegetable> carryingVegetables = new List<Vegetable>();
    public List<Vegetable> vegetableOnPlate = new List<Vegetable>();

    public int score;//Current score of the player 

    //Player UI
    public PlayerUI playerUI;

    //Player Movement
    private Movement playerMovement;

    private string vegetablesInBag = "";
    private string choppedVegetables = "";
    private string vegetablesOnPlate = "";

    //Order
    private int orderSum;//Sum of weights of currently chopped vegetables.

    //Booleans
    private bool isChopping = false;

    private void Start()
    {
        playerMovement = GetComponent<Movement>();
        OnPlayerTimeOver += OnTimerOver;
        InitPlayer();
    }

    public void InitPlayer()
    {
        ResetData();
        playerMovement.CanMove = true;
        StartTimer(GameConfig.GAME_TIME);
    }

    void OnTimerOver()
    {
        playerMovement.StopPlayer();
        GameManager.OnPlayerTimeOver?.Invoke(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Player1 chopping board
        if ((other.gameObject.CompareTag(GameConfig.CHOPPING_BOARD_1) && playerID == 1) ||
            (other.gameObject.CompareTag(GameConfig.CHOPPING_BOARD_2) && playerID == 2))
        {
            ChopVegetable();
        }

        //Player1 plate
        if ((other.gameObject.CompareTag(GameConfig.PLATE_1) && playerID == 1) ||
            (other.gameObject.CompareTag(GameConfig.PLATE_2) && playerID == 2))
        {
            ExecutePlateProcess();
        }

        if (carryingVegetables.Count == GameConfig.MAX_CARRYING_CAPACITY)//Limit player to carry only two items at once.
            return;

        //If collision happens with any of the vegetable shop. Pick up the vegetable.
        if (other.gameObject.CompareTag(GameConfig.A) || other.gameObject.CompareTag(GameConfig.B)
            || other.gameObject.CompareTag(GameConfig.C) || other.gameObject.CompareTag(GameConfig.D)
            || other.gameObject.CompareTag(GameConfig.E) || other.gameObject.CompareTag(GameConfig.F))
        {
            int veg = int.Parse(other.gameObject.tag);
            AddVegetableToBag(veg);
        }
    }

    void UpdateVegetablesInBag()
    {
        carryingVegetables.RemoveAt(0);//Remove vegetable from bag.

        if (vegetablesInBag.Length == 1)
        {
            vegetablesInBag = "";
        }
        else
        {
            string temp = vegetablesInBag;
            temp = temp.Remove(0, 1);
            vegetablesInBag = temp;
        }

        playerUI.UpdateBag(vegetablesInBag);
    }

    IEnumerator Chopping(Vegetable v)
    {
        playerMovement.CanMove = false;
        playerUI.ShowStatusBar(v.chopTime, v.name);

        yield return new WaitForSeconds(v.chopTime);//Wait for vegetable to be chopped.

        RemoveChoppedVegetableFromBag();
        GameManager.Instance.ProcessOrder(orderSum, this);
        playerMovement.CanMove = true;
        yield return null;
    }

    public void UpdateScore(int score)
    {
        this.score += score;
        playerUI.UpdateScore(this.score);
    }

    public void OnOrderProcessed()
    {
        orderSum = 0;
        choppedVegetables = "";
        playerUI.UpdateChoppedVegetables(choppedVegetables);
    }

    #region InterfaceMethods
    /// <summary>
    /// Adds a vegetable to bag after pick up or from plate.
    /// </summary>
    /// <param name="tag"></param>
    public void AddVegetableToBag(int veg)
    {
        Vegetable vegetable = new Vegetable(GameManager.vegetableWeightMapping[veg], veg,
                                            GameManager.vegetableChopTimes[veg], 25);
        carryingVegetables.Add(vegetable);

        vegetablesInBag += vegetable.name;
        playerUI.UpdateBag(vegetablesInBag);
    }

    /// <summary>
    /// Removes a vegetable from bag after its chopped.
    /// </summary>
    public void RemoveChoppedVegetableFromBag()
    {
        choppedVegetables += carryingVegetables[0].name;

        orderSum += carryingVegetables[0].weight;//Update current combination sum.
        UpdateVegetablesInBag();
        playerUI.UpdateChoppedVegetables(choppedVegetables);
        playerUI.HideStatusBar();

        isChopping = false;//Set ischopping to false.
    }

    public void ChopVegetable()
    {
        if (carryingVegetables.Count > 0 && !isChopping)
        {
            isChopping = true;

            Vegetable vegetableToChop = carryingVegetables[0];

            StartCoroutine(Chopping(vegetableToChop));
        }
    }

    public void ExecutePlateProcess()
    {
        if (vegetableOnPlate.Count > 0)
        {
            RemoveVegetableFromPlate();
        }
        else
        {
            PutVegetableOnPlate();
        }
    }

    public void PutVegetableOnPlate()
    {
        if(carryingVegetables.Count > 0)
        {
            Vegetable vegetable = new Vegetable(carryingVegetables[0]);

            vegetableOnPlate.Add(vegetable);
            vegetablesOnPlate += vegetable.name;
            playerUI.UpdateVegetableOnPlate(vegetablesOnPlate);

            UpdateVegetablesInBag();
        }
    }

    public void RemoveVegetableFromPlate()
    {
        if(vegetableOnPlate.Count > 0)
        {
            AddVegetableToBag(vegetableOnPlate[0].weight);
        }

        vegetableOnPlate.Clear();
        vegetablesOnPlate = "";
        playerUI.UpdateVegetableOnPlate(vegetablesOnPlate);
    }
    #endregion

    public void ResetData()
    {
        carryingVegetables.Clear();
        score = 0;
        vegetablesInBag = "";
        choppedVegetables = "";
        vegetablesOnPlate = "";
        isChopping = false;
        orderSum = 0;

        playerUI.Reset();
    }

    void OnDisable()
    {
        OnPlayerTimeOver -= OnTimerOver;
    }
}