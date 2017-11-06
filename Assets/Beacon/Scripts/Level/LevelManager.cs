using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
//	public static LevelManager instance{ private set; get; }

	public bool isUpStair{ get { return _isUpstairs; }}
	bool _isUpstairs;

//	void Awake()
//	{
//		instance = this; 
//	}

	public void InitLevel()
	{
		_isUpstairs = true;
	}

	public void NextLevel(int nextLevel)
	{
//		Singleton._archiveManager.SaveFloor(); 
		#if TEST
//		nextLevel = GameData._MaxLevel; 
		#endif

//		Debug.LogError("next: " + nextLevel); 
		if (nextLevel > GameData._MaxLevel || nextLevel <= 0)
		{
			if (nextLevel > GameData._MaxLevel)
			{
				UIManager._Instance.Open(EView.Win); 
			}
			return; 
		}
		if (nextLevel > GameData._CurLevel)
		{
			_isUpstairs = true; 
		}
		else
		{
			_isUpstairs = false; 
		}
		GameData._CurLevel = nextLevel; 
		Player._Instance.ChangeStep(GameData._Step + 1); 
		Player._Instance.Reset(); 

		if (GameData._CurLevel == 2 && !GameData._CanRotateCamera
//			&& GameData._isOpenMask
			&& (int)PlotManager.status < (int)EPlotStatus.Meet
		)
		{
			UIManager._Instance.SetSysMsgInfo(SystemMessage._getLost); 
			GameData._CanRotateCamera = true; 
		}

		if (GameData._CurLevel == 3 && UIManager._Instance._MaxTipCount == 12) // MaxTipCount应该放到GameData
		{
			UIManager._Instance.SetSysMsgInfo(SystemMessage._beExhausted); 
			UIManager._Instance._MaxTipCount /= 2; 
		}
		if (GameData._curMeetHint == -1 || GameData._curMeetHint == 0)
		{
			// 开启游戏之后
			if (PlotManager.status == EPlotStatus.Start)
			{
				if (GameData._CurLevel == 4 && _isUpstairs)
				{ 
					GameData._curMeetHint = 0; 
					UIManager._Instance.SetSysMsgInfo(SystemMessage._meetHint_Up);
				}
				else if (GameData._CurLevel == 3 && !_isUpstairs)
				{
					GameData._curMeetHint = 0; 
					UIManager._Instance.SetSysMsgInfo(SystemMessage._meetHint_Down); 
				}
			}
		}
		if (GameData._CurLevel == 5 && 
			(PlotManager.status == EPlotStatus.Meet || PlotManager.status == EPlotStatus.Start)) // 遇见孙女和没有遇见两种情况
			// 都可以触发后续剧情
		{
			GameData._isLockDoor = true; 
			PlotManager.status = EPlotStatus.Battle_Before; 
		}

			
		// 存储玩家信息
//		var playerPos = Player._Instance.GetPos(); 
//		Singleton._archiveManager.SavePlayer(ConstValue._playerId, GameData._CurLevel, Player._Instance._playerHurt._curHP, MapManager.CurIndex(playerPos._x, playerPos._y)); 


		// 为寻路系统注入数据
//		List<PathNode> nodes = new List<PathNode>();
//		char[] map = MapManager.GetCurMap().ToCharArray(); 
//		for (int i = 0, max = map.Length; i < max; i++)
//		{
//			PathNode n = new PathNode(); 
//			var m = map[i]; 
//			n._pos = MapManager.Index2Pos(i).ToVector3(); 
//			n._isWall = m != MapCode.NONE; 
//			n._index = i; 
//			nodes.Add(n);
//		}
//		Singleton._pathFindystem._posLib = nodes; 
	}
}
