using UnityEngine;

public interface Inter_ShopSellerNPC

{
   //NOTE: All NPC's SHOULD have their own JSON files for their dialogue, with some JSON exceptions for universal small talk remarks! 
   //Each method equates to a string[i] JSON object, where i
   //Co-responds to the index [i] needed for the scenario which i selected is either procedurally generated or hard-coded.

   //THIS INTERFACE TAGGED TO A NPC CLASS MEANS THEY OWN A SHOP IN TOWN AND THE PLAYER IS A CUSTOMER TO THE NPC!

   //Methods for NPC Shop Seller dialogue.

   //For when the player first enters the shop UI or buys items.
   string npcSellerQuip(int sellingQuipIndexSell);
   //For when the player leaves the shop.
   string npcSellerGoodbye(int goodbyeQuipIndexSell);
   //For NPC explination of items or story-driven dialogue.
   string npcSellerImportant(int storyDialogueIndexSell);
   //Other dialogue not related to the above conditions.
   string npcSellerEtc(int otherDialogueIndexSell);
}