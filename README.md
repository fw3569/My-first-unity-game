代码地址：https://github.com/fw3569/My-project.git

运行环境：Windows/键鼠

引擎：Unity6000.0.34f1

玩家人数：１人

操作说明：  
方向键↑↓←→：移动  
Shift：朝向锁定  
12345：道具  
QWERASDFZXCV：技能  
Enter：对话和确认  
Ecs：取消  
I：道具菜单  
K：技能菜单  
U：装备菜单  
P：保存菜单

制作团队：个人制作

特征：
1. 有限的视野  
  视野会被障碍物遮蔽。因此，在经过门等狭小场所时会感到紧张感和好奇心。在洞窟等地方会进一步限制可视距离，更有压迫感。通过限制玩家能看到的信息，使其难以把握自己的位置，在大绕圈之后回到原来位置时理解地图结构的顿悟感很有趣。有限视野也是2D平面上无加载的多层地图成为可能。缺点是由于全地图无法被同时看见，整体的设计感比较弱。
2. 爽快的战斗  
  大部分的技能的前后摇都很短。另外后摇是可部分取消的。相应的有设置技能冷却时间。玩家连续快速输入不同的多个按键，形成爽快的战斗体验和操作手感。
3. 鼓励积极进攻的眩晕机制  
  流行的处决机制中，处决动画前后无交互的时间太长，会打断游戏节奏导致玩家情绪下跌。我希望使用鼓励积极进攻的眩晕机制。攻击眩晕状态的敌人会进一步延长眩晕时间，以鼓励玩家不断进攻。

代码说明：
代码都在".\Assets\MyAssets"目录下。基本上从GameManager.cs（菜单控制、全局数据、保存）、Creature.cs、Player.cs、Item.cs、AttackBehaviour.cs开始看好。BackgroundControl.cs是玩家当前所属区域的更新。ItemManager.cs是拥有道具的更新和道具预制件的实例管理。EnvMaskShader.shader是随光强度变透明的材质。为了能使Player流畅的运动、可以看下Animator Controller中的设定。其他就没有了。

游玩录像：
https://drive.google.com/file/d/1lGRsrOR51Qp3196lhv7DVh9B4rfYBzGg/view?usp=sharing
