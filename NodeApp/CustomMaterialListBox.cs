using MaterialSkin.Controls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NodeApp
{
    // 简化版：仅支持纯色背景的自定义列表
    public class CustomMaterialListBox : MaterialListBox
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            // 1. 基础绘图优化（保证背景色渲染平滑）
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            // 2. 绘制纯色背景（核心）
            e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            // 3. 调用原方法绘制列表项（保留原有功能）
            base.OnPaint(e);
        }

        // 背景色变化时刷新界面
        protected override void OnBackColorChanged(System.EventArgs e)
        {
            base.OnBackColorChanged(e);
            Invalidate();
        }
    }
}