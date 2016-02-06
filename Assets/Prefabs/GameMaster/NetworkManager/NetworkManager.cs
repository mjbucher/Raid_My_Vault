using UnityEngine;
using System.Collections;

public class NetworkManager : GameMaster 
{
	[HideInInspector] public string mainWebAddress = "http://www.mjbucher.com/SPROJ/PlayerProfiles/";
	// need to add
	[HideInInspector] public string accountExtension;

	#region Update Interval
	// Enum list for Time Units
	public enum UpdateUnit 
	{
		Second,
		Minute,
		Hour
	};
	// variables for time
	[Header("Update Interval")]
	public UpdateUnit updateUnit = UpdateUnit.Minute;
	public int updateTime = 5;
	[HideInInspector] public int updateTimeInSeconds;
	#endregion

	#region Account Info
	[Header("Account Info")]
	// need yet - talk to GM or server
	public string profileName ;
	public string personalWebRedirect ;
	public enum _RecoveryQuestions
	{
		None
	};

	// random salt + hash these later
	public string _email;
	public string _password ;
	public _RecoveryQuestions _recoveryQuestion = _RecoveryQuestions.None;
	public string _recoveryAnswer;

	#endregion

	#region Seed Info
	[Header("Seed Info")]
	public int _currency;
	// later this will be the file path
	public string _seedPath;  
	public int _levelDifficulty;
	// make a 'Reward' class associated with GM and Dungeon Manager
	public int _levelReward;

	#endregion

	#region Social Info
	[Header("Social Info")]
	public string[] _privateMessages;
	public string[] _attackHistory;
	public string[] _defenseHistory;
	public string[] _friendList;

	#endregion


	/// <summary>
	/// Converts updateTime and updateUnit into seconds.
	/// Stored in UpdateTimeInSeconds.
	/// </summary>
	public void Convert_To_Second ()
	{
		int _conversionMultiplier = 0;
		switch (updateUnit)
		{
			case UpdateUnit.Second:
				_conversionMultiplier = 1;
				break;
			case UpdateUnit.Minute:
				_conversionMultiplier = 60;
				break;
			case UpdateUnit.Hour:
				_conversionMultiplier = 60 * 60;
				break;
			default:
				Debug.Log("Missing Update Unit Type");
				break;
		}
		updateTimeInSeconds = updateTime * _conversionMultiplier;
	}

//	public IEnumerator Update_And_Wait ()
//	{
//		
//	}


	/// <summary>
	/// Updates the info on server or locally. 
	/// Use "UPLOAD" or "DOWNLOAD". *Not case sensitive
	/// </summary>
	/// <param name="_action">Use 'UPLOAD' or 'DOWNLOAD'. *Not case sensitive*</param>
	public void Update_Info (string _action)
	{
		if (_action.ToLower() == "upload")
		{
			Upload_Info();
		}
		else if (_action.ToLower() == "download")
		{
			Download_Info();
		}
		else
		{
			Debug.Log("Not a valid option, please use 'UPLOAD' or 'DOWNLOAD'. *Not case sensitive*");
		}
	}

	/// <summary>
	/// Uploads the local info to the server.
	/// </summary>
	public void Upload_Info ()
	{
		
	}

	/// <summary>
	/// Downloads the server info to local.
	/// </summary>
	public void Download_Info ()
	{
		
	}

}
