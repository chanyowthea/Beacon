using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System; 

public class MessageView : BaseView
{
	[SerializeField] Text _msgText; 

	public string message
	{
		set
		{ 
			_msgText.text = value; 
		}
		get
		{ 
			return _msgText.text; 
		}
	}

	public override void Open()
	{
		base.Open();
		Invoke("Close", 2); 
	}

	public override void Close()
	{
		CancelInvoke("Close");
		base.Close();
	}
}
