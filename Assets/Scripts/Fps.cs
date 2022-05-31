using UnityEngine;
using System.Collections;
using TMPro;

public class Fps : MonoBehaviour {

    string label = "";
	float count;
	public TextMeshProUGUI fpsText;
	IEnumerator Start ()
	{
		GUI.depth = 2;
		while (true) {
			if (Time.timeScale == 1) {
				yield return new WaitForSeconds (0.1f);
				count = (1 / Time.deltaTime);
				label = "FPS :" + (Mathf.Round (count));
			}
			else
			{
				label = "Pause";
			}
			yield return new WaitForSeconds (0.5f);
		}
	}
    private void Update()
    {
		fpsText.text = label;
    }

  /*  void OnGUI ()
	{
		GUI.Label (new Rect (5, 40, 300, 75), label);
	}
  */
}
