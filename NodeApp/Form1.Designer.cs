// 定义项目核心命名空间（与主窗体类一致）
namespace NodeApp
{
    // Form1的部分类：控件初始化代码（WinForms自动生成，手动补充注释）
    // partial关键字表示类拆分到多个文件，此处为控件布局代码
    partial class Form1
    {
        /// <summary>
        /// 组件容器：管理所有窗体控件的生命周期（自动释放资源）
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 重写Dispose方法：释放控件资源（WinForms标准写法）
        /// </summary>
        /// <param name="disposing">是否释放托管资源</param>
        protected override void Dispose(bool disposing)
        {
            // 如果需要释放托管资源，且组件容器不为空
            if (disposing && (components != null))
            {
                // 释放所有控件资源（避免内存泄漏）
                components.Dispose();
            }
            // 调用基类的Dispose方法（释放非托管资源）
            base.Dispose(disposing);
        }

        /// <summary>
        /// 初始化组件：核心控件布局、属性、事件绑定
        /// WinForms自动生成，手动调整了查询按钮的布局和属性
        /// </summary>
        private void InitializeComponent()
        {
            // 1. 实例化控件对象（从最外层容器开始）
            // 分割容器：将窗体分为左右两部分（左侧列表，右侧编辑区）
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            // 左侧面板：承载备忘录列表和操作按钮
            this.panelLeft = new System.Windows.Forms.Panel();
            // 删除按钮：MaterialSkin样式的按钮
            this.btnDelete = new MaterialSkin.Controls.MaterialButton();
            // 新增：查询按钮（MaterialSkin样式）
            this.btnSearch = new MaterialSkin.Controls.MaterialButton();
            // 新增按钮：MaterialSkin样式的按钮
            this.btnAdd = new MaterialSkin.Controls.MaterialButton();
            // 自定义备忘录列表控件（继承自MaterialListBox）
            this.lstMemos = new NodeApp.CustomMaterialListBox();
            // 右侧面板：承载富文本编辑框和保存按钮
            this.panelRight = new System.Windows.Forms.Panel();
            // 底部面板：承载文件信息标签
            this.panelBottom = new System.Windows.Forms.Panel();
            // 文件信息标签：显示文件名、创建/修改时间
            this.lblFileInfo = new MaterialSkin.Controls.MaterialLabel();
            // 保存按钮：MaterialSkin样式的按钮
            this.btnSave = new MaterialSkin.Controls.MaterialButton();
            // 富文本编辑框：编辑备忘录内容
            this.rtbContent = new System.Windows.Forms.RichTextBox();

            // 2. 初始化分割容器的布局（BeginInit/EndInit是WinForms标准写法，避免布局闪烁）
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            // 暂停左侧面板布局（批量设置属性，提升性能）
            this.splitContainer.Panel1.SuspendLayout();
            // 暂停右侧面板布局
            this.splitContainer.Panel2.SuspendLayout();
            // 暂停分割容器布局
            this.splitContainer.SuspendLayout();
            // 暂停左侧面板布局
            this.panelLeft.SuspendLayout();
            // 暂停右侧面板布局
            this.panelRight.SuspendLayout();
            // 暂停底部面板布局
            this.panelBottom.SuspendLayout();
            // 暂停整个窗体布局
            this.SuspendLayout();

            // ===================== 分割容器（splitContainer）属性设置 =====================
            // 
            // splitContainer
            // 
            // 停靠方式：填充整个父容器（窗体）
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            // 位置：相对于窗体的坐标（X=3, Y=76，对应窗体Padding的3,99,3,3）
            this.splitContainer.Location = new System.Drawing.Point(3, 76);
            // 控件名称（用于代码引用）
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1（左侧面板容器）
            // 
            // 左侧面板添加子控件：panelLeft
            this.splitContainer.Panel1.Controls.Add(this.panelLeft);
            // 
            // splitContainer.Panel2（右侧面板容器）
            // 
            // 右侧面板添加子控件：panelRight
            this.splitContainer.Panel2.Controls.Add(this.panelRight);
            // 分割容器尺寸：宽1570px，高789px
            this.splitContainer.Size = new System.Drawing.Size(1570, 789);
            // 分割线位置：左侧面板宽度391px，右侧自动填充剩余空间
            this.splitContainer.SplitterDistance = 391;
            // Tab索引：控件切换顺序（0为第一个）
            this.splitContainer.TabIndex = 0;

            // ===================== 左侧面板（panelLeft）属性设置 =====================
            // 
            // panelLeft
            // 
            // 添加子控件：删除按钮、查询按钮、新增按钮、备忘录列表
            this.panelLeft.Controls.Add(this.btnDelete);
            this.panelLeft.Controls.Add(this.btnSearch); // 新增：添加查询按钮到面板
            this.panelLeft.Controls.Add(this.btnAdd);
            this.panelLeft.Controls.Add(this.lstMemos);
            // 停靠方式：填充分割容器左侧面板
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            // 位置：相对于分割容器Panel1的坐标（0,0）
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            // 控件名称
            this.panelLeft.Name = "panelLeft";
            // 内边距：上下左右各12px（避免控件贴边）
            this.panelLeft.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            // 尺寸：宽391px，高789px（与分割容器Panel1一致）
            this.panelLeft.Size = new System.Drawing.Size(391, 789);
            // Tab索引
            this.panelLeft.TabIndex = 0;

            // ===================== 删除按钮（btnDelete）属性设置 =====================
            // 
            // btnDelete
            // 
            // 自动尺寸模式：根据内容自动增长/收缩
            this.btnDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // MaterialButton密度：默认（Compact=紧凑，Default=默认，Comfortable=宽松）
            this.btnDelete.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            // MaterialSkin阴影深度：0（无阴影）
            this.btnDelete.Depth = 0;
            // 停靠方式：底部停靠（宽度填充面板，高度自适应）
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Bottom;
            // 高对比度：文字/背景更醒目
            this.btnDelete.HighEmphasis = true;
            // 按钮图标：无
            this.btnDelete.Icon = null;
            // 位置：相对于panelLeft的坐标（X=12, Y=669），为查询按钮腾出空间
            this.btnDelete.Location = new System.Drawing.Point(12, 669);
            // 外边距：上下8px，左右4px（与其他按钮间距一致）
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            // 默认鼠标状态：悬浮（HOVER）
            this.btnDelete.MouseState = MaterialSkin.MouseState.HOVER;
            // 控件名称
            this.btnDelete.Name = "btnDelete";
            // 无强调色时的文字颜色：使用默认
            this.btnDelete.NoAccentTextColor = System.Drawing.Color.Empty;
            // 尺寸：宽367px（panelLeft宽度 - 左右内边距12*2），高36px
            this.btnDelete.Size = new System.Drawing.Size(367, 36);
            // Tab索引：3（查询=2，新增=1，列表=0）
            this.btnDelete.TabIndex = 3;
            // 按钮文字
            this.btnDelete.Text = "删除备忘录";
            // MaterialButton类型：包含式（Contained，有背景色）
            this.btnDelete.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            // 不使用强调色（使用主色）
            this.btnDelete.UseAccentColor = false;
            // 使用系统默认背景色
            this.btnDelete.UseVisualStyleBackColor = true;
            // 绑定点击事件：触发btnDelete_Click方法
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // ===================== 查询按钮（btnSearch）属性设置（新增） =====================
            // 
            // btnSearch（新增：查询备忘录按钮）
            // 
            // 自动尺寸模式：根据内容自动增长/收缩
            this.btnSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // MaterialButton密度：默认
            this.btnSearch.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            // 阴影深度：0
            this.btnSearch.Depth = 0;
            // 停靠方式：底部停靠（在删除按钮上方）
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
            // 高对比度
            this.btnSearch.HighEmphasis = true;
            // 按钮图标：无
            this.btnSearch.Icon = null;
            // 位置：相对于panelLeft的坐标（X=12, Y=705），在删除按钮上方
            this.btnSearch.Location = new System.Drawing.Point(12, 705);
            // 外边距：与其他按钮一致
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            // 默认鼠标状态：悬浮
            this.btnSearch.MouseState = MaterialSkin.MouseState.HOVER;
            // 控件名称
            this.btnSearch.Name = "btnSearch";
            // 无强调色时的文字颜色：默认
            this.btnSearch.NoAccentTextColor = System.Drawing.Color.Empty;
            // 尺寸：与删除按钮一致（宽367px，高36px）
            this.btnSearch.Size = new System.Drawing.Size(367, 36);
            // Tab索引：2（介于新增和删除之间）
            this.btnSearch.TabIndex = 2;
            // 按钮文字
            this.btnSearch.Text = "查询备忘录";
            // MaterialButton类型：包含式
            this.btnSearch.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            // 不使用强调色
            this.btnSearch.UseAccentColor = false;
            // 使用系统默认背景色
            this.btnSearch.UseVisualStyleBackColor = true;
            // 自定义背景色：钢蓝色（ARGB：70,130,180）
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(70, 130, 180);
            // 文字颜色：白色
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            // 绑定点击事件：触发btnSearch_Click方法
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            // ===================== 新增按钮（btnAdd）属性设置 =====================
            // 
            // btnAdd
            // 
            // 自动尺寸模式：根据内容自动增长/收缩
            this.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // MaterialButton密度：默认
            this.btnAdd.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            // 阴影深度：0
            this.btnAdd.Depth = 0;
            // 停靠方式：底部停靠（在查询按钮上方）
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            // 高对比度
            this.btnAdd.HighEmphasis = true;
            // 按钮图标：无
            this.btnAdd.Icon = null;
            // 位置：相对于panelLeft的坐标（X=12, Y=741），在查询按钮上方
            this.btnAdd.Location = new System.Drawing.Point(12, 741);
            // 外边距：与其他按钮一致
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            // 默认鼠标状态：悬浮
            this.btnAdd.MouseState = MaterialSkin.MouseState.HOVER;
            // 控件名称
            this.btnAdd.Name = "btnAdd";
            // 无强调色时的文字颜色：默认
            this.btnAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            // 尺寸：与其他按钮一致（宽367px，高36px）
            this.btnAdd.Size = new System.Drawing.Size(367, 36);
            // Tab索引：1
            this.btnAdd.TabIndex = 1;
            // 按钮文字
            this.btnAdd.Text = "新增备忘录";
            // MaterialButton类型：包含式
            this.btnAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            // 不使用强调色
            this.btnAdd.UseAccentColor = false;
            // 使用系统默认背景色
            this.btnAdd.UseVisualStyleBackColor = true;
            // 绑定点击事件：触发btnAdd_Click方法
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // ===================== 备忘录列表（lstMemos）属性设置 =====================
            // 
            // lstMemos
            // 
            // 背景色：白色
            this.lstMemos.BackColor = System.Drawing.Color.White;
            // 边框颜色：浅灰色
            this.lstMemos.BorderColor = System.Drawing.Color.LightGray;
            // MaterialSkin阴影深度：0
            this.lstMemos.Depth = 0;
            // 停靠方式：填充面板剩余空间（按钮下方的所有区域）
            this.lstMemos.Dock = System.Windows.Forms.DockStyle.Fill;
            // 字体：微软雅黑，10号
            this.lstMemos.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            // 位置：相对于panelLeft的坐标（X=12, Y=12），对应内边距
            this.lstMemos.Location = new System.Drawing.Point(12, 12);
            // 默认鼠标状态：悬浮
            this.lstMemos.MouseState = MaterialSkin.MouseState.HOVER;
            // 控件名称
            this.lstMemos.Name = "lstMemos";
            // 默认选中索引：-1（未选中任何项）
            this.lstMemos.SelectedIndex = -1;
            // 默认选中项：null（未选中任何项）
            this.lstMemos.SelectedItem = null;
            // 尺寸：宽367px（panelLeft宽度 - 内边距），高765px
            this.lstMemos.Size = new System.Drawing.Size(367, 765);
            // Tab索引：0（第一个可聚焦的控件）
            this.lstMemos.TabIndex = 0;

            // ===================== 右侧面板（panelRight）属性设置 =====================
            // 
            // panelRight
            // 
            // 添加子控件：底部面板、保存按钮、富文本编辑框
            this.panelRight.Controls.Add(this.panelBottom);
            this.panelRight.Controls.Add(this.btnSave);
            this.panelRight.Controls.Add(this.rtbContent);
            // 停靠方式：填充分割容器右侧面板
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            // 位置：相对于分割容器Panel2的坐标（0,0）
            this.panelRight.Location = new System.Drawing.Point(0, 0);
            // 控件名称
            this.panelRight.Name = "panelRight";
            // 内边距：上下左右各12px
            this.panelRight.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            // 尺寸：宽1175px（分割容器宽度 - 左侧391px - 分割线宽度），高789px
            this.panelRight.Size = new System.Drawing.Size(1175, 789);
            // Tab索引
            this.panelRight.TabIndex = 0;

            // ===================== 底部面板（panelBottom）属性设置 =====================
            // 
            // panelBottom
            // 
            // 添加子控件：文件信息标签
            this.panelBottom.Controls.Add(this.lblFileInfo);
            // 停靠方式：底部停靠
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            // 位置：相对于panelRight的坐标（X=12, Y=705）
            this.panelBottom.Location = new System.Drawing.Point(12, 705);
            // 控件名称
            this.panelBottom.Name = "panelBottom";
            // 尺寸：宽1151px（panelRight宽度 - 内边距），高36px
            this.panelBottom.Size = new System.Drawing.Size(1151, 36);
            // Tab索引
            this.panelBottom.TabIndex = 2;

            // ===================== 文件信息标签（lblFileInfo）属性设置 =====================
            // 
            // lblFileInfo
            // 
            // MaterialSkin阴影深度：0
            this.lblFileInfo.Depth = 0;
            // 停靠方式：填充底部面板
            this.lblFileInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            // 字体：Roboto（MaterialSkin默认字体），14px，常规样式
            this.lblFileInfo.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            // 位置：相对于panelBottom的坐标（0,0）
            this.lblFileInfo.Location = new System.Drawing.Point(0, 0);
            // 默认鼠标状态：悬浮
            this.lblFileInfo.MouseState = MaterialSkin.MouseState.HOVER;
            // 控件名称
            this.lblFileInfo.Name = "lblFileInfo";
            // 尺寸：宽1151px，高36px
            this.lblFileInfo.Size = new System.Drawing.Size(1151, 36);
            // Tab索引
            this.lblFileInfo.TabIndex = 0;
            // 标签文字：默认提示（未选择文件）
            this.lblFileInfo.Text = "未选择文件 | 创建时间：- | 修改时间：-";
            // 文字对齐方式：水平左对齐，垂直居中
            this.lblFileInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ===================== 保存按钮（btnSave）属性设置 =====================
            // 
            // btnSave
            // 
            // 自动尺寸模式：根据内容自动增长/收缩
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // MaterialButton密度：默认
            this.btnSave.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            // 阴影深度：0
            this.btnSave.Depth = 0;
            // 停靠方式：底部停靠（在底部面板上方）
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            // 高对比度
            this.btnSave.HighEmphasis = true;
            // 按钮图标：无
            this.btnSave.Icon = null;
            // 位置：相对于panelRight的坐标（X=12, Y=741）
            this.btnSave.Location = new System.Drawing.Point(12, 741);
            // 外边距：与其他按钮一致
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            // 默认鼠标状态：悬浮
            this.btnSave.MouseState = MaterialSkin.MouseState.HOVER;
            // 控件名称
            this.btnSave.Name = "btnSave";
            // 无强调色时的文字颜色：默认
            this.btnSave.NoAccentTextColor = System.Drawing.Color.Empty;
            // 尺寸：宽1151px（panelRight宽度 - 内边距），高36px
            this.btnSave.Size = new System.Drawing.Size(1151, 36);
            // Tab索引：1
            this.btnSave.TabIndex = 1;
            // 按钮文字
            this.btnSave.Text = "保存备忘录";
            // MaterialButton类型：包含式
            this.btnSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            // 不使用强调色
            this.btnSave.UseAccentColor = false;
            // 使用系统默认背景色
            this.btnSave.UseVisualStyleBackColor = true;
            // 绑定点击事件：触发btnSave_Click方法
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // ===================== 富文本编辑框（rtbContent）属性设置 =====================
            // 
            // rtbContent
            // 
            // 停靠方式：填充panelRight剩余空间（保存按钮上方）
            this.rtbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            // 位置：相对于panelRight的坐标（X=12, Y=12），对应内边距
            this.rtbContent.Location = new System.Drawing.Point(12, 12);
            // 控件名称
            this.rtbContent.Name = "rtbContent";
            // 尺寸：宽1151px，高765px
            this.rtbContent.Size = new System.Drawing.Size(1151, 765);
            // Tab索引：0
            this.rtbContent.TabIndex = 0;
            // 默认文字：空
            this.rtbContent.Text = "";
            // 绑定文本变更事件：触发rtbContent_TextChanged方法
            this.rtbContent.TextChanged += new System.EventHandler(this.rtbContent_TextChanged);

            // ===================== 主窗体（Form1）属性设置 =====================
            // 
            // Form1
            // 
            // 自动缩放尺寸：96DPI（1F=96DPI，9F/18F对应字体缩放）
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            // 自动缩放模式：按字体缩放（适配不同DPI）
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            // 客户端区域尺寸：宽1576px，高868px
            this.ClientSize = new System.Drawing.Size(1576, 868);
            // 添加子控件：分割容器
            this.Controls.Add(this.splitContainer);
            // MaterialSkin属性：隐藏抽屉时显示图标
            this.DrawerShowIconsWhenHidden = true;
            // 窗体名称
            this.Name = "Form1";
            // 窗体内边距：上99px（MaterialSkin标题栏高度），左右3px，下3px
            this.Padding = new System.Windows.Forms.Padding(3, 99, 3, 3);
            // 窗体标题
            this.Text = "POWER BY 第八组备忘录";
            // 窗体启动状态：最大化
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            // 绑定窗体加载事件：触发Form1_Load方法
            this.Load += new System.EventHandler(this.Form1_Load);

            // 3. 恢复布局（与BeginInit对应，完成布局初始化）
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            // 强制布局更新（解决控件位置偏移问题）
            this.panelLeft.PerformLayout();
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        // ===================== 控件声明（供代码引用） =====================
        // 新增：声明查询按钮控件（必须与实例化的变量名一致）
        private MaterialSkin.Controls.MaterialButton btnSearch;
        // 原有控件声明
        private System.Windows.Forms.SplitContainer splitContainer;       // 分割容器
        private System.Windows.Forms.Panel panelLeft;                     // 左侧面板
        private MaterialSkin.Controls.MaterialButton btnDelete;           // 删除按钮
        private MaterialSkin.Controls.MaterialButton btnAdd;              // 新增按钮
        private NodeApp.CustomMaterialListBox lstMemos;                   // 自定义备忘录列表
        private System.Windows.Forms.Panel panelRight;                    // 右侧面板
        private System.Windows.Forms.Panel panelBottom;                   // 底部面板
        private MaterialSkin.Controls.MaterialLabel lblFileInfo;          // 文件信息标签
        private MaterialSkin.Controls.MaterialButton btnSave;             // 保存按钮
        private System.Windows.Forms.RichTextBox rtbContent;              // 富文本编辑框
    }
}