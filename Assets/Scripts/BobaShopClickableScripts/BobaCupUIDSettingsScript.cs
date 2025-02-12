using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobaCupUIDSettingsScript : MonoBehaviour
{
    // String for specific UID of this drink.
    public string drinkUID;

    //The bool if the drink has a fruit/veggie ingredient hunted from the hunt round inserted via the top of the mixer machine.
    public bool hasIngredientBlended;

    // Check if the drink has any tea overlays.
    public bool hasTeaOverlay;

    // Save what type of drink overlay this drink has.
    // Tea # break down | 0 = Green Tea | 1 = Oolong Tea | 2 = Black Tea |
    public int teaOverlaySelectionNumber;

    // Check if the drink has any additional overlays.
    public bool hasAdditionalFlavorOverlay;

    // Save what type of drink overlay this drink has.
    // AdditionalFlavorOverlay # break down | 0 = Milk | 1 = Water |
    public int additionalOverlaySelectionNumber;

    // Check if the drink has any additional topping overlays.
    public bool hasToppingsOverlay;

    // Save what type of drink overlay this drink has.
    // AdditionalFlavorOverlay # break down | 0 = Boba |
    public int toppingInTheDrink;

    //
    //FOR FUTURE DRINK ADJUSTMENTS
    //

    // Check if the drink is cold or hot.
    public bool drinkIsCold;

    // Save what type of cold drink this drink is (Slushe or iced).
    public int coldType;

    // Check if the drink has different sweetness levels.
    public bool drinkHasCustomSweetness;

    // Save how much sweeter the drink is (By %. 100 is normal sweetness).
    public int sweetnessLevel;

}
