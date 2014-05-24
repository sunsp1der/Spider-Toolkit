// *******************************************************
// Copyright 2013 Daikon Forge, all rights reserved under 
// US Copyright Law and international treaties
// *******************************************************
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class OnUnityLoad
{
	
	static OnUnityLoad()
	{
	
		EditorApplication.playmodeStateChanged = () =>
		{
			// added stMain autosave to condition below
			if( EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying && PlayerPrefs.GetInt("AutoSave") != 0) 
			{
				
				Debug.Log( "Auto-Saving scene before entering Play mode: " + EditorApplication.currentScene );
				
				EditorApplication.SaveScene();
				EditorApplication.SaveAssets();
			}
			
		};
		
	}
	
}