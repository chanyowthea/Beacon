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

	public void Init()
	{
//		Debug.Log("isUpstairs: " + _isUpstairs); 
		Singleton._levelManager.InitLevel(); 
		_playerHurt.Init(new PlayerDie(), transform, 4); 
		_playerMove.onMinusHP = (int value) =>
		{
			_playerHurt.Hurt(value); 
		}; 
		Reset(); 

		// 读取存档并设置数据
		var playerSaveInfo = Singleton._archiveManager.LoadPlayer(ConstValue._playerId); 
		if (playerSaveInfo != null)
		{
			GameData._CurLevel = playerSaveInfo._curLevel; 
			_playerMove.SetPos(MapManager.Index2Pos(playerSaveInfo._posIndex)); 
			UIManager._Instance.SetCurLevel(GameData._CurLevel); 
			_playerHurt.Reset(playerSaveInfo._hp); 

			GameData._Step = playerSaveInfo._Step; 
			UIManager._Instance.SetStep(GameData._Step); 
			PlotManager.status = (EPlotStatus)playerSaveInfo._plotStatus; 
			GameData._IsOpenTutorial = playerSaveInfo._IsOpenTutorial; 
			GameData._IsShowedOpenDoorTutorial = playerSaveInfo._IsShowedOpenDoorTutorial; 
			GameData._CanRotateCamera = playerSaveInfo._CanRotateCamera; 
			GameData._HasRotated = playerSaveInfo._HasRotated; 
			GameData._curMeetHint = playerSaveInfo._curMeetHint; 

			GameData._isOpenMask = playerSaveInfo._isOpenMask; 
			UIManager._Instance.SetMaskEnable(GameData._isOpenMask); 

			Player._Instance._playerMove.SetRotateAngle(playerSaveInfo._rotateAngle); 

			GameData._isLockDoor = playerSaveInfo._isLockDoor; 
			GameData._isGrandDaughterRebel = playerSaveInfo._isGrandDaughterRebel; 
			GameData._isGrandDaughterInQueue = playerSaveInfo._isGrandDaughterInQueue; 
			UIManager._Instance._MaxTipCount = playerSaveInfo._maxTipCount; 
		}
		// 重置地图数据
		MapManager.DestroyWall(); 
		MapManager.LoadMap(); 
		MapManager.GenerateWall(); 
		Singleton._archiveManager.LoadFloor(); 

		if (playerSaveInfo != null)
		{
			// 由于上面重新生成了Wall，因此数据应该重新注入
			var obj = MapManager.GetObj(MapCode.NPC_GRAND_DAUGHTER);
			if (obj != null)
			{
				obj.GetComponent<GrandDaughter>()._allureCount = playerSaveInfo._allureCount;
			}
			obj = MapManager.GetObj(MapCode.NPC_DARK_PRINCE);
			if (obj != null)
			{
				obj.GetComponent<DarkPrince>()._startMove = playerSaveInfo._isDarkPrinceStartMove; 
			}
		}
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
