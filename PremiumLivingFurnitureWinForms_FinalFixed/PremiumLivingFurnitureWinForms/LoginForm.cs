using System;
using System.Drawing;
using System.Windows.Forms;

namespace PremiumLivingFurnitureWinForms;

public class LoginForm : Form
{
    private readonly TextBox txtUsername = new();
    private readonly TextBox txtPassword = new();
    private readonly ComboBox cboRole = new();
    private readonly CheckBox chkRemember = new();

    public LoginForm()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        Text = "Premium Living Furniture - Login";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1250, 900);
        MinimumSize = new Size(900, 560);
        BackColor = ModernTheme.Background;
        Font = ModernTheme.DefaultFont;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(42) };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 47));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 53));

        var hero = new GradientPanel { Dock = DockStyle.Fill, Radius = 26, Margin = new Padding(0, 0, 24, 0), Padding = new Padding(34) };
        hero.Controls.Add(new Label { Text = "Premium Living\nFurniture", Dock = DockStyle.Top, Height = 125, ForeColor = Color.White, Font = new Font("Segoe UI", 25, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft });
        hero.Controls.Add(new Label { Text = "Centralized Management System\nSearch • Select Options • Language • Theme", Dock = DockStyle.Top, Height = 90, ForeColor = Color.FromArgb(226, 232, 240), Font = new Font("Segoe UI", 11), TextAlign = ContentAlignment.MiddleLeft });

        var card = new RoundedPanel { Dock = DockStyle.Fill, Radius = 24, BackColor = ModernTheme.Surface, Padding = new Padding(42) };
        card.Controls.Add(BuildLoginContent());

        root.Controls.Add(hero, 0, 0);
        root.Controls.Add(card, 1, 0);
        Controls.Add(root);
    }

    private Control BuildLoginContent()
    {
        var panel = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 6, ColumnCount = 1 };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 245));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        panel.Controls.Add(new Label { Text = "Welcome back", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 24, FontStyle.Bold), ForeColor = ModernTheme.TextDark }, 0, 0);
        panel.Controls.Add(new Label { Text = "Login to manage daily furniture operations.", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10), ForeColor = ModernTheme.TextMuted }, 0, 1);

        var form = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 6, Padding = new Padding(0, 18, 0, 0) };
        txtUsername.Text = "admin";
        txtPassword.Text = "1234";
        txtPassword.UseSystemPasswordChar = true;
        cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
        cboRole.Items.AddRange(new object[] { "Admin", "Manager", "Sales Staff", "Logistics Clerk", "Warehouse Clerk", "Finance Staff", "Customer Service" });
        cboRole.SelectedIndex = 0;
        AddField(form, "Username", txtUsername);
        AddField(form, "Password", txtPassword);
        AddField(form, "Role", cboRole);
        panel.Controls.Add(form, 0, 2);

        chkRemember.Text = "Remember me";
        chkRemember.ForeColor = ModernTheme.TextMuted;
        chkRemember.Dock = DockStyle.Fill;
        panel.Controls.Add(chkRemember, 0, 3);

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        var btnLogin = ModernTheme.PrimaryButton("Login", 145);
        var btnClear = ModernTheme.SecondaryButton("Clear", 115);
        btnLogin.Click += (_, _) => Login();
        btnClear.Click += (_, _) => { txtUsername.Clear(); txtPassword.Clear(); cboRole.SelectedIndex = 0; chkRemember.Checked = false; };
        buttons.Controls.Add(btnLogin);
        buttons.Controls.Add(btnClear);
        panel.Controls.Add(buttons, 0, 4);
        return panel;
    }

    private static void AddField(TableLayoutPanel form, string label, Control input)
    {
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        form.Controls.Add(new Label { Text = label, Dock = DockStyle.Fill, ForeColor = ModernTheme.TextMuted, Font = new Font("Segoe UI", 9, FontStyle.Bold) });
        input.Dock = DockStyle.Fill;
        input.Margin = new Padding(0, 4, 0, 12);
        input.Font = new Font("Segoe UI", 11);
        form.Controls.Add(input);
    }

    private void Login()
    {
        if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            MessageBox.Show("Please enter username and password.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        Hide();
        var main = new MainForm(txtUsername.Text.Trim(), cboRole.Text);
        main.FormClosed += (_, _) => Close();
        main.Show();
    }
}
