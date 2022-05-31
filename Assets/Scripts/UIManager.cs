using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace StarterAssets
{
    public class UIManager : MonoBehaviour
    {
        private UI_Input input;
        private StarterAssetsInputs starterAssetsInputs;
        public GameObject inventoryPanel;
        // Start is called before the first frame update
        private void Awake()
        {
            input = GetComponent<UI_Input>();
            starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        }
        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {
            if (input.toggleInventoryCanvas)
            {
                ToggleCanvas();

            }
        }
        public void ToggleCanvas()
        {
            if (!inventoryPanel.activeInHierarchy)
            {
                Debug.Log("Canvas Switched On");
                inventoryPanel.SetActive(true);
                starterAssetsInputs.SetCursorState(false);
                input.toggleInventoryCanvas = false;
              
            }

            else
            {
                Debug.Log("Canvas switched off");
                inventoryPanel.SetActive(false);
                starterAssetsInputs.SetCursorState(true);
                input.toggleInventoryCanvas = false;
                
            }



        }
    }
}
