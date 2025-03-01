using UnityEngine;

public interface Inter_TownNPC

{
   //NOTE: All NPC's SHOULD have their own JSON files for their dialogue! Each method equates to the string[] JSON
   //Co-responding to the index [i] needed for the scenario either procedurally generated or hard-coded.

   //THIS INTERFACE TAGGED TO A NPC CLASS MEANS THEY CAN APPEAR IN THE TOWN LEVELS AS AN NPC!
   //This is also used for NPC's DURING ANY TOWN CINIMATICS, or when the player explores (EXCLUDING beginning tutorial, or NPC SHOPS)!

   //Methods for TOWN exploration dialogue.

   //Method for small talk during the Town exploration rounds.
   string smallTalkTown(int smallTalkDialogueIndexTown);
   //Method for important, cinimatic, or story-based dialogue during the Town exploration rounds.
   string importantDialogueTown(int importantDialogueIndexTown);
   //Other dialogue not related to the above conditions.
   string otherDialogueTown(int ectDialogueIndexTown);
}