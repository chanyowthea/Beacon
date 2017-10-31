using UnityEngine;
using System.Collections;
using System; 

public class InputManager : MonoBehaviour
{
//	public event Action<float> onXChanged; 
	//	public event Action<float> onYChanged; 
	public event Action<Pos> onValueChanged; 

	[SerializeField] float _coolDownTime = 0.5f; 
	[SerializeField] float _curTime;

//	public void OnHorizontal(float value)
//	{
//		if (_curTime > 0)
//		{
//			return; 
//		}
//		_curTime = _coolDownTime; 
//		if (onXChanged != null)
//		{
//			onXChanged(value); 
//		}
//	}
//
//	public void OnVertical(float value)
//	{
//		if (_curTime > 0)
//		{
//			return; 
//		}
//		_curTime = _coolDownTime; 
//		if (onYChanged != null)
//		{
//			onYChanged(value); 
//		}
//	}

	public void OnValueChanged(Vector2 value)
	{
		if (_curTime > 0)
		{
			return; 
		}
		_curTime = _coolDownTime; 
		if (onValueChanged != null)
		{
			Pos pos; 
			if (Math.Abs(value.x) > Math.Abs(value.y))
			{
				pos = new Pos(1, 0) * Mathf.Sign(value.x); 
			}
			else
			{
				pos = new Pos(0, 1) * Mathf.Sign(value.y); 
			}
			onValueChanged(pos); 
		}
	}

	void Update()
	{
		if (_curTime <= 0)
		{
			return; 
		}
		_curTime -= Time.deltaTime; 
	}

	void Awake()
	{
//		onXChanged += OnXChanged;
//		onYChanged += OnYChanged;

//		onValueChanged += OnValueChang_Out; 
	}

//	void OnXChanged(float value)
//	{
//		Debug.LogError("OnXChanged=" + value); 
//	}
//
//	void OnYChanged(float value)
//	{
//		Debug.LogError("OnYChanged=" + value); 
//	}

//	void OnValueChang_Out(Vector2 value)
//	{
////		Debug.LogError("OnValueChanged, x=" + value.x + ", y=" + value.y); 
//		Pos pos; 
//		if (Math.Abs(value.x) > Math.Abs(value.y))
//		{
//			pos = new Pos(1, 0) * Mathf.Sign(value.x); 
//		}
//		else
//		{
//			pos = new Pos(0, 1) * Mathf.Sign(value.y); 
//		}
//		Debug.LogError("OnValueChanged, pos=" + pos); 
//	}
}
