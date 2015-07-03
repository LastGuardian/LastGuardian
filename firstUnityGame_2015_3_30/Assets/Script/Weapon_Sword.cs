//------------------------------------------------------------------------------------------------------------------------------------------------------------
// 支持中文注解
// 初次编码时间：2015年04月13日19:27:36
// 最新修改时间：2015年04月13日19:27:39
// 作者：朱鹰仁
// 修改人员：朱鹰仁
// 脚本概要：武器-剑
//------------------------------------------------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/Weapon_Sword")]
public class Weapon_Sword : MonoBehaviour
{
	public Transform m_transform;
	public GameObject m_attackAE;

	public static bool m_bIsAttack = false;

	// Use this for initialization
	void Start ()
	{
		m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
	// 此函数只有在碰撞体互相接触时才会被触发
	void OnTriggerEnter(Collider other)
	{
		if(other.tag.CompareTo ("Enemy") != 0) 
			return;
		
		if(m_bIsAttack)
		{
			// 生成攻击特效
			Instantiate(m_attackAE, other.transform.position, Quaternion.identity);
			m_bIsAttack = false;
		}
	}
}
