using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class UI_Input : MonoBehaviour
    {

		[Header("UI Input Values")]	
		public bool toggleInventoryCanvas;



#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		
		public void OnToggleInventoryCanvas(InputValue value)
		{
			ToggleInventoryCanvas(value.isPressed);
		}

#endif


		public void ToggleInventoryCanvas(bool newToggle)
		{
			toggleInventoryCanvas = newToggle;
		}




	}
}
