# CustHintShow
CustHintShow(CHS)一个scp sl的消息显示api|╭(╯^╰)╮
# 使用
示例代码\n
```Csharp
var hint = new CustHintShow.Hints.Hint
{
    X = 960,
    Y = 600,
    Size = 20,
    Text = "欢迎游玩服务器"
};
var showplayer = CustHintShow.Display.Get(player);
showplayer.Add(hint);
```
#### 属性
| 属性 | 描述 |
| Y | 文本的垂直位置。Y 坐标越大，文本显示越靠下 |
| X | 文本的水平位置。X 坐标越大，文本显示越靠右 |
| Size | 文本的大小 |
| Text | 内容 |
 |
