using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartView : BaseView
{
	[SerializeField] Text[] _loadTexts; 
	[SerializeField] Button _saveBtn; 

	bool _canLoad; 
	public bool canLoad
	{
		get
		{ 
			return _canLoad; 
		}

		set
		{ 
			_canLoad = value; 
			_saveBtn.interactable = value; 
			_loadTexts[0].enabled = value; 
			_loadTexts[1].enabled = !value; 
		}
	}

	public override void Open()
	{
		base.Open();
		canLoad = Singleton._archiveManager.LoadPlayer(ConstValue._playerId) != null; 
		Singleton._audioManager.PlayerTitle(true); 
	}

	public override void Close()
	{
		base.Close();
		Singleton._audioManager.PlayerTitle(false); 
	}

	public override void OnClick0()
	{
		UIManager._Instance.Open(EView.Plot); 
		base.OnClick0();
	}

	public override void OnClick1()
	{
		UIManager._Instance.Open(EView.HUD); 
		GameManager._Instance.Init(true); 
		base.OnClick0();
	}

	public override void OnClick2()
	{
		GameManager._Instance.Clear(); 
		Application.Quit();
		base.OnClick0();
	}

	public void OnClickCredits()
	{
		
	}
}
