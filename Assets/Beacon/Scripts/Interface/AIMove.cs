using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AIMove : IMove
{
	[NonSerialized] public Action<Pos> onMove; 
	[NonSerialized] public Action onAllure; 
	[NonSerialized] public Transform _followTarget; 
	int _x; 
	int _y; 
	Transform _transform; 

	System.Random random = new System.Random(); 
	Pos[] coordinates = new Pos[4]{new Pos(-1, 0), new Pos(1, 0), new Pos(0, -1), new Pos(0, 1)}; 

	public void Init(Pos pos, Transform transform)
	{
		_transform = transform; 
		_x = pos._x; 
		_y = pos._y; 
	}

	public void Clear()
	{
		_transform = null; 
		onMove = null; 
	}

	public void Update()
	{
		if (Player._Instance.isLockMove)
		{
			return; 
		}
		if (
//			Input.GetKeyDown(KeyCode.Space)
			(int)(random.Next(100)) == 0
		)
		{
			//			Debug.Log("DarkPrice Move"); 
			// 为寻路系统注入数据
			List<PathNode> nodes = new List<PathNode>();
			char[] map = MapManager.GetCurMap().ToCharArray(); 
			for (int i = 0, max = map.Length; i < max; i++)
			{
				PathNode n = new PathNode(); 
				var m = map[i]; 
				n._pos = MapManager.Index2Pos(i).ToVector3(); 
				n._isWall = m != MapCode.NONE; 
				n._index = i; 
				nodes.Add(n);
			}
			Singleton._pathFindystem._posLib = nodes; 
			Singleton._pathFindystem._startPos = new PathNode{_pos = _transform.position}; 
			Singleton._pathFindystem._endPos = new PathNode{_pos = _followTarget.position};
			var path = Singleton._pathFindystem.Find(); 
			if (path != null && path.Length > 0)
			{
				int index = GetIndex(path[0]._pos, _transform.localPosition); 
				Pos pos = coordinates[index]; 
				Move(pos._x, pos._y); 
				if (onMove != null)
				{
					onMove(pos); 
				}
			}
			// 攻击
			else
			{
				if ((int)(random.Next(20)) == 0)
				{
					// 应该要把人物（Player）的识别信息也写到地图里面啊
					int index = GetIndex(Player._Instance.GetPos().ToVector3(), _transform.localPosition); 
					Pos pos = coordinates[index]; 
					Move(pos._x, pos._y); 
				}
			}
		}
		if (
//			Input.GetKeyDown(KeyCode.L) 
			random.Next(600) == 0
		)
		{
			if (onAllure != null)
			{
				onAllure(); 
			}
		}
	}

	/// <summary>
	/// 输入偏移量
	/// </summary>
	public void Move(int x, int y)
	{
		var pos = _transform.localPosition.ToPos(); 
		MoveUtil.Move(pos, x, y, _transform); 
	} 

	int GetIndex(Vector3 targetPos, Vector3 curPos)
	{
		int x = Mathf.RoundToInt(Mathf.Clamp(targetPos.x - curPos.x, -1, 1)); 
		int y = Mathf.RoundToInt(Mathf.Clamp(targetPos.y - curPos.y, -1, 1)); 
		for (int i = 0, max = coordinates.Length; i < max; i++)
		{
			var c = coordinates[i]; 
			if (c._x == x && c._y == y)
			{
				return i; 
			}
		}
		return 0; 
	}
}
