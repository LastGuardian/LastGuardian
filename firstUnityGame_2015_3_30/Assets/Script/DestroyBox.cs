//------------------------------------------------------------------------------------------------------------------------------------------------------------
// 支持中文注解
// 初次编码时间：2015年04月16日20:12:05
// 最新修改时间：2015年04月16日20:12:03
// 作者：朱鹰仁
// 修改人员：朱鹰仁
// 脚本概要：消除魔法阵
//------------------------------------------------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/DestroyBox")]
public class DestroyBox : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	// 此函数只有在碰撞体互相接触时才会被触发
	void OnTriggerEnter(Collider other)
	{
		// 如果没有碰到敌人则不作处理
		// 标签比较，比较的两边都需要一个值
		// Is Trigger 需要打开
		if(other.tag.CompareTo ("MagicRange") != 0) return;
		Destroy(other.transform.parent.gameObject);
	}
}
