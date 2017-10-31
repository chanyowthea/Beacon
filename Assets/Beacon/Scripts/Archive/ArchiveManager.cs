using UnityEngine;
using System.Collections;

public class SaveObjInfo
{
	public int _hp;
	public int _posIndex;

	public string ToSaveString()
	{
		return string.Format("_hp={0}, _posIndex={1}", _hp, _posIndex); 
	}

	public static SaveObjInfo ParseFromSaveString(string s)
	{
		SaveObjInfo info = new SaveObjInfo(); 
		string[] ss = s.Split(','); 
		if (ss.Length < 2)
		{
			return null; 
		}
		string tempHP = ss[0].Substring(ss[0].IndexOf('=') + 1);

		if (!int.TryParse(tempHP, out info._hp))
		{
			return null; 
		}
		string tempPosIndex = ss[1].Substring(ss[1].IndexOf('=') + 1);
		if (!int.TryParse(tempPosIndex, out info._posIndex))
		{
			return null; 
		}
		return info; 
	}
}

public class ArchiveManager
{
	public string _objKey = "_obj_id_{0}_level_{1}";
	public string _tempKey = "_temp_id_{0}_level_{1}";
	public string _playerKey = "_player_id_{0}";
	// 把首次从文件中加载出来的index作为id

	public void SaveGame()
	{
		// 每一层敌人血量
		// 对话进程

		// 孙女相关
		// 

		// 黑王子相关
		// 引诱次数
		// 

		// 爷爷相关
		// 
	}

	public void LoadGame()
	{
		
	}

	public void SaveObj(int id, int hp, int posIndex, int level)
	{
		SaveObjInfo info = new SaveObjInfo(); 
		info._hp = hp; 
		info._posIndex = posIndex; 
		PlayerPrefs.SetString(string.Format(_objKey, id, level), info.ToSaveString()); 
		Debug.LogError("save obj: " + "id=" + id + ", " + "level=" + level + ", " + info.ToSaveString()); 
	}

	public SaveObjInfo LoadObj(int id, int level)
	{
		string temp = PlayerPrefs.GetString(string.Format(_objKey, id, level));
		var info = SaveObjInfo.ParseFromSaveString(temp); 
		if (info != null)
		{
			Debug.LogError("load obj: " + "id=" + id + ", " + "level=" + level + ", " + info.ToSaveString()); 
		}
		return info; 
	}

	public void SaveTemp(int id, int hp, int posIndex, int level)
	{
		SaveObjInfo info = new SaveObjInfo(); 
		info._hp = hp; 
		info._posIndex = posIndex; 
		PlayerPrefs.SetString(string.Format(_tempKey, id, level), info.ToSaveString()); 
		Debug.LogError("save temp: " + "id=" + id + ", " + "level=" + level + ", " + info.ToSaveString()); 
	}

	public SaveObjInfo LoadTemp(int id, int level)
	{
		string temp = PlayerPrefs.GetString(string.Format(_tempKey, id, level));
		var info = SaveObjInfo.ParseFromSaveString(temp); 
		if (info != null)
		{
			Debug.LogError("load temp: " + "id=" + id + ", " + "level=" + level + ", " + info.ToSaveString()); 
		}
		return info; 
	}

	public void DeleteTempData()
	{
//		Debug.LogError("DeleteTempData"); 
		for (int i = 1, max = GameData._MaxLevel; i <= max; i++)
		{
			for (int index = 0, max1 = MapManager._width * MapManager._height; index < max1; index++)
			{
				string key = string.Format(_tempKey, index, i); 
				if (PlayerPrefs.HasKey(key))
				{
					PlayerPrefs.DeleteKey(key); 
				}
			}
		}
	}

	public void SavePlayer(int id, int level, int hp, int posIndex)
	{
		PlayerSaveInfo info = new PlayerSaveInfo(); 
		info._curLevel = level; 
		info._hp = hp; 
		info._posIndex = posIndex; 
		info._Step = GameData._Step; 
		info._plotStatus = (int)PlotManager.status; 
		info._IsOpenTutorial = GameData._IsOpenTutorial;
		info._IsShowedOpenDoorTutorial = GameData._IsShowedOpenDoorTutorial; 
		info._CanRotateCamera = GameData._CanRotateCamera; 
		info._HasRotated = GameData._HasRotated;
		info._rotateAngle = (int)Player._Instance.transform.localEulerAngles.z; 
		info._curMeetHint = GameData._curMeetHint; 
		info._isOpenMask = GameData._isOpenMask; 
		info._isLockDoor = GameData._isLockDoor; 
		info._isGrandDaughterRebel = GameData._isGrandDaughterRebel; 
		info._isGrandDaughterInQueue = GameData._isGrandDaughterInQueue; 
		info._maxTipCount = UIManager._Instance._MaxTipCount; 

		var obj = MapManager.GetObj(MapCode.NPC_GRAND_DAUGHTER);
		if(obj != null)
		{
			info._allureCount = obj.GetComponent<GrandDaughter>().allureCount; 
		}
		obj = MapManager.GetObj(MapCode.NPC_DARK_PRINCE);
		Debug.LogError("obj is empty =" + (obj == null)); 
		if (obj != null)
		{
			Debug.LogError("startMove=" + obj.GetComponent<DarkPrince>()._startMove); 
			info._isDarkPrinceStartMove = obj.GetComponent<DarkPrince>()._startMove; 
		}
		PlayerPrefs.SetString(string.Format(_playerKey, id), info.ToSaveString()); 
		Debug.LogError("save player: " + "id=" + id + ", " + info.ToSaveString()); 
	}

	public PlayerSaveInfo LoadPlayer(int id)
	{
		string temp = PlayerPrefs.GetString(string.Format(_playerKey, id));
		var info = PlayerSaveInfo.ParseFromSaveString(temp); 
		if (info != null)
		{
			Debug.LogError("load player: " + "id=" + id + ", " + info.ToSaveString()); 
		}
		return info; 
	}

	public void SaveFloor()
	{
		// 敌人血量，位置
		// 孙女血量，位置
		// 黑王子血量，位置
		Debug.LogError("Save Floor"); 
		var gos = MapManager._gos; 
		for (int i = 0, max = gos.Length; i < max; i++)
		{
			var go = gos[i]; 
			if (go == null)
			{
				continue; 
			}
			var n = go.GetComponent<MapNode>();
			if (n != null)
			{
				int hp = 0;
				if (MapManager._curMap[i] == MapCode.ENEMY)
				{
					var enemy = go.GetComponent<Enemy>();
					if (enemy != null)
					{
						hp = enemy._iHP._curHP; 
					}
					SaveObj(n._id, hp, n._posIndex, GameData._CurLevel); 
				}
				else if (MapManager._curMap[i] == MapCode.NPC_DARK_PRINCE)
				{
					var npc = go.GetComponent<DarkPrince>();
					if (npc != null)
					{
						hp = npc._iHP._curHP; 
					}
					SaveObj(n._id, hp, n._posIndex, GameData._CurLevel); 
				}
				else if (MapManager._curMap[i] == MapCode.NPC_GRAND_DAUGHTER)
				{
					var npc = go.GetComponent<GrandDaughter>();
					if (npc != null)
					{
						hp = npc._iHP._curHP; 
					}
					SaveObj(n._id, hp, n._posIndex, GameData._CurLevel);
				}
			}
		}

		// 将临时数据转化为持久化数据
		for (int i = 1, max = GameData._MaxLevel; i <= max; i++)
		{
			for (int index = 0, max1 = MapManager._width * MapManager._height; index < max1; index++)
			{
				string key = string.Format(_tempKey, index, i); 
				if (PlayerPrefs.HasKey(key))
				{
					SaveObj(index, 0, i, GameData._CurLevel); 
					PlayerPrefs.DeleteKey(key); 
				}
			}
		}
	}

	public void LoadFloor()
	{
//		return; 

		var map = MapManager.GetCurMap(); 
		for (int i = 0, max = map.Length; i < max; i++)
		{
			var m = map[i]; 
			if (m == MapCode.ENEMY || m == MapCode.NPC_DARK_PRINCE || m == MapCode.NPC_GRAND_DAUGHTER)
			{
				var origin = MapManager._gos[i]; 
				var info = LoadObj(i, GameData._CurLevel); 
				var temp = LoadTemp(i, GameData._CurLevel); 
				if (info == null)
				{
					continue; 
				}

				// 如果temp中有存储这个物体，那么加载的时候销毁掉这个物体
				// 如果存储数据中有这个key，并且hp为零，那么销毁该物体
				if ((temp != null && temp._hp == 0) || info._hp == 0)
				{
					MapManager.ResetMap(i); 
					continue; 
				}

				// 如果有移动，那么移动
				if (info._posIndex != i)
				{
					MapManager.ResetMap(info._posIndex, i); 
				}
				var go = MapManager._gos[info._posIndex]; 
				Debug.LogError("LoadFloor i: " + i); 
				Debug.LogError("LoadFloor index: " + info._posIndex); 
				if (go != null)
				{
					var curPos = MapManager.Index2Pos(info._posIndex).ToVector3(); 
					Debug.LogError("currenct pos: " + curPos); 
//					go.transform.position = curPos; 
					var n = go.GetComponent<Enemy>(); 
					if (n != null)
					{
						Debug.LogError("LoadFloor Reset: " + info._hp); 
						n._iHP.Reset(info._hp); 
						n._iMove.SetPos(MapManager.Index2Pos(info._posIndex)); 
					}

					var price = go.GetComponent<DarkPrince>(); 
					if (price != null)
					{
						Debug.LogError("LoadFloor DarkPrince hp: " + info._hp); 
						price._iHP.Reset(info._hp); 

						Debug.LogError("ai pos: " + info._posIndex); 
						price._iMove.SetPos(MapManager.Index2Pos(info._posIndex)); 
					}

					var daughter = go.GetComponent<GrandDaughter>(); 
					if (daughter != null)
					{
						Debug.LogError("LoadFloor GrandDaughter hp: " + info._hp); 
						daughter._iHP.Reset(info._hp); 
//						daughter._iMove.SetPos(MapManager.Index2Pos(info._posIndex)); 
					}
				}
			}
			else
			{
				// 仅用于生成孙女的特殊情况
				if (m == MapCode.NONE)
				{
					string key = string.Format(_objKey, i, GameData._CurLevel); 
					if (PlayerPrefs.HasKey(key))
					{
						var obj = LoadObj(i, GameData._CurLevel); 
						if (obj != null && obj._hp > 0)
						{
							MapManager.ResetMap(obj._posIndex, MapCode.NPC_GRAND_DAUGHTER, true); 
						}
					}
				}
			}
		}
	}
}
