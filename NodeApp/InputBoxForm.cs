// 引入MaterialSkin皮肤核心命名空间（管理皮肤样式）
using MaterialSkin;
// 引入MaterialSkin控件命名空间（MaterialForm、MaterialButton等）
using MaterialSkin.Controls;
// 引入系统核心命名空间（基础类型、异常、委托等）
using System;
// 引入绘图命名空间（颜色、字体、尺寸、位置等）
using System.Drawing;
// 引入WinForms基础控件命名空间（窗体、按钮、文本框等）
using System.Windows.Forms;

// 定义项目核心命名空间（与主窗体一致）
namespace NodeApp
{
    /// <summary>
    /// 自定义输入框窗体：继承自MaterialForm（MaterialSkin美化的窗体）
    /// 功能：弹出式输入框，支持中文输入、输入验证、动画效果、错误提示
    /// </summary>
    public partial class InputBoxForm : MaterialForm
    {
        /// <summary>
        /// 公共属性：获取用户输入的文本（只读，仅内部赋值）
        /// 供调用方获取最终验证通过的输入内容
        /// </summary>
        public string InputText { get; private set; }

      

        /// <summary>
        /// 输入验证委托：自定义验证逻辑
        /// 返回值：(是否有效, 错误提示信息)
        /// 供调用方传入自定义的验证规则（如文件名重复检查）
        /// </summary>
        public Func<string, (bool isValid, string errorMessage)> ValidationFunc { get; set; }

        /// <summary>
        /// 构造函数：初始化输入框窗体
        /// </summary>
        /// <param name="title">窗体标题</param>
        /// <param name="prompt">提示文本（可选，默认空）</param>
        public InputBoxForm(string title, string prompt = "")
        {
            // 初始化控件（设置窗体基础属性）
            InitializeComponents();
            // 配置MaterialSkin皮肤
            SetupMaterialSkin();

            // 窗体基础设置
            Text = title;                              // 设置窗体标题
            StartPosition = FormStartPosition.CenterParent; // 相对于主窗体居中显示
            FormBorderStyle = FormBorderStyle.FixedSingle;  // 固定边框（不可调整大小）
            MaximizeBox = false;                        // 禁用最大化按钮
            MinimizeBox = false;                        // 禁用最小化按钮
            Padding = new Padding(20);                  // 窗体内边距（避免控件贴边）

            // 创建UI元素（按顺序：提示标签→输入框→按钮面板）
            CreatePromptLabel(prompt);  // 创建提示文字标签
            CreateInputTextBox();       // 创建输入文本框（核心控件）
            CreateButtonPanel();        // 创建确定/取消按钮面板

            // 绑定窗体事件
            this.Load += OnFormLoad;    // 窗体加载事件（初始化透明度）
            this.Shown += OnFormShown;  // 窗体显示事件（淡入动画+聚焦输入框）
        }

        /// <summary>
        /// 配置MaterialSkin皮肤：启用皮肤样式
        /// </summary>
        private void SetupMaterialSkin()
        {
            // 获取MaterialSkin单例管理器
            var materialSkinManager = MaterialSkinManager.Instance;
            // 将当前窗体加入皮肤管理（应用Material样式）
            materialSkinManager.AddFormToManage(this);
            // 设置皮肤主题：浅色主题
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            //// 应用自定义颜色方案（暂时禁用）
            //materialSkinManager.ColorScheme = MainColorScheme;
        }

        // ===================== 控件字段声明（供类内所有方法访问） =====================
        private Label promptLabel;          // 提示文字标签
        private TextBox inputTextBox;       // 原生TextBox（支持中文输入法，替代MaterialTextBox）
        private MaterialButton okButton;    // 确定按钮（MaterialSkin样式）
        private MaterialButton cancelButton;// 取消按钮（MaterialSkin样式）
        private Label errorLabel;           // 错误提示标签

        /// <summary>
        /// 初始化控件基础属性：设置窗体大小，暂停/恢复布局
        /// 替代WinForms自动生成的InitializeComponent方法
        /// </summary>
        private void InitializeComponents()
        {
            // 暂停布局（批量设置属性，提升性能，避免闪烁）
            SuspendLayout();
            // 设置窗体客户端区域尺寸：宽450px，高350px
            ClientSize = new Size(450, 350);
            // 设置窗体名称（用于代码引用/调试）
            Name = "InputBoxForm";
            // 恢复布局（完成基础属性设置）
            ResumeLayout(false);
        }

        /// <summary>
        /// 创建提示文字标签：显示输入说明（如“请输入文件名”）
        /// </summary>
        /// <param name="text">提示文本内容</param>
        private void CreatePromptLabel(string text)
        {
            // 实例化标签控件并设置属性
            promptLabel = new Label
            {
                Text = text,                                      // 提示文本
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point), // 字体：Segoe UI 10号
                ForeColor = Color.FromArgb(66, 66, 66),           // 文字颜色：深灰色
                // 位置：水平居中（窗体宽度-标签宽度)/2，垂直居中偏上（窗体高度/2 -50）
                Location = new Point((this.ClientSize.Width - 340) / 2, (this.ClientSize.Height / 2) - 50),
                Size = new Size(340, 30),                         // 尺寸：宽340px，高30px
                TextAlign = ContentAlignment.MiddleCenter,        // 文字对齐：水平+垂直居中
                Visible = !string.IsNullOrEmpty(text)             // 可见性：文本非空时显示
            };
            // 将标签添加到窗体控件集合
            Controls.Add(promptLabel);
        }

        /// <summary>
        /// 创建输入文本框：核心输入控件，支持中文、占位提示、输入验证
        /// </summary>
        private void CreateInputTextBox()
        {
            // 计算输入框位置：水平居中，垂直居中偏下
            int inputX = (this.ClientSize.Width - 340) / 2;
            int inputY = this.ClientSize.Height / 2 - 20;

            // 实例化原生TextBox（解决MaterialTextBox中文输入法兼容问题）
            inputTextBox = new TextBox
            {
                Text = "请输入文件名",                             // 占位提示文字（初始显示）
                Location = new Point(inputX, inputY),              // 位置：水平居中，垂直居中偏下
                Size = new Size(340, 50),                          // 尺寸：宽340px，高50px
                MaxLength = 50,                                    // 最大输入长度：50个字符
                ImeMode = ImeMode.On,                              // 启用中文输入法（关键：解决中文输入问题）
                Font = new Font("微软雅黑", 10F),                  // 字体：微软雅黑10号（适配中文）
                BorderStyle = BorderStyle.FixedSingle,             // 边框样式：固定单边框
                Padding = new Padding(10),                         // 内边距：文字与边框间距10px
                BackColor = Color.White,                           // 背景色：白色
                ForeColor = Color.FromArgb(150, 150, 150)          // 占位文字颜色：浅灰色
            };

            // 绑定输入框获取焦点事件（Enter）：清空占位提示，修改样式
            inputTextBox.Enter += (s, e) =>
            {
                inputTextBox.BackColor = Color.FromArgb(245, 245, 245); // 背景色：浅灰色（聚焦状态）
                // 如果当前是占位提示文字，清空并恢复正常文字颜色
                if (inputTextBox.Text == "请输入文件名")
                {
                    inputTextBox.Text = "";                                // 清空文本
                    inputTextBox.ForeColor = Color.FromArgb(66, 66, 66);    // 正常输入文字颜色：深灰色
                }
            };

            // 绑定输入框失去焦点事件（Leave）：恢复占位提示，触发验证
            inputTextBox.Leave += (s, e) =>
            {
                // 如果输入为空，恢复占位提示
                if (string.IsNullOrWhiteSpace(inputTextBox.Text))
                {
                    inputTextBox.Text = "请输入文件名";                     // 恢复占位提示
                    inputTextBox.ForeColor = Color.FromArgb(150, 150, 150); // 恢复浅灰色
                }
                inputTextBox.BackColor = Color.White;                       // 恢复白色背景
                ValidateInput();                                            // 触发输入验证
            };

            // 绑定快捷键事件：Enter=确定，Escape=取消
            inputTextBox.KeyDown += (s, e) =>
            {
                // 按Enter键：触发确定按钮逻辑（阻止默认换行）
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;  // 阻止Enter键的默认行为（如换行）
                    OnOkClick(s, e);            // 调用确定按钮点击逻辑
                }
                // 按Escape键：取消并关闭窗体
                else if (e.KeyCode == Keys.Escape)
                {
                    DialogResult = DialogResult.Cancel; // 设置对话框结果为取消
                    Close();                            // 关闭窗体
                }
            };

            // 将输入框添加到窗体控件集合
            Controls.Add(inputTextBox);

            // 创建错误提示标签（输入验证失败时显示）
            errorLabel = new Label
            {
                Text = "",                                          // 初始为空
                Font = new Font("Segoe UI", 9F),                     // 字体：Segoe UI 9号
                ForeColor = Color.FromArgb(211, 47, 47),            // 文字颜色：红色（错误提示）
                Location = new Point(inputX, inputY + 50),          // 位置：输入框下方50px
                Size = new Size(340, 20),                           // 尺寸：宽340px，高20px
                TextAlign = ContentAlignment.MiddleCenter,          // 文字居中
                Visible = false                                     // 初始隐藏
            };
            // 将错误标签添加到窗体控件集合
            Controls.Add(errorLabel);
        }

        /// <summary>
        /// 创建按钮面板：包含确定/取消按钮，统一管理位置和样式
        /// </summary>
        private void CreateButtonPanel()
        {
            // 计算按钮面板位置：窗体底部70px处，水平居中
            int buttonPanelY = this.ClientSize.Height - 70;
            int buttonPanelX = (this.ClientSize.Width - 340) / 2;

            // 实例化按钮面板（容器）
            var buttonPanel = new Panel
            {
                Location = new Point(buttonPanelX, buttonPanelY),  // 位置：水平居中，窗体底部
                Size = new Size(340, 40),                          // 尺寸：宽340px，高40px
                Dock = DockStyle.None                              // 停靠方式：不自动停靠（固定位置）
            };

            // 创建确定按钮
            okButton = new MaterialButton
            {
                Text = "确定",                                      // 按钮文字
                Size = new Size(120, 40),                           // 尺寸：宽120px，高40px
                Location = new Point(50, 0),                        // 位置：面板内左侧50px
                Depth = 2,                                          // MaterialSkin阴影深度：2（有轻微阴影）
                HighEmphasis = true                                 // 高对比度（文字/背景更醒目）
            };
            // 绑定确定按钮点击事件
            okButton.Click += OnOkClick;

            // 创建取消按钮
            cancelButton = new MaterialButton
            {
                Text = "取消",                                      // 按钮文字
                Size = new Size(120, 40),                           // 尺寸：与确定按钮一致
                Location = new Point(220, 0),                       // 位置：面板内右侧220px（与确定按钮间距50px）
                Depth = 2,                                          // 阴影深度：2
                HighEmphasis = true                                 // 高对比度
            };
            // 绑定取消按钮点击事件：设置结果为取消并关闭窗体
            cancelButton.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            // 将按钮添加到面板
            buttonPanel.Controls.Add(okButton);
            buttonPanel.Controls.Add(cancelButton);
            // 将面板添加到窗体
            Controls.Add(buttonPanel);
        }

        /// <summary>
        /// 窗体加载事件：初始化透明度为0（准备淡入动画）
        /// </summary>
        /// <param name="sender">触发事件的控件（窗体）</param>
        /// <param name="e">事件参数</param>
        private void OnFormLoad(object sender, EventArgs e)
        {
            Opacity = 0; // 设置窗体透明度为0（完全透明）
        }

        /// <summary>
        /// 窗体显示事件：执行淡入动画，聚焦输入框
        /// </summary>
        /// <param name="sender">触发事件的控件（窗体）</param>
        /// <param name="e">事件参数</param>
        private void OnFormShown(object sender, EventArgs e)
        {
            // 淡入动画：透明度从0→100%，每次增加1%，间隔5ms
            for (int i = 0; i <= 100; i++)
            {
                Opacity = i / 100.0;          // 设置透明度（0.0~1.0）
                Application.DoEvents();       // 处理窗体消息（避免卡顿）
                System.Threading.Thread.Sleep(5); // 暂停5ms（控制动画速度）
            }
            inputTextBox.Focus();             // 聚焦输入框（用户可直接输入）
        }

        /// <summary>
        /// 确定按钮点击事件：验证输入，通过则返回结果并关闭窗体
        /// </summary>
        /// <param name="sender">触发事件的控件（确定按钮/Enter键）</param>
        /// <param name="e">事件参数</param>
        private void OnOkClick(object sender, EventArgs e)
        {
            // 验证输入：通过则执行后续逻辑
            if (ValidateInput())
            {
                InputText = inputTextBox.Text.Trim(); // 保存输入文本（去空格）
                DialogResult = DialogResult.OK;       // 设置对话框结果为确定

                // 淡出动画：透明度从100%→0，每次减少1%，间隔3ms
                for (int i = 100; i >= 0; i--)
                {
                    Opacity = i / 100.0;
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(3);
                }
                Close(); // 关闭窗体
            }
        }

        /// <summary>
        /// 输入验证核心方法：检查空值、非法字符、自定义验证
        /// </summary>
        /// <returns>验证结果：true=通过，false=失败</returns>
        private bool ValidateInput()
        {
            var input = inputTextBox.Text.Trim(); // 获取输入文本并去空格

            // 校验1：排除占位提示文字/空值
            if (input == "请输入文件名" || string.IsNullOrWhiteSpace(input))
            {
                ShowError("文件名不能为空"); // 显示错误提示
                return false;                // 验证失败
            }

            // 校验2：检查文件名非法字符（如\/:*?"<>|等）
            var invalidChars = System.IO.Path.GetInvalidFileNameChars(); // 获取系统禁止的文件名字符
            if (Array.Exists(invalidChars, c => input.Contains(c.ToString())))
            {
                ShowError("文件名包含非法字符"); // 显示错误提示
                return false;                    // 验证失败
            }

            // 校验3：自定义验证（调用方传入的验证逻辑）
            if (ValidationFunc != null)
            {
                var (isValid, errorMsg) = ValidationFunc(input); // 执行自定义验证
                if (!isValid)
                {
                    ShowError(errorMsg); // 显示自定义错误提示
                    return false;        // 验证失败
                }
            }

            HideError(); // 隐藏错误提示（验证通过）
            return true; // 验证通过
        }

        /// <summary>
        /// 显示错误提示：更新错误标签、修改输入框样式、执行抖动动画
        /// </summary>
        /// <param name="message">错误提示文本</param>
        private void ShowError(string message)
        {
            errorLabel.Text = message;                          // 设置错误文本
            errorLabel.Visible = true;                          // 显示错误标签
            inputTextBox.BackColor = Color.FromArgb(255, 248, 248); // 输入框背景色：浅红色（错误状态）

            // 抖动动画：输入框左右晃动3次，提示用户输入错误
            var originalLocation = inputTextBox.Location; // 保存原始位置
            for (int i = 0; i < 3; i++)
            {
                inputTextBox.Location = new Point(originalLocation.X - 5, originalLocation.Y); // 左移5px
                Application.DoEvents();                                                       // 刷新界面
                System.Threading.Thread.Sleep(30);                                            // 暂停30ms
                inputTextBox.Location = originalLocation;                                     // 恢复原始位置
                Application.DoEvents();                                                       // 刷新界面
                System.Threading.Thread.Sleep(30);                                            // 暂停30ms
            }
        }

        /// <summary>
        /// 隐藏错误提示：恢复输入框样式，隐藏错误标签
        /// </summary>
        private void HideError()
        {
            errorLabel.Visible = false; // 隐藏错误标签
            inputTextBox.BackColor = Color.White; // 恢复输入框白色背景
        }

        /// <summary>
        /// 窗体加载事件（预留）：暂无实现，可扩展初始化逻辑
        /// </summary>
        /// <param name="sender">触发事件的控件（窗体）</param>
        /// <param name="e">事件参数</param>
        private void InputBoxForm_Load(object sender, EventArgs e)
        {
            // 预留扩展：如加载配置、初始化默认值等
        }
    }
}