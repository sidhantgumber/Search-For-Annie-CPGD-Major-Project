using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class PlayerConversation : MonoBehaviour
{
        [SerializeField] Dialogue testDialogue;
        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        bool isChoosing = false;
    [SerializeField] string playerName;
      
         
        IEnumerator Start()
        {
        yield return new WaitForSeconds(2);
        StartDialogue(testDialogue);
        }

        public void StartDialogue(Dialogue dialogue)
        {
        currentDialogue = dialogue;
        currentNode = currentDialogue.GetRootNode();
        OnConversationUpdated();
        }

    public void Quit()
    {
        currentDialogue = null;
        currentNode = null;
        isChoosing = false;
        OnConversationUpdated();
    }
    public event Action OnConversationUpdated;
    public bool IsActive()
    {
        return currentDialogue != null;
    }
        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if (currentNode == null)
            {
                return "";
            }

            return currentNode.GetText();
        }

    public string GetCurrentSpeakerName()
    {
       if(isChoosing)
        {
            return playerName;
        }
       else
        {
            return "Billy";
        }
    }

    public void SelectChoice(DialogueNode chosenNode)
    {
        currentNode = chosenNode;
        isChoosing = false;
        Next();
    }
    public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);
        }
    
        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                OnConversationUpdated();
                return;
            }

            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            currentNode = children[randomIndex];
        OnConversationUpdated();
        }

        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }
}
