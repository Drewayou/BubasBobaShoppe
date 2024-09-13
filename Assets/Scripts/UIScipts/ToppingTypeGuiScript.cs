using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToppingTypeGuiScript : MonoBehaviour
{
    //Get the scripts of the topping jar from the parent object.
    ToppingsJarScript parentToppingJarScript;

    //Get the object inside this topping that shows the level of toppings left.
    [SerializeField]
    [Tooltip("Get the object inside this topping that shows the level of toppings left.")]
    GameObject parentObject;

    //Get the object inside this topping that shows the level of toppings left.
    Image fillerToppingVisual;

    // Start is called before the first frame update
    void Start()
    {
        parentToppingJarScript = parentObject.GetComponent<ToppingsJarScript>();
        fillerToppingVisual = gameObject.GetComponent<Image>();
    }

    // LateUpdate is called after all other Updates
    void Update()
    {
        fillerToppingVisual.fillAmount = (float)parentToppingJarScript.toppingsIngredientsInJarCurrentAmount / parentToppingJarScript.bobaToppingMaximumInJar;
    }
}
