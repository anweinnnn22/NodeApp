// 引入系统核心命名空间（提供基础类型、异常处理、日期时间等核心功能）
using System;
// 引入泛型集合命名空间（List、Dictionary等集合类型）
using System.Collections.Generic;
// 引入数据表格命名空间（DataTable、DataRow等数据容器）
using System.Data;
// 引入SQLite数据库操作命名空间（SQLiteConnection、SQLiteParameter等）
using System.Data.SQLite;
// 引入绘图命名空间（Color、Font、Size等样式相关类型）
using System.Drawing;
// 引入反射命名空间（获取控件属性、动态调用方法）
using System.Reflection;
// 引入Windows窗体命名空间（Form、Button、RichTextBox等控件）
using System.Windows.Forms;
// 引入MaterialSkin美化库命名空间（核心皮肤管理）
using MaterialSkin;
// 引入MaterialSkin控件命名空间（美化后的按钮、列表等）
using MaterialSkin.Controls;
// 引入自定义工具类命名空间（SQLiteHelper等）
using NodeApp.Helpers;

// 定义程序集命名空间（组织代码结构，避免类名冲突）
namespace NodeApp
{
    /// <summary>
    /// 主窗体类：继承自MaterialForm（MaterialSkin美化的窗体基类）
    /// 核心功能：备忘录的增删改查、列表加载、样式初始化、关闭提醒等
    /// </summary>
    public partial class Form1 : MaterialForm
    {
        /// <summary>
        /// 私有字段：存储当前选中的备忘录文件名
        /// 初始值为空字符串，表示未选中任何文件
        /// </summary>
        private string _currentFileName = string.Empty;

        // 新增字段：存储当前查询关键词（用于关键词高亮功能，暂未实现完整逻辑）
        private string _currentSearchKeyword = string.Empty;

        // 新增字段：存储富文本框原始内容（未修改的基准）
        // 用于对比判断是否有未保存的修改
        private string _rtbOriginalContent = string.Empty;

        /// <summary>
        /// 私有字段：重置列表按钮（动态创建，非设计器生成）
        /// </summary>
        private MaterialButton btnResetList;

        /// <summary>
        /// 窗体构造函数：窗体创建时自动执行的核心初始化方法
        /// 负责控件初始化、事件绑定、样式配置、数据加载
        /// </summary>
        public Form1()
        {
            // 调用设计器自动生成的控件初始化方法
            // 作用：创建窗体上的所有控件（按钮、列表、文本框、面板等）
            InitializeComponent();

            // ===== 动态创建“重置列表”按钮开始 =====
            // 实例化MaterialButton控件（美化样式）
            btnResetList = new MaterialSkin.Controls.MaterialButton
            {
                Text = "重置列表",          // 按钮显示文本
                Dock = DockStyle.Bottom,    // 按钮停靠在面板底部
                Margin = new Padding(0, 8, 0, 0), // 与上方控件保持8px上边距
                Depth = 0,                 // MaterialSkin深度（0=平面）
                HighEmphasis = true,       // 高强调色（突出显示）
                UseAccentColor = false,    // 不使用强调色
                AutoSizeMode = AutoSizeMode.GrowAndShrink // 自动适应文本大小
            };
            // 绑定按钮点击事件：点击时执行btnResetList_Click方法
            btnResetList.Click += btnResetList_Click;
            // 将按钮添加到左侧面板（panelLeft）的控件集合中
            panelLeft.Controls.Add(btnResetList);
            // 将按钮置于控件层级顶层（确保显示在查询按钮上方）
            btnResetList.BringToFront();
            // ===== 动态创建“重置列表”按钮结束 =====

            // 将当前窗体加入MaterialSkin管理器
            // 作用：启用MaterialSkin皮肤样式，统一控件外观
            MaterialSkinManager.Instance.AddFormToManage(this);

            // 绑定备忘录列表（lstMemos）选中项变更事件（Lambda表达式简写）
            // 触发时机：用户点击/切换列表中的备忘录项
            lstMemos.SelectedIndexChanged += (sender, e) =>
            {
                // 安全校验：如果未选中任何项，直接返回（避免空指针异常）
                if (lstMemos.SelectedItem == null) return;

                // 获取选中项的文件名：将选中项转为MaterialListBoxItem类型，读取Text属性
                _currentFileName = (lstMemos.SelectedItem as MaterialListBoxItem).Text;

                // 调用SQLiteHelper执行查询：根据文件名获取备忘录详情
                // SQL语句：查询Content（内容）、CreateTime（创建时间）、ModifyTime（修改时间）
                // 参数化查询：@FileName为参数占位符，避免SQL注入
                var dt = SQLiteHelper.ExecuteQuery(
                    "SELECT Content, CreateTime, ModifyTime FROM Memos WHERE FileName = @FileName",
                    new SQLiteParameter("@FileName", _currentFileName));

                // 校验查询结果：如果存在数据（行数>0）
                if (dt.Rows.Count > 0)
                {
                    // 给富文本框赋值：显示备忘录的内容
                    rtbContent.Text = dt.Rows[0]["Content"].ToString();
                    // 记录富文本框原始内容（未修改状态），作为后续对比基准
                    _rtbOriginalContent = rtbContent.Text;

                    // 将数据库中的创建时间字符串转为DateTime类型
                    DateTime createTime = Convert.ToDateTime(dt.Rows[0]["CreateTime"]);
                    // 将数据库中的修改时间字符串转为DateTime类型
                    DateTime modifyTime = Convert.ToDateTime(dt.Rows[0]["ModifyTime"]);

                    // 更新状态栏标签：显示文件名+格式化的时间（yyyy-MM-dd HH:mm:ss）
                    lblFileInfo.Text = $"当前文件：{_currentFileName} | 创建时间：{createTime:yyyy-MM-dd HH:mm:ss} | 修改时间：{modifyTime:yyyy-MM-dd HH:mm:ss}";
                }
            };

            // 初始化SQLite数据库：创建表（若不存在）、建立连接等
            SQLiteHelper.Init();
            // 加载备忘录列表：默认加载全部数据，按修改时间降序
            LoadMemoList();
            // 初始化富文本框样式：统一字体、背景色、滚动条等
            InitRichTextBoxStyle();
        }

        /// <summary>
        /// 重置列表按钮点击事件处理方法
        /// 功能：恢复完整备忘录列表、清空编辑状态、提示用户
        /// </summary>
        /// <param name="sender">触发事件的控件（btnResetList）</param>
        /// <param name="e">事件参数（无额外数据）</param>
        private void btnResetList_Click(object sender, EventArgs e)
        {
            // 调用无参LoadMemoList：加载全部备忘录，恢复默认排序
            LoadMemoList();
            // 清空当前编辑状态：重置文本框、文件名、状态栏
            ClearCurrentEdit();
            // 弹出提示框：告知用户重置成功
            MessageBox.Show("已返回完整备忘录列表", "重置成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 列表选中项变更事件（备用方法，与Lambda表达式事件二选一）
        /// 功能：处理列表选中项变更逻辑（实际使用的是Lambda版本）
        /// </summary>
        /// <param name="sender">触发事件的控件（lstMemos）</param>
        /// <param name="selectedItem">选中的列表项</param>
        private void lstMemos_SelectedIndexChanged(object sender, MaterialListBoxItem selectedItem)
        {
            // 安全校验：未选中项则返回
            if (selectedItem == null) return;
            // 赋值当前文件名（与Lambda事件逻辑一致）
            _currentFileName = selectedItem.Text;
            // 预留：后续可添加加载备忘录内容的逻辑
        }

        /// <summary>
        /// 初始化富文本框样式方法
        /// 功能：统一设置富文本框（rtbContent）的外观和交互属性
        /// </summary>
        private void InitRichTextBoxStyle()
        {
            // 设置字体：等宽字体Consolas，字号10.5pt（适合文本/代码编辑）
            rtbContent.Font = new Font("Consolas", 10.5f);
            // 设置背景色：白色（基础样式）
            rtbContent.BackColor = Color.White;
            // 设置文字色：黑色（基础样式）
            rtbContent.ForeColor = Color.Black;
            // 设置边框样式：固定单边框（替代默认3D边框）
            rtbContent.BorderStyle = BorderStyle.FixedSingle;
            // 允许按Tab键输入制表符（默认Tab键切换控件，此处改为输入\t）
            rtbContent.AcceptsTab = true;
            // 关闭自动换行（横向滚动，适合代码/长文本编辑）
            rtbContent.WordWrap = false;
            // 设置滚动条：同时显示水平和垂直滚动条
            rtbContent.ScrollBars = RichTextBoxScrollBars.Both;
        }

        /// <summary>
        /// 加载备忘录列表方法（核心数据加载逻辑）
        /// 支持关键词筛选、指定字段排序、升降序控制
        /// </summary>
        /// <param name="keyword">查询关键词（默认空字符串=加载全部）</param>
        /// <param name="sortField">排序字段（CreateTime/ModifyTime，默认ModifyTime）</param>
        /// <param name="isDesc">是否降序（true=降序，false=升序，默认true）</param>
        private void LoadMemoList(string keyword = "", string sortField = "ModifyTime", bool isDesc = true)
        {
            // 清空列表：避免重复加载数据，防止列表项叠加
            lstMemos.Items.Clear();

            // 定义SQL语句字符串（存储最终执行的查询语句）
            string sql = string.Empty;
            // 定义SQL参数集合（存储参数化查询的参数，避免SQL注入）
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

            // 校验排序字段合法性：仅允许CreateTime/ModifyTime，非法则重置为ModifyTime
            if (sortField != "CreateTime" && sortField != "ModifyTime")
            {
                sortField = "ModifyTime";
            }

            // 拼接排序方向：DESC（降序）/ASC（升序）
            string sortDirection = isDesc ? "DESC" : "ASC";

            // 分支1：无关键词（加载全部备忘录）
            if (string.IsNullOrEmpty(keyword))
            {
                // 构建SQL：查询所有文件名，按指定字段+方向排序
                sql = $"SELECT FileName FROM Memos ORDER BY {sortField} {sortDirection}";
            }
            // 分支2：有关键词（模糊查询）
            else
            {
                // 构建SQL：模糊匹配文件名/内容，按指定字段+方向排序
                // LIKE @Keyword：匹配包含关键词的内容（%为通配符）
                sql = $"SELECT FileName FROM Memos WHERE FileName LIKE @Keyword OR Content LIKE @Keyword ORDER BY {sortField} {sortDirection}";
                // 添加模糊查询参数：关键词前后加%，实现“包含匹配”
                parameters.Add(new SQLiteParameter("@Keyword", "%" + keyword + "%"));
            }

            // 执行SQL查询：调用SQLiteHelper，传入SQL和参数数组
            // 返回DataTable：存储查询结果（文件名列表）
            var dt = SQLiteHelper.ExecuteQuery(sql, parameters.ToArray());

            // 遍历查询结果的每一行数据
            foreach (DataRow row in dt.Rows)
            {
                // 读取FileName列的值（当前行的文件名）
                string fileName = row["FileName"].ToString();
                // 创建MaterialListBoxItem并添加到列表
                lstMemos.Items.Add(new MaterialListBoxItem(fileName));
            }

            // 清空当前编辑状态：未选中任何文件时的默认状态
            ClearCurrentEdit();
        }

        /// <summary>
        /// 清空当前编辑状态方法
        /// 功能：重置富文本框、文件名、原始内容、状态栏标签
        /// </summary>
        private void ClearCurrentEdit()
        {
            // 清空富文本框内容
            rtbContent.Clear();
            // 重置当前文件名（为空字符串）
            _currentFileName = string.Empty;
            // 重置富文本框原始内容（为空字符串）
            _rtbOriginalContent = string.Empty;
            // 重置状态栏标签：显示默认提示文本
            lblFileInfo.Text = "未选择文件 | 创建时间：- | 修改时间：-";
        }

        /// <summary>
        /// 新增备忘录按钮点击事件
        /// 功能：弹出文件名输入框，验证后插入新备忘录到数据库
        /// </summary>
        /// <param name="sender">触发事件的控件（btnAdd）</param>
        /// <param name="e">事件参数</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 使用using语句创建输入窗口：自动释放资源，避免内存泄漏
            using (var inputDialog = new InputBoxForm("文件名窗口", ""))
            {
                // 显示输入窗口：如果用户点击“确定”（DialogResult.OK）
                if (inputDialog.ShowDialog() == DialogResult.OK)
                {
                    // 获取用户输入的文件名
                    string fileName = inputDialog.InputText;

                    // 校验1：文件名不能为空
                    if (string.IsNullOrEmpty(fileName))
                    {
                        MessageBox.Show("文件名不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // 终止方法执行
                    }

                    // 校验2：文件名是否重复（执行查询，存在则返回1，否则返回null）
                    var exists = SQLiteHelper.ExecuteScalar(
                        "SELECT 1 FROM Memos WHERE FileName = @FileName",
                        new SQLiteParameter("@FileName", fileName));

                    // 如果文件名已存在，提示并返回
                    if (exists != null)
                    {
                        MessageBox.Show("该文件名已存在，请更换！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // 终止方法执行
                    }

                    // 获取当前时间：作为创建时间和初始修改时间
                    DateTime now = DateTime.Now;

                    // 执行插入操作：新增备忘录到数据库
                    SQLiteHelper.ExecuteNonQuery(
                        "INSERT INTO Memos (FileName, Content, CreateTime, ModifyTime) VALUES (@FileName, @Content, @CreateTime, @ModifyTime)",
                        new SQLiteParameter("@FileName", fileName),    // 文件名参数
                        new SQLiteParameter("@Content", string.Empty), // 初始内容为空
                        new SQLiteParameter("@CreateTime", now),       // 创建时间
                        new SQLiteParameter("@ModifyTime", now));      // 修改时间

                    // 刷新备忘录列表：显示新增的文件
                    LoadMemoList();

                    // 遍历列表：选中刚创建的备忘录项
                    foreach (MaterialListBoxItem item in lstMemos.Items)
                    {
                        if (item.Text == fileName)
                        {
                            lstMemos.SelectedItem = item;
                            break; // 找到后退出循环，提升性能
                        }
                    }

                    // 提示用户：创建成功
                    MessageBox.Show("备忘录创建成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 删除备忘录按钮点击事件
        /// 功能：弹出确认框（带倒计时），确认后删除选中的备忘录
        /// </summary>
        /// <param name="sender">触发事件的控件（btnDelete）</param>
        /// <param name="e">事件参数</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 校验：未选中任何备忘录则提示并返回
            if (lstMemos.SelectedItem == null)
            {
                MessageBox.Show("请先选择要删除的备忘录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 获取选中的文件名
            string fileName = (lstMemos.SelectedItem as MaterialListBoxItem).Text;

            // 使用using创建确认窗体：自动释放资源
            using (var confirmForm = new MaterialForm())
            {
                // ===== 确认窗体基础配置 =====
                confirmForm.Text = "确认删除";          // 窗体标题
                confirmForm.Size = new Size(400, 220);  // 窗体大小
                confirmForm.StartPosition = FormStartPosition.CenterParent; // 居中显示
                confirmForm.FormBorderStyle = FormBorderStyle.FixedSingle; // 固定边框（不可缩放）
                confirmForm.MaximizeBox = false;        // 禁用最大化按钮
                confirmForm.MinimizeBox = false;        // 禁用最小化按钮

                // ===== 创建提示标签 =====
                var label = new MaterialLabel
                {
                    Text = $"确定要删除「{fileName}」吗？\n\n请等待3秒后点击确认", // 提示文本（带倒计时说明）
                    Dock = DockStyle.Top,                 // 停靠顶部
                    Height = 100,                         // 高度100px
                    TextAlign = ContentAlignment.MiddleCenter, // 文字居中
                    Padding = new Padding(10),            // 内边距10px
                    Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Pixel) // 字体样式
                };
                confirmForm.Controls.Add(label); // 添加标签到窗体

                // ===== 创建按钮面板 =====
                var buttonPanel = new Panel
                {
                    Dock = DockStyle.Bottom, // 停靠底部
                    Height = 47,             // 高度47px
                    Padding = new Padding(10, 5, 10, 5) // 内边距
                };

                // ===== 创建“是”按钮（确认删除）=====
                var yesBtn = new MaterialButton
                {
                    Text = "是",                          // 按钮文本
                    Size = new Size(80, 35),              // 按钮大小
                    Location = new Point(100, 5),         // 按钮位置
                    Enabled = false,                      // 初始禁用（倒计时后启用）
                    Depth = 2,                           // MaterialSkin深度
                    HighEmphasis = true,                 // 高强调色
                    UseAccentColor = true,               // 使用强调色
                    NoAccentTextColor = Color.White,     // 无强调色时文字白色
                    MouseState = MaterialSkin.MouseState.HOVER, // 鼠标悬浮状态
                    BackColor = Color.FromArgb(180, 63, 81, 181), // 背景色
                    FlatStyle = FlatStyle.Flat,          // 扁平样式
                    FlatAppearance =                     // 扁平外观配置
                    {
                        BorderSize = 0,                  // 无边框
                        MouseDownBackColor = Color.FromArgb(200, 63, 81, 181), // 按下时背景色
                        MouseOverBackColor = Color.FromArgb(220, 63, 81, 181)  // 悬浮时背景色
                    }
                };
                // 绑定“是”按钮点击事件：设置对话框结果为Yes并关闭
                yesBtn.Click += (s, e2) => { confirmForm.DialogResult = DialogResult.Yes; };

                // ===== 创建“否”按钮（取消删除）=====
                var noBtn = new MaterialButton
                {
                    Text = "否",          // 按钮文本
                    Size = new Size(80, 35), // 按钮大小
                    Location = new Point(200, 5) // 按钮位置
                };
                // 绑定“否”按钮点击事件：设置对话框结果为No并关闭
                noBtn.Click += (s, e2) => { confirmForm.DialogResult = DialogResult.No; };

                // 将按钮添加到面板
                buttonPanel.Controls.Add(yesBtn);
                buttonPanel.Controls.Add(noBtn);
                // 将面板添加到窗体
                confirmForm.Controls.Add(buttonPanel);

                // ===== 创建倒计时计时器 =====
                var timer = new System.Windows.Forms.Timer { Interval = 3000 }; // 3秒间隔
                // 计时器触发事件（3秒后执行）
                timer.Tick += (s, e2) =>
                {
                    yesBtn.Enabled = true; // 启用“是”按钮
                    label.Text = $"确定要删除「{fileName}」吗？\n\n可以点击确认了"; // 更新提示文本
                    timer.Stop(); // 停止计时器
                    timer.Dispose(); // 释放计时器资源
                };
                timer.Start(); // 启动计时器

                // ===== 显示确认窗体并处理结果 =====
                if (confirmForm.ShowDialog() == DialogResult.Yes)
                {
                    // 执行删除操作：从数据库删除选中的备忘录
                    SQLiteHelper.ExecuteNonQuery(
                        "DELETE FROM Memos WHERE FileName = @FileName",
                        new SQLiteParameter("@FileName", fileName));

                    // 刷新备忘录列表：移除已删除的项
                    LoadMemoList();
                    // 提示用户：删除成功
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 保存备忘录按钮点击事件
        /// 功能：将富文本框内容更新到数据库，同步修改时间，刷新列表
        /// </summary>
        /// <param name="sender">触发事件的控件（btnSave）</param>
        /// <param name="e">事件参数</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 校验：未选中任何备忘录则提示并返回
            if (string.IsNullOrEmpty(_currentFileName))
            {
                MessageBox.Show("请先选择或创建一个备忘录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 获取当前时间：作为最新修改时间
            DateTime now = DateTime.Now;

            // 执行更新操作：更新备忘录内容和修改时间
            SQLiteHelper.ExecuteNonQuery(
                "UPDATE Memos SET Content = @Content, ModifyTime = @ModifyTime WHERE FileName = @FileName",
                new SQLiteParameter("@Content", rtbContent.Text),    // 富文本框当前内容
                new SQLiteParameter("@ModifyTime", now),             // 最新修改时间
                new SQLiteParameter("@FileName", _currentFileName)); // 选中的文件名

            // 同步富文本框原始内容：标记为“已保存”，避免关闭时重复提醒
            _rtbOriginalContent = rtbContent.Text;

            // 刷新备忘录列表：按修改时间重新排序
            LoadMemoList();

            // 重新选中当前文件：保持用户操作连贯性
            foreach (MaterialListBoxItem item in lstMemos.Items)
            {
                if (item.Text == _currentFileName)
                {
                    lstMemos.SelectedItem = item;
                    break; // 找到后退出循环
                }
            }

            // 提示用户：保存成功
            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 查询备忘录按钮点击事件
        /// 功能：弹出查询窗口，支持关键词筛选、排序字段/方向选择
        /// </summary>
        /// <param name="sender">触发事件的控件（btnSearch）</param>
        /// <param name="e">事件参数</param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // 提前定义查询参数：关键词、排序字段、排序方向
            string keyword = string.Empty;
            string selectedSortField = "ModifyTime";
            bool selectedIsDesc = true;

            // 使用using创建查询窗体：自动释放资源
            using (Form inputForm = new Form())
            {
                // ===== 查询窗体基础配置 =====
                inputForm.Text = "查询备忘录";          // 窗体标题
                inputForm.Size = new Size(400, 250);    // 窗体大小
                inputForm.StartPosition = FormStartPosition.CenterParent; // 居中显示

                // ===== 创建关键词输入框（带占位符）=====
                TextBox txtKeyword = new TextBox
                {
                    Text = "请输入查询关键词", // 占位符文本
                    ForeColor = Color.Gray,    // 占位符文字灰色
                    Dock = DockStyle.Top,      // 停靠顶部
                    Margin = new Padding(10),  // 外边距10px
                    Padding = new Padding(5)   // 内边距5px
                };
                // 绑定焦点获取事件：清空占位符，恢复黑色文字
                txtKeyword.GotFocus += (s, ev) =>
                {
                    if (txtKeyword.Text == "请输入查询关键词" && txtKeyword.ForeColor == Color.Gray)
                    {
                        txtKeyword.Text = "";
                        txtKeyword.ForeColor = Color.Black;
                    }
                };
                // 绑定焦点丢失事件：恢复占位符（若为空）
                txtKeyword.LostFocus += (s, ev) =>
                {
                    if (string.IsNullOrEmpty(txtKeyword.Text))
                    {
                        txtKeyword.Text = "请输入查询关键词";
                        txtKeyword.ForeColor = Color.Gray;
                    }
                };

                // ===== 创建排序字段标签 =====
                Label lblSortField = new Label
                {
                    Text = "排序字段：",                // 标签文本
                    Dock = DockStyle.Top,              // 停靠顶部
                    Height = 25,                      // 高度25px
                    Margin = new Padding(10, 5, 10, 0), // 外边距
                    TextAlign = ContentAlignment.MiddleLeft // 文字左对齐
                };

                // ===== 创建排序字段下拉框 =====
                ComboBox cboSortField = new ComboBox
                {
                    Dock = DockStyle.Top,              // 停靠顶部
                    Margin = new Padding(10, 0, 10, 5), // 外边距
                    DropDownStyle = ComboBoxStyle.DropDownList // 仅可选择，不可输入
                };
                // 添加排序选项：修改时间、创建时间
                cboSortField.Items.AddRange(new string[] { "修改时间", "创建时间" });
                cboSortField.SelectedIndex = 0; // 默认选中“修改时间”

                // ===== 创建排序方向标签 =====
                Label lblSortDir = new Label
                {
                    Text = "排序方向：",                // 标签文本
                    Dock = DockStyle.Top,              // 停靠顶部
                    Height = 25,                      // 高度25px
                    Margin = new Padding(10, 5, 10, 0), // 外边距
                    TextAlign = ContentAlignment.MiddleLeft // 文字左对齐
                };

                // ===== 创建排序方向下拉框 =====
                ComboBox cboSortDir = new ComboBox
                {
                    Dock = DockStyle.Top,              // 停靠顶部
                    Margin = new Padding(10, 0, 10, 5), // 外边距
                    DropDownStyle = ComboBoxStyle.DropDownList // 仅可选择，不可输入
                };
                // 添加排序方向选项：降序、升序
                cboSortDir.Items.AddRange(new string[] { "降序（最新在前）", "升序（最早在前）" });
                cboSortDir.SelectedIndex = 0; // 默认选中“降序”

                // ===== 创建按钮面板 =====
                Panel btnPanel = new Panel
                {
                    Dock = DockStyle.Bottom, // 停靠底部
                    Height = 50,             // 高度50px
                    Padding = new Padding(50, 5, 50, 5) // 内边距
                };

                // ===== 创建“查询”按钮 =====
                Button btnConfirm = new Button
                {
                    Text = "查询",          // 按钮文本
                    Size = new Size(80, 35), // 按钮大小
                    Location = new Point(20, 5) // 按钮位置
                };

                // ===== 创建“取消”按钮 =====
                Button btnCancel = new Button
                {
                    Text = "取消",          // 按钮文本
                    Size = new Size(80, 35), // 按钮大小
                    Location = new Point(120, 5) // 按钮位置
                };

                // 标记是否确认查询（默认false）
                bool isConfirmed = false;

                // ===== 绑定“查询”按钮点击事件 =====
                btnConfirm.Click += (s, ev) =>
                {
                    isConfirmed = true; // 标记为确认查询
                    keyword = txtKeyword.Text.Trim(); // 获取并修剪关键词
                    // 转换排序字段：下拉框选中项 → 数据库字段名
                    selectedSortField = cboSortField.SelectedIndex == 0 ? "ModifyTime" : "CreateTime";
                    // 转换排序方向：下拉框选中项 → 布尔值
                    selectedIsDesc = cboSortDir.SelectedIndex == 0;
                    inputForm.Close(); // 关闭查询窗口
                };

                // ===== 绑定“取消”按钮点击事件 =====
                btnCancel.Click += (s, ev) =>
                {
                    isConfirmed = false; // 标记为取消查询
                    inputForm.Close(); // 关闭查询窗口
                };

                // ===== 添加控件到窗体（注意顺序：从下到上）=====
                btnPanel.Controls.Add(btnConfirm);
                btnPanel.Controls.Add(btnCancel);
                inputForm.Controls.Add(btnPanel);
                inputForm.Controls.Add(lblSortDir);
                inputForm.Controls.Add(cboSortDir);
                inputForm.Controls.Add(lblSortField);
                inputForm.Controls.Add(cboSortField);
                inputForm.Controls.Add(txtKeyword);

                // ===== 显示查询窗口 =====
                inputForm.ShowDialog();

                // ===== 处理查询结果 =====
                if (isConfirmed)
                {
                    // 分支1：有效关键词（非空/非占位符）
                    if (keyword != "" && keyword != "请输入查询关键词")
                    {
                        // 加载筛选后的列表
                        LoadMemoList(keyword, selectedSortField, selectedIsDesc);
                        // 转换排序提示文本
                        string sortTip = selectedIsDesc ? "降序" : "升序";
                        string fieldTip = selectedSortField == "ModifyTime" ? "修改时间" : "创建时间";
                        // 提示查询结果
                        MessageBox.Show($"查询完成！共找到 {lstMemos.Items.Count} 条匹配记录（按{fieldTip}{sortTip}排序）", "查询成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    // 分支2：无效关键词（空/占位符）→ 加载全部
                    else
                    {
                        LoadMemoList("", selectedSortField, selectedIsDesc);
                        // 转换排序提示文本
                        string sortTip = selectedIsDesc ? "降序" : "升序";
                        string fieldTip = selectedSortField == "ModifyTime" ? "修改时间" : "创建时间";
                        // 提示加载全部
                        MessageBox.Show($"已加载全部备忘录（按{fieldTip}{sortTip}排序）", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                // 分支3：取消查询 → 恢复默认列表
                else
                {
                    LoadMemoList();
                    MessageBox.Show("已取消查询，返回默认列表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 富文本框内容变更事件（预留）
        /// 触发时机：富文本框（rtbContent）内容发生任何修改时
        /// </summary>
        /// <param name="sender">触发事件的控件（rtbContent）</param>
        /// <param name="e">事件参数</param>
        private void rtbContent_TextChanged(object sender, EventArgs e)
        {
            // 暂无实现：预留扩展（如实时保存、字数统计等）
        }

        /// <summary>
        /// 重写窗体关闭事件（核心：未保存修改提醒）
        /// 触发时机：点击右上角×、调用Close()、退出程序等所有关闭场景
        /// </summary>
        /// <param name="e">关闭事件参数（可通过e.Cancel阻止关闭）</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 调用基类的关闭逻辑（必须保留，保证窗体基础关闭流程）
            base.OnFormClosing(e);

            // 校验条件：①有选中的备忘录 ②富文本内容与原始内容不一致（有未保存修改）
            if (!string.IsNullOrEmpty(_currentFileName) && rtbContent.Text != _rtbOriginalContent)
            {
                // 弹出确认框：Yes=保存，No=不保存，Cancel=取消关闭
                DialogResult result = MessageBox.Show(
                    $"「{_currentFileName}」有未保存的修改，是否保存后再退出？", // 提示文本
                    "未保存的修改",                                           // 弹窗标题
                    MessageBoxButtons.YesNoCancel,                           // 按钮类型
                    MessageBoxIcon.Warning);                                 // 警告图标

                // 根据用户选择处理
                switch (result)
                {
                    case DialogResult.Yes:
                        // 选择“保存”：复用保存按钮逻辑，自动保存修改
                        btnSave_Click(this, EventArgs.Empty);
                        break;
                    case DialogResult.No:
                        // 选择“不保存”：直接退出（无需处理，e.Cancel=false）
                        break;
                    case DialogResult.Cancel:
                        // 选择“取消”：阻止窗体关闭（核心）
                        e.Cancel = true;
                        break;
                }
            }
            // 无修改/无选中文件：直接执行关闭流程（无需处理）
        }

        /// <summary>
        /// 窗体加载完成事件
        /// 触发时机：窗体完全加载并显示后
        /// </summary>
        /// <param name="sender">触发事件的控件（Form1）</param>
        /// <param name="e">事件参数</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // 强制设置富文本框背景色（覆盖InitRichTextBoxStyle的设置）
            rtbContent.BackColor = Color.FromArgb(240, 248, 255);
            // 强制设置富文本框文字色
            rtbContent.ForeColor = Color.FromArgb(20, 60, 20);
            // 刷新富文本框：立即应用样式修改
            rtbContent.Refresh();

            // ===== 修复列表滚动条（反射方式）=====
            // 获取lstMemos的ScrollAlwaysVisible属性（垂直滚动条）
            var scrollAlwaysVisibleProp = lstMemos.GetType().GetProperty("ScrollAlwaysVisible", BindingFlags.Instance | BindingFlags.Public);
            // 获取lstMemos的IntegralHeight属性（整行显示）
            var integralHeightProp = lstMemos.GetType().GetProperty("IntegralHeight", BindingFlags.Instance | BindingFlags.Public);
            // 获取lstMemos的HorizontalScrollbar属性（水平滚动条）
            var horizontalScrollProp = lstMemos.GetType().GetProperty("HorizontalScrollbar", BindingFlags.Instance | BindingFlags.Public);
            // 获取lstMemos的MultiColumn属性（多列显示）
            var multiColumnProp = lstMemos.GetType().GetProperty("MultiColumn", BindingFlags.Instance | BindingFlags.Public);

            // 启用垂直滚动条（强制显示）
            scrollAlwaysVisibleProp?.SetValue(lstMemos, true);
            // 禁用整行显示（允许部分行显示）
            integralHeightProp?.SetValue(lstMemos, false);
            // 启用水平滚动条（文件名过长时横向滚动）
            horizontalScrollProp?.SetValue(lstMemos, true);
            // 禁用多列显示（单列显示）
            multiColumnProp?.SetValue(lstMemos, false);
        }
    }
}