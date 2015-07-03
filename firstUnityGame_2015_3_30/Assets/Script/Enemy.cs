//------------------------------------------------------------------------------------------------------------------------------------------------------------
// 支持中文注解
// 初次编码时间：2015年04月15日20:20:23
// 最新修改时间：2015年04月15日20:20:27
// 作者：朱鹰仁
// 修改人员：朱鹰仁
// 脚本概要：敌人设置
//------------------------------------------------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/Enemy")]
public class Enemy : MonoBehaviour
{
	public static Enemy	Instance;
	public float	m_fMoveSpeed = 5.0f;		// 移动速度
	public int		m_nLife = 15;				// 生命值
	public float	m_fDisToMagic = 10;			// 与魔法阵的距离

	Transform			m_transform;			// Transform组件
	CharacterController	m_chControll;			// 角色控制器
	Base				m_base;					// 基地组件
	Player				m_player;				// 玩家
	SkinnedMeshRenderer	skinnedMeshRenderer;

	NavMeshAgent	m_navMeshAgent;				// 寻路组件

	float			m_fRotSpeed = 60;			// 角色旋转速度 用于控制旋转速度，当敌人进攻玩家时，将始终旋转到面向玩家的角度
	float			m_fTimer = 2;				// 计时器 计算时间间隔 用于定时更新寻路等
	float			m_fAttackTimer = 2;			// （暂定）攻击时间间隔
	float			m_fGravity = 2.0f;			// 重力值

	bool			m_bIsAttack = false;		// 攻击开关
	bool			m_bIsBeTarge = false;		// 是否成为目标


	protected EnemySpawn m_spawn;				// 生成点

	public void Init(EnemySpawn spawn)
	{
		m_spawn = spawn;
		// 计算敌人数量
		m_spawn.m_nEnemyCount++;
	}

	void Awake()
	{
		skinnedMeshRenderer = this.GetComponentInChildren<SkinnedMeshRenderer> ();
//		skinnedMeshRenderer = this.GetComponent<SkinnedMeshRenderer> ();
	}

	// Use this for initialization
	void Start ()
	{
		// 获取组件
		m_transform = this.transform;
		// 获取导航组件
		m_navMeshAgent = this.GetComponent<NavMeshAgent>();
		//
		m_chControll = this.GetComponent<CharacterController>();
		// 获取玩家
		m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		// 获取基地位置
		m_base = GameObject.FindGameObjectWithTag("Base").GetComponent<Base>();
	}
	
	// Update is called once per frame
	void Update ()
	{
//		Debug.Log("Time in Updata" + Time.time);
		// 如果基地空血则返回
		if(m_base.m_nLife <= 0)return;
		// 待机一定时间
		m_fTimer -= Time.deltaTime;
		if(m_fTimer > 0)	return;
		// 当待机时间超过时，如果距离基地1.5米以内，则进入攻击状态
		// Vector3.Distance函数可以计算两个坐标点之间的距离
		if(Vector3.Distance(m_transform.position, m_base.m_transform.position) < 15)
		{
			m_bIsAttack = true;
		}
		else
		{
			m_bIsAttack = false;
			m_fTimer = 1;
			// 设置寻路目标
			m_navMeshAgent.SetDestination(m_base.m_transform.position);
			//m_navMeshAgent.destination = m_player.m_transform.position;
			// 追向玩家
			//MoveTo();
		}

		if(m_bIsAttack)
		{
			// 面向玩家
			RotateTo();

			m_fAttackTimer -= Time.deltaTime;
			if(m_fAttackTimer <= 0)
			{
				m_fAttackTimer = 2;
				m_base.SetDamage(1);
			}
		}

		ColorChange();
	}

	public void MoveTo()
	{
//		float fSpeed = m_fMoveSpeed * Time.deltaTime;
		float fYMove = 0;
		// 重力运动
		fYMove -= m_fGravity * Time.deltaTime;
		// 沿着世界的Z轴移动
		m_navMeshAgent.Move (m_transform.TransformDirection (new Vector3(0, 0, m_fMoveSpeed * Time.deltaTime)));
		//m_navMeshAgent.stoppingDistance = 5.0f;
		m_chControll.Move(m_transform.TransformDirection(new Vector3(0, fYMove, 0)));
	}

	// 转向目标点
	void RotateTo()
	{
		// 当前角度
		Vector3 oldAngle = m_transform.eulerAngles;
		
		// 获得面向玩家的角度
		m_transform.LookAt(m_base.m_transform);
		float fTarget = m_transform.eulerAngles.y;
		
		// 转向基地
		float fSpeed = m_fRotSpeed * Time.deltaTime;
		// MoveTowardsAngle函数作用，基于旋转速度，计算出当前角度转向目标角度的旋转角度
		// 转向只需要在Y轴上转动即可
		float fAngle = Mathf.MoveTowardsAngle(	oldAngle.y, 		// 当前角度
												fTarget, 			// 目标角度
												fSpeed);			// 旋转速度
		m_transform.eulerAngles = new Vector3(0, fAngle, 0);
	}

	// 颜色改变
	void ColorChange()
	{
//		Debug.Log("在循环开头");
//		GameObject obj = GameObject.FindGameObjectWithTag("MagicRange");
//		if(obj != null)
//		{
//			if(Vector3.Distance(obj.transform.position, m_transform.position) < m_fDisToMagic)
//			{
//				Debug.Log("设置边缘材质-未判断为空");
//				if(this.skinnedMeshRenderer == null) return;
//				Debug.Log("设置边缘材质-有判断");
//				// 设置边缘材质
//				MaterialController.SetOutlineMaterial(skinnedMeshRenderer, Random.Range(1, 4));
//			}
//			else
//			{
//				if(this.skinnedMeshRenderer == null) return;
//				Debug.Log("取消设置边缘材质");
//				// 取消设置边缘材质
//				MaterialController.CancelSetOutlineMaterial(skinnedMeshRenderer);
//			}
//		}

		
		// 采用触发器判断是否在魔法阵范围效果不好，除了反映过慢以外在一段时间之后就会失去判断
//		if(m_bIsBeTarge) 
//		{
//			if(skinnedMeshRenderer == null) return;
//			// 设置边缘材质
//			Debug.Log("设置边缘材质-有判断");
//			MaterialController.SetOutlineMaterial(skinnedMeshRenderer, Random.Range(1, 4));
//		}
//		else
//		{
//			if(skinnedMeshRenderer == null) return;
//			// 取消设置边缘材质
//			Debug.Log("取消设置边缘材质");
//			MaterialController.CancelSetOutlineMaterial(skinnedMeshRenderer);
//		}
	}


	// 此函数只有在碰撞体互相接触时才会被触发
	void OnTriggerEnter(Collider other)
	{
		// 如果在AOE范围中，则改变颜色
		if(other.tag.CompareTo ("MagicRange") == 0)
		{
			Debug.Log("在范围中");
			m_bIsBeTarge = true;

			if(skinnedMeshRenderer == null) return;
			// 设置边缘材质
			Debug.Log("设置边缘材质-有判断");
			MaterialController.SetOutlineMaterial(skinnedMeshRenderer, Random.Range(1, 4));
		}
	}
	void OnTriggerExit(Collider other)
	{
		// 如果在AOE范围中，则改变颜色
		if(other.tag.CompareTo ("MagicRange") == 0)
		{
			Debug.Log("离开范围");
			m_bIsBeTarge = false;

			if(skinnedMeshRenderer == null) return;
			// 取消设置边缘材质
			Debug.Log("取消设置边缘材质");
			MaterialController.CancelSetOutlineMaterial(skinnedMeshRenderer);
		}
	}
}
