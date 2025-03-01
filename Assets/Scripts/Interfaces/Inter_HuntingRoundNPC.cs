using UnityEngine;

public interface Inter_HuntingRoundNPC

{
   //NOTE: All NPC's SHOULD have their own JSON files for their dialogue! Each method equates to the string[] JSON
   //Co-responding to the index [i] needed for the scenario either procedurally generated or hard-coded.

   //THIS INTERFACE TAGGED TO A NPC CLASS MEANS THEY CAN APPEAR IN THE HUNTING ROUND LEVELS AS AN NPC!
   //This is also used for NPC's DURING THE TUTORIAL (EXCLUDING boba shop tutorial or extra dialogue in the boba shop though)!
   //This Interface can be used for enemies/fruit monsters as well!

   //Methods for NPC hunting round dialogue.

   //Method for small talk during the hunting rounds.
   string smallTalkHunting(int smallTalkDialogueIndexHunt);
   //Method for important, cinimatic, or story-based dialogue during the Hunting exploration rounds.
   string importantDialogueHunting(int importantDialogueIndexHunt);
   //Other dialogue not related to the above conditions.
   string otherDialogueHunting(int ectDialogueIndexHunt);
}