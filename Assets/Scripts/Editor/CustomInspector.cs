using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInspector : MonoBehaviour {

	#if UNITY_EDITOR
	[UnityEditor.MenuItem("Tools/Clear Player Prefs")]
	private static void ClearPlayerPrefs(){
		PlayerPrefsManager.ResetProgress ();
	}

	[UnityEditor.MenuItem("Tools/Unlock Some Levels")]
	private static void UnlockLevels(){
		PlayerPrefsManager.UnlockLevel ("102");	
		PlayerPrefsManager.UnlockLevel ("104");	
		PlayerPrefsManager.UnlockLevel ("106");	
		PlayerPrefsManager.UnlockLevel ("108");	
		PlayerPrefsManager.UnlockLevel ("302");	
	}
	#endif
}
