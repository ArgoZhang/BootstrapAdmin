## 提交模板配置

### Windows Fork 

Windows 版本的 Fork 提供了提交模板 （commit message template）功能，配置步骤如下

#### git 配置文件

1. 拷贝仓库 `scripts\git\commit_msg_template.txt` 文件到当前用户根目录下 `C:\Users\[用户名]\.commit_msg_template.txt`
2. 配置 git 全局配置文件 `C:\Users\[用户名]\.gitconfig` (此文件为隐藏文件)
3. 更新 commit 配置项

```log
[commit]
    template = /Users/argo/.commit_msg_template.txt
```

注意原始文件不是 . 开头拷贝到跟目录下为 . 开头文件名（点号开头文件默认为隐藏文件）

#### Fork 配置步骤

1. 打开要配置的仓库
2. 点击菜单栏第二个 **仓库** 菜单（Repository）
3. 下拉菜单中选中最后一个菜单项 **仓库设置** 子菜单（Settings for this repository）

如下图所示  
![输入图片说明](https://images.gitee.com/uploads/images/2020/0327/123310_1b9b4af3_554725.png "Screen Shot 2020-03-27 at 12.30.38.png")

4. 切换到 **提交模板** 面板（Commit Template）
5. 勾选使用 **全局配置文件** （Use global git configuration file）

下面的文本框内即出现提交模板内容
