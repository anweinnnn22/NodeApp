// 引入MaterialSkin控件库（自定义按钮继承自MaterialButton）
using MaterialSkin.Controls;
// 引入绘图相关命名空间（用于绘制圆角、渐变、阴影）
using System.Drawing;
// 引入高级绘图命名空间（用于抗锯齿、渐变等高质量绘图）
using System.Drawing.Drawing2D;
// 引入WinForms基础控件命名空间
using System.Windows.Forms;

// 定义命名空间，与项目整体结构保持一致
namespace NodeApp
{
    /// <summary>
    /// 自定义MaterialButton控件
    /// 扩展功能：圆角、阴影、渐变背景、自定义样式属性
    /// 继承自MaterialSkin的MaterialButton，保留原有交互逻辑，仅重写绘制逻辑
    /// </summary>
    public class CustomMaterialButton : MaterialButton
    {
        #region 自定义样式属性（可外部配置，提供默认值）
        /// <summary>
        /// 按钮圆角半径（默认10px，值越大圆角越明显）
        /// </summary>
        public int CornerRadius { get; set; } = 10;

        /// <summary>
        /// 阴影偏移量（X/Y轴，默认向右/向下各2px）
        /// Point(X,Y)：X正=右移，Y正=下移；负值则反向偏移
        /// </summary>
        public Point ShadowOffset { get; set; } = new Point(2, 2);

        /// <summary>
        /// 阴影颜色（默认灰色）
        /// 可外部修改为任意Color，如Color.Black/Color.LightGray
        /// </summary>
        public Color ShadowColor { get; set; } = Color.Gray;

        /// <summary>
        /// 阴影不透明度（0-255，默认80）
        /// 0=完全透明，255=完全不透明，80为半透明效果
        /// </summary>
        public int ShadowOpacity { get; set; } = 80;

        /// <summary>
        /// 是否启用渐变背景（默认启用）
        /// false=纯色背景，true=基于BackColor的浅渐变背景
        /// </summary>
        public bool UseGradient { get; set; } = true;
        #endregion

        #region 重写绘制方法（核心：自定义按钮外观）
        /// <summary>
        /// 重写OnPaint方法，自定义按钮的绘制逻辑
        /// OnPaint是WinForms控件的核心绘制方法，每次控件刷新都会执行
        /// </summary>
        /// <param name="e">绘图事件参数，包含绘图画布(Graphics)和绘图区域</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // 1. 基础绘图优化（必加，解决锯齿/颜色丢失/模糊问题）
            // 开启抗锯齿，让圆角、文字边缘更平滑
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            // 开启高质量合成，保证渐变/阴影颜色过渡自然
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            // 清空按钮背景（使用父容器背景色），避免多次绘制导致颜色叠加异常
            e.Graphics.Clear(Parent.BackColor);

            // 2. 绘制按钮阴影（视觉分层，让按钮有立体感）
            // 使用using语句自动释放画笔资源，避免内存泄漏
            using (var shadowBrush = new SolidBrush(Color.FromArgb(ShadowOpacity, ShadowColor)))
            {
                // 计算阴影的绘制区域：基于按钮原始区域 + 阴影偏移量
                var shadowRect = new Rectangle(
                    ClientRectangle.X + ShadowOffset.X,  // 阴影X坐标 = 按钮X + 偏移X
                    ClientRectangle.Y + ShadowOffset.Y,  // 阴影Y坐标 = 按钮Y + 偏移Y
                    ClientRectangle.Width,               // 阴影宽度 = 按钮宽度
                    ClientRectangle.Height);              // 阴影高度 = 按钮高度

                // 创建阴影的圆角路径（与按钮主体圆角一致）
                using (var shadowPath = new GraphicsPath())
                {
                    // 绘制左上角圆角（角度180°-270°，半径=CornerRadius*2）
                    shadowPath.AddArc(shadowRect.X, shadowRect.Y, CornerRadius * 2, CornerRadius * 2, 180, 90);
                    // 绘制右上角圆角（角度270°-360°）
                    shadowPath.AddArc(shadowRect.Right - CornerRadius * 2, shadowRect.Y, CornerRadius * 2, CornerRadius * 2, 270, 90);
                    // 绘制右下角圆角（角度0°-90°）
                    shadowPath.AddArc(shadowRect.Right - CornerRadius * 2, shadowRect.Bottom - CornerRadius * 2, CornerRadius * 2, CornerRadius * 2, 0, 90);
                    // 绘制左下角圆角（角度90°-180°）
                    shadowPath.AddArc(shadowRect.X, shadowRect.Bottom - CornerRadius * 2, CornerRadius * 2, CornerRadius * 2, 90, 90);
                    // 闭合路径（将四个圆角连接成完整的矩形）
                    shadowPath.CloseAllFigures();
                    // 用阴影画刷填充圆角路径，绘制最终阴影
                    e.Graphics.FillPath(shadowBrush, shadowPath);
                }
            }

            // 3. 绘制按钮主体（核心：圆角+渐变/纯色背景）
            // 创建按钮主体的圆角路径（与阴影路径形状一致，无偏移）
            using (var buttonPath = new GraphicsPath())
            {
                // 绘制左上角圆角（基于按钮原始区域）
                buttonPath.AddArc(ClientRectangle.X, ClientRectangle.Y, CornerRadius * 2, CornerRadius * 2, 180, 90);
                // 绘制右上角圆角
                buttonPath.AddArc(ClientRectangle.Right - CornerRadius * 2, ClientRectangle.Y, CornerRadius * 2, CornerRadius * 2, 270, 90);
                // 绘制右下角圆角
                buttonPath.AddArc(ClientRectangle.Right - CornerRadius * 2, ClientRectangle.Bottom - CornerRadius * 2, CornerRadius * 2, CornerRadius * 2, 0, 90);
                // 绘制左下角圆角
                buttonPath.AddArc(ClientRectangle.X, ClientRectangle.Bottom - CornerRadius * 2, CornerRadius * 2, CornerRadius * 2, 90, 90);
                // 闭合路径，形成完整的圆角按钮轮廓
                buttonPath.CloseAllFigures();

                // 定义按钮背景画刷（渐变/纯色二选一）
                Brush buttonBrush;
                // 判断是否启用渐变，且按钮背景色非透明
                if (UseGradient && BackColor != Color.Transparent)
                {
                    // 生成渐变起始色：基于BackColor浅15%（避免颜色偏差过大）
                    Color lightColor = ControlPaint.Light(BackColor, 0.15f);
                    // 创建线性渐变画刷（从左上到右下的渐变）
                    buttonBrush = new LinearGradientBrush(
                        ClientRectangle,       // 渐变区域=按钮区域
                        BackColor,             // 渐变起始色=按钮背景色
                        lightColor,            // 渐变结束色=浅15%的背景色
                        LinearGradientMode.ForwardDiagonal); // 渐变方向：左上→右下
                }
                else
                {
                    // 禁用渐变时，使用纯色画刷（兜底方案，确保颜色100%显示）
                    buttonBrush = new SolidBrush(BackColor);
                }

                // 用画刷填充按钮圆角路径，绘制按钮背景
                e.Graphics.FillPath(buttonBrush, buttonPath);
                // 手动释放画刷资源（using仅包裹path，画刷需单独释放）
                buttonBrush.Dispose();
            }

            // 4. 绘制按钮文字（居中显示，保证文字颜色/字体正常）
            // 创建文字格式对象，设置文字水平+垂直居中
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,    // 水平居中
                LineAlignment = StringAlignment.Center // 垂直居中
            };
            // 创建文字画刷（使用按钮的ForeColor作为文字颜色）
            using (var textBrush = new SolidBrush(ForeColor))
            {
                // 绘制文字：内容(Text)、字体(Font)、画刷、区域、格式
                e.Graphics.DrawString(Text, Font, textBrush, ClientRectangle, sf);
            }
        }
        #endregion
    }
}