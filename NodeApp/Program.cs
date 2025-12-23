using System;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace NodeApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ========== 1. 全局主题配置（必须在窗体创建前） ==========
            var materialSkinManager = MaterialSkinManager.Instance;
            // 先清空已绑定的窗体（避免缓存问题）
           // materialSkinManager.RemoveAllFormsFromManage();
            // 设置主题模式（LIGHT/DARK）
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            // 设置绿色主题（使用正确的枚举值）
            materialSkinManager.ColorScheme = new ColorScheme(
                  Primary.Red300,          // 主色调：豆沙粉（红调浅粉）
                    Primary.Red500,          // 主色调深色：深豆沙粉
                    Primary.Red100,          // 主色调浅色：极浅豆沙粉
                    Accent.Red200,           // 强调色：豆沙粉高亮
                    TextShade.WHITE

            );

            // ========== 2. 启动主窗体 ==========
            Application.Run(new Form1());
        }
    }
}