using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System; 

public class PopupView : BaseView
{
	public override void OnClick0()
	{
		base.OnClick0();
		GameManager._Instance.Clear(); 
		Application.Quit();
	}

	public override void OnClick1()
	{
		base.OnClick1();
		Singleton._archiveManager.SaveGame();
		var view = UIManager._Instance.Open<MessageView>(EView.Message, false); 
		view.message = "存档成功！"; 
		Close(); 
	}

	public override void OnClick2()
	{
		base.OnClick2();
		UIManager._Instance.SetMaskEnable(!GameData._isOpenMask); 
	}
}
