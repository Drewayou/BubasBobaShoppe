using TMPro;
using UnityEngine;

public class MoveUpMoneyTxt : MonoBehaviour
{
    // Get the boba shop game manager script to pull data from.
    BobaShopRoundManagerScript thisRoundOverallInstanceScript;
    // input the float of how high the money should rise.
    public float moneyRise = 10f;
    // input the float of how long the text should stay.
    public float timeToShow = 2f;
    // Stagnant float to save the starting time for computation (Will match "timeToShow" at start).
    public float startingTimef;
    // input the float of how big text should be (coresponds to ammount earned [Default = 0.1f]).
    float flexMoneySize = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    void Start()
    {
        //Find and load the BobaShopRound data.
        thisRoundOverallInstanceScript = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>();

        //Save the starting time.
        startingTimef = timeToShow;

        //Scale the object based on ammount (Through initialization).
        this.gameObject.transform.localScale = new Vector3(flexMoneySize, flexMoneySize, flexMoneySize);

    }

    // Update is called once per frame
    void Update()
    {
        //Rise up txt based on input, then destroy when finished.
        if (timeToShow >= 0)
        {
            timeToShow -= Time.deltaTime;
            float vectorYOfMoney = 5f + (moneyRise * timeToShow / startingTimef);
            this.gameObject.transform.localPosition = new Vector3(0, vectorYOfMoney, 0);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
