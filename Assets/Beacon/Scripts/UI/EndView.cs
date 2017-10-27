using UnityEngine;
using System.Collections;

public class EndView : BaseView
{
	public override void Open()
	{
		base.Open();
	}
	public override void OnClick0()
	{
		UIManager._Instance.Open(EView.Start); 
		base.OnClick0();
	}
}
