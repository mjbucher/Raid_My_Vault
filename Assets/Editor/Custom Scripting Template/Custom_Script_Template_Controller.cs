//Assets/Editor/KeywordReplace.cs
using UnityEngine;
using UnityEditor;
using System.Collections;

public class KeywordReplace : UnityEditor.AssetModificationProcessor 
{
	public static void OnWillCreateAsset ( string path ) 
	{
		// *** Find path for each file
		path = path.Replace( ".meta", "" );
		int index = path.LastIndexOf( "." );
		string file = path.Substring( index );
		if ( file != ".cs" && file != ".js" && file != ".boo" ) return;
		index = Application.dataPath.LastIndexOf( "Assets" );
		path = Application.dataPath.Substring( 0, index ) + path;
		file = System.IO.File.ReadAllText( path );
	
		#region Namespace-Developer
		// Format spaces out of company name
	    string _namespaceDevelopers = PlayerSettings.companyName.Replace(" ", "_") + ".";
		// Check if there was a non default compnay name
		_namespaceDevelopers = _namespaceDevelopers == "DefaultCompany." ? "" : _namespaceDevelopers;
		#endregion

		#region Naming-Conventions
		// naming convention rules
		string [] _namingConventions = 
		{
			"scripts = CapitalFirstLetters",
			"functions = Capital_Pot_Case",
			"public variables = switchCase",
			"local variables = _switchCase"
		};
		// top formatting for structure
		string _formattedNaming = "/// <summary> \n /// *** Naming Conventions *** \n";
		// adds everything in the _namingConventions array and formats it properly
		foreach (string item in _namingConventions)
		{
			_formattedNaming += "/// " + item + " \n";
		}
		// ending formatting added
		_formattedNaming += "/// </summary>"; 
		#endregion

		#region Order of Execution
		string[] _orderOfExecution = 
		{
			"Awake () - on scene load",
			"OnEnable () - after scene load",
			"Start () - before first frame",
			"FixedUpdate () - per set-amount (0.02 by default) frames - Set in Time Manager",
			"Update () - every frame",
			"LateUpdate() - after Update ()"
		};
		// top formatting for structure
		string _formattedOrderOfExecution = "/// <summary> \n /// *** Order of execution *** \n";
		// adds everythign in the _orderOfExecutions array and formats it properly
		foreach (string item in _orderOfExecution)
		{
			_formattedOrderOfExecution += "/// " + item + " \n";
		}
		// ending formatting added
		_formattedOrderOfExecution += "/// </summary>";
		#endregion

		#region Formatting
		string _division = "/// <summary> \n /// ----------------------- \n /// </summary>";
		#endregion


		#region Tag-Creation
		/// <summary>
		/// file = file.Replace("#TAG#", value);
		/// </summary>
		file = file.Replace( "#DATECREATED#", System.DateTime.Now + "" );
		file = file.Replace( "#PROJECTNAME#", PlayerSettings.productName );
		file = file.Replace( "#PROJECTDEVELOPERS#", PlayerSettings.companyName );
		file = file.Replace( "#NAMINGCONVENTIONS#", _formattedNaming);
		file = file.Replace( "#ORDEROFEXECUTION#", _formattedOrderOfExecution);
		file = file.Replace( "#NAMESPACEDEVELOPERS#", _namespaceDevelopers);
		file = file.Replace( "#DIVISION#", _division);
		#endregion
	
		System.IO.File.WriteAllText( path, file );
		AssetDatabase.Refresh();
	}
}