using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This script calculates how much money the player gets according to the drink uuid matching.
public class MoneyJarScript : MonoBehaviour
{
    // Get the boba shop game manager script to pull data from.
    BobaShopRoundManagerScript thisRoundOverallInstanceScript;

    [SerializeField]
    [Header("Money Earned Txt")]
    [Tooltip("Put the game's \"MoneyEarnedTxt\" game object to spawn instances of how much money the player earned.")]
    // Get the boba shop game manager script to pull data from.
    GameObject moneyEarnedTxt;

    [SerializeField]
    [Header("shopBuySfx")]
    [Tooltip("Put the \"ShopBuySfx\" game object to spawn instances coin drop SFX!")]
    // Get the boba shop game manager script to pull data from.
    GameObject shopBuySfx;

    [SerializeField]
    [Header("Money Jar Img")]
    [Tooltip("Put the game's \"Image\" by draging the money jar game object.")]
    // Pull the money jar img to render money.
    Image moneyJarImg;

    [SerializeField]
    [Header("Jar Img")]
    [Tooltip("Pull the sprite image of the jar")]
    // Pull the money jar img to render money.
    Sprite JarImg;

    [SerializeField]
    [Header("Jar Img1")]
    [Tooltip("Pull the sprite image of the jar")]
    // Pull the money jar img to render money.
    Sprite JarImg1;

    [SerializeField]
    [Header("Jar Img2")]
    [Tooltip("Pull the sprite image of the jar")]
    // Pull the money jar img to render money.
    Sprite JarImg2;

    [SerializeField]
    [Header("Jar Img3")]
    [Tooltip("Pull the sprite image of the jar")]
    // Pull the money jar img to render money.
    Sprite JarImg3;

    [SerializeField]
    [Header("Jar Img4")]
    [Tooltip("Pull the sprite image of the jar")]
    // Pull the money jar img to render money.
    Sprite JarImg4;

    [SerializeField]
    [Header("Jar Img5")]
    [Tooltip("Pull the sprite image of the jar")]
    // Pull the money jar img to render money.
    Sprite JarImg5;

    [SerializeField]
    [Header("Jar Img6")]
    [Tooltip("Pull the sprite image of the jar")]
    // Pull the money jar img to render money.
    Sprite JarImg6;

    [SerializeField]
    [Header("Jar Img7")]
    [Tooltip("Pull the sprite image of the jar")]
    // Pull the money jar img to render money.
    Sprite JarImg7;

    // Money earned this round will be saved HERE
    public double moneyEarnedThisRound = 0f;

    // This float will show how much money the player is earning as compared to the last round if above 100!
    public double moneyEarnedLastRound = 0f;

    void Start()
    {
        //Find and load the BobaShopRound data.
        thisRoundOverallInstanceScript = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>();

        //FIXME need to pull moneyEarnedLastRound data from game instance.
        //Set money jar thresholds.
    }

    public void AddMoneyThisRound(double moneyEarned)
    {
        bool generatedAVisualPriceForThisDrink = false;

        moneyEarnedThisRound += moneyEarned;
        thisRoundOverallInstanceScript.playerEarnedCoins = moneyEarnedThisRound;

        if (!generatedAVisualPriceForThisDrink)
        {
            GameObject moneyTxt = Instantiate(moneyEarnedTxt, this.gameObject.transform);
            moneyTxt.GetComponent<TMP_Text>().SetText("+$" + moneyEarned.ToString("F2"));

            switch (this.gameObject.transform.childCount)
            {
                case 1:
                    moneyTxt.GetComponent<MoveUpMoneyTxt>().moneyRise = 20f;
                    break;

                case 2:
                    moneyTxt.GetComponent<MoveUpMoneyTxt>().moneyRise = 30f;
                    break;

                case 3:
                    moneyTxt.GetComponent<MoveUpMoneyTxt>().moneyRise = 40f;
                    break;

                default:
                    // Child count is 0 or 4+
                    break;
            }
            generatedAVisualPriceForThisDrink = true;
        }
        RenderJarMoney();
        Instantiate(shopBuySfx);
    }

    public void SubtractMoneyThisRound(double moneyLossed)
    {
        moneyEarnedThisRound -= moneyLossed;
        thisRoundOverallInstanceScript.playerEarnedCoins = moneyEarnedThisRound;
        RenderJarMoney();
    }

    public double ReturnMoneyThisRound()
    {
        return moneyEarnedThisRound;
    }

    public void RenderJarMoney()
    {

        if (moneyEarnedLastRound > 100)
        {
            double result1 = (moneyEarnedLastRound / (moneyEarnedLastRound * 7));
            double result2 = (moneyEarnedLastRound / (moneyEarnedLastRound * 7));
            double result3 = (moneyEarnedLastRound / (moneyEarnedLastRound * 7));
            double result4 = (moneyEarnedLastRound / (moneyEarnedLastRound * 7));
            double result5 = (moneyEarnedLastRound / (moneyEarnedLastRound * 7));
            double result6 = (moneyEarnedLastRound / (moneyEarnedLastRound * 7));
            double result7 = (moneyEarnedLastRound / (moneyEarnedLastRound * 7));

            if (moneyEarnedThisRound > 0 && moneyEarnedThisRound <= 10)
            {
                moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg;
            }
            if (moneyEarnedThisRound > result1)
            {
                moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg1;
            }
            if (moneyEarnedThisRound > result2)
            {
                moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg2;
            }
            if (moneyEarnedThisRound > result3)
            {
                moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg3;
            }
            if (moneyEarnedThisRound > result4)
            {
                moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg4;
            }
            if (moneyEarnedThisRound > result5)
            {
               moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg5;
            }
            if (moneyEarnedThisRound > result6)
            {
               moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg6;
            }
            if (moneyEarnedThisRound > result7)
            {
                moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg7;
            }
        }
        else
        {
            if (moneyEarnedThisRound > 0)
            {
                moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg1;
            }
            if (moneyEarnedThisRound > 2)
            {
                moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg2;
            }
            if (moneyEarnedThisRound > 14)
            {
                moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg3;
            }
            if (moneyEarnedThisRound > 21)
            {
                moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg4;
            }
            if (moneyEarnedThisRound > 42)
            {
                moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg5;
            }
            if (moneyEarnedThisRound > 78)
            {
                moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg6;
            }
            if (moneyEarnedThisRound >= 100)
            {
                moneyJarImg.gameObject.GetComponent<Image>().sprite = JarImg7;
            }
        }
    }
}
