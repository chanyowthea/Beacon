using UnityEngine;
using System.Collections;

public class WinView : BaseView
{
	public override void OnClick0()
	{
		UIManager._Instance.Open(EView.Start); 
		base.OnClick0();
	}
}
