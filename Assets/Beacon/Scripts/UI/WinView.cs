using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System; 

public class WinView : BaseView
{
	public override void Open()
	{
		InitTip(); 
		PlotManager.instance.Win(null); 
		base.Open();
		Singleton._audioManager.PlayDefeat(true); 
	}

	public override void Close()
	{
		Singleton._audioManager.PlayDefeat(false); 
		base.Close();
		ClearTip(); 
	}

	public override void OnClick0()
	{
		UIManager._Instance.Open(EView.Start); 
		base.OnClick0();
	}

	#region Tip

	[NonSerialized] public int _MaxTipCount = 12; 
	[SerializeField] Text _tipText; 
	int _curTipCount; 
	IEnumerator _tipShadeRoutine; 
	float _time = 0; 
	float _maxTime = 5f; 

	void InitTip()
	{
		_tipText.text = ""; 
		_curTipCount = 0; 
		_time = 0; 
	}

	public void SetTipInfo(string info) 
	{
		//		Debug.LogError("info: " + info); 
		if (string.IsNullOrEmpty(info))
		{
			return; 
		}

		string s = _tipText.text; 
		if (_curTipCount >= _MaxTipCount)
		{
			s = TipBoost(s); 
		}
		else
		{
			++_curTipCount; 
		}
		_tipText.text = s + (string.IsNullOrEmpty(s) ? "" : "\n") + info; 
		//		Debug.LogError("info: " + _tipText.text); 
	}

	void ClearTip()
	{
		InitTip(); 
	}

	void TipShade()
	{
		//		Debug.LogError("Time: " + _time); 
		_time += Time.deltaTime; 
		if (_time > _maxTime)
		{
			//			Debug.LogError("Time1: " + _time);
			--_curTipCount; 
			_time = 0; 
			_tipText.text = TipBoost(_tipText.text); 
		}
	}

	string TipBoost(string s)
	{
		string[] tips = s.Split('\n'); 
		string totalTip = ""; 
		for (int i = 1; i < tips.Length; ++i)
		{
			totalTip += (string.IsNullOrEmpty(totalTip) ? "" : "\n") + tips[i]; 
		}
		return totalTip; 
	}

	void Update()
	{
		if (!(string.IsNullOrEmpty(_tipText.text)))
		{
			TipShade(); 
		}
	}

	#endregion
}
