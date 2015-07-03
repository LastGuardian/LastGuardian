//------------------------------------------------------------------------------------------------------------------------------------------------------------
// 支持中文注解
// 初次编码时间：2015年04月17日00:21:33
// 最新修改时间：2015年04月17日00:21:31
// 作者：朱鹰仁
// 修改人员：朱鹰仁
// 脚本概要：限制相机跟随父物体旋转
//------------------------------------------------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/PlayerCam")]
public class PlayerCam : MonoBehaviour
{
	Transform m_transform;
	public Transform lookPoint;
	public float m_fMoveSpeed = 2;
	public float m_fDistance = 5;

	// Use this for initialization
	void Start ()
	{
		m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 tempPlayerPos = Vector3.Lerp(m_transform.position, lookPoint.position, m_fMoveSpeed * Time.deltaTime);
		Vector3 tempPos = Vector3.zero;

		tempPos.x = tempPlayerPos.x;
		tempPos.y = m_transform.position.y;
		tempPos.z = lookPoint.position.z - m_fDistance;
		m_transform.position = tempPos;

	}
}
