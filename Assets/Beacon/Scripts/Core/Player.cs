using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{
	#region Main

	public static Player _Instance;
	// 只有高层管理器可以是单例
	[SerializeField] public PlayerMove _playerMove;
	public IHP _playerHurt = new NormalHP();
	IDie _playerDie;

	void Awake()
	{
		_Instance = this; 
	}

	public void Init(bool isLoad = false)
	{
//		Debug.Log("isUpstairs: " + _isUpstairs); 
		Singleton._levelManager.InitLevel(); 
		_playerHurt.Init(new PlayerDie(), transform, 4); 
		_playerMove.onMinusHP = (int value) =>
		{
			_playerHurt.Hurt(value); 
		}; 
		Reset(); 

		// 如果不读档就直接中断
		if (!isLoad)
		{
			return; 
		}
		Singleton._archiveManager.LoadGame(); 
	}

	public void Reset(bool isResetHP = false)
	{
		UIManager._Instance.Reset(); 
		UIManager._Instance.SetStep(GameData._Step); 

		// 重置地图数据
		MapManager.DestroyWall(); 
		MapManager.LoadMap(); 
		MapManager.GenerateWall(); 
		Singleton._archiveManager.LoadFloor(); 

		// 重置玩家数据
		_playerMove._isUpstairs = Singleton._levelManager.isUpStair; 
		_playerMove.ResetPos(); 

//		var info = Singleton._archiveManager.LoadObj(999, GameData._CurLevel); 
//		if (info != null)
//		{
//			_playerHurt.Reset(info._hp); 
//			_playerMove.SetPos(MapManager.Index2Pos(info._posIndex)); 
//		}

		SetRoleInfo(ERole.Grandpa); 

		UIManager._Instance.SetCurLevel(GameData._CurLevel); 
		if (isResetHP)
		{
			_playerHurt.Reset(_playerHurt._MaxHP); 
		}
	}

	public void Clear()
	{
		_playerMove.Clear(); 
		ClearRoleInfo(); 
//		ClearMeet(); 
	}

	#endregion


	#region Role Info

	//	[SerializeField] TextMesh _nameText;

	void SetRoleInfo(ERole roleIdent)
	{
//		RoleConf role = GameData._Instance._roleLib.GetRole(roleIdent); 
//		if (role != null)
//		{ 
//			_nameText.text = role._name; 
//		}
	}

	void ClearRoleInfo()
	{
//		_nameText.text = null; 
	}

	#endregion

	#region Die

	[NonSerialized] public Action _OnClear;
	[NonSerialized] public Action _OnReset;

	//	void Die()
	//	{
	//		_OnClear();
	//		InitLevel();
	//		_OnReset();
	//	}
	//

	#endregion

	public Pos GetPos()
	{
		return _playerMove.GetPos(); 
	}

	public bool MinusHP(int value)
	{
		_playerHurt.Hurt(value); 
		return true; 
	}

	public void ChangeStep(int step)
	{
		_playerMove.ChangeStep(step); 
	}

	public bool isLockMove { get { return _playerMove.isLockMove; } set { _playerMove.isLockMove = value; } }
}
