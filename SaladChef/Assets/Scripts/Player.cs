﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerUI))]
public class Player : MonoBehaviour, IPlayerMechanics
{
    public int playerID;
    public List<Vegetable> carryingVegetables = new List<Vegetable>();
    public List<Vegetable> vegetableOnPlate = new List<Vegetable>();
    public int score;//Current score of the player 
    public int time;//Time left for the user

    //Player UI
    public PlayerUI playerUI;

    //Player Movement
    private PlayerMovement playerMovement;

    private string vegetablesInBag = "";
    private string choppedVegetables = "";
    private string vegetablesOnPlate = "";

    //Order
    private int orderSum;//Sum of weights of currently chopped vegetables.

    //Booleans
    private bool isChopping = false;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Player1 chopping board
        if (other.gameObject.CompareTag(GameConfig.CHOPPING_BOARD_1) && playerID == 1)
        {
            ChopVegetable();
        }

        //Player2 chopping board
        if (other.gameObject.CompareTag(GameConfig.PLATE_1) && playerID == 1)
        {
            ExecutePlateProcess();
        }

        //Player1 Plate
        if (other.gameObject.CompareTag(GameConfig.PLATE_2) && playerID == 2)
        {

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

    public void ChopVegetable()
    {
        if (carryingVegetables.Count > 0 && !isChopping)
        {
            isChopping = true;

            Vegetable vegetableToChop = carryingVegetables[0];

            StartCoroutine(Chopping(vegetableToChop));
        }
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
        playerUI.UpdateScore(score);
    }

    public void OnOrderProcessed()
    {
        orderSum = 0;
        choppedVegetables = "";
        playerUI.UpdateChoppedVegetables(choppedVegetables);
    }

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

    public void ResetData()
    {
        carryingVegetables.Clear();
        score = 0;
        time = 0;
        vegetablesInBag = "";
        choppedVegetables = "";
        vegetablesOnPlate = "";
        isChopping = false;
        orderSum = 0;
    }
}