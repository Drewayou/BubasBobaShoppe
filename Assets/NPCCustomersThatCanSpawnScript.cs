using System.Collections.Generic;
using UnityEngine;

public class NPCCustomersThatCanSpawnScript : MonoBehaviour
{
    // This script essentially saves, loads, and renders the possible customers that can spawn in the boba shop round.

    // Round manager script to determin player data and / or seasonal round data.
    // Usefull to determine the player stats for what drinks they unlocked.
    BobaShopRoundManagerScript currentRoundManagerInstance;

    // This GameObject list is for the all the possible customers that can spawn, after the start() method scans.
    [SerializeField]
    [Tooltip("Generated of customers for this round.")]
    public List<GameObject> thisRoundOfPossibleCustomers = new List<GameObject>();

    // This GameObject list is for the general possible customers that can spawn.
    [SerializeField]
    [Tooltip("Manual list of customers for this round.")]
    private List<GameObject> normalBobaShopCustomerNPCs = new List<GameObject>();

    // This GameObject list is for the seasonal possible customers that can spawn.
    [SerializeField]
    [Tooltip("Manual list of SEASONAL customers for this round.")]
    private List<GameObject> seasonalBobaShopCustomerNPCs = new List<GameObject>();

    // This GameObject list is for the "special" possible customers that can spawn.
    // "Special" customers have each a unique script that checks player data if they can be spawned for the round.
    [SerializeField]
    [Tooltip("Generated list of special customers for this round.")]
    private List<GameObject> specialBobaShopCustomerNPCs = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentRoundManagerInstance = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>();
        LoadThePossibleCustomersForThisRound();
    }

    //This method happens in Start() and pulls the possible customers into the chance to spawn list.
    public void LoadThePossibleCustomersForThisRound(){
        foreach(GameObject customer in normalBobaShopCustomerNPCs){
            thisRoundOfPossibleCustomers.Add(customer);
        }
        foreach(GameObject seasonalCustomer in seasonalBobaShopCustomerNPCs){
            /*FIXME: Add scripts in customers to check if the season/weather matches
            if(seasonalCustomer.weatherCheck){
                thisRoundOfPossibleCustomers.Add(seasonalCustomer);
            }
            */
        }
        foreach(GameObject specialCustomer in specialBobaShopCustomerNPCs){
            /*FIXME: Add scripts in customers to check if the special condition matches
            if(specialCustomer.conditionToSpawnCheck){
                thisRoundOfPossibleCustomers.Add(specialCustomer);
            }
            */
        }
    }

    //This method loads a random customer encounter
    public GameObject LoadRandomCustomerFromList(){
        //Check if list has characters loaded.
        if(thisRoundOfPossibleCustomers.Count == 0){
            print("ERROR: Customer list may be empty.");
            return null;
        }else{
            GameObject customerThatCanSpawn = thisRoundOfPossibleCustomers[Random.Range(0, thisRoundOfPossibleCustomers.Count)];
            return customerThatCanSpawn;
        } 
    }

    //This method loads a specific customer encounter via their number.
    //Note: customers are indexed from normalBobaShopCustomerNPCs>Seasonal>specialBobaShopCustomerNPCs
    public GameObject LoadIndexCustomerFromList(int customerIndex){
        //Check if list has characters loaded.
        if(thisRoundOfPossibleCustomers.Count == 0){
            print("ERROR: Customer list may be empty.");
            return null;
        }else{
            GameObject customerThatCanSpawn = thisRoundOfPossibleCustomers[customerIndex];
            return customerThatCanSpawn;
        } 
    }

    //This method loads a specific customer encounter via their name.
    //Note: customers names are case sensitive.
    public GameObject LoadCustomerFromListViaName(string customerName){
        foreach(GameObject CustomerWanted in thisRoundOfPossibleCustomers){
            if(CustomerWanted.name == customerName){
                return CustomerWanted;
                
            }
            print("ERROR: No customer with that name can be spawned. Check name spelling, customer list, and conditions for spawn.");
        }
        return null;
    }
}
