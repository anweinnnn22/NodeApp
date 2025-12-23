// 引入系统核心命名空间（基础类型、异常、日期等）
using System;
// 引入数据操作命名空间（DataTable、DataRow等）
using System.Data;
// 引入绘图命名空间（颜色、字体、尺寸等）
using System.Drawing;
// 引入WinForms基础控件命名空间（窗体、按钮、文本框等）
using System.Windows.Forms;
// 引入项目自定义工具类命名空间（SQLiteHelper）
using NodeApp.Helpers;
// 引入SQLite数据库操作命名空间
using System.Data.SQLite;
// 引入MaterialSkin皮肤核心命名空间
using MaterialSkin;
// 引入MaterialSkin控件命名空间（MaterialForm、MaterialButton等）
using MaterialSkin.Controls;

// 定义项目核心命名空间
namespace NodeApp
{
    /// <summary>
    /// 主窗体类：继承自MaterialForm（MaterialSkin美化的窗体）
    /// 功能：备忘录的增删改查、列表加载、样式初始化等核心逻辑
    /// </summary>
    public partial class Form1 : MaterialForm
    {
        /// <summary>
        /// 私有字段：存储当前选中的备忘录文件名
        /// 初始值为空字符串，表示未选中任何文件
        /// </summary>
        private string _currentFileName = string.Empty;

        /// <summary>
        /// 窗体构造函数：初始化窗体和核心逻辑
        /// 窗体创建时自动执行，是WinForms程序的入口之一
        /// </summary>
        public Form1()
        {
       

            // 初始化窗体控件（自动生成的代码，创建按钮、列表、文本框等控件）
            InitializeComponent();

            // 将当前窗体加入MaterialSkin管理（启用皮肤样式）
            MaterialSkinManager.Instance.AddFormToManage(this);

            // 绑定备忘录列表选中项变更事件（Lambda表达式简化写法）
            // 当用户点击列表中的备忘录项时触发
            lstMemos.SelectedIndexChanged += (sender, e) =>
            {
                // 安全校验：如果未选中任何项，直接返回（避免空指针）
                if (lstMemos.SelectedItem == null) return;

                // 获取选中项的文件名：将选中项转为MaterialListBoxItem，取Text属性
                _currentFileName = (lstMemos.SelectedItem as MaterialListBoxItem).Text;

                // 调用SQLiteHelper执行查询：根据文件名获取备忘录详情
                // 参数化查询：避免SQL注入，@FileName是参数占位符
                var dt = SQLiteHelper.ExecuteQuery(
                    "SELECT Content, CreateTime, ModifyTime FROM Memos WHERE FileName = @FileName",
                    new SQLiteParameter("@FileName", _currentFileName));

                // 校验查询结果：如果有数据（行数>0）
                if (dt.Rows.Count > 0)
                {
                    // 给富文本框赋值：显示备忘录内容
                    rtbContent.Text = dt.Rows[0]["Content"].ToString();
                    // 转换创建时间：从数据库字符串转为DateTime类型
                    DateTime createTime = Convert.ToDateTime(dt.Rows[0]["CreateTime"]);
                    // 转换修改时间：从数据库字符串转为DateTime类型
                    DateTime modifyTime = Convert.ToDateTime(dt.Rows[0]["ModifyTime"]);
                    // 更新状态栏标签：显示文件名+格式化的时间（yyyy-MM-dd HH:mm:ss）
                    lblFileInfo.Text = $"当前文件：{_currentFileName} | 创建时间：{createTime:yyyy-MM-dd HH:mm:ss} | 修改时间：{modifyTime:yyyy-MM-dd HH:mm:ss}";
                }
            };

            // 初始化SQLite数据库（创建表、连接数据库等）
            SQLiteHelper.Init();
            // 加载备忘录列表（从数据库读取所有文件名并显示）
            LoadMemoList();
            // 初始化富文本框样式（字体、背景色、滚动条等）
            InitRichTextBoxStyle();
        }

        /// <summary>
        /// 初始化富文本框样式：统一设置外观和交互属性
        /// 保证文本框样式一致，提升用户体验
        /// </summary>
        private void InitRichTextBoxStyle()
        {
            // 设置字体：等宽字体Consolas，字号10.5pt（适合代码/文本编辑）
            rtbContent.Font = new Font("Consolas", 10.5f);
            // 设置背景色：白色（基础样式）
            rtbContent.BackColor = Color.White;
            // 设置文字色：黑色（基础样式）
            rtbContent.ForeColor = Color.Black;
            // 设置边框样式：固定单边框（替代默认的3D边框）
            rtbContent.BorderStyle = BorderStyle.FixedSingle;
            // 允许按Tab键输入制表符（默认Tab键切换控件，这里改为输入\t）
            rtbContent.AcceptsTab = true;
            // 关闭自动换行（横向滚动，适合代码编辑）
            rtbContent.WordWrap = false;
            // 设置滚动条：同时显示水平和垂直滚动条
            rtbContent.ScrollBars = RichTextBoxScrollBars.Both;
        }

        /// <summary>
        /// 加载备忘录列表：从数据库读取所有文件名，显示到lstMemos控件
        /// 每次增删改后调用，保证列表与数据库同步
        /// </summary>
        private void LoadMemoList()
        {
            // 清空列表：避免重复加载数据
            lstMemos.Items.Clear();
            // 执行查询：按修改时间降序读取所有文件名（最新修改的在最前）
            var dt = SQLiteHelper.ExecuteQuery("SELECT FileName FROM Memos ORDER BY ModifyTime DESC");
            // 遍历查询结果的每一行
            foreach (DataRow row in dt.Rows)
            {
                // 获取文件名：从DataRow中读取FileName列的值
                string fileName = row["FileName"].ToString();
                // 添加到列表：创建MaterialListBoxItem（MaterialSkin样式的列表项）
                lstMemos.Items.Add(new MaterialListBoxItem(fileName));
            }
            // 清空当前编辑状态：未选中任何文件时的默认状态
            ClearCurrentEdit();
        }

        /// <summary>
        /// 清空当前编辑状态：重置文本框、文件名、状态栏标签
        /// 用于列表清空/未选中项时的初始化
        /// </summary>
        private void ClearCurrentEdit()
        {
            // 清空富文本框内容
            rtbContent.Clear();
            // 重置当前文件名：为空字符串
            _currentFileName = string.Empty;
            // 重置状态栏标签：显示默认提示文本
            lblFileInfo.Text = "未选择文件 | 创建时间：- | 修改时间：-";
        }

        /// <summary>
        /// 新增备忘录按钮点击事件
        /// 功能：弹出文件名输入框，验证后插入新备忘录到数据库
        /// </summary>
        /// <param name="sender">触发事件的控件（btnAdd按钮）</param>
        /// <param name="e">事件参数（无额外数据）</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 调用自定义输入窗口：using自动释放窗体资源，避免内存泄漏
            // 参数1：窗口标题，参数2：默认输入文本（空）
            using (var inputDialog = new InputBoxForm("文件名窗口", ""))
            {
                // 显示输入窗口：如果用户点击“确定”（DialogResult.OK）
                if (inputDialog.ShowDialog() == DialogResult.OK)
                {
                    // 获取用户输入的文件名
                    string fileName = inputDialog.InputText;
                    // 校验：文件名不能为空
                    if (string.IsNullOrEmpty(fileName))
                    {
                        // 弹出警告提示：MessageBox是WinForms内置提示框
                        MessageBox.Show("文件名不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // 返回：终止后续逻辑
                        return;
                    }

                    // 校验：文件名是否重复（执行查询，存在则返回1，否则返回null）
                    var exists = SQLiteHelper.ExecuteScalar(
                        "SELECT 1 FROM Memos WHERE FileName = @FileName",
                        new SQLiteParameter("@FileName", fileName));
                    // 如果存在重复文件名
                    if (exists != null)
                    {
                        // 弹出警告提示
                        MessageBox.Show("该文件名已存在，请更换！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // 返回：终止后续逻辑
                        return;
                    }

                    // 获取当前时间：作为创建时间和初始修改时间
                    DateTime now = DateTime.Now;
                    // 执行插入操作：新增备忘录到数据库
                    SQLiteHelper.ExecuteNonQuery(
                        "INSERT INTO Memos (FileName, Content, CreateTime, ModifyTime) VALUES (@FileName, @Content, @CreateTime, @ModifyTime)",
                        new SQLiteParameter("@FileName", fileName),       // 文件名参数
                        new SQLiteParameter("@Content", string.Empty),   // 初始内容为空
                        new SQLiteParameter("@CreateTime", now),         // 创建时间
                        new SQLiteParameter("@ModifyTime", now));        // 修改时间

                    // 刷新备忘录列表：显示新增的文件
                    LoadMemoList();
                    // 遍历列表：选中刚创建的备忘录项
                    foreach (MaterialListBoxItem item in lstMemos.Items)
                    {
                        // 匹配文件名
                        if (item.Text == fileName)
                        {
                            // 设置选中项
                            lstMemos.SelectedItem = item;
                            // 退出循环：避免无效遍历
                            break;
                        }
                    }
                    // 弹出成功提示
                    MessageBox.Show("备忘录创建成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 删除备忘录按钮点击事件
        /// 功能：确认删除后，从数据库删除选中的备忘录
        /// </summary>
        /// <param name="sender">触发事件的控件（btnDelete按钮）</param>
        /// <param name="e">事件参数</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 校验：是否选中要删除的备忘录
            if (lstMemos.SelectedItem == null)
            {
                // 弹出警告提示
                MessageBox.Show("请先选择要删除的备忘录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // 返回：终止后续逻辑
                return;
            }

            // 获取选中的文件名
            string fileName = (lstMemos.SelectedItem as MaterialListBoxItem).Text;

            // 创建自定义确认对话框：using自动释放资源
            using (var confirmForm = new MaterialForm())
            {
                // 1. 设置对话框基础属性
                confirmForm.Text = "确认删除";                          // 窗口标题
                confirmForm.Size = new Size(400, 220);                  // 窗口尺寸（宽400，高220）
                confirmForm.StartPosition = FormStartPosition.CenterParent; // 居中显示（相对于主窗体）
                confirmForm.FormBorderStyle = FormBorderStyle.FixedSingle;  // 固定边框（不可调整大小）
                confirmForm.MaximizeBox = false;                        // 禁用最大化按钮
                confirmForm.MinimizeBox = false;                        // 禁用最小化按钮

                // 2. 创建提示文本标签
                var label = new MaterialLabel
                {
                    Text = $"确定要删除「{fileName}」吗？\n\n请等待3秒后点击确认", // 提示文本（换行+倒计时提示）
                    Dock = DockStyle.Top,                                    // 顶部停靠（占满宽度）
                    Height = 100,                                            // 标签高度
                    TextAlign = ContentAlignment.MiddleCenter,               // 文字居中（水平+垂直）
                    Padding = new Padding(10),                               // 内边距（避免文字贴边）
                    // 设置字体：Roboto（MaterialSkin默认字体），12px，常规样式
                    Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Pixel)
                };
                // 将标签添加到对话框
                confirmForm.Controls.Add(label);

                // 3. 创建按钮面板（统一管理确认/取消按钮）
                var buttonPanel = new Panel
                {
                    Dock = DockStyle.Bottom,   // 底部停靠
                    Height = 47,               // 面板高度
                    Padding = new Padding(10, 5, 10, 5) // 内边距（上下5px，左右10px）
                };

                // 4. 创建“是”按钮（确认删除）
                var yesBtn = new MaterialButton
                {
                    Text = "是",                              // 按钮文字
                    Size = new Size(80, 35),                   // 按钮尺寸（宽80，高35）
                    Location = new Point(100, 5),              // 按钮位置（X=100，Y=5）
                    Enabled = false,                           // 初始禁用（倒计时3秒后启用）

                    // MaterialSkin内置样式属性
                    Depth = 2,                                 // 阴影深度（0-5，数值越大阴影越明显）
                    HighEmphasis = true,                       // 高对比度（文字/背景更醒目）
                    UseAccentColor = true,                     // 使用强调色（搭配ColorScheme）
                    NoAccentTextColor = Color.White,           // 无强调色时的文字颜色
                    MouseState = MaterialSkin.MouseState.HOVER, // 默认鼠标悬浮状态

                    // 自定义透明样式
                    BackColor = Color.FromArgb(180, 63, 81, 181), // 背景色（ARGB：180=透明度，后三位=紫色）
                    FlatStyle = FlatStyle.Flat,                   // 扁平样式（取消默认边框）
                    FlatAppearance =
                    {
                        BorderSize = 0,                              // 取消边框
                        MouseDownBackColor = Color.FromArgb(200, 63, 81, 181), // 按下时的背景色（透明度提高）
                        MouseOverBackColor = Color.FromArgb(220, 63, 81, 181)  // 悬浮时的背景色（透明度提高）
                    }
                };
                // 绑定“是”按钮点击事件：设置对话框结果为Yes并关闭
                yesBtn.Click += (s, e2) => { confirmForm.DialogResult = DialogResult.Yes; };

                // 5. 创建“否”按钮（取消删除）
                var noBtn = new MaterialButton
                {
                    Text = "否",                  // 按钮文字
                    Size = new Size(80, 35),       // 统一尺寸（与“是”按钮一致）
                    Location = new Point(200, 5),  // 位置（与“是”按钮间距20px）
                };
                // 绑定“否”按钮点击事件：设置对话框结果为No并关闭
                noBtn.Click += (s, e2) => { confirmForm.DialogResult = DialogResult.No; };

                // 将按钮添加到面板
                buttonPanel.Controls.Add(yesBtn);
                buttonPanel.Controls.Add(noBtn);
                // 将面板添加到对话框
                confirmForm.Controls.Add(buttonPanel);

                // 6. 创建倒计时计时器（3秒后启用“是”按钮）
                var timer = new System.Windows.Forms.Timer { Interval = 3000 }; // Interval=3000ms=3秒
                // 绑定计时器Tick事件（每3秒触发一次）
                timer.Tick += (s, e2) =>
                {
                    yesBtn.Enabled = true;                          // 启用“是”按钮
                    label.Text = $"确定要删除「{fileName}」吗？\n\n可以点击确认了"; // 更新提示文本
                    timer.Stop();                                   // 停止计时器
                    timer.Dispose();                                // 释放计时器资源
                };
                timer.Start(); // 启动计时器

                // 7. 显示确认对话框：如果用户点击“是”
                if (confirmForm.ShowDialog() == DialogResult.Yes)
                {
                    // 执行删除操作：从数据库删除选中的备忘录
                    SQLiteHelper.ExecuteNonQuery(
                        "DELETE FROM Memos WHERE FileName = @FileName",
                        new SQLiteParameter("@FileName", fileName));

                    // 刷新备忘录列表：移除已删除的项
                    LoadMemoList();
                    // 弹出成功提示
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 保存备忘录按钮点击事件
        /// 功能：将富文本框的内容更新到数据库，并刷新列表
        /// </summary>
        /// <param name="sender">触发事件的控件（btnSave按钮）</param>
        /// <param name="e">事件参数</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 校验：是否选中要保存的备忘录
            if (string.IsNullOrEmpty(_currentFileName))
            {
                // 弹出警告提示
                MessageBox.Show("请先选择或创建一个备忘录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // 返回：终止后续逻辑
                return;
            }

            // 获取当前时间：作为最新修改时间
            DateTime now = DateTime.Now;
            // 执行更新操作：保存内容和修改时间
            SQLiteHelper.ExecuteNonQuery(
                "UPDATE Memos SET Content = @Content, ModifyTime = @ModifyTime WHERE FileName = @FileName",
                new SQLiteParameter("@Content", rtbContent.Text),    // 富文本框的内容
                new SQLiteParameter("@ModifyTime", now),             // 最新修改时间
                new SQLiteParameter("@FileName", _currentFileName)); // 当前选中的文件名

            // 刷新备忘录列表：按修改时间重新排序
            LoadMemoList();
            // 遍历列表：重新选中当前文件（避免刷新后选中状态丢失）
            foreach (MaterialListBoxItem item in lstMemos.Items)
            {
                if (item.Text == _currentFileName)
                {
                    lstMemos.SelectedItem = item;
                    break;
                }
            }
            // 弹出成功提示
            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 查询备忘录按钮点击事件（兼容所有.NET Framework版本）
        /// 功能：弹出输入框，获取关键词后执行查询（当前为示例，可扩展）
        /// </summary>
        /// <param name="sender">触发事件的控件（btnSearch按钮）</param>
        /// <param name="e">事件参数</param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // 创建自定义输入窗体：using自动释放资源
            using (Form inputForm = new Form())
            {
                // 设置输入窗体基础属性
                inputForm.Text = "查询备忘录";                          // 窗口标题
                inputForm.Size = new Size(400, 150);                    // 窗口尺寸
                inputForm.StartPosition = FormStartPosition.CenterParent; // 居中显示

                // 创建关键词输入框
                TextBox txtKeyword = new TextBox();
                // 模拟占位符：初始显示提示文本（低版本.NET无PlaceholderText属性）
                txtKeyword.Text = "请输入查询关键词";
                txtKeyword.ForeColor = Color.Gray; // 占位符文字设为灰色
                txtKeyword.Dock = DockStyle.Top;   // 顶部停靠
                txtKeyword.Margin = new Padding(10); // 外边距（避免贴边）
                txtKeyword.Padding = new Padding(5); // 内边距（文字与边框间距）

                // 绑定获取焦点事件：清空占位符文本
                txtKeyword.GotFocus += (s, ev) =>
                {
                    // 校验：是占位符文本且颜色为灰色
                    if (txtKeyword.Text == "请输入查询关键词" && txtKeyword.ForeColor == Color.Gray)
                    {
                        txtKeyword.Text = "";                // 清空文本
                        txtKeyword.ForeColor = Color.Black;  // 恢复黑色文字
                    }
                };
                // 绑定失去焦点事件：恢复占位符文本（如果输入为空）
                txtKeyword.LostFocus += (s, ev) =>
                {
                    if (string.IsNullOrEmpty(txtKeyword.Text))
                    {
                        txtKeyword.Text = "请输入查询关键词"; // 恢复占位符
                        txtKeyword.ForeColor = Color.Gray;    // 恢复灰色文字
                    }
                };

                // 创建查询按钮
                Button btnConfirm = new Button();
                btnConfirm.Text = "查询";               // 按钮文字
                btnConfirm.Dock = DockStyle.Bottom;     // 底部停靠
                btnConfirm.Margin = new Padding(10);    // 外边距
                bool isConfirmed = false;               // 标记是否点击了查询按钮

                // 绑定查询按钮点击事件：标记为确认并关闭窗体
                btnConfirm.Click += (s, ev) => { isConfirmed = true; inputForm.Close(); };

                // 将控件添加到窗体（注意顺序：后加的控件在上方）
                inputForm.Controls.Add(btnConfirm);
                inputForm.Controls.Add(txtKeyword);

                // 显示输入窗体
                inputForm.ShowDialog();

                // 处理查询逻辑：获取输入的关键词并去空格
                string keyword = txtKeyword.Text.Trim();
                // 如果用户点击了查询按钮
                if (isConfirmed)
                {
                    // 校验：排除占位符和空值
                    if (keyword != "" && keyword != "请输入查询关键词")
                    {
                        // 示例提示：实际项目中替换为数据库查询逻辑
                        MessageBox.Show($"正在查询包含「{keyword}」的备忘录...", "查询提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // 弹出警告提示
                        MessageBox.Show("请输入查询关键词！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        /// <summary>
        /// 富文本框内容变更事件（预留）
        /// 功能：可扩展为实时保存、字数统计等逻辑
        /// </summary>
        /// <param name="sender">触发事件的控件（rtbContent）</param>
        /// <param name="e">事件参数</param>
        private void rtbContent_TextChanged(object sender, EventArgs e)
        {
            // 暂无实现：预留扩展
        }

        /// <summary>
        /// 窗体加载完成事件
        /// 功能：窗体显示后执行的初始化逻辑（强制设置文本框样式）
        /// </summary>
        /// <param name="sender">触发事件的控件（Form1）</param>
        /// <param name="e">事件参数</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // 强制设置富文本框背景色：浅薄荷绿（ARGB：240,248,255）
            rtbContent.BackColor = Color.FromArgb(240, 248, 255);
            // 强制设置文字色：深绿色（ARGB：20,60,20）
            rtbContent.ForeColor = Color.FromArgb(20, 60, 20);
            // 刷新控件：确保样式立即生效（解决MaterialSkin覆盖样式的问题）
            rtbContent.Refresh();
        }
    }
}