// 引入Windows窗体核心命名空间（提供Form、Button、Panel等控件基类）
using System.Windows.Forms;

// 定义命名空间：与主窗体类保持一致，保证partial类合并
namespace NodeApp
{
    // Form1的部分类：控件初始化代码（WinForms自动生成，partial拆分到多个文件）
    // 作用：仅负责控件的实例化、布局、属性配置、事件绑定，不包含业务逻辑
    partial class Form1
    {
        /// <summary>
        /// 组件容器：WinForms标准控件资源管理器
        /// 作用：统一管理所有窗体控件的生命周期，自动释放资源避免内存泄漏
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 重写Dispose方法：WinForms标准资源释放逻辑
        /// 作用：释放托管资源（控件）和非托管资源（句柄、文件流等）
        /// </summary>
        /// <param name="disposing">是否释放托管资源（true=释放，false=仅释放非托管）</param>
        protected override void Dispose(bool disposing)
        {
            // 条件：需要释放托管资源 且 组件容器不为空
            if (disposing && (components != null))
            {
                // 释放所有控件资源（由components统一管理）
                components.Dispose();
            }
            // 调用基类Dispose：释放非托管资源（如窗体句柄）
            base.Dispose(disposing);
        }

        /// <summary>
        /// 初始化组件：核心控件布局配置方法（WinForms自动生成，手动补充注释）
        /// 作用：实例化所有控件、设置布局/样式/事件，是窗体UI的核心初始化逻辑
        /// </summary>
        private void InitializeComponent()
        {
            // ========== 第一步：实例化所有控件对象（从外层容器到内层控件） ==========
            // 实例化分割容器：将窗体分为左右两部分（左侧列表区，右侧编辑区）
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            // 实例化左侧面板：承载备忘录列表和操作按钮（新增/查询/删除）
            this.panelLeft = new System.Windows.Forms.Panel();
            // 实例化删除按钮：MaterialSkin美化样式的按钮
            this.btnDelete = new MaterialSkin.Controls.MaterialButton();
            // 实例化查询按钮：新增功能，MaterialSkin样式（核心新增控件）
            this.btnSearch = new MaterialSkin.Controls.MaterialButton();
            // 实例化新增按钮：MaterialSkin美化样式的按钮
            this.btnAdd = new MaterialSkin.Controls.MaterialButton();
            // 实例化自定义备忘录列表：继承自MaterialListBox的自定义控件
            this.lstMemos = new NodeApp.CustomMaterialListBox();
            // 实例化右侧面板：承载富文本编辑框、保存按钮、信息标签
            this.panelRight = new System.Windows.Forms.Panel();
            // 实例化底部面板：仅承载文件信息标签（lblFileInfo）
            this.panelBottom = new System.Windows.Forms.Panel();
            // 实例化文件信息标签：显示文件名、创建/修改时间
            this.lblFileInfo = new MaterialSkin.Controls.MaterialLabel();
            // 实例化保存按钮：MaterialSkin美化样式的按钮
            this.btnSave = new MaterialSkin.Controls.MaterialButton();
            // 实例化富文本编辑框：核心编辑控件，用于输入/显示备忘录内容
            this.rtbContent = new System.Windows.Forms.RichTextBox();

            // ========== 第二步：暂停布局更新（避免多次重绘，提升性能） ==========
            // 暂停分割容器的布局初始化（BeginInit/EndInit是WinForms标准写法）
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            // 暂停分割容器左侧面板的布局
            this.splitContainer.Panel1.SuspendLayout();
            // 暂停分割容器右侧面板的布局
            this.splitContainer.Panel2.SuspendLayout();
            // 暂停分割容器整体布局
            this.splitContainer.SuspendLayout();
            // 暂停左侧面板布局
            this.panelLeft.SuspendLayout();
            // 暂停右侧面板布局
            this.panelRight.SuspendLayout();
            // 暂停底部面板布局
            this.panelBottom.SuspendLayout();
            // 暂停整个窗体布局
            this.SuspendLayout();

            // ========== 第三步：配置分割容器（splitContainer）属性 ==========
            // 
            // splitContainer（核心布局容器）
            // 
            // 停靠方式：填充整个窗体客户端区域（覆盖所有可用空间）
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            // 位置：相对于窗体的坐标（X=3, Y=76），适配窗体上内边距99px
            this.splitContainer.Location = new System.Drawing.Point(3, 76);
            // 控件名称：供代码引用（如splitContainer.Panel1）
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1（左侧面板容器）
            // 
            // 左侧面板添加子控件：panelLeft（承载列表和按钮）
            this.splitContainer.Panel1.Controls.Add(this.panelLeft);
            // 
            // splitContainer.Panel2（右侧面板容器）
            // 
            // 右侧面板添加子控件：panelRight（承载编辑区）
            this.splitContainer.Panel2.Controls.Add(this.panelRight);
            // 分割容器尺寸：宽1570px，高789px（适配窗体最大化尺寸）
            this.splitContainer.Size = new System.Drawing.Size(1570, 789);
            // 分割线位置：左侧面板宽度391px，右侧自动填充剩余空间
            this.splitContainer.SplitterDistance = 391;
            // Tab索引：控件切换顺序（0=第一个可聚焦的容器）
            this.splitContainer.TabIndex = 0;

            // ========== 第四步：配置左侧面板（panelLeft）属性 ==========
            // 
            // panelLeft（左侧功能区容器）
            // 
            // 关闭自动滚动：避免面板滚动条与列表滚动条冲突
            this.panelLeft.AutoScroll = false;
            // 隐藏垂直滚动条（强制关闭，避免冗余）
            this.panelLeft.VerticalScroll.Visible = false;
            // 添加子控件：按层级顺序（列表在最上层，按钮在底部）
            this.panelLeft.Controls.Add(this.lstMemos);    // 备忘录列表（填充剩余空间）
            this.panelLeft.Controls.Add(this.btnDelete);  // 删除按钮（底部）
            this.panelLeft.Controls.Add(this.btnSearch);  // 查询按钮（删除按钮上方）
            this.panelLeft.Controls.Add(this.btnAdd);     // 新增按钮（查询按钮上方）
            // 停靠方式：填充分割容器左侧面板（Panel1）
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            // 位置：相对于分割容器Panel1的坐标（0,0）
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            // 控件名称：供代码引用
            this.panelLeft.Name = "panelLeft";
            // 内边距：上下左右各12px（控件与面板边缘的间距）
            this.panelLeft.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            // 尺寸：宽391px（分割容器Panel1宽度），高789px
            this.panelLeft.Size = new System.Drawing.Size(391, 789);
            // Tab索引：0（容器级索引）
            this.panelLeft.TabIndex = 0;

            // ========== 第五步：配置删除按钮（btnDelete）属性 ==========
            // 
            // btnDelete（删除备忘录按钮）
            // 
            // 自动尺寸模式：根据文字内容自动调整大小（避免文字截断）
            this.btnDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // MaterialButton密度：默认（Compact=紧凑，Default=常规，Comfortable=宽松）
            this.btnDelete.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            // MaterialSkin阴影深度：0（无阴影，平面样式）
            this.btnDelete.Depth = 0;
            // 停靠方式：底部停靠（宽度填充面板，高度自适应）
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Bottom;
            // 高强调色：文字/背景对比度更高，更醒目
            this.btnDelete.HighEmphasis = true;
            // 按钮图标：无（纯文字按钮）
            this.btnDelete.Icon = null;
            // 位置：相对于panelLeft的坐标（X=12, Y=669），适配内边距和上方按钮高度
            this.btnDelete.Location = new System.Drawing.Point(12, 669);
            // 外边距：上下8px，左右4px（与其他按钮保持统一间距）
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            // 默认鼠标状态：悬浮（HOVER），提升交互体验
            this.btnDelete.MouseState = MaterialSkin.MouseState.HOVER;
            // 控件名称：供代码引用（如btnDelete.Click）
            this.btnDelete.Name = "btnDelete";
            // 无强调色时的文字颜色：使用系统默认（避免文字与背景冲突）
            this.btnDelete.NoAccentTextColor = System.Drawing.Color.Empty;
            // 尺寸：宽367px（面板宽度-内边距12*2），高36px（统一按钮高度）
            this.btnDelete.Size = new System.Drawing.Size(367, 36);
            // Tab索引：3（切换顺序：列表=0→新增=1→查询=2→删除=3）
            this.btnDelete.TabIndex = 3;
            // 按钮显示文字
            this.btnDelete.Text = "删除备忘录";
            // MaterialButton类型：包含式（Contained，有背景色的实心按钮）
            this.btnDelete.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            // 不使用强调色：使用MaterialSkin主色（而非次要强调色）
            this.btnDelete.UseAccentColor = false;
            // 使用系统默认背景色：适配MaterialSkin主题
            this.btnDelete.UseVisualStyleBackColor = true;
            // 绑定点击事件：触发btnDelete_Click方法（业务逻辑在Form1.cs）
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // ========== 第六步：配置查询按钮（btnSearch）属性（核心新增） ==========
            // 
            // btnSearch（查询备忘录按钮）
            // 
            // 自动尺寸模式：根据文字自动调整大小
            this.btnSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // MaterialButton密度：默认
            this.btnSearch.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            // 阴影深度：0（平面样式）
            this.btnSearch.Depth = 0;
            // 停靠方式：底部停靠（在删除按钮上方）
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
            // 高强调色：提升视觉优先级
            this.btnSearch.HighEmphasis = true;
            // 按钮图标：无
            this.btnSearch.Icon = null;
            // 位置：相对于panelLeft的坐标（X=12, Y=705），在删除按钮上方
            this.btnSearch.Location = new System.Drawing.Point(12, 705);
            // 外边距：与其他按钮统一（上下8px，左右4px）
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            // 默认鼠标状态：悬浮
            this.btnSearch.MouseState = MaterialSkin.MouseState.HOVER;
            // 控件名称：供代码引用
            this.btnSearch.Name = "btnSearch";
            // 无强调色时的文字颜色：默认
            this.btnSearch.NoAccentTextColor = System.Drawing.Color.Empty;
            // 尺寸：与删除按钮一致（宽367px，高36px），保持布局统一
            this.btnSearch.Size = new System.Drawing.Size(367, 36);
            // Tab索引：2（介于新增和删除之间）
            this.btnSearch.TabIndex = 2;
            // 按钮显示文字
            this.btnSearch.Text = "查询备忘录";
            // MaterialButton类型：包含式
            this.btnSearch.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            // 不使用强调色
            this.btnSearch.UseAccentColor = false;
            // 使用系统默认背景色（基础样式）
            this.btnSearch.UseVisualStyleBackColor = true;
            // 自定义背景色：钢蓝色（ARGB：70,130,180），区分查询按钮
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(70, 130, 180);
            // 自定义文字颜色：白色，提升对比度
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            // 绑定点击事件：触发btnSearch_Click方法（业务逻辑在Form1.cs）
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            // ========== 第七步：配置新增按钮（btnAdd）属性 ==========
            // 
            // btnAdd（新增备忘录按钮）
            // 
            // 自动尺寸模式：根据文字自动调整大小
            this.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // MaterialButton密度：默认
            this.btnAdd.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            // 阴影深度：0（平面样式）
            this.btnAdd.Depth = 0;
            // 停靠方式：底部停靠（在查询按钮上方）
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            // 高强调色：提升视觉优先级
            this.btnAdd.HighEmphasis = true;
            // 按钮图标：无
            this.btnAdd.Icon = null;
            // 位置：相对于panelLeft的坐标（X=12, Y=741），在查询按钮上方
            this.btnAdd.Location = new System.Drawing.Point(12, 741);
            // 外边距：与其他按钮统一
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            // 默认鼠标状态：悬浮
            this.btnAdd.MouseState = MaterialSkin.MouseState.HOVER;
            // 控件名称：供代码引用
            this.btnAdd.Name = "btnAdd";
            // 无强调色时的文字颜色：默认
            this.btnAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            // 尺寸：与其他按钮统一（宽367px，高36px）
            this.btnAdd.Size = new System.Drawing.Size(367, 36);
            // Tab索引：1（列表=0→新增=1→查询=2→删除=3）
            this.btnAdd.TabIndex = 1;
            // 按钮显示文字
            this.btnAdd.Text = "新增备忘录";
            // MaterialButton类型：包含式
            this.btnAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            // 不使用强调色
            this.btnAdd.UseAccentColor = false;
            // 使用系统默认背景色
            this.btnAdd.UseVisualStyleBackColor = true;
            // 绑定点击事件：触发btnAdd_Click方法（业务逻辑在Form1.cs）
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // ========== 第八步：配置备忘录列表（lstMemos）属性 ==========
            // 
            // lstMemos（核心列表控件）
            // 
            // 列表背景色：白色（适配MaterialSkin主题）
            this.lstMemos.BackColor = System.Drawing.Color.White;
            // 列表边框颜色：浅灰色（弱化边框，提升美观度）
            this.lstMemos.BorderColor = System.Drawing.Color.LightGray;
            // MaterialSkin阴影深度：0（平面样式）
            this.lstMemos.Depth = 0;
            // 停靠方式：填充面板剩余空间（按钮下方的所有区域）
            this.lstMemos.Dock = System.Windows.Forms.DockStyle.Fill;
            // 外边距：底部100px（为底部按钮预留空间）
            this.lstMemos.Margin = new System.Windows.Forms.Padding(0, 0, 0, 100);
            // 列表尺寸：宽367px（面板宽度-内边距），高689px
            this.lstMemos.Size = new System.Drawing.Size(367, 689);
            // 重复设置边框颜色：确保样式生效（兼容自定义控件）
            this.lstMemos.BorderColor = System.Drawing.Color.LightGray;
            // 列表字体：微软雅黑，10号（清晰易读）
            this.lstMemos.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            // 位置：相对于panelLeft的坐标（X=12, Y=12），适配内边距
            this.lstMemos.Location = new System.Drawing.Point(12, 12);
            // 默认鼠标状态：悬浮
            this.lstMemos.MouseState = MaterialSkin.MouseState.HOVER;
            // 控件名称：供代码引用（如lstMemos.SelectedItem）
            this.lstMemos.Name = "lstMemos";
            // 默认选中索引：-1（未选中任何项，避免默认选中第一个）
            this.lstMemos.SelectedIndex = -1;
            // 默认选中项：null（未选中任何项）
            this.lstMemos.SelectedItem = null;
            // 列表尺寸（最终）：宽367px，高765px（填充按钮上方所有空间）
            this.lstMemos.Size = new System.Drawing.Size(367, 765);
            // Tab索引：0（第一个可聚焦的控件，优先响应键盘操作）
            this.lstMemos.TabIndex = 0;

            // ========== 第九步：配置右侧面板（panelRight）属性 ==========
            // 
            // panelRight（右侧编辑区容器）
            // 
            // 添加子控件：按层级（底部面板→保存按钮→富文本框）
            this.panelRight.Controls.Add(this.panelBottom);  // 底部信息面板
            this.panelRight.Controls.Add(this.btnSave);      // 保存按钮
            this.panelRight.Controls.Add(this.rtbContent);   // 富文本编辑框
            // 停靠方式：填充分割容器右侧面板（Panel2）
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            // 位置：相对于分割容器Panel2的坐标（0,0）
            this.panelRight.Location = new System.Drawing.Point(0, 0);
            // 控件名称：供代码引用
            this.panelRight.Name = "panelRight";
            // 内边距：上下左右各12px（控件与面板边缘的间距）
            this.panelRight.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            // 尺寸：宽1175px（分割容器宽度-左侧391px-分割线宽度），高789px
            this.panelRight.Size = new System.Drawing.Size(1175, 789);
            // Tab索引：0（容器级索引）
            this.panelRight.TabIndex = 0;

            // ========== 第十步：配置底部面板（panelBottom）属性 ==========
            // 
            // panelBottom（信息标签容器）
            // 
            // 添加子控件：文件信息标签（lblFileInfo）
            this.panelBottom.Controls.Add(this.lblFileInfo);
            // 停靠方式：底部停靠（宽度填充右侧面板）
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            // 位置：相对于panelRight的坐标（X=12, Y=705），适配内边距
            this.panelBottom.Location = new System.Drawing.Point(12, 705);
            // 控件名称：供代码引用
            this.panelBottom.Name = "panelBottom";
            // 尺寸：宽1151px（右侧面板宽度-内边距），高36px（与按钮高度统一）
            this.panelBottom.Size = new System.Drawing.Size(1151, 36);
            // Tab索引：2（保存按钮=1→底部面板=2）
            this.panelBottom.TabIndex = 2;

            // ========== 第十一步：配置文件信息标签（lblFileInfo）属性 ==========
            // 
            // lblFileInfo（状态提示标签）
            // 
            // MaterialSkin阴影深度：0（平面样式）
            this.lblFileInfo.Depth = 0;
            // 停靠方式：填充底部面板（宽度自适应）
            this.lblFileInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            // 字体：Roboto（MaterialSkin默认字体），14px，常规样式（适配主题）
            this.lblFileInfo.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            // 位置：相对于panelBottom的坐标（0,0）
            this.lblFileInfo.Location = new System.Drawing.Point(0, 0);
            // 默认鼠标状态：悬浮
            this.lblFileInfo.MouseState = MaterialSkin.MouseState.HOVER;
            // 控件名称：供代码引用（如lblFileInfo.Text = "xxx"）
            this.lblFileInfo.Name = "lblFileInfo";
            // 尺寸：宽1151px，高36px（填充底部面板）
            this.lblFileInfo.Size = new System.Drawing.Size(1151, 36);
            // Tab索引：0（面板内唯一控件）
            this.lblFileInfo.TabIndex = 0;
            // 默认提示文字：未选择文件时的占位符
            this.lblFileInfo.Text = "未选择文件 | 创建时间：- | 修改时间：-";
            // 文字对齐方式：水平左对齐，垂直居中（提升可读性）
            this.lblFileInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ========== 第十二步：配置保存按钮（btnSave）属性 ==========
            // 
            // btnSave（保存备忘录按钮）
            // 
            // 自动尺寸模式：根据文字自动调整大小
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // MaterialButton密度：默认
            this.btnSave.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            // 阴影深度：0（平面样式）
            this.btnSave.Depth = 0;
            // 停靠方式：底部停靠（在底部面板上方）
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            // 高强调色：提升视觉优先级
            this.btnSave.HighEmphasis = true;
            // 按钮图标：无
            this.btnSave.Icon = null;
            // 位置：相对于panelRight的坐标（X=12, Y=741），适配内边距
            this.btnSave.Location = new System.Drawing.Point(12, 741);
            // 外边距：与其他按钮统一
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            // 默认鼠标状态：悬浮
            this.btnSave.MouseState = MaterialSkin.MouseState.HOVER;
            // 控件名称：供代码引用
            this.btnSave.Name = "btnSave";
            // 无强调色时的文字颜色：默认
            this.btnSave.NoAccentTextColor = System.Drawing.Color.Empty;
            // 尺寸：宽1151px（右侧面板宽度-内边距），高36px
            this.btnSave.Size = new System.Drawing.Size(1151, 36);
            // Tab索引：1（富文本框=0→保存按钮=1）
            this.btnSave.TabIndex = 1;
            // 按钮显示文字
            this.btnSave.Text = "保存备忘录";
            // MaterialButton类型：包含式
            this.btnSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            // 不使用强调色
            this.btnSave.UseAccentColor = false;
            // 使用系统默认背景色
            this.btnSave.UseVisualStyleBackColor = true;
            // 绑定点击事件：触发btnSave_Click方法（业务逻辑在Form1.cs）
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // ========== 第十三步：配置富文本编辑框（rtbContent）属性 ==========
            // 
            // rtbContent（核心编辑控件）
            // 
            // 滚动条样式：仅垂直滚动条（避免水平滚动条冗余）
            this.rtbContent.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            // 停靠方式：填充右侧面板剩余空间（保存按钮上方）
            this.rtbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            // 位置：相对于panelRight的坐标（X=12, Y=12），适配内边距
            this.rtbContent.Location = new System.Drawing.Point(12, 12);
            // 控件名称：供代码引用（如rtbContent.Text = "xxx"）
            this.rtbContent.Name = "rtbContent";
            // 尺寸：宽1151px（右侧面板宽度-内边距），高765px
            this.rtbContent.Size = new System.Drawing.Size(1151, 765);
            // Tab索引：0（右侧面板第一个可聚焦控件）
            this.rtbContent.TabIndex = 0;
            // 默认文本：空字符串（无初始内容）
            this.rtbContent.Text = "";
            // 绑定内容变更事件：触发rtbContent_TextChanged（预留扩展）
            this.rtbContent.TextChanged += new System.EventHandler(this.rtbContent_TextChanged);
            // 重复设置滚动条：确保样式生效（兼容不同系统）
            this.rtbContent.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;

            // ========== 第十四步：配置主窗体（Form1）属性 ==========
            // 
            // Form1（整个应用的主窗体）
            // 
            // 自动缩放尺寸：96DPI（1F=96DPI，适配不同屏幕分辨率）
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            // 自动缩放模式：按字体缩放（保证文字大小适配）
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            // 客户端区域尺寸：宽1576px，高868px（最大化时的基础尺寸）
            this.ClientSize = new System.Drawing.Size(1576, 868);
            // 添加子控件：分割容器（唯一顶层控件）
            this.Controls.Add(this.splitContainer);
            // MaterialSkin属性：隐藏抽屉时显示图标（适配MaterialSkin主题）
            this.DrawerShowIconsWhenHidden = true;
            // 窗体名称：供代码引用（如Application.OpenForms["Form1"]）
            this.Name = "Form1";
            // 窗体内边距：上99px（MaterialSkin标题栏高度），左右3px，下3px
            this.Padding = new System.Windows.Forms.Padding(3, 99, 3, 3);
            // 窗体标题：显示在标题栏
            this.Text = "POWER BY 第八组备忘录";
            // 窗体启动状态：最大化（提升用户体验，利用全屏空间）
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            // 绑定窗体加载事件：触发Form1_Load方法（初始化样式/滚动条）
            this.Load += new System.EventHandler(this.Form1_Load);

            // ========== 第十五步：恢复布局更新（与暂停对应） ==========
            // 恢复分割容器左侧面板布局
            this.splitContainer.Panel1.ResumeLayout(false);
            // 恢复分割容器右侧面板布局
            this.splitContainer.Panel2.ResumeLayout(false);
            // 结束分割容器布局初始化
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            // 恢复分割容器整体布局
            this.splitContainer.ResumeLayout(false);
            // 恢复左侧面板布局
            this.panelLeft.ResumeLayout(false);
            // 强制更新左侧面板布局（解决控件位置偏移问题）
            this.panelLeft.PerformLayout();
            // 恢复右侧面板布局
            this.panelRight.ResumeLayout(false);
            // 强制更新右侧面板布局
            this.panelRight.PerformLayout();
            // 恢复底部面板布局
            this.panelBottom.ResumeLayout(false);
            // 恢复整个窗体布局
            this.ResumeLayout(false);
        }

        // ========== 控件声明（供Form1.cs业务逻辑引用） ==========
        // 新增：查询按钮控件声明（必须与实例化变量名一致，否则代码无法引用）
        private MaterialSkin.Controls.MaterialButton btnSearch;
        // 原有控件声明：分割容器（核心布局容器）
        private System.Windows.Forms.SplitContainer splitContainer;
        // 原有控件声明：左侧面板（列表+按钮容器）
        private System.Windows.Forms.Panel panelLeft;
        // 原有控件声明：删除按钮
        private MaterialSkin.Controls.MaterialButton btnDelete;
        // 原有控件声明：新增按钮
        private MaterialSkin.Controls.MaterialButton btnAdd;
        // 原有控件声明：自定义备忘录列表
        private NodeApp.CustomMaterialListBox lstMemos;
        // 原有控件声明：右侧面板（编辑区容器）
        private System.Windows.Forms.Panel panelRight;
        // 原有控件声明：底部面板（信息标签容器）
        private System.Windows.Forms.Panel panelBottom;
        // 原有控件声明：文件信息标签
        private MaterialSkin.Controls.MaterialLabel lblFileInfo;
        // 原有控件声明：保存按钮
        private MaterialSkin.Controls.MaterialButton btnSave;
        // 原有控件声明：富文本编辑框
        private System.Windows.Forms.RichTextBox rtbContent;
    }
}