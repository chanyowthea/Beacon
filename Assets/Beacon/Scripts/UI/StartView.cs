using UnityEngine;
using System.Collections;

public class StartView : BaseView
{
	public override void Open()
	{
		base.Open();
	}

	public override void OnClick0()
	{
		UIManager._Instance.Open(EView.Plot); 
		base.OnClick0();
	}
}
