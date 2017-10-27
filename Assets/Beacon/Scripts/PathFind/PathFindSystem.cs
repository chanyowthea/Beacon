using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathNode
{
	public Vector3 _pos;
	public bool _isWall;
	public int _index;
	public PathNode _parent;
	public int _g;
	public int _h;

	public int _f{ get { return _g + _h; } }

	public override string ToString()
	{
		//        return string.Format("_index={0},_f={1},_g={2},_h={3}]", _index, _f, _g, _h);
		return string.Format("[PathNode: _pos={0}, _isWall={1}, _index={2}, _f={3}, _g={4}, _h={5}]", _pos, _isWall, _index, _f, _g, _h);
	}
}

//F = G + H
//F - 方块的总移动代价
//G - 开始点到当前方块的移动代价
//H - 当前方块到结束点的预估移动代价

public class PathFindSystem
{
	public List<PathNode> _posLib;
	public PathNode _startPos;
	// 这个不一定是lib中的堆
	public PathNode _endPos;
	// 两个网格中心的间距
	public float _nodeGap = 1;

	public PathNode[] Find()
	{
		var openSet = new List<PathNode>(); 
		var closeSet = new List<PathNode>(); 

		var startNode = GetNearestNodeInLib(_startPos._pos);  
		var endNode = GetNearestNodeInLib(_endPos._pos);  
		openSet.Add(startNode); 
		while (openSet.Count > 0)
		{
			// 只要有路走，那么总有一个是值最小的，因此这里不用判断最小值
			// 这里不可能出现curPos为空的情况，因为openSet已经保证Count大于0
			openSet.Reverse(); // 为了下面的使用最近添加的点反转
			var curPos = GetMinCoseNode(openSet.ToArray(), endNode); 

			if (!closeSet.Contains(curPos))
			{
				closeSet.Add(curPos); 
			}
			else
			{
				Debug.LogError("closeSet contains! " + curPos._index); 
			}
			openSet.Remove(curPos); 
			if (curPos == endNode
				//				|| index == 80
			)
			{
				List<PathNode> path = new List<PathNode>(); 
				var temp = endNode._parent; 
				while(temp != startNode)
				{
					// path中不加入中点
					path.Add(temp); 
					temp = temp._parent; 
				}
				path.Reverse(); 

//				string s = ""; 
//				for (int i = 0, max = path.Count; i < max; i++)
//				{
//					s += path[i].ToString() + "\n"; 
//				}
//				Debug.LogError("Find path! " + s); 
				return path.ToArray(); 
			}

			// 在周边寻路
			var neighbours = GetNeighbours(curPos); 
			if (neighbours == null || neighbours.Length == 0)
			{
				continue; 
			}
			for (int i = 0, max = neighbours.Length; i < max; i++)
			{
				var n = neighbours[i]; 
				if (closeSet.Contains(n))
				{
					continue; 
				}
				int newCost = curPos._g + ClacCost(curPos, n);
				if(newCost < n._g || !openSet.Contains(n))
				{
					n._g = newCost; 
					n._h = ClacCost(startNode, endNode); 
					n._parent = curPos; 
					if (!openSet.Contains(n))
					{
						openSet.Add(n);
					}
				}
			}
		}

		//		Debug.LogError("Cannot find path! "); 
		return null; 
	}

	PathNode GetMinCoseNode(PathNode[] nodes, PathNode endNode)
	{
		if (nodes == null || nodes.Length == 0)
		{
			return null; 
		}
		PathNode tempMinNode = nodes[0]; 
		for (int i = 0, max = nodes.Length; i < max; i++)
		{
			var n = nodes[i]; 
			// 优先使用f判断，如果相同那么继续判断h值
			if (n._f < tempMinNode._f)
			{
				tempMinNode = n; 
			}
			else
			{
				if (n._h < tempMinNode._h)
				{
					tempMinNode = n; 
				}
			}
		}
		return tempMinNode; 
	}

	// 注意扩展斜着走
	int ClacCost(PathNode originPos, PathNode targetPos)
	{
		int x = Mathf.RoundToInt(Math.Abs((originPos._pos.x - targetPos._pos.x) / _nodeGap)); 
		int y = Mathf.RoundToInt(Math.Abs((originPos._pos.y - targetPos._pos.y) / _nodeGap)); 
		// 判断到底是那个轴相差的距离更远
		// 是否使用斜角行走
		//		if (x > y)
		//		{
		//			return 14 * y + 10 * (x - y);
		//		}
		//		else
		//		{
		//			return 14 * x + 10 * (y - x);
		//		}
		return x + y; 
	}

	// 先不管斜着走的情况
	PathNode[] GetNeighbours(PathNode node)
	{
		List<PathNode> nodes = new List<PathNode>(); 
		// 算法有待优化，可以折半查找
		for (int i = 0, max = _posLib.Count; i < max; i++)
		{
			var n = _posLib[i]; 
			int x = Mathf.RoundToInt(Math.Abs((n._pos.x - node._pos.x) / _nodeGap)); 
			int y = Mathf.RoundToInt(Math.Abs((n._pos.y - node._pos.y) / _nodeGap)); 
			if ((x | y) == 1 
				&& (x & y) == 0 // 是否使用斜角行走
				&& !n._isWall)
			{
				nodes.Add(n); 
			}
		}
		return nodes.ToArray(); 
	}

	PathNode GetNearestNodeInLib(Vector3 pos)
	{
		PathNode nearestNode = null; 
		int min = int.MaxValue; 
		for (int i = 0, max = _posLib.Count; i < max; i++)
		{
			var n = _posLib[i]; 
			int x = Mathf.RoundToInt(Math.Abs((n._pos.x - pos.x) / _nodeGap)); 
			int y = Mathf.RoundToInt(Math.Abs((n._pos.y - pos.y) / _nodeGap)); 
			if (x + y < min)
			{
				min = x + y; 
				nearestNode = n; 
			}
		}
		if (nearestNode == null)
		{
			Debug.LogError("Cannot find node in pos lib! "); 
		}
		return nearestNode; 
	}
}
