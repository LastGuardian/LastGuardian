//------------------------------------------------------------------------------------------------------------------------------------------------------------
// 支持中文注解
// 初次编码时间：2015年04月03日18:37:39
// 最新修改时间：2015年04月03日18:37:39
// 作者：朱鹰仁
// 修改人员：朱鹰仁
// 脚本概要：特效时间间隔控制
//------------------------------------------------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/AttackEF")]
public class AttackEF : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void IsReadyToAttack()
	{
		Weapon_Sword.m_bIsAttack = true;
	}
}
