using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DialogueUI : MonoBehaviour
{
    PlayerConversation playerConversation;
    [SerializeField] TextMeshProUGUI AIText;
    [SerializeField] Button nextButton;
    [SerializeField] Button quitButton;
    [SerializeField] GameObject AIResponse;
    [SerializeField] Transform choiceRoot;
    [SerializeField] GameObject choicePrefab;
    [SerializeField] TextMeshProUGUI speakerName;
    // Start is called before the first frame update
    void Start()
    {
        playerConversation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversation>();
        playerConversation.OnConversationUpdated += UpdateUI;
        nextButton.onClick.AddListener(() => playerConversation.Next());
        quitButton.onClick.AddListener(() =>playerConversation.Quit());
        UpdateUI();
    }



    void UpdateUI()
    {
        gameObject.SetActive(playerConversation.IsActive());
        if (!playerConversation.IsActive()) return;
        speakerName.text = playerConversation.GetCurrentSpeakerName();
        AIResponse.SetActive(!playerConversation.IsChoosing());
        choiceRoot.gameObject.SetActive(playerConversation.IsChoosing());
        if(playerConversation.IsChoosing())
        {
            BuildChoiceList();
        }
        else
        {
            AIText.text = playerConversation.GetText();
            nextButton.gameObject.SetActive(playerConversation.HasNext());
        }
      
    }

    

    private void BuildChoiceList()
    {
        foreach (Transform item in choiceRoot)
        {
            Destroy(item.gameObject);
        }
        foreach (DialogueNode choice in playerConversation.GetChoices())
        {
            GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
            var textComp = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
            textComp.text = choice.GetText();
            Button button = choiceInstance.GetComponentInChildren<Button>();
            button.onClick.AddListener(() =>                        // lambda function : Built the function for each button call here           
            {
                playerConversation.SelectChoice(choice);
                //UpdateUI();
            });
        }
    }
  }

