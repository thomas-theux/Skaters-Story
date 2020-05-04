using UnityEngine;
using System.Collections;
 
public class KeyTest : MonoBehaviour {
 
	private KeyCombo falconPunch = new KeyCombo(new string[] {"down", "up", "right"});
	private KeyCombo falconKick = new KeyCombo(new string[] {"down", "left", "up"});
 
	void Update () {
		if (falconPunch.Check()) {
			// do the falcon punch
			Debug.Log("PUNCH"); 
        }

		if (falconKick.Check()) {
			// do the falcon punch
			Debug.Log("KICK"); 
		}
	}
}