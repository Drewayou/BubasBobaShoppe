using UnityEngine;

public interface Inter_BobaShopNPC

{
   //NOTE: All NPC's SHOULD have their own JSON files for their dialogue, with some JSON exceptions for universal small talk remarks! 
   //Each method equates to a string[i] JSON object, where i
   //Co-responds to the index [i] needed for the scenario which i selected is either procedurally generated or hard-coded.

   //THIS INTERFACE TAGGED TO A NPC CLASS MEANS THEY CAN APPEAR IN THE BOBA SHOP AS A CUSTOMER!

   //Methods for BobaShop NPC ordering dialogue.

   //Dialogue when the customer is orders a drink (May reference a standard JSON for normal orders or a special character ordering JSON).
   string customerOrdersAtShopBobaShop(int orderNormalIndexBShop);
   //Dialogue when the customer is greatful.
   string customerThanksBobaShop(int thanksDialogueIndexBShop);
   //Dialogue when the customer is upset.
   string customerNegatesBobaShop(int negationDialogueIndexBShop);
   //Other dialogue not related to the above conditions.
   string customerOrdersSpecialBobaShop(int orderSpecialIndexBShop);
}