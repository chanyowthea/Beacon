using UnityEngine;
using System.Collections;
using System; 

public interface IDie
{
	Action onDie{ get; set; } 
	void Die(Pos pos); 
}

public class NormalDie : IDie
{
	public Action onDie{ get; set; } 
	public void Die(Pos pos)
	{
		if (onDie != null)
		{
			onDie(); 
		} 

		int index = MapManager.CurIndex(pos._x, pos._y);
		// 此处会销毁数据已经不在地图中的物体
		if (index >= 0)
		{
			MapManager.ResetMap(index); 
		}
	}
}

public class PlayerDie : IDie
{
	public Action onDie{ get; set; } 
	public void Die(Pos pos)
	{
		Debug.LogError("Game Over! "); 
		UIManager._Instance.Open(EView.End);
	}
}