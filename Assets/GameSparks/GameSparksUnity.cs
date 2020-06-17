using UnityEngine;
using System.Collections.Generic;
using System;
using GameSparks;
using GameSparks.Core;
using GameSparks.Platforms;
using GameSparks.Platforms.WebGL;
using GameSparks.Platforms.Native;

/// <summary>
/// This is the starting point for GameSparks in your Unity game.
/// Add this class to a GameObject to get started.
/// </summary>
public class GameSparksUnity : MonoBehaviour
{
	/// <summary>
	/// You can override which connection settings GameSparks uses to connect to the backend with this member.
	/// </summary>
    public GameSparksSettings settings;

	void Start()
	{

//#if ((UNITY_PS4 || UNITY_XBOXONE) && !UNITY_EDITOR) || GS_FORCE_NATIVE_PLATFORM
#if (UNITY_XBOXONE && !UNITY_EDITOR) || GS_FORCE_NATIVE_PLATFORM
        this.gameObject.AddComponent<NativePlatform>();
#elif UNITY_WEBGL && !UNITY_EDITOR
		this.gameObject.AddComponent<WebGLPlatform>();
#else
        this.gameObject.AddComponent<DefaultPlatform>();
#endif
//		System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
//		GameSparksWebSocket.Proxy = new System.Net.DnsEndPoint("localhost", 8888);
//		GS.TraceMessages = true;
//		GameSparks.Core.GameSparksUtil.LogMessageHandler = Debug.Log;
		#if UNITY_IOS && !UNITY_EDITOR
		GSGetProxySettings(this.name);
		#endif
	}

	void OnGUI () {
		if (GameSparksSettings.PreviewBuild == true) {
			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));

			GUILayout.BeginVertical ();

			GUILayout.FlexibleSpace ();

			GUILayout.BeginHorizontal ();

			GUILayout.Space (10);

			GUILayout.Label ("GameSparks Preview mode", GUILayout.Width (200), GUILayout.Height (25));

			GUILayout.EndHorizontal ();

			GUILayout.EndVertical ();

			GUILayout.EndArea ();
		}
	}

	#if UNITY_IOS && !UNITY_EDITOR
	public void GSSetProxySettings(String proxyString){

		if(proxyString.IndexOf(":") != -1){
			String[] parts = proxyString.Split(':');
			System.Net.IPAddress parsedIP;
			System.Net.IPAddress.TryParse(parts[0], out parsedIP);
			if(parsedIP == null) {
				GameSparksWebSocket.Proxy = new System.Net.IPEndPoint(parsedIP, int.Parse(parts[1]));
			} else {
				GameSparksWebSocket.Proxy = new System.Net.DnsEndPoint(parts[0], int.Parse(parts[1]));
			}
		}

		Debug.Log("Proxy settings : " + proxyString);
	}

	[System.Runtime.InteropServices.DllImport("__Internal")]
	private static extern void GSGetProxySettings(string gameObjectName);

	#endif
}
