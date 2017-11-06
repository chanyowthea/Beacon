using UnityEngine;
using System.Collections;
using System;

public class PlayerMove : BaseMove
{
	public bool _isUpstairs;
	[NonSerialized] public Action<int> onMinusHP;

	public void Clear()
	{
		Singleton._inputManager.onValueChanged -= OnMove;
	}

	public override void ResetPos()
	{
		base.ResetPos(); 
		char role = _isUpstairs ? '8' : '9'; // 上楼后达到的位置
		var pos = MapManager.GetPos(role); 
		SetPos(pos); 

		Singleton._inputManager.onValueChanged -= OnMove;
		Singleton._inputManager.onValueChanged += OnMove;
	}

	public void SetPos(Pos pos)
	{
		_x = pos._x; 
		_y = pos._y; 
		transform.position = new Vector3(_x + 0.5f, _y + 0.5f, 0);
		Vector2[] dirs = new Vector2[4]; 
		dirs[0] = new Vector2(1, 0); 
		dirs[1] = new Vector2(-1, 0); 
		dirs[2] = new Vector2(0, -1); 
		dirs[3] = new Vector2(0, 1); 
		float[] angles = new float[]{ -90, 90, 180, 0 }; // 此处旋转角度与上面的朝向对应
		transform.eulerAngles = Vector3.zero; 

		//		if (!GameData._CanRotateCamera)
		//		{
		//			return; 
		//		}

		// 检测角色的上下左右的地块
		for (int i = 0, count = dirs.Length; i < count; ++i)
		{
			int index = MapManager.CurIndex(_x + (int)dirs[i].x, _y + (int)dirs[i].y); // 检查是否越界
			if (index > 0)
			{
				char c = MapManager._curMap[index]; 
				if (c == MapCode.NONE) // 如果是空地
				{
					// 如果摄像机不旋转，那么要判断并播放人物朝向的动画
					if (!GameData._CanRotateCamera)
					{
						_anim.SetFloat("h", dirs[i].x); 
						_anim.SetFloat("v", dirs[i].y); 
					}
					else
					{
						Vector3 angle = new Vector3(0, 0, angles[i]); // euler角以逆时针为正，所以要面向右边就要旋转-90度
						transform.localEulerAngles += angle; 
						UIManager._Instance.ChangeCompassDir(-angle); 
						GameData._HasRotated = i <= 1; // 这是angles旋转角度为90和-90的时候才认为旋转了，否则不认为旋转
						//					Debug.Log("HasRotated: " + GameData._HasRotated); 
					}
					break; 
				}
			}
		}
	}

	protected override void Move(int x, int y) // 3 Enemy, 2 Pit, 1 Wall, 0 Road, 
	{
		int originX = _x;
		int originY = _y; 
		int newX = _x + (int)x; 
		int newY = _y + (int)y; 
		int curIndex = MapManager.CurIndex(newX, newY); 

//		Debug.LogError("Move curIndex: " + curIndex); 
		if (curIndex >= 0)
		{
			//			Debug.Log("curIndex: " + curIndex + ", curMap: " + MapManager._curMap[curIndex]); 
			if (MapManager._curMap[curIndex] == MapCode.NONE || MapManager._curMap[curIndex] == MapCode.BEFORE_UPSTAIR ||
			    MapManager._curMap[curIndex] == MapCode.BEFORE_DOWNSTAIR)
			{
				//如果人物移动，那么播放音效 
				Singleton._audioManager.Walk(); 

				//				int index = MapManager.CurIndex(_x, _y); 
				_x = newX; 
				_y = newY; 
				ChangeStep(GameData._Step + 1);
				CalculateDirection();

				if (PlotManager.status == EPlotStatus.Battle && !GameData._isGrandDaughterInQueue && !GameData._isGrandDaughterRebel)
				{
					MoveUtil.ShowDangerTip(new Pos(newX, newY)); 
				}

				int enemyX; 
				int enemyY; 
				// 检测周围的八个点
				if (MapManager.IsExistCodeInRange(newX, newY, out enemyX, out enemyY, MapCode.NPC_GRAND_DAUGHTER)
				    || MapManager.IsExistCodeInRange(newX, newY, out enemyX, out enemyY, MapCode.NPC_DARK_PRINCE))
				{
					int idx = MapManager.CurIndex(enemyX, enemyY); 
					if (
//						GameData._curMeetHint <= 0 && 
						MapManager._curMap[idx] == MapCode.NPC_GRAND_DAUGHTER)
					{
						if (PlotManager.status == EPlotStatus.Start)
						{
							GameObject go = MapManager.GetObj(idx); 
							NPC npc = null; 
							if (go != null)
							{
								npc = go.GetComponent<NPC>();
							}
							if (npc != null && npc._roleIdent == ERole.GrandDaughter)
							{
								if (GameData._curMeetHint == 0)
								{
									UIManager._Instance.SetSysMsgInfo(SystemMessage._meetHint_Find); 
								}
								else
								{
									UIManager._Instance.SetSysMsgInfo(SystemMessage._meet_Find); 
								}
								GameData._curMeetHint = 1; 

								// start plot
								isLockMove = true; 
								PlotManager.instance.Meet(npc._roleIdent, () =>
									{
										isLockMove = false; 
									}); 
							}
						}
						else if (PlotManager.status == EPlotStatus.Battle)
						{
							GameObject go = MapManager.GetObj(idx); 
							NPC npc = null; 
							if (go != null)
							{
								npc = go.GetComponent<NPC>();
							}
							// 如果是孙女
							if (npc != null && npc._roleIdent == ERole.GrandDaughter)
							{
								int dis; 
								EFaceDirection face; 
								bool rs = MoveUtil.GetFaceDirection(out dis, out face, GetPos(), MapCode.NPC_GRAND_DAUGHTER, 
									          (int)(Camera.main.transform.eulerAngles.z / 90)); 
								if (rs && (int)face <= (int)EFaceDirection.Right)
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
										case EFaceDirection.Left: 
											label = "左"; 
											break; 
										case EFaceDirection.Right: 
											label = "右"; 
											break; 
									}
									UIManager._Instance.SetTipInfo(string.Format(TipMessage._grandDaughterNear, label)); 
								}
							}
						}
					}
					else if (MapManager._curMap[idx] == MapCode.NPC_DARK_PRINCE)
					{	
						// 开始剧情
						// 孙女加入与否都可以见到黑王子
						if (PlotManager.status == EPlotStatus.Battle_Before)
						{
							GameObject go = MapManager.GetObj(idx); 
							NPC npc = null; 
							if (go != null)
							{
								npc = go.GetComponent<NPC>();
							}
							if (npc != null && npc._roleIdent == ERole.DarkPrince)
							{
								isLockMove = true; 
								npc.GetComponent<DarkPrince>()._startMove = true; 
								PlotManager.instance.Battle(() =>
									{
										isLockMove = false; 
									}); 
							}
						}
					}
				}
			}
			else
			{

				// 下面都是未移动
				if (MapManager._curMap[curIndex] == MapCode.ENEMY)
				{
					Singleton._audioManager.Attack();
					// do hurt
					GameObject go = MapManager.GetObj(curIndex); 
					if (go != null)
					{
						Enemy enemy = go.GetComponent<Enemy>(); 
						int count = 1;
						enemy.Hurt(count); 
						// TODO 要设置敌人的攻击力，玩家的防御力
						UIManager._Instance.SetSysMsgInfo(string.Format(SystemMessage._attack, "敌人", count)); 
					}
				}
				else if (MapManager._curMap[curIndex] == MapCode.PIT)
				{
					Singleton._audioManager.Attack();
					int count = 1; 
					if (onMinusHP != null)
					{
						onMinusHP(count); 
					}
					UIManager._Instance.SetSysMsgInfo(string.Format(SystemMessage._hurt, count)); 
				}
				// 如果将要移动的地方已经有孙女在那个位置，那么对那个位置的目标造成伤害
				else if (MapManager._curMap[curIndex] == MapCode.NPC_GRAND_DAUGHTER)
				{
					Singleton._audioManager.Attack();
					GameObject go = MapManager.GetObj(curIndex); 
					if (go != null)
					{
						GrandDaughter grandDaughter = go.GetComponent<GrandDaughter>(); 
						int count = 1;
						grandDaughter.Hurt(count); 
						// TODO 要设置敌人的攻击力，玩家的防御力
						UIManager._Instance.SetSysMsgInfo(string.Format(SystemMessage._attack, GameData._Instance._roleLib.GetRole(ERole.GrandDaughter)._name, count)); 
					}
				}
				else if (MapManager._curMap[curIndex] == MapCode.NPC_DARK_PRINCE)
				{
					// 由于这个时候，孙女离开队伍，爷爷一个人会迷失方向，只能攻击前面的对象
					if (MoveUtil.IsExistCodeFront(new Pos(originX, originY), MapCode.NPC_DARK_PRINCE))
					{
						Singleton._audioManager.Attack(); 
						// do hurt
						GameObject tempObj = MapManager.GetObj(curIndex); 
						if (tempObj != null)
						{
							DarkPrince price = tempObj.GetComponent<DarkPrince>(); 
							int count = 1;
							price.Hurt(count); 
							// TODO 要设置敌人的攻击力，玩家的防御力
							UIManager._Instance.SetSysMsgInfo(string.Format(SystemMessage._attack, GameData._Instance._roleLib.GetRole(ERole.DarkPrince)._name, count)); 
						}
					}
					else
					{
						Singleton._audioManager.CannotMove(); 
						CalculateDirection(false); 
					}
				}
				else // 不是可移动区域并且没有进行攻击
				{
					Singleton._audioManager.CannotMove(); 
					CalculateDirection(false); // 切换下一关不要显示tip
				}
			}
		}

		// 如果将要移动的区域超出边界，那么判定在边缘，可以切换地图，
		// 这样不好，如果传送门在地图中间或者边缘为空并不是地图，那么就会出BUG
		else
		{
			if (GameData._isLockDoor)
			{
				Singleton._audioManager.CannotMove(); 
				UIManager._Instance.SetTipInfo(Tip._lockDoor); 
				return; 
			}
			CalculateDirection(); // 为什么这里要Calc？当无法切换关卡时的处理，不应该这样 
//			Debug.LogError("curLevel: " + GameData._CurLevel + ", code: " + MapManager._curMap[MapManager.CurIndex(_x, _y)]); 
			Singleton._levelManager.NextLevel(GameData._CurLevel + (MapManager._curMap[MapManager.CurIndex(_x, _y)] == '9' ? 1 : -1)); 
		}
	}

	void CalculateDirection(bool isMoved = true)
	{ 
		Vector2[] dirPos = new Vector2[4]
		{ new Vector2(_x + 1, _y), new Vector2(_x - 1, _y), 
			new Vector2(_x, _y - 1), new Vector2(_x, _y + 1)
		}; 
		byte dir = 0; 
		for (int i = 0; i < dirPos.Length; ++i)
		{
			// touch the door
			if (MapManager.Check(ref dir, MapManager._constDirs[i], dirPos[i], MapCode.WALL) == 0)
			{
				UIManager._Instance.SetTipInfo((isMoved ? "" : Tip._cannotMove) + Tip.GetShowTips(dir, true)); 
				if (GameData._IsShowedOpenDoorTutorial
				    && GameData._IsOpenTutorial)
				{
					GameData._IsShowedOpenDoorTutorial = false; 
					UIManager._Instance.SetSysMsgInfo(SystemMessage._tutorialString_OpenDoor); 
				}
				return; 
			}
		}
		UIManager._Instance.SetTipInfo((isMoved ? "" : Tip._cannotMove) + Tip.GetShowTips(dir)); 

		dir = 0; 
		for (int i = 0; i < dirPos.Length; ++i)
		{
			MapManager.Check(ref dir, MapManager._constDirs[i], dirPos[i], MapCode.PIT); 
		}
		string pitString = Tip.GetPitTips(dir); 
		if (!(string.IsNullOrEmpty(pitString)))
		{
			UIManager._Instance.SetTipInfo(pitString); 
		}

		dir = 0; 
		for (int i = 0; i < dirPos.Length; ++i)
		{
			MapManager.Check(ref dir, MapManager._constDirs[i], dirPos[i], MapCode.ENEMY); 
		}
		string enemyString = Tip.GetEnemyTips(dir); 
		if (!(string.IsNullOrEmpty(enemyString)))
		{
			UIManager._Instance.SetTipInfo(enemyString); 
		}
	}

	public void ChangeStep(int step)
	{
		GameData._Step = step; // 新的x，y必须是可以行走的才加步数 
		UIManager._Instance.SetStep(GameData._Step); 
	}

	[SerializeField] Animator _anim;

	public void SetRotateAngle(int angle)
	{
		var temp = new Vector3(0, 0, angle); 
		Player._Instance.transform.eulerAngles += temp; 
		UIManager._Instance.ChangeCompassDir(-temp); 
	}

//	void Update()
//	{
//		if (isLockMove)
//		{
//			return; 
//		}
//		float h = Input.GetAxisRaw("Horizontal"); 
//		float v = Input.GetAxisRaw("Vertical"); 
//		if (Input.GetButtonDown("Horizontal"))
//		{
//			if (GameData._CanRotateCamera)
//			{
//				Vector3 angle = new Vector3(0, 0, (h < 0 ? 1 : -1) * 90); 
//				SetRotateAngle((int)angle.z); 
//				GameData._HasRotated = !(GameData._HasRotated); 
//			}
//			else
//			{
//				_anim.SetFloat("h", h); 
//				_anim.SetFloat("v", v);
//				Move((int)h, (int)v); 
//			}
//		}
//		else if (Input.GetButtonDown("Vertical"))
//		{
//			_anim.SetFloat("h", h); 
//			_anim.SetFloat("v", v);
////			Debug.LogError("can rotate: " + GameData._CanRotateCamera + ", is rotate: " + GameData._HasRotated); 
//			if (GameData._CanRotateCamera)
//			{
//				int rotateTimes = (int)(Camera.main.transform.eulerAngles.z / 90); 
//				int changeValue = (rotateTimes == 1 || rotateTimes == 2) ? -(int)v : (int)v; // 是否取反
//				if (GameData._HasRotated)
//				{
//					Move(changeValue, 0); 
//				}
//				else
//				{
//					Move(0, changeValue); 
//				}
//			}
//			else
//			{ 
//				Move((int)h, (int)v); 
//			}
//		}
//		transform.position = new Vector3(_x + 0.5f, _y + 0.5f, 0); 
//	}

	void OnMove(Pos pos)
	{
		if (isLockMove)
		{
			return; 
		}
		int h = pos._x; 
		int v = pos._y; 
		if (h !=0 && v == 0)
		{
			if (GameData._CanRotateCamera)
			{
				Vector3 angle = new Vector3(0, 0, (h < 0 ? 1 : -1) * 90); 
				SetRotateAngle((int)angle.z); 
				GameData._HasRotated = !(GameData._HasRotated); 
			}
			else
			{
				_anim.SetFloat("h", h); 
				_anim.SetFloat("v", v);
				Move(h, v); 
			}
		}
		if (v !=0 && h == 0)
		{
			_anim.SetFloat("h", h); 
			_anim.SetFloat("v", v);
			//			Debug.LogError("can rotate: " + GameData._CanRotateCamera + ", is rotate: " + GameData._HasRotated); 
			if (GameData._CanRotateCamera)
			{
				int rotateTimes = (int)(Camera.main.transform.eulerAngles.z / 90); 
				int changeValue = (rotateTimes == 1 || rotateTimes == 2) ? -(int)v : (int)v; // 是否取反
				if (GameData._HasRotated)
				{
					Move(changeValue, 0); 
				}
				else
				{
					Move(0, changeValue); 
				}
			}
			else
			{ 
				Move(h, v); 
			}
		}
		transform.position = new Vector3(_x + 0.5f, _y + 0.5f, 0); 
	}
}