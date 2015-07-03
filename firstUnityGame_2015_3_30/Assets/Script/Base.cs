//------------------------------------------------------------------------------------------------------------------------------------------------------------
// 支持中文注解
// 初次编码时间：2015年03月30日16:06:08
// 最新修改时间：2015年03月30日16:06:08
// 作者：朱鹰仁
// 修改人员：朱鹰仁
// 脚本概要：基地设置
//------------------------------------------------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/Base")]
public class Base : MonoBehaviour
{
	public Transform m_transform;
	public int m_nLife = 100;
	// Use this for initialization
	void Start ()
	{
		m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	// 更新生命值
	public void SetDamage(int nDamage)
	{
		m_nLife -= nDamage;
	}
}
