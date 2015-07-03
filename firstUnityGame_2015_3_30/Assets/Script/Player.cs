//------------------------------------------------------------------------------------------------------------------------------------------------------------
// 支持中文注解
// 初次编码时间：2015年03月30日10:07:29
// 最新修改时间：2015年03月30日10:07:29
// 作者：朱鹰仁
// 修改人员：朱鹰仁
// 脚本概要：玩家设置
//------------------------------------------------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/Player")]
public class Player : MonoBehaviour
{
	public Transform m_transform;
	public Animator m_anim;									// 获取要控制的人物
	public Transform m_bullet;								// 子弹的Prefab
	public Transform m_magicRange;							// 魔法范围提示
	public Transform m_firePoint;							// 射击点
	public GameObject m_magicAE;							// 魔法效果
	public GameObject m_telesportAE;						// 瞬移后效果
	public GameObject m_weapon;								// 获取武器

	public string m_nPlayerID = "999";						// 玩家ID，用于对生成子弹编号
	public int m_nLife = 5;									// 玩家生命值
	public float m_fMoveSpeed = 3.0f;						// 角色水平移动速度
	public float m_fRotSpeed = 120.0f;						// 角色转向速度
	public float m_CD = 0;									// 攻击时间间隔
	public float m_fTelesportDis = 5;						// 瞬间移动距离
	protected float m_fFireSpeed = 0;						// 攻击时间间隔
	public TrailRenderer m_TrEF;							// 获得轨迹线
	public float m_fMesHeiOff = 1.0f;						// 视角非平视，实际对话框高度需要一定量的偏移
	Transform m_magicMap;									// 所生成的魔法阵图形

	bool m_bIsRot = true;									// 是否转向
	bool m_bIsMagicOn = false;								// 是否释放魔法
	bool m_bIsTelesport = false;							// 是否瞬间移动

	float m_fPlayerHeight;									// 玩家高度
	string m_message;										// 对话框信息

	SkinnedMeshRenderer skinnedMeshRenderer;
	void Awake()
	{
		skinnedMeshRenderer = this.GetComponentInChildren<SkinnedMeshRenderer> ();
	}


	// Use this for initialization
	void Start ()
	{
		// 获取组件
		m_transform = this.transform;
//		m_txt_life = this.transform.FindChild("txt_life").GetComponent<Text>();

		//得到模型原始高度
		float fSizeY = collider.bounds.size.y;
		//得到模型缩放比例
		float fScalY = transform.localScale.y;
		//它们的乘积就是高度
		m_fPlayerHeight = (fSizeY *fScalY) ;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(m_nLife <= 0)
			return;
		// 获取当前动作状态
		AnimatorStateInfo stateInfo = m_anim.GetCurrentAnimatorStateInfo(0);

//		if(m_anim.animation["Attack"].time >= 0.5f)
//		{
//			Weapon_Sword.m_bIsAttack = true;
//		}
//		if(m_anim.animation["Attack"].time >= 1.0f)
//		{
//			Weapon_Sword.m_bIsAttack = false;
//		}
		// 如果攻击动作结束，则关闭武器的碰撞判断
		if(	stateInfo.nameHash != Animator.StringToHash("Base Layer.Attack"))
			Weapon_Sword.m_bIsAttack = false;


		GetButton();
		GetAxis();
		// 轨迹动画的时间以动作curve中设定的关键帧为主
		m_TrEF.time = m_anim.GetFloat("TrailEF");
	}


	// 获得轴操作信息
	public void GetAxis()
	{
		// 无操作时动作归位
		m_anim.SetBool("Run", false);

		// 如果魔法阵展开，则玩家不允许移动
		if(!m_bIsMagicOn)
		{
			// 左边操纵杆左右
			if(Input.GetAxisRaw("X_axis") > 0.3 || Input.GetAxisRaw("X_axis") < -0.3)
			{
				m_anim.SetBool("Run", true);

				if(Input.GetAxisRaw("X_axis") > 0.3)
				{
					if(m_bIsRot)
					{
						// 角度旋转设定
						if(m_transform.eulerAngles.y >= 90 && m_transform.eulerAngles.y < 270)
						{
							m_transform.Rotate(new Vector3(0, +m_fRotSpeed * Time.deltaTime, 0), Space.Self);
							// 控制角度溢出
							if(m_transform.eulerAngles.y > 270)
								m_transform.eulerAngles = new Vector3(0, 270, 0);
						}
						if( (m_transform.eulerAngles.y < 90 && m_transform.eulerAngles.y >= 0) ||
							(m_transform.eulerAngles.y <= 360 && m_transform.eulerAngles.y > 270))
						{
							m_transform.Rotate(new Vector3(0, -m_fRotSpeed * Time.deltaTime, 0), Space.Self);
							// 控制角度溢出
							if(m_transform.eulerAngles.y < 270 && m_transform.eulerAngles.y > 180)
								m_transform.eulerAngles = new Vector3(0, 270, 0);
						}
					}

					// 位移设置
					m_transform.Translate (new Vector3(-m_fMoveSpeed * Time.deltaTime, 0, 0), Space.World);
					Debug.Log("左边操纵杆左");
				}
				else
				{
					if(m_bIsRot)
					{
						// 角度旋转设定
						if( (m_transform.eulerAngles.y >= 270 && m_transform.eulerAngles.y <= 360) || 
							(m_transform.eulerAngles.y >= 0 && m_transform.eulerAngles.y < 90))
						{
							m_transform.Rotate(new Vector3(0, +m_fRotSpeed * Time.deltaTime, 0), Space.Self);
							// 控制角度溢出
							if(m_transform.eulerAngles.y > 90 && m_transform.eulerAngles.y < 180)
								m_transform.eulerAngles = new Vector3(0, 90, 0);
						}
						if(m_transform.eulerAngles.y < 270 && m_transform.eulerAngles.y > 90)
						{
							m_transform.Rotate(new Vector3(0, -m_fRotSpeed * Time.deltaTime, 0), Space.Self);
							// 控制角度溢出
							if(m_transform.eulerAngles.y < 90)
								m_transform.eulerAngles = new Vector3(0, 90, 0);
						}
					}

					// 位移设置
					m_transform.Translate (new Vector3(m_fMoveSpeed * Time.deltaTime, 0, 0), Space.World);
					Debug.Log("左边操纵杆右");
				}
			}
			// 左边操纵杆上下
			if(Input.GetAxisRaw("Y_axis") > 0.3 || Input.GetAxisRaw("Y_axis") < -0.3)
			{
				m_anim.SetBool("Run", true);
				if(Input.GetAxisRaw("Y_axis") > 0.3)
				{
					if(m_bIsRot)
					{
						// 角度旋转设定
						if(m_transform.eulerAngles.y >= 180 && m_transform.eulerAngles.y < 360)
						{
							m_transform.Rotate(new Vector3(0, +m_fRotSpeed * Time.deltaTime, 0), Space.Self);
							// 控制角度溢出
							if(m_transform.eulerAngles.y > 360)
								m_transform.eulerAngles = new Vector3(0, 360, 0);
						}
						if(m_transform.eulerAngles.y < 180 && m_transform.eulerAngles.y > 0)
						{
							m_transform.Rotate(new Vector3(0, -m_fRotSpeed * Time.deltaTime, 0), Space.Self);
							// 控制角度溢出
							if(m_transform.eulerAngles.y < 0)
								m_transform.eulerAngles = new Vector3(0, 0, 0);
						}
					}

					// 位移设置
					m_transform.Translate (new Vector3(0, 0, m_fMoveSpeed * Time.deltaTime), Space.World);
					Debug.Log("左边操纵杆上");
				}
				else if(Input.GetAxisRaw("Y_axis") < 0.3)
				{
					if(m_bIsRot)
					{
						// 角度旋转设定
						if(m_transform.eulerAngles.y <= 180 && m_transform.eulerAngles.y >0)
						{
							m_transform.Rotate(new Vector3(0, +m_fRotSpeed * Time.deltaTime, 0));
							if(m_transform.eulerAngles.y > 180)
								m_transform.eulerAngles = new Vector3(0, 180, 0);
						}
						else if(m_transform.eulerAngles.y > 180 && m_transform.eulerAngles.y < 360)
						{
							m_transform.Rotate(new Vector3(0, -m_fRotSpeed * Time.deltaTime, 0));
							if(m_transform.eulerAngles.y < 180)
								m_transform.eulerAngles = new Vector3(0, 180, 0);
						}
					}

					// 位移设置
					m_transform.Translate (new Vector3(0, 0, -m_fMoveSpeed * Time.deltaTime), Space.World);
					Debug.Log("左边操纵杆下");
				}
			}
		}
		else
		{
			// 左边操纵杆左右
			if(Input.GetAxisRaw("X_axis") > 0.3 || Input.GetAxisRaw("X_axis") < -0.3)
			{
				if(Input.GetAxisRaw("X_axis") > 0.3)
				{
					m_magicMap.transform.Translate(new Vector3(-m_fMoveSpeed * Time.deltaTime, 0, 0), Space.World);
				}
				else
				{
					m_magicMap.transform.Translate(new Vector3(m_fMoveSpeed * Time.deltaTime, 0, 0), Space.World);
				}
			}
			// 左边操纵杆上下
			if(Input.GetAxisRaw("Y_axis") > 0.3 || Input.GetAxisRaw("Y_axis") < -0.3)
			{
				if(Input.GetAxisRaw("Y_axis") > 0.3)
				{
					m_magicMap.transform.Translate(new Vector3(0, 0, m_fMoveSpeed * Time.deltaTime), Space.World);
				}
				else
				{
					m_magicMap.transform.Translate(new Vector3(0, 0, -m_fMoveSpeed * Time.deltaTime), Space.World);
				}
			}
		}

		// 斜方向的处理必须优先与正方向，因为斜方向的其中一个条件成立都可以促使正方向条件成立。
		// 右边操纵杆斜方向【左上，左下】
		if(Input.GetAxisRaw("3rd_axis") > 0.2 && (Input.GetAxisRaw("4th_axis") > 0.2 || Input.GetAxisRaw("4th_axis") < -0.2))
		{
			if(m_bIsTelesport)
			{
				if(Input.GetAxisRaw("4th_axis") > 0.2)
				{
					m_transform.Translate(new Vector3(-m_fTelesportDis, 0, m_fTelesportDis), Space.World);
					m_bIsTelesport = false;
					Debug.Log("右边操纵杆【左上】");
				}
				else
				{
					m_transform.Translate(new Vector3(-m_fTelesportDis, 0, -m_fTelesportDis), Space.World);
					m_bIsTelesport = false;
					Debug.Log("右边操纵杆【左下】");
				}
				Instantiate(m_telesportAE, m_transform.position, m_telesportAE.transform.rotation);
			}
		}
		// 右边操纵杆斜方向【右上，右下】
		if(Input.GetAxisRaw("3rd_axis") < -0.2 && (Input.GetAxisRaw("4th_axis") > 0.2 || Input.GetAxisRaw("4th_axis") < -0.2))
		{
			if(m_bIsTelesport)
			{
				if(Input.GetAxisRaw("4th_axis") > 0.2)
				{
					m_transform.Translate(new Vector3(m_fTelesportDis, 0, m_fTelesportDis), Space.World);
					m_bIsTelesport = false;
					Debug.Log("右边操纵杆【右上】");
				}
				else
				{
					m_transform.Translate(new Vector3(m_fTelesportDis, 0, -m_fTelesportDis), Space.World);
					m_bIsTelesport = false;
					Debug.Log("右边操纵杆【右下】");
				}
				Instantiate(m_telesportAE, m_transform.position, m_telesportAE.transform.rotation);
			}
		}


		// 右边操纵杆左右
		if(Input.GetAxisRaw("3rd_axis") > 0.3 || Input.GetAxisRaw("3rd_axis") < -0.3)
		{
			// 如果当前状态允许移动闪避
			if(m_bIsTelesport)
			{
				if(Input.GetAxisRaw("3rd_axis") > 0.3)
				{
					m_transform.Translate(new Vector3(-m_fTelesportDis, 0, 0), Space.World);
					m_bIsTelesport = false;
					Debug.Log("右边操纵杆左");
				}
				else
				{
					m_transform.Translate(new Vector3(m_fTelesportDis, 0, 0), Space.World);
					m_bIsTelesport = false;
					Debug.Log("右边操纵杆右");
				}
				Instantiate(m_telesportAE, m_transform.position, m_telesportAE.transform.rotation);
			}
		}
		// 右边操纵杆上下
		if(Input.GetAxisRaw("4th_axis") > 0.3 || Input.GetAxisRaw("4th_axis") < -0.3)
		{
			// 如果当前状态允许移动闪避
			if(m_bIsTelesport)
			{
				if(Input.GetAxisRaw("4th_axis") > 0.3)
				{
					m_transform.Translate(new Vector3(0, 0, m_fTelesportDis), Space.World);
					m_bIsTelesport = false;
					Debug.Log("右边操纵杆上");
				}
				else
				{
					m_transform.Translate(new Vector3(0, 0, -m_fTelesportDis), Space.World);
					m_bIsTelesport = false;
					Debug.Log("右边操纵杆下");
				}
				Instantiate(m_telesportAE, m_transform.position, m_telesportAE.transform.rotation);
			}
		}



		if(!(Input.GetAxisRaw("3rd_axis") > 0.3 || Input.GetAxisRaw("3rd_axis") < -0.3) &&
			!(Input.GetAxisRaw("4th_axis") > 0.3 || Input.GetAxisRaw("4th_axis") < -0.3))
		{
			m_bIsTelesport = true;
		}



		// 左边十字方向键左右
		if(Input.GetAxisRaw("5th_axis") > 0.3 || Input.GetAxisRaw("5th_axis") < -0.3)
		{
			if(Input.GetAxisRaw("5th_axis") > 0.3)
				m_message = "跟我来~";
//				Debug.Log("跟我来~");
			else
				m_message = "赶紧回城！！";
//				Debug.Log("赶紧回城！！");
		}
		// 左边十字方向键上下
		if(Input.GetAxisRaw("6th_axis") > 0.3 || Input.GetAxisRaw("6th_axis") < -0.3)
		{
			if(Input.GetAxisRaw("6th_axis") > 0.3)
				m_message = "这边需要支援！！！";
//				Debug.Log("这边需要支援！！！");
			else
				m_message = "我去引怪！！！";
//				Debug.Log("我去引怪！！！");
		}
	}
	

	// 获得手柄按键信息
	public void GetButton()
	{
		// 无动作时初始化
		m_anim.SetBool("Attack", false);

		m_fFireSpeed -= Time.deltaTime;
		// 子弹发射
		if(Input.GetKey (KeyCode.Space))
		{
			if(m_fFireSpeed > 0)	return;
			m_fFireSpeed = m_CD;
			Transform obj = (Transform)Instantiate(m_bullet, m_firePoint.transform.position, m_firePoint.transform.rotation);
			// 获取敌人脚本
			Bullet bullet = obj.GetComponent<Bullet>();
			// 把玩家ID赋予每个生成子弹
			obj.GetComponent<Bullet>().SetName(m_nPlayerID);
		}

		// 右边【左】按键
		// 子弹发射
		if(Input.GetKey(KeyCode.JoystickButton0))
		{
			if(m_fFireSpeed > 0)	return;
			m_fFireSpeed = m_CD;
			Transform obj = (Transform)Instantiate(m_bullet, m_firePoint.transform.position, m_firePoint.transform.rotation);
			// 获取敌人脚本
			Bullet bullet = obj.GetComponent<Bullet>();

			Debug.Log("JoystickButton0");
		}
		// 右边【下】按键
		if(Input.GetKey(KeyCode.JoystickButton1))
		{
			// 近身攻击
			m_anim.SetBool("Attack", true);
//			Weapon_Sword.m_bIsAttack = true;
			Debug.Log("JoystickButton1");
		}
		// 右边【右】按键
		if(Input.GetKey(KeyCode.JoystickButton2))
		{
			Debug.Log("JoystickButton2");
		}
		if(!m_bIsMagicOn)
		{
			// 右边【上】按键
			if(Input.GetKeyUp(KeyCode.JoystickButton3))
			{
				// 魔法攻击范围提示
				Vector3 magPos = m_transform.position;
				magPos.y = m_transform.position.y + 0.1f;
				m_magicMap = (Transform)Instantiate(m_magicRange, magPos, m_magicRange.rotation);
				m_bIsMagicOn = true;
				Debug.Log("右边【上】按键");
			}
		}
		else
		{
			// 右边【上】按键
			if(Input.GetKeyUp(KeyCode.JoystickButton3))
			{
				m_bIsMagicOn = false;
				m_magicMap.transform.Translate(new Vector3(0, -50, 0), Space.World);

				Vector3 Pos = m_magicMap.transform.position;
				Pos.y = 0;
				Instantiate(m_magicAE, Pos, m_magicAE.transform.rotation);
			}
		}

		if(Input.GetKey(KeyCode.JoystickButton4))
		{
			m_transform.Rotate(new Vector3(0, -m_fRotSpeed * Time.deltaTime, 0));
			Debug.Log("L1");
		}
		if(Input.GetKey(KeyCode.JoystickButton5))
		{
			m_transform.Rotate(new Vector3(0, m_fRotSpeed * Time.deltaTime, 0));
			Debug.Log("R1");
		}
		if(Input.GetKey(KeyCode.JoystickButton6))
		{
			Debug.Log("L2");
		}

		if(Input.GetKey(KeyCode.JoystickButton7))
		{
			m_bIsRot = false;
			Debug.Log("R2");
		}
		else
			m_bIsRot = true;
	}


	void OnGUI()
	{
		// 得到玩家头顶在3D世界中的坐标
		// 默认玩家坐标点在脚底下，且视角非平视，实际对话框高度需要一定量的偏移，所以这里加上模型的高度和偏移量。
		Vector3 worldPos = new Vector3(m_transform.position.x, m_transform.position.y + m_fPlayerHeight + m_fMesHeiOff, m_transform.position.z);
		// 转换3D坐标到屏幕坐标
		GameObject obj = GameObject.Find("PlayerCam");
		Vector2 pos = obj.camera.WorldToScreenPoint(worldPos);
		// 2D坐标屏幕左下角的点为0.0 ，屏幕右上角的坐标为1.1 
		pos = new Vector2(pos.x, Screen.height - pos.y);

		// 计算NPC名称的宽高
		// 通过字符串来获取它整体的宽度与高度，因为信息是可变的，所以需要动态的获取整体的显示区域。
		Vector2 messageSize = GUI.skin.label.CalcSize (new GUIContent(m_message));
		// 设置显示颜色为黄色
		GUI.color  = Color.yellow;
		// 绘制NPC名称
		GUI.Label(new Rect(pos.x - (messageSize.x/2), pos.y - messageSize.y, messageSize.x, messageSize.y), m_message);
	}
}
