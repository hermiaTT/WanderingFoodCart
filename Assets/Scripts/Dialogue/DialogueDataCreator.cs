using UnityEngine;

[CreateAssetMenu(fileName = "Chapter1Dialogue", menuName = "Game/Chapter Dialogue")]
public class DialogueDataCreator : ScriptableObject
{
    public DialogueData CreateOpeningDialogue()
    {
        DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
        dialogue.type = DialogueData.DialogueType.Opening;
        
        dialogue.sections = new DialogueData.DialogueSection[]
        {
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "我妈硬是撇脱，留张条条说'18岁该出去见世面咯'，转身就把我爸勒个古董三轮过户给我咯——您倒是留点启动资金嘛！",
                    "勒个破车比我年纪都大，链条比村头老黄狗叫得还响...",
                    "哎哟喂！那边勒个大爷看倒点路嘛！",
                    "算咯算咯，第一站就去成都，我爸勒个旅行笔记上说那儿勒辣椒巴适得很，治低血压——",
                    "要遭！勒个刹车硬是摆设嗦！！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "系统提示",
                sentences = new string[]
                {
                    "正在前往成都...（车费-200元）"
                },
                triggerSceneChange = true,
                nextScene = SceneManager.GameScene.BusToChengdu
            }
        };
        
        Debug.Log($"Created opening dialogue with {dialogue.sections.Length} sections");
        Debug.Log($"Second section triggerSceneChange = {dialogue.sections[1].triggerSceneChange}");
        
        return dialogue;
    }

    public DialogueData CreateBusToChengduDialogue()
    {
        DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
        dialogue.type = DialogueData.DialogueType.BusToChengdu;
        
        dialogue.sections = new DialogueData.DialogueSection[]
        {
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "哎呀，勒个成都硬是大得很嘛！",
                    "乖乖，勒些房子咋个修勒么高哦，不怕倒咯嗦？",
                    "咦？勒个地下铁是啥子名堂嘛，钻地洞勒火车？",
                    "糟咯糟咯，我勒个乡坝头口音，等哈儿咋个跟成都人摆龙门阵嘛..."
                },
                triggerSceneChange = true,
                nextScene = SceneManager.GameScene.Kuanzhai
            }
        };
        
        return dialogue;
    }

    public DialogueData CreateKuanzhaiDialogue()
    {
        DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
        dialogue.type = DialogueData.DialogueType.WideAlley;
        
        dialogue.sections = new DialogueData.DialogueSection[]
        {
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "来来来，自贡特色小吃咯！",
                    "凉面凉皮凉粉凉糕，样样都凉！",
                    "呃...好像不太对...",
                    "麻辣鲜香，巴适得很！",
                    "咋个都没得人来看一眼嘛..."
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "一位白发老头正坐在竹椅上晒太阳，慢悠悠地品着盖碗茶，时不时瞥一眼主角的摊位。",
                    "半小时后...",
                    "主角蹲在摊位后面，无聊地数着地上的蚂蚁。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "小伙子，你勒个吆喝方式要不得哦！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "大爷，您要吃点啥子嘛？"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "我不是来吃东西勒，我是看你勒个样子着急！",
                    "你晓得这是啥子地方不？宽窄巷子！成都勒美食名片！你勒个'自贡特色'，哪个成都人买账嘛！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "那...那咋个办嘛..."
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "你要学会入乡随俗！",
                    "看到没得？正宗郫县豆瓣酱！有了勒个，你勒菜才叫正宗川菜！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "但是...我不会用勒个做菜啊..."
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "哎呀，简单得很！来来来，我教你做麻婆豆腐，保证巴适！",
                    "（小声）顺便把勒罐豆瓣酱卖给你..."
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "嘿嘿，遇到贵人咯！",
                    "不过...勒个大爷该不会是卖豆瓣酱勒托儿嘛？",
                    "管他勒，反正我勒生意已经差到不能再差咯..."
                },
                triggerSceneChange = true,
                nextScene = SceneManager.GameScene.CookingSchool
            }
        };
        
        return dialogue;
    }

    public DialogueData CreateCookingSchoolDialogue()
    {
        DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
        dialogue.type = DialogueData.DialogueType.CookingSchool;
        
        dialogue.sections = new DialogueData.DialogueSection[]
        {
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "穿过弯弯曲曲的小巷，来到一家挂着《百年老豆瓣》招牌的老店。门口摆着几十个晒酱的大缸，空气中弥漫着浓郁的酱香。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "看到没得？勒些都是我勒宝贝！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "老头拍着一个酱缸，满脸自豪。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "勒缸晒了五年咯，里头勒豆瓣酱比黄金还贵！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "五年？！大爷您勒豆瓣酱是拿来收藏勒嘛？"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "你懂啥子！好勒豆瓣酱就跟好酒一样，越陈越香！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "老头突然压低声音。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "不过今天教你做菜，用普通勒就行咯...",
                    "做麻婆豆腐，讲究'麻、辣、烫、香、酥、嫩、活'七个字！",
                    "首先，豆腐要切得方方正正，像麻将块一样！",
                    "豆瓣酱要宰的细细的，这样才阔以更好的释放味道！",
                    "先这样&……%%%，再这样&6%%￥￥……"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "老头一边说，主角一边备菜，手起刀落十分熟练。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "（没想到这娃儿基本功这么好）"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "嗯。。你勒个刀工还要多练练！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "老头递过干辣椒和花椒。"
                },
                triggerGameplay = true,
                gameplayType = "CookingTutorial"
            }
        };
        
        Debug.Log($"Created cooking school dialogue with {dialogue.sections.Length} sections");
        return dialogue;
    }

    public DialogueData CreateFirstSuccessDialogue()
    {
        DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
        dialogue.type = DialogueData.DialogueType.FirstSuccess;
        
        dialogue.sections = new DialogueData.DialogueSection[]
        {
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "马马虎虎，勉强能吃"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "不过对付游客应该够咯！勒罐豆瓣酱算你便宜点，200块！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "200？！大爷您勒豆瓣酱是金子做勒嘛？"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "你懂啥子！好勒豆瓣酱才能做出好菜！要不要随你！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "主角抱着豆瓣酱往回走。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "勒个大爷硬是会做生意..."
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "主角闻了闻豆瓣酱。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "不过确实香得很，希望勒个投资值得！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "完咯，还没学会咋个用勒个酱做其他菜..."
                },
                triggerSceneChange = false
            }
        };
        
        Debug.Log($"Created first success dialogue with {dialogue.sections.Length} sections");
        return dialogue;
    }

    public DialogueData CreateBusinessStartDialogue()
    {
        DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
        dialogue.type = DialogueData.DialogueType.Business;
        
        dialogue.sections = new DialogueData.DialogueSection[]
        {
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "好嘞，有了勒个豆瓣酱，我勒麻婆豆腐应该能卖得好！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "主角开始准备摊位，摆上了新做好的麻婆豆腐。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "来嘞来嘞，正宗川味麻婆豆腐，香得很嘞！"
                },
                triggerGameplay = true,
                gameplayType = "BusinessGame"
            }
        };
        
        Debug.Log($"Created business start dialogue with {dialogue.sections.Length} sections");
        return dialogue;
    }

    public DialogueData CreateBusinessEndDialogue()
    {
        DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
        dialogue.type = DialogueData.DialogueType.Business;
        
        dialogue.sections = new DialogueData.DialogueSection[]
        {
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "夕阳西下，主角正蹲在摊位旁数钱，突然被一道阴影笼罩。抬头看见老头抱着手臂站在三轮车前，背后晒酱的竹匾在晚风中咯吱作响。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "我说娃儿，你勒个麻婆豆腐——（拉长音）",
                    "花椒放得像撒化肥！豆腐嫩得像豆花！牛肉末少得要用放大镜找！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "大爷您莫乱说！我今天赚了七百五！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "主角掏出手机。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "看嘛，还有人夸我'麻得地道'！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "七百五？二十年前陈麻婆的店，一天要卖这个数——"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "三百份？"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "三千份！人家那豆腐，吃一口舌头要跳三天坝坝舞！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "可惜啊...现在再也吃不到咯..."
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "主角眼睛突然发亮，三轮车上的铜铃铛无风自动。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "大爷！陈麻婆勒店在哪？我马上骑车去学！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "急啥子！听说现在勒老板，做不出当年的味道..."
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "老头掏出发黄的地址条。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "去找现在勒老板，就说——（咳嗽）说你是王豆瓣叫你去的！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "王豆瓣？？"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "找到老板就晓得咯！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "系统提示",
                sentences = new string[]
                {
                    "获得任务《传说中的麻婆豆腐》"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "主角推着三轮车在夜色中前行。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "勒个老头神戳戳勒..."
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "主角看着手中的地址条。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "不过勒传说中的麻婆豆腐，听起来就巴适！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "主角突然急刹车。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "等一哈！我刚买的兔脑壳去哪了"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "巷子尽头，老头正翘着二郎腿啃着兔头。"
                },
                triggerSceneChange = false
            }
        };
        
        Debug.Log($"Created business end dialogue with {dialogue.sections.Length} sections");
        return dialogue;
    }

    public DialogueData CreateChenShopDialogue()
    {
        DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
        dialogue.type = DialogueData.DialogueType.ChenShop;
        
        dialogue.sections = new DialogueData.DialogueSection[]
        {
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "主角来到一家略显陈旧的小店，门口挂着《陈麻婆豆腐》的招牌，店内冷冷清清。老板正在擦拭一张泛黄的老照片。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老板",
                sentences = new string[]
                {
                    "你是王豆瓣介绍来的？"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "老板叹了口气。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老板",
                sentences = new string[]
                {
                    "我妈走后，我天天按她勒菜谱做，可老顾客都说不是那个味道..."
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "老板指着墙上褪色的奖状。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老板",
                sentences = new string[]
                {
                    "看嘛，'成都最巴适麻婆豆腐'，现在都没得人信咯。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "勒个是陈麻婆阿姨？看起来好年轻哦！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "诶？勒个男勒是哪个？"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老板",
                sentences = new string[]
                {
                    "是我爸...我从来没见过他。我妈说他当年不辞而别..."
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "老板拿出一本泛黄的菜谱。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老板",
                sentences = new string[]
                {
                    "你看，我妈在豆豉勒地方画了个叉，说再也不用咯。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "诶？勒个豆豉勒配方咋个是空白勒？"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老板",
                sentences = new string[]
                {
                    "听说是我爸特制勒，我妈一直没写下来..."
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "老板突然眼睛一亮。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老板",
                sentences = new string[]
                {
                    "诶！你既然是王豆瓣介绍来勒，要不要帮我找找勒个配方？",
                    "我可以付钱！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "钱就不必咯！不过..."
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "主角的眼睛瞄向厨房。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "要是找到配方，能不能教我正宗勒做法？"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老板",
                sentences = new string[]
                {
                    "要得！你要是真能找到，我把我妈勒独门秘方都教你！"
                },
                triggerSceneChange = false,
                triggerGameplay = true,
                gameplayType = "FindRecipe"  // 触发寻找配方的游戏玩法
            }
        };
        
        Debug.Log($"Created Chen shop dialogue with {dialogue.sections.Length} sections");
        return dialogue;
    }

    public DialogueData CreateReunionDialogue()
    {
        DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
        dialogue.type = DialogueData.DialogueType.Truth;
        
        dialogue.sections = new DialogueData.DialogueSection[]
        {
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "父子相拥而泣。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "老板用新配方做出麻婆豆腐。",
                    "老顾客们闻香而来，店里重现往日热闹。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "勒才叫真正的麻婆豆腐嘛！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "等一哈...王豆瓣该不会就是..."
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "老头在店外偷看，抹了抹眼泪，转身消失在巷子深处。他腰间别着的豆瓣酱葫芦上，隐约可见《王》字。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "系统提示",
                sentences = new string[]
                {
                    "解锁S级菜谱：传说中的麻婆豆腐"
                },
                triggerSceneChange = false
            }
        };
        
        Debug.Log($"Created reunion dialogue with {dialogue.sections.Length} sections");
        return dialogue;
    }

    public DialogueData CreateNewJourneyDialogue()
    {
        DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
        dialogue.type = DialogueData.DialogueType.NewJourney;
        
        dialogue.sections = new DialogueData.DialogueSection[]
        {
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "主角的餐车挂着《正宗陈麻婆豆腐》的招牌，排队的人群拐了好几个弯。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "来来来，正宗麻婆豆腐！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "主角熟练地翻炒。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "麻辣鲜香，巴适得板！",
                    "莫急莫急，人人有份！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "客人A",
                sentences = new string[]
                {
                    "爽！舌头都要跳起来咯！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "客人B",
                sentences = new string[]
                {
                    "传说中的味道！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "外国游客",
                sentences = new string[]
                {
                    "Spicy! Amazing!"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "娃儿，手艺见长啊！"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "老头神秘地说道。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "想不想学更霸道勒菜？"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "主角",
                sentences = new string[]
                {
                    "先说好，这次要好多钱？"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "不要钱，只要你..."
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "旁白",
                sentences = new string[]
                {
                    "老头掏出一张车票。"
                },
                triggerSceneChange = false
            },
            new DialogueData.DialogueSection
            {
                speakerName = "老头",
                sentences = new string[]
                {
                    "去广东！"
                },
                triggerSceneChange = false
            }
        };
        
        Debug.Log($"Created new journey dialogue with {dialogue.sections.Length} sections");
        return dialogue;
    }
} 