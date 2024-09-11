using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToppingTypeGuiScript : MonoBehaviour
{
    //Get the object parent for the values of this topping jar.
    GameObject parentToppingJarObject;

    //Get the scripts of the topping jar from the parent object.
    ToppingsJarScript parentToppingJarScript;

    //Get the object inside this topping that shows the level of toppings left.
    GameObject fillerObject;

    //Get the object inside this topping that shows the level of toppings left.
    Image fillerToppingVisual;

    //The Gameobject UI for how many items are in the jar.
    GameObject inJarAmmountofToppingsObject;

    //The TMP UI for how many items are in the jar in text.
    TMP_Text inJarAmmountofToppingsTMP;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the objects and components.
        parentToppingJarObject = this.gameObject.transform.parent.gameObject;
        parentToppingJarScript = parentToppingJarObject.GetComponent<ToppingsJarScript>();
        fillerObject = this.gameObject.transform.GetChild(0).gameObject;
        fillerToppingVisual = fillerObject.GetComponent<Image>();
        inJarAmmountofToppingsObject = this.gameObject.transform.GetChild(1).gameObject;
        inJarAmmountofToppingsTMP = inJarAmmountofToppingsObject.GetComponent<TMP_Text>();
    }

    // LateUpdate is called after all other Updates
    void LateUpdate()
    {
        fillerToppingVisual.fillAmount = parentToppingJarScript.toppingsIngredientsInJarCurrentAmount / parentToppingJarScript.bobaToppingMaximumInJar;
    }

    //Hover scripts to show the value of the jars in text format as well.
    public void OnMouseEnter(){
        inJarAmmountofToppingsObject.SetActive(true);

    }

    public void OnMouseOver(){
        inJarAmmountofToppingsTMP.text = parentToppingJarScript.toppingsIngredientsInJarCurrentAmount.ToString() + "/" + parentToppingJarScript.bobaToppingMaximumInJar.ToString();
    }

    public void OnMouseExit(){
        inJarAmmountofToppingsObject.SetActive(false);
    }
}
