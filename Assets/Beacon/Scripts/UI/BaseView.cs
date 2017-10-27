using UnityEngine;
using System.Collections;
using System;

public class BaseView : MonoBehaviour
{
	public event Action onOpen;
	public event Action onClose;

	public Action onClick0;
	public Action onClick1;
	public Action onClick2;

	public virtual void Open()
	{
		if (onOpen != null)
		{
			onOpen(); 
		}
		gameObject.SetActive(true); 
	}

	public virtual void Close()
	{
		if (onClose != null)
		{
			onClose(); 
		}
		gameObject.SetActive(false); 
	}

	public virtual void Show()
	{
		gameObject.SetActive(false); 
	}

	public virtual void Hide()
	{
		gameObject.SetActive(false); 
	}

	public virtual void OnClickClose()
	{
		Close(); 
	}

	public virtual void OnClick0()
	{
		if (onClick0 != null)
		{
			onClick0(); 
		}
	}

	public virtual void OnClick1()
	{
		if (onClick1 != null)
		{
			onClick1(); 
		}
	}

	public virtual void OnClick2()
	{
		if (onClick2 != null)
		{
			onClick2(); 
		}
	}
}
