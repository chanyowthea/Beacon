using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
	#region Main

	public static UIManager _Instance;

	void Awake()
	{
		_Instance = this; 
	}

	public void Init()
	{
		
	}

	public void Reset()
	{
		_hudView.Reset(); 
	}

	public void Clear()
	{
		
	}

	public int _MaxTipCount { get { return _hudView._MaxTipCount; } set { _hudView._MaxTipCount = value; } }

	public void SetPlotInfo(string info)
	{
		_plotView.SetTipInfo(info); 
	}

	public void SetPlotInfo_Win(string info)
	{
		_winView.SetTipInfo(info); 
	}

	public void SetTipInfo(string info)
	{
		_hudView.SetTipInfo(info); 
	}

	public void SetSysMsgInfo(string info)
	{
		_hudView.SetSysMsgInfo(info); 
	}

	public void SetCurLevel(int curLevel)
	{
		_hudView.SetCurLevel(curLevel); 
	}

	public void SetStep(int step)
	{
		_hudView.SetStep(step); 
	}

	public void ChangeCompassDir(Vector3 angle)
	{
		_hudView.ChangeCompassDir(angle); 
	}

	public void SetMaskEnable(bool isEnable)
	{
		_hudView.SetMaskEnable(isEnable); 
	}

	#endregion

	#region UI

	[SerializeField] StartView _startView;
	[SerializeField] EndView _endView;
	[SerializeField] HUDView _hudView;
	[SerializeField] WinView _winView;
	[SerializeField] PlotView _plotView;

	public T Open<T>(EView view) where T : BaseView
	{
		CloseAll(); 
		switch (view)
		{
			case EView.Start:
				_startView.Open(); 
				object obj = _startView; 
				return (T)obj; 
			case EView.End:
				_endView.Open(); 
				obj = _endView; 
				return (T)obj; 
			case EView.HUD:
				_hudView.Open(); 
				obj = _hudView; 
				return (T)obj; 
			case EView.Win:
				_winView.Open(); 
				obj = _winView; 
				return (T)obj; 
			case EView.Plot:
				_plotView.Open(); 
				obj = _plotView; 
				return (T)obj; 
		}
		return default(T);
	}

	public void Open(EView view)
	{
		CloseAll(); 
		switch (view)
		{
			case EView.Start:
				_startView.Open(); 
				break; 
			case EView.End:
				_endView.Open(); 
				break; 
			case EView.HUD:
				_hudView.Open(); 
				break; 
			case EView.Win:
				_winView.Open(); 
				break; 
			case EView.Plot:
				_plotView.Open(); 
				break; 
		}
	}


	void CloseAll()
	{
		_startView.Close(); 
		_endView.Close(); 
		_hudView.Close(); 
		_winView.Close(); 
		_plotView.Close(); 
	}

	public void GameOver()
	{
		MapManager.DestroyWall();
		PlotManager.instance.Clear();
		if (Player._Instance != null)
		{
			Player._Instance.Clear(); 
		}
		GameData._Instance.Clear(); 
	}

	#endregion
}

public enum EView
{
	Start,
	End,
	HUD,
	Win,
	Plot,
}
