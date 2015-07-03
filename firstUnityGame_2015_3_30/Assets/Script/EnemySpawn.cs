//------------------------------------------------------------------------------------------------------------------------------------------------------------
// 支持中文注解
// 初次编码时间：
// 最新修改时间：
// 作者：朱鹰仁
// 修改人员：朱鹰仁
// 脚本概要：
//------------------------------------------------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/EnemySpawn")]
public class EnemySpawn : MonoBehaviour
{
	// 敌人的Prefab
	public Transform m_enemy;
	// 生成的敌人数量
	public int m_nEnemyCount = 0;
	// 生成的最大敌人数量
	public int m_nMaxEnemyNum = 3;
	// 生成敌人的时间间隔
	public float m_fTimer = 0;
	
	protected Transform m_transform;
	// Use this for initialization
	void Start ()
	{
		m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// 如果生成的敌人数量达到最大值，则停止生成敌人
		if(m_nEnemyCount >= m_nMaxEnemyNum)	return;
		m_fTimer -= Time.deltaTime;
		if(m_fTimer <= 0)
		{
			m_fTimer = Random.value * 10.0f + 5.0f;
			// 生成敌人
			Transform obj = (Transform)Instantiate(m_enemy, m_transform.position, Quaternion.identity);
			// 获取敌人脚本
			Enemy enemy = obj.GetComponent<Enemy>();

			// 初始化敌人
//			enemy.Init(this);
		}
	}
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(this.transform.position, "item.png", true);
	}
}
