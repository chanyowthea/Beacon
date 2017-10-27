using UnityEngine;
using System.Collections;

public enum EDirection
{
	East,
	West,
	South,
	North
}

public enum EFaceDirection
{
	Forward,
	Backward,
	Left,
	Right, 
	LeftForward, 
	RightForward, 
	LeftBackward, 
	RightBackward
}

public static class MoveUtil
{
	public const byte _DIR_EAST = 1;
	public const byte _DIR_WEST = 2;
	public const byte _DIR_SOUTH = 4;
	public const byte _DIR_NORTH = 8; 

	static Pos[] offsets = new Pos[]{new Pos(0, 1), new Pos(0, 2), new Pos(0, -1), new Pos(0, -2), // 前方，后方
		new Pos(1, 0), new Pos(2, 0), new Pos(-1, 0), new Pos(-2, 0), // 右方，左方
		new Pos(1, 1), new Pos(-1, -1), new Pos(1, -1), new Pos(-1, 1)}; // 右前，左后，右后，左前
	static Pos[] dirs = new Pos[4]{new Pos(0, 1), new Pos(-1, 0), new Pos(0, -1), new Pos(1, 0)}; 

	/// <param name="rotateTimes">0向上, 90向左, -90(270)向右, 180向下</param>
	public static bool GetFaceDirection(out int distance, out EFaceDirection faceDirection, Pos checkPos, char checkCode, int rotateTimes = 0)
	{
		faceDirection = EFaceDirection.Forward;
		distance = 0; 
		var rawDir = dirs[rotateTimes]; 
		var dir = rawDir / rawDir.Length; 
		for (int i = 0, max = offsets.Length; i < max; i++)
		{
			var v = offsets[i]; 
			if (MapManager.GetCode(checkPos._x + v._x, checkPos._y + v._y) != checkCode)
			{
				continue; 
			}
//			Debug.LogError("v: " + v); 
			string label = ""; 
			if (dir._y == 0) // 角色朝向为左右
			{
				Pos xp = new Pos(0, v._y); 
				Pos yp = new Pos(v._x, 0); 
//				// 正交，判断角色左右
//				if (xp.Length != 0)
//				{
//					label += dir._x * v._y > 0 ? "左" : "右"; 
//				}
//				// 平行，判断角色前后
//				if(yp.Length != 0)
//				{
//					label += (dir._x * v._x > 0 ? "前" : "后");
//				}

				// 确定朝向
				if (xp.Length == 0)
				{
					faceDirection = dir._x * v._x > 0 ? EFaceDirection.Forward : EFaceDirection.Backward; 
				}
				else if (yp.Length == 0)
				{
					faceDirection = dir._x * v._y > 0 ? EFaceDirection.Left : EFaceDirection.Right; 
				}
				else
				{
					if (dir._x * v._y > 0)
					{
						faceDirection = dir._x * v._x > 0 ? EFaceDirection.LeftForward : EFaceDirection.LeftBackward; 
					}
					else
					{
						faceDirection = dir._x * v._x > 0 ? EFaceDirection.RightForward : EFaceDirection.RightBackward; 
					}
				}
			}
			if (dir._x == 0) // 角色朝向为上下
			{
				// 正交，判断角色左右
				Pos xp = new Pos(v._x, 0); 
				Pos yp = new Pos(0, v._y); 
//				if (xp.Length != 0)
//				{
//					label += dir._y * v._x < 0 ? "左" : "右"; 
//				}
//				// 平行，判断角色前后
//				if(yp.Length != 0)
//				{
//					label += (dir._y * v._y > 0 ? "前" : "后");
//				}

				// 确定朝向
				if (xp.Length == 0)
				{
					faceDirection = dir._y * v._y > 0 ? EFaceDirection.Forward : EFaceDirection.Backward; 
				}
				else if (yp.Length == 0)
				{
					faceDirection = dir._y * v._x < 0 ? EFaceDirection.Left : EFaceDirection.Right; 
				}
				else
				{
					if (dir._y * v._x < 0)
					{
						faceDirection = dir._y * v._y > 0 ? EFaceDirection.LeftForward : EFaceDirection.LeftBackward; 
					}
					else
					{
						faceDirection = dir._y * v._y > 0 ? EFaceDirection.RightForward : EFaceDirection.RightBackward; 
					}
				}
			} 
//			label += "方" + (v.Length == 1 ? "一" : "两") + "米";
			distance = v.Length; 
//			UIManager._Instance.SetTipInfo(string.Format(TipMessage._dirTip, label)); 
			return true; 
		}
		return false; 
	}

	public static void ShowDangerTip(Pos pos)
	{
		// 如果孙女反叛就不显示危险提示
		if (GameData._isGrandDaughterRebel || !Singleton._npcManager.Contains(ERole.GrandDaughter) || !GameData._isGrandDaughterInQueue)
		{
			return; 
		}

		int dis; 
		EFaceDirection face; 
		bool rs = MoveUtil.GetFaceDirection(out dis, out face, pos, MapCode.NPC_DARK_PRINCE, 
			(int)(Camera.main.transform.eulerAngles.z / 90)); 
		if (rs)
		{
			string label = ""; 
			switch (face)
			{
				case EFaceDirection.Forward: 
					label = "前"; 
					break; 
				case EFaceDirection.Backward: 
					label = "后"; 
					break; 
				case EFaceDirection.LeftForward: 
					label = "左前"; 
					break; 
				case EFaceDirection.RightForward: 
					label = "右前"; 
					break; 
				case EFaceDirection.LeftBackward: 
					label = "左后"; 
					break; 
				case EFaceDirection.RightBackward: 
					label = "右后"; 
					break; 
				case EFaceDirection.Left: 
					label = "左"; 
					break; 
				case EFaceDirection.Right: 
					label = "右"; 
					break; 
			}
			label += "方"; 
			label += (dis == 1 ? "一" : "两") + "米"; 
			UIManager._Instance.SetTipInfo(string.Format(TipMessage._dirTip, label)); 
		}
	}
		
	public static bool IsExistCodeFront(Pos pos, char code)
	{
		int dis; 
		EFaceDirection face; 
		bool rs = MoveUtil.GetFaceDirection(out dis, out face, pos, MapCode.NPC_DARK_PRINCE, 
			(int)(Camera.main.transform.eulerAngles.z / 90)); 
		return rs && dis == 1 && face == EFaceDirection.Forward; 
	}

	public static void Move(Pos curPos, int x, int y, Transform transform)
	{
		int newX = curPos._x + (int)x; 
		int newY = curPos._y + (int)y; 
		int curIndex = MapManager.CurIndex(newX, newY); 

		if (curIndex >= 0)
		{
//			Debug.Log("x, y: " + x + ", " + y); 
			//			Debug.Log("curIndex: " + curIndex + ", curMap: " + MapManager._curMap[curIndex]); 
			if (Player._Instance.GetPos() == new Pos(newX, newY)) // MapManager._curMap[curIndex] == MapCode.PIT
			{
				Player player = Player._Instance; 
				int count = 1; 
				bool rs = player.MinusHP(count); 
				if (rs)
				{
					UIManager._Instance.SetSysMsgInfo(string.Format("你受到{0}点伤害！", count)); // TODO 要设置敌人的攻击力，玩家的防御力
				}
			}
			else if (MapManager._curMap[curIndex] == MapCode.NONE || MapManager._curMap[curIndex] == MapCode.BEFORE_UPSTAIR 
				|| MapManager._curMap[curIndex] == MapCode.BEFORE_DOWNSTAIR)
			{
				int index = MapManager.CurIndex(curPos._x, curPos._y);
				curPos._x = newX; 
				curPos._y = newY; 

				transform.position = new Vector3(curPos._x + 0.5f, curPos._y + 0.5f, 0); 

				if (index >= 0)
				{
					MapManager.ResetMap(curIndex, index);
				}
				else
				{
					Debug.LogError("Index is minus! "); 
				}
			}
		}
	}
}
