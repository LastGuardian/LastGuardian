//------------------------------------------------------------------------------------------------------------------------------------------------------------
// 支持中文注解
// 初次编码时间：2015年04月01日17:52:13
// 最新修改时间：2015年04月01日17:52:13
// 作者：朱鹰仁
// 修改人员：朱鹰仁
// 脚本概要：子弹
//------------------------------------------------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/Bullet")]
public class Bullet : MonoBehaviour
{
	protected Transform m_transform;

//	public Transform m_bullet;									// 子弹的Prefab
	public float m_fMoveSpeed = 20;								// 子弹的速度
	public GameObject m_attackAE;

	protected string m_NameFrom;
	// Use this for initialization
	void Start ()
	{
		// 获取组件
		m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		m_transform.Translate(new Vector3(0, 0, 3));
	}

	public void SetName(string name)
	{
		m_NameFrom = name;
	}
	public string GetName()
	{
		return m_NameFrom;
	}

	// 此函数只有在碰撞体互相接触时才会被触发
	void OnTriggerEnter(Collider other)
	{
		// 如果没有碰到敌人则不作处理
		// 标签比较，比较的两边都需要一个值
		// Is Trigger 需要打开
		if(other.tag.CompareTo ("Enemy") != 0) return;
		// 生成攻击特效
		Instantiate(m_attackAE, m_transform.position, Quaternion.identity);
		// 如果子弹碰到敌人则消失
		Destroy (this.gameObject);
	}
}
