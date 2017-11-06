using UnityEngine;
using System.Collections;

public class EndView : BaseView
{
	public override void Open()
	{
		base.Open();
		Singleton._audioManager.PlayDefeat(true); 
	}

	public override void Close()
	{
		Singleton._audioManager.PlayDefeat(false); 
		base.Close();
	}

	public override void OnClick0()
	{
		UIManager._Instance.Open(EView.Start); 
		base.OnClick0();
	}
}
