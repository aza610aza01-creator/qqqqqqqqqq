using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PremiumLivingFurnitureWinForms;

public enum AppTheme
{
    Light,
    Dark,
    Blue
}

public enum AppLanguage
{
    English,
    TraditionalChinese,
    SimplifiedChinese
}

public static class ModernTheme
{
    public static AppTheme CurrentTheme { get; private set; } = AppTheme.Light;
    public static AppLanguage CurrentLanguage { get; private set; } = AppLanguage.English;

    public static Color Background { get; private set; } = Color.FromArgb(200, 200, 202);
    public static Color Surface { get; private set; } = Color.LightGray;
    public static Color Sidebar { get; private set; } = Color.FromArgb(15, 23, 42);
    public static Color SidebarHover { get; private set; } = Color.FromArgb(30, 41, 59);
    public static Color Primary { get; private set; } = Color.FromArgb(37, 99, 235);
    public static Color PrimaryLight { get; private set; } = Color.FromArgb(219, 234, 254);
    public static Color Border { get; private set; } = Color.FromArgb(226, 232, 240);
    public static Color TextDark { get; private set; } = Color.FromArgb(15, 23, 42);
    public static Color TextMuted { get; private set; } = Color.FromArgb(100, 116, 139);
    public static readonly Font DefaultFont = new("Segoe UI", 10);

    public static void SetTheme(AppTheme theme)
    {
        CurrentTheme = theme;
        if (theme == AppTheme.Dark)
        {
            Background = Color.FromArgb(15, 23, 42);
            Surface = Color.FromArgb(30, 41, 59);
            Sidebar = Color.FromArgb(2, 6, 23);
            SidebarHover = Color.FromArgb(51, 65, 85);
            Primary = Color.FromArgb(96, 165, 250);
            PrimaryLight = Color.FromArgb(30, 64, 175);
            Border = Color.FromArgb(71, 85, 105);
            TextDark = Color.FromArgb(248, 250, 252);
            TextMuted = Color.FromArgb(203, 213, 225);
        }
        else if (theme == AppTheme.Blue)
        {
            Background = Color.FromArgb(239, 246, 255);
            Surface = Color.White;
            Sidebar = Color.FromArgb(30, 58, 138);
            SidebarHover = Color.FromArgb(37, 99, 235);
            Primary = Color.FromArgb(29, 78, 216);
            PrimaryLight = Color.FromArgb(219, 234, 254);
            Border = Color.FromArgb(191, 219, 254);
            TextDark = Color.FromArgb(15, 23, 42);
            TextMuted = Color.FromArgb(71, 85, 105);
        }
        else
        {
            Background = Color.FromArgb(248, 250, 252);
            Surface = Color.White;
            Sidebar = Color.FromArgb(15, 23, 42);
            SidebarHover = Color.FromArgb(30, 41, 59);
            Primary = Color.FromArgb(37, 99, 235);
            PrimaryLight = Color.FromArgb(219, 234, 254);
            Border = Color.FromArgb(226, 232, 240);
            TextDark = Color.FromArgb(15, 23, 42);
            TextMuted = Color.FromArgb(100, 116, 139);
        }
    }

    public static void SetLanguage(AppLanguage language) => CurrentLanguage = language;

    public static string T(string key)
    {
        if (CurrentLanguage == AppLanguage.TraditionalChinese)
        {
            return key switch
            {
                "Dashboard" => "儀表板",
                "Search" => "搜尋",
                "FilterBy" => "篩選",
                "All" => "全部",
                "Theme" => "主題",
                "Language" => "語言",
                "Settings" => "設定",
                "Order Management" => "訂單管理",
                "Delivery Notes" => "送貨單",
                "Reply Slips" => "回條",
                "Invoices / AR" => "發票 / 應收帳",
                "Stock Management" => "庫存管理",
                "Purchase Orders / AP" => "採購單 / 應付帳",
                "Complaints" => "投訴管理",
                "Users & Roles" => "用戶及角色",
                "Controls Demo" => "控制項示範",
                "Sales / Quotation" => "銷售 / 報價",
                "Add Row" => "新增列",
                "Add Column" => "新增欄",
                "Modify Data" => "修改資料",
                "Lock Data" => "鎖定資料",
                _ => key
            };
        }
        if (CurrentLanguage == AppLanguage.SimplifiedChinese)
        {
            return key switch
            {
                "Dashboard" => "仪表板",
                "Search" => "搜索",
                "FilterBy" => "筛选",
                "All" => "全部",
                "Theme" => "主题",
                "Language" => "语言",
                "Settings" => "设置",
                "Order Management" => "订单管理",
                "Delivery Notes" => "送货单",
                "Reply Slips" => "回条",
                "Invoices / AR" => "发票 / 应收账",
                "Stock Management" => "库存管理",
                "Purchase Orders / AP" => "采购单 / 应付账",
                "Complaints" => "投诉管理",
                "Users & Roles" => "用户及角色",
                "Controls Demo" => "控件演示",
                "Sales / Quotation" => "销售 / 报价",
                "Add Row" => "新增行",
                "Add Column" => "新增列",
                "Modify Data" => "修改数据",
                "Lock Data" => "锁定数据",
                _ => key
            };
        }
        return key;
    }

    public static Button PrimaryButton(string text, int width = 145)
    {
        var b = new Button { Text = text, Width = width, Height = 40, FlatStyle = FlatStyle.Flat, BackColor = Primary, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Margin = new Padding(0, 8, 10, 8), Cursor = Cursors.Hand };
        b.FlatAppearance.BorderSize = 0;
        return b;
    }

    public static Button SecondaryButton(string text, int width = 125)
    {
        var b = new Button { Text = text, Width = width, Height = 40, FlatStyle = FlatStyle.Flat, BackColor = Surface, ForeColor = TextDark, Font = new Font("Segoe UI", 9, FontStyle.Bold), Margin = new Padding(0, 8, 10, 8), Cursor = Cursors.Hand };
        b.FlatAppearance.BorderColor = Border;
        b.FlatAppearance.BorderSize = 1;
        return b;
    }

    public static TextBox TextInput(string text = "") => new()
    {
        Text = text,
        Dock = DockStyle.Fill,
        Font = new Font("Segoe UI", 10),
        Margin = new Padding(0, 6, 20, 6),
        BorderStyle = BorderStyle.FixedSingle,
        BackColor = Surface,
        ForeColor = TextDark
    };

    public static ComboBox SelectInput(string[] options, string selected = "")
    {
        var c = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10), Margin = new Padding(0, 6, 20, 6), BackColor = Surface, ForeColor = TextDark };
        c.Items.AddRange(options);
        if (!string.IsNullOrWhiteSpace(selected) && c.Items.Contains(selected)) c.SelectedItem = selected;
        else if (c.Items.Count > 0) c.SelectedIndex = 0;
        return c;
    }
}

public class RoundedPanel : Panel
{
    public int Radius { get; set; } = 18;
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var path = ModernThemeHelpers.RoundRect(ClientRectangle, Radius);
        using var brush = new SolidBrush(BackColor);
        e.Graphics.FillPath(brush, path);
    }
    protected override void OnResize(EventArgs e) { base.OnResize(e); Invalidate(); }
}

public class GradientPanel : RoundedPanel
{
    public Color StartColor { get; set; } = ModernTheme.Primary;
    public Color EndColor { get; set; } = Color.FromArgb(14, 165, 233);
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var path = ModernThemeHelpers.RoundRect(ClientRectangle, Radius);
        using var brush = new LinearGradientBrush(ClientRectangle, StartColor, EndColor, 45f);
        e.Graphics.FillPath(brush, path);
    }
}

public static class ModernThemeHelpers
{
    public static GraphicsPath RoundRect(Rectangle bounds, int radius)
    {
        if (bounds.Width <= 0 || bounds.Height <= 0) return new GraphicsPath();
        int d = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
