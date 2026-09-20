using UnityEngine;

public class CPROHScript : MonoBehaviour
{
    // Get the boba shop game manager script to pull data from.
    BobaShopRoundManagerScript thisRoundOverallInstanceScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Find and load the BobaShopRound data.
        thisRoundOverallInstanceScript = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable()
    {
        if (this.gameObject.transform) {
            foreach (Transform customer in this.gameObject.transform)
            {
                customer.localPosition = new Vector3(-1600, -55, 0);
                Destroy(customer.gameObject);
            }
        }
    }
}
