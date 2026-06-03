using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PremiumLivingFurnitureWinForms;

public class MainForm : Form
{
    private readonly Panel content = new() { Dock = DockStyle.Fill };
    private readonly FlowLayoutPanel nav = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, Padding = new Padding(14, 12, 14, 12) };
    private readonly string currentUser;
    private readonly string currentRole;
    private ComboBox? languageSelect;
    private ComboBox? themeSelect;

    public MainForm(string user, string role)
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        currentUser = user;
        currentRole = role;
        Text = "Premium Living Furniture - Search / Language / Theme Prototype";
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(1240, 780);
        Font = ModernTheme.DefaultFont;
        BuildShell();
        ShowDashboard();
    }

    private void BuildShell()
    {
        Controls.Clear();
        BackColor = ModernTheme.Background;
        content.BackColor = ModernTheme.Background;
        Controls.Add(content);
        Controls.Add(BuildHeader());
        Controls.Add(BuildSidebar());
    }

    private Panel BuildHeader()
    {
        var header = new Panel { Dock = DockStyle.Top, Height = 76, BackColor = ModernTheme.Surface, Padding = new Padding(24, 12, 28, 12) };
        header.Controls.Add(new Label { Text = $"{currentUser} • {currentRole}", Dock = DockStyle.Right, Width = 220, TextAlign = ContentAlignment.MiddleRight, ForeColor = ModernTheme.TextMuted, Font = new Font("Segoe UI", 10, FontStyle.Bold) });

        var settingsPanel = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 470, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(0, 4, 0, 0) };
        settingsPanel.Controls.Add(new Label { Text = ModernTheme.T("Language"), Width = 75, Height = 36, TextAlign = ContentAlignment.MiddleRight, ForeColor = ModernTheme.TextMuted });
        languageSelect = ModernTheme.SelectInput(new[] { "English", "繁體中文", "简体中文" }, ModernTheme.CurrentLanguage == AppLanguage.TraditionalChinese ? "繁體中文" : ModernTheme.CurrentLanguage == AppLanguage.SimplifiedChinese ? "简体中文" : "English");
        languageSelect.Width = 120;
        languageSelect.SelectedIndexChanged += (_, _) => ChangeLanguage(languageSelect.Text);
        settingsPanel.Controls.Add(languageSelect);

        settingsPanel.Controls.Add(new Label { Text = ModernTheme.T("Theme"), Width = 60, Height = 36, TextAlign = ContentAlignment.MiddleRight, ForeColor = ModernTheme.TextMuted });
        themeSelect = ModernTheme.SelectInput(new[] { "Light", "Dark", "Blue" }, ModernTheme.CurrentTheme.ToString());
        themeSelect.Width = 105;
        themeSelect.SelectedIndexChanged += (_, _) => ChangeTheme(themeSelect.Text);
        settingsPanel.Controls.Add(themeSelect);
        header.Controls.Add(settingsPanel);

        header.Controls.Add(new Label { Text = "Premium Living Furniture Co. Ltd.", Dock = DockStyle.Left, Width = 430, TextAlign = ContentAlignment.MiddleLeft, ForeColor = ModernTheme.TextDark, Font = new Font("Segoe UI", 16, FontStyle.Bold) });
        return header;
    }

    private Panel BuildSidebar()
    {
        var sidebar = new Panel { Dock = DockStyle.Left, Width = 252, BackColor = ModernTheme.Sidebar };
        var brand = new Label { Text = "  PLF System", Dock = DockStyle.Top, Height = 72, ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft };
        nav.Controls.Clear();
        nav.BackColor = ModernTheme.Sidebar;
        sidebar.Controls.Add(nav);
        sidebar.Controls.Add(brand);
        AddNav("Dashboard", ShowDashboard);
        AddNav("Sales / Quotation", () => ShowSimpleCrud("Sales / Quotation", "Maintain quotations and customer sales records", BuildSalesTable(), "Status"));
        AddNav("Order Management", ShowOrderManagement);
        AddNav("Delivery Notes", ShowDeliveryNote);
        AddNav("Reply Slips", ShowReplySlip);
        AddNav("Invoices / AR", ShowInvoice);
        AddNav("Stock Management", ShowStockManagement);
        AddNav("Purchase Orders / AP", ShowPurchaseOrder);
        AddNav("Complaints", ShowComplaintManagement);
        AddNav("Controls Demo", ShowControlsDemo);
        AddNav("Users & Roles", ShowUserManagement);
        AddNav("Settings", ShowSettings);
        return sidebar;
    }

    private void AddNav(string key, Action action)
    {
        var b = new Button { Text = "   " + ModernTheme.T(key), Width = 216, Height = 44, FlatStyle = FlatStyle.Flat, BackColor = ModernTheme.Sidebar, ForeColor = Color.FromArgb(226, 232, 240), TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10, FontStyle.Bold), Cursor = Cursors.Hand, Margin = new Padding(0, 3, 0, 3) };
        b.FlatAppearance.BorderSize = 0;
        b.MouseEnter += (_, _) => b.BackColor = ModernTheme.SidebarHover;
        b.MouseLeave += (_, _) => b.BackColor = ModernTheme.Sidebar;
        b.Click += (_, _) => action();
        nav.Controls.Add(b);
    }

    private void ChangeLanguage(string value)
    {
        ModernTheme.SetLanguage(value == "繁體中文" ? AppLanguage.TraditionalChinese : value == "简体中文" ? AppLanguage.SimplifiedChinese : AppLanguage.English);
        BuildShell();
        ShowSettings();
    }

    private void ChangeTheme(string value)
    {
        ModernTheme.SetTheme(value == "Dark" ? AppTheme.Dark : value == "Blue" ? AppTheme.Blue : AppTheme.Light);
        BuildShell();
        ShowSettings();
    }

    private void SetContent(Control control)
    {
        content.Controls.Clear();
        control.Dock = DockStyle.Fill;
        content.Controls.Add(control);
    }

    private static Control PageTitle(string title, string subtitle)
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            ColumnCount = 1,
            BackColor = ModernTheme.Background,
            Padding = new Padding(24, 14, 24, 8)
        };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        panel.Controls.Add(new Label
        {
            Text = ModernTheme.T(title),
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            ForeColor = ModernTheme.TextDark,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true
        }, 0, 0);
        panel.Controls.Add(new Label
        {
            Text = subtitle,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = ModernTheme.TextDark,
            TextAlign = ContentAlignment.TopLeft,
            AutoEllipsis = true
        }, 0, 1);
        return panel;
    }

    private void ShowDashboard()
    {
        var page = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1, BackColor = ModernTheme.Background };
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 132));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        page.Controls.Add(PageTitle("Dashboard", "KPI summary, searchable tables, language and theme controls"), 0, 0);
        page.Controls.Add(BuildDashboardGrid(), 0, 1);
        SetContent(page);
    }

    private Control BuildDashboardGrid()
    {
        var grid = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(18, 0, 18, 18), ColumnCount = 4, RowCount = 3 };
        for (int i = 0; i < 4; i++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 180));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
        grid.Controls.Add(KpiCard("Total Orders", "128", "Confirmed + draft", Color.FromArgb(37, 99, 235)), 0, 0);
        grid.Controls.Add(KpiCard("Pending Deliveries", "24", "Need note / reply slip", Color.FromArgb(249, 115, 22)), 1, 0);
        grid.Controls.Add(KpiCard("Low Stock Alerts", "9", "Below reorder point", Color.FromArgb(220, 38, 38)), 2, 0);
        grid.Controls.Add(KpiCard("Open Complaints", "6", "Open / in progress", Color.FromArgb(124, 58, 237)), 3, 0);
        var orders = DataGridCard("Recent Orders", BuildOrderTable(), "Status");
        grid.SetColumnSpan(orders, 2);
        grid.Controls.Add(orders, 0, 1);
        var stock = DataGridCard("Stock Alerts", BuildStockTable(), "Status");
        grid.SetColumnSpan(stock, 2);
        grid.Controls.Add(stock, 2, 1);
        var info = new RoundedPanel { Dock = DockStyle.Fill, Radius = 18, BackColor = ModernTheme.Surface, Margin = new Padding(8), Padding = new Padding(18) };
        info.Controls.Add(new Label { Dock = DockStyle.Fill, Text = "New: every main page has search. ComboBox select/option fields are used for status, priority, role, warehouse, language and theme.", Font = new Font("Segoe UI", 11), ForeColor = ModernTheme.TextMuted, TextAlign = ContentAlignment.MiddleLeft });
        grid.SetColumnSpan(info, 4);
        grid.Controls.Add(info, 0, 2);
        return grid;
    }

    private static Control KpiCard(string title, string value, string subtitle, Color accent)
    {
        var outer = new RoundedPanel
        {
            Dock = DockStyle.Fill,
            Radius = 20,
            BackColor = ModernTheme.Surface,
            Margin = new Padding(8),
            Padding = new Padding(18, 14, 18, 14)
        };
        var bar = new Panel { Dock = DockStyle.Left, Width = 5, BackColor = accent };
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 4,
            ColumnCount = 1,
            Padding = new Padding(16, 0, 0, 0),
            BackColor = ModernTheme.Surface
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.Controls.Add(new Label { Text = title, Dock = DockStyle.Fill, ForeColor = ModernTheme.TextMuted, Font = new Font("Segoe UI", 10, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft, AutoEllipsis = true }, 0, 0);
        layout.Controls.Add(new Label { Text = value, Dock = DockStyle.Fill, ForeColor = ModernTheme.TextDark, Font = new Font("Segoe UI", 28, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft, AutoEllipsis = true }, 0, 1);
        layout.Controls.Add(new Label { Text = subtitle, Dock = DockStyle.Fill, ForeColor = ModernTheme.TextMuted, Font = new Font("Segoe UI", 9), TextAlign = ContentAlignment.TopLeft, AutoEllipsis = true }, 0, 2);
        outer.Controls.Add(layout);
        outer.Controls.Add(bar);
        return outer;
    }

    private static Control DataGridCard(string title, DataTable data, string filterColumn)
    {
        var card = new RoundedPanel { Dock = DockStyle.Fill, Radius = 18, BackColor = ModernTheme.Surface, Margin = new Padding(8), Padding = new Padding(16) };
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1, BackColor = ModernTheme.Surface };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.Controls.Add(new Label { Text = title, Dock = DockStyle.Fill, Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = ModernTheme.TextDark }, 0, 0);
        var searchPanel = BuildSearchPanel(data, filterColumn, out var view, out var grid);
        layout.Controls.Add(searchPanel, 0, 1);
        layout.Controls.Add(grid, 0, 2);
        card.Controls.Add(layout);
        return card;
    }

    private static string GenerateNextRowCode(DataTable data)
    {
        if (data.Columns.Count == 0 || data.Rows.Count == 0) return "P001";
        string? prefix = null;
        var width = 3;
        var maxNumber = 0;
        foreach (DataRow existingRow in data.Rows)
        {
            var value = existingRow[0]?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(value)) continue;
            var digitStart = value.Length;
            while (digitStart > 0 && char.IsDigit(value[digitStart - 1])) digitStart--;
            if (digitStart == value.Length) continue;
            var currentPrefix = value[..digitStart];
            var numberText = value[digitStart..];
            if (!int.TryParse(numberText, out var number)) continue;
            prefix ??= string.IsNullOrWhiteSpace(currentPrefix) ? "P" : currentPrefix;
            if (currentPrefix != prefix) continue;
            width = Math.Max(width, numberText.Length);
            maxNumber = Math.Max(maxNumber, number);
        }
        prefix ??= "P";
        return prefix + (maxNumber + 1).ToString("D" + width);
    }

    private static FlowLayoutPanel BuildSearchPanel(DataTable data, string filterColumn, out DataView view, out DataGridView grid)
    {
        view = new DataView(data);
        grid = CreateModernGrid(view, true);
        var localView = view;
        var localGrid = grid;
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, BackColor = ModernTheme.Surface, Padding = new Padding(0, 8, 0, 0) };
        var txt = new TextBox { Width = 200, Height = 34, Font = new Font("Segoe UI", 10), PlaceholderText = ModernTheme.T("Search"), BackColor = ModernTheme.Surface, ForeColor = ModernTheme.TextDark };
        var cmb = new ComboBox { Width = 145, Height = 34, DropDownStyle = ComboBoxStyle.DropDownList, BackColor = ModernTheme.Surface, ForeColor = ModernTheme.TextDark };

        void RefreshFilterOptions()
        {
            var selected = cmb.Text;
            cmb.Items.Clear();
            cmb.Items.Add(ModernTheme.T("All"));
            if (data.Columns.Contains(filterColumn))
                foreach (var value in data.AsEnumerable().Select(r => r[filterColumn]?.ToString() ?? "").Distinct().Where(v => v.Length > 0)) cmb.Items.Add(value);
            cmb.SelectedItem = cmb.Items.Contains(selected) ? selected : ModernTheme.T("All");
        }

        void ApplyFilter()
        {
            var parts = new System.Collections.Generic.List<string>();
            var s = txt.Text.Replace("'", "''");
            if (!string.IsNullOrWhiteSpace(s))
            {
                var cols = data.Columns.Cast<DataColumn>().Select(c => $"CONVERT([{c.ColumnName}], 'System.String') LIKE '%{s}%'");
                parts.Add("(" + string.Join(" OR ", cols) + ")");
            }
            if (cmb.SelectedIndex > 0 && data.Columns.Contains(filterColumn))
            {
                var v = cmb.Text.Replace("'", "''");
                parts.Add($"[{filterColumn}] = '{v}'");
            }
            localView.RowFilter = string.Join(" AND ", parts);
        }

        RefreshFilterOptions();
        txt.TextChanged += (_, _) => ApplyFilter();
        cmb.SelectedIndexChanged += (_, _) => ApplyFilter();

        var btnAddRow = ModernTheme.SecondaryButton(ModernTheme.T("Add Row"), 118);
        btnAddRow.Height = 34;
        btnAddRow.Margin = new Padding(8, 0, 8, 0);
        btnAddRow.Click += (_, _) =>
        {
            var row = data.NewRow();
            for (int i = 0; i < data.Columns.Count; i++) row[i] = i == 0 ? GenerateNextRowCode(data) : "";
            data.Rows.Add(row);
            RefreshFilterOptions();
            ApplyFilter();
            if (localGrid.Rows.Count > 0)
            {
                localGrid.ClearSelection();
                localGrid.Rows[localGrid.Rows.Count - 1].Selected = true;
            }
        };

        var btnAddColumn = ModernTheme.SecondaryButton(ModernTheme.T("Add Column"), 135);
        btnAddColumn.Height = 34;
        btnAddColumn.Margin = new Padding(0, 0, 8, 0);
        btnAddColumn.Click += (_, _) =>
        {
            var index = 1;
            var columnName = $"New Column {index}";
            while (data.Columns.Contains(columnName)) columnName = $"New Column {++index}";
            data.Columns.Add(columnName, typeof(string));
            foreach (DataRow row in data.Rows) row[columnName] = "";
            ApplyFilter();
        };

        var editMode = false;
        var btnModifyData = ModernTheme.SecondaryButton(ModernTheme.T("Modify Data"), 145);
        btnModifyData.Height = 34;
        btnModifyData.Margin = new Padding(0, 0, 8, 0);
        btnModifyData.Click += (_, _) =>
        {
            editMode = !editMode;
            localGrid.ReadOnly = !editMode;
            localGrid.AllowUserToDeleteRows = editMode;
            btnModifyData.Text = editMode ? ModernTheme.T("Lock Data") : ModernTheme.T("Modify Data");
            MessageBox.Show(editMode ? "Modify mode is ON. You can now edit table cells directly." : "Modify mode is OFF. Data is locked again.", "Modify Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };

        panel.Controls.Add(new Label { Text = ModernTheme.T("Search"), Width = 58, Height = 34, TextAlign = ContentAlignment.MiddleLeft, ForeColor = ModernTheme.TextMuted });
        panel.Controls.Add(txt);
        panel.Controls.Add(new Label { Text = ModernTheme.T("FilterBy"), Width = 72, Height = 34, TextAlign = ContentAlignment.MiddleRight, ForeColor = ModernTheme.TextMuted });
        panel.Controls.Add(cmb);
        panel.Controls.Add(btnAddRow);
        panel.Controls.Add(btnAddColumn);
        panel.Controls.Add(btnModifyData);
        return panel;
    }

    private static DataGridView CreateModernGrid(object dataSource, bool readOnly)
    {
        var grid = new DataGridView { DataSource = dataSource, Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = readOnly, BorderStyle = BorderStyle.None, BackgroundColor = ModernTheme.Surface, EnableHeadersVisualStyles = false, RowHeadersWidth = 28, AllowUserToAddRows = !readOnly, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
        grid.ColumnHeadersDefaultCellStyle.BackColor = ModernTheme.PrimaryLight;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = ModernTheme.TextDark;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        grid.DefaultCellStyle.BackColor = ModernTheme.Surface;
        grid.DefaultCellStyle.ForeColor = ModernTheme.TextDark;
        grid.DefaultCellStyle.Font = new Font("Segoe UI", 9);
        grid.DefaultCellStyle.SelectionBackColor = ModernTheme.PrimaryLight;
        grid.DefaultCellStyle.SelectionForeColor = ModernTheme.TextDark;
        return grid;
    }

    private void ShowControlsDemo()
    {
        var page = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1, BackColor = ModernTheme.Background };
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 132)); page.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        page.Controls.Add(PageTitle("Controls Demo", "Radio, text, checkbox, popup, range and select/options"), 0, 0);
        var card = new RoundedPanel { Dock = DockStyle.Fill, Radius = 20, BackColor = ModernTheme.Surface, Margin = new Padding(24, 0, 24, 24), Padding = new Padding(24) };
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 8, BackColor = ModernTheme.Surface };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220)); layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        for (int i = 0; i < 8; i++) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, i == 7 ? 85 : 60));
        var txtCustomer = ModernTheme.TextInput("C0001 - B2B Customer");
        var selectWarehouse = ModernTheme.SelectInput(new[] { "WH-HK-01", "WH-CN-01", "WH-VN-01", "WH-TH-01" }, "WH-HK-01");
        var selectStatus = ModernTheme.SelectInput(new[] { "Draft", "Confirmed", "Processing", "Delivered", "Closed" }, "Confirmed");
        var radioPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = ModernTheme.Surface };
        var rbB2B = new RadioButton { Text = "B2B", Checked = true, Width = 90, ForeColor = ModernTheme.TextDark };
        var rbB2C = new RadioButton { Text = "B2C", Width = 90, ForeColor = ModernTheme.TextDark };
        radioPanel.Controls.Add(rbB2B); radioPanel.Controls.Add(rbB2C);
        var checkPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = ModernTheme.Surface };
        var chkUrgent = new CheckBox { Text = "Urgent", Width = 100, ForeColor = ModernTheme.TextDark };
        var chkInvoice = new CheckBox { Text = "Need invoice", Checked = true, Width = 130, ForeColor = ModernTheme.TextDark };
        var chkCall = new CheckBox { Text = "Call before delivery", Width = 170, ForeColor = ModernTheme.TextDark };
        checkPanel.Controls.Add(chkUrgent); checkPanel.Controls.Add(chkInvoice); checkPanel.Controls.Add(chkCall);
        var rangePanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, BackColor = ModernTheme.Surface };
        rangePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); rangePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
        var track = new TrackBar { Dock = DockStyle.Fill, Minimum = 1, Maximum = 10, TickFrequency = 1, Value = 5 };
        var lblRange = new Label { Text = "5 / 10", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = ModernTheme.TextDark, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        track.Scroll += (_, _) => lblRange.Text = track.Value + " / 10";
        rangePanel.Controls.Add(track, 0, 0); rangePanel.Controls.Add(lblRange, 1, 0);
        var txtRemarks = ModernTheme.TextInput("Need morning delivery."); txtRemarks.Multiline = true;
        AddLabelAndControl(layout, 0, "Text", txtCustomer); AddLabelAndControl(layout, 1, "Select / Option", selectWarehouse); AddLabelAndControl(layout, 2, "Status Select", selectStatus); AddLabelAndControl(layout, 3, "Radio", radioPanel); AddLabelAndControl(layout, 4, "Checkbox", checkPanel); AddLabelAndControl(layout, 5, "Range", rangePanel); AddLabelAndControl(layout, 6, "Remarks", txtRemarks);
        var btns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, BackColor = ModernTheme.Surface };
        var popup = ModernTheme.PrimaryButton("Show Popup", 145);
        popup.Click += (_, _) => MessageBox.Show($"Customer: {txtCustomer.Text}\nWarehouse: {selectWarehouse.Text}\nStatus: {selectStatus.Text}\nType: {(rbB2B.Checked ? "B2B" : "B2C")}\nUrgent: {chkUrgent.Checked}\nNeed invoice: {chkInvoice.Checked}\nCall before delivery: {chkCall.Checked}\nPriority: {track.Value}/10", "Popup Window", MessageBoxButtons.OK, MessageBoxIcon.Information);
        btns.Controls.Add(popup); layout.Controls.Add(btns, 1, 7);
        card.Controls.Add(layout); page.Controls.Add(card, 0, 1); SetContent(page);
    }

    private static void AddLabelAndControl(TableLayoutPanel layout, int row, string label, Control control)
    {
        layout.Controls.Add(new Label { Text = label, Dock = DockStyle.Fill, ForeColor = ModernTheme.TextMuted, Font = new Font("Segoe UI", 9, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft }, 0, row);
        layout.Controls.Add(control, 1, row);
    }

    private void ShowOrderManagement() => SetContent(BuildEntryPage("Order Management", "Create, update, confirm and track customer orders", new[] { Field("Order No", "Text", "SO-2026-0001"), Field("Customer", "Text", "C0001 - B2B Customer"), Field("Priority", "Select", "Normal|Urgent|VIP"), Field("Status", "Select", "Draft|Confirmed|Processing|Delivered|Closed"), Field("Delivery Method", "Select", "Company Truck|Courier|Customer Pickup"), Field("Order Date", "Text", DateTime.Today.ToShortDateString()) }, BuildOrderItemsTable(), "Product ID", "Save Order", "Update Order", "Generate Delivery Note"));
    private void ShowDeliveryNote() => SetContent(BuildEntryPage("Delivery Notes", "Record dispatch details and outstanding quantity", new[] { Field("Delivery Note No", "Text", "DN-2026-0001"), Field("Order No", "Text", "SO-2026-0001"), Field("Warehouse", "Select", "WH-HK-01|WH-CN-01|WH-VN-01"), Field("Delivery Method", "Select", "Company Truck|Courier|Customer Pickup"), Field("Dispatch Date", "Text", DateTime.Today.ToShortDateString()), Field("Status", "Select", "Draft|Dispatched|Returned|Closed") }, BuildDeliveryTable(), "Outstanding", "Generate", "Print Preview", "Mark Dispatched"));
    private void ShowReplySlip() => SetContent(BuildEntryPage("Reply Slips", "Capture proof of delivery and customer receipt", new[] { Field("Reply Slip No", "Text", "RS-2026-0001"), Field("Delivery Note No", "Text", "DN-2026-0001"), Field("Received By", "Text", ""), Field("Status", "Select", "Pending|Received|Disputed|Closed"), Field("Signature Ref", "Text", "signature/photo path"), Field("Returned Date", "Text", DateTime.Today.ToShortDateString()) }, BuildDeliveryTable(), "Outstanding", "Finalize", "Trigger Complaint", "Close Delivery"));
    private void ShowInvoice() => SetContent(BuildEntryPage("Invoices / AR", "Issue billing documents and track payment status", new[] { Field("Invoice No", "Text", "INV-2026-0001"), Field("Order No", "Text", "SO-2026-0001"), Field("Payment Status", "Select", "Unpaid|Partially Paid|Paid|Overdue"), Field("Currency", "Select", "HKD|CNY|VND"), Field("Payment Ref", "Text", ""), Field("Invoice Date", "Text", DateTime.Today.ToShortDateString()) }, BuildInvoiceTable(), "Item ID", "Issue Invoice", "Record Payment", "Print Invoice"));
    private void ShowStockManagement() => SetContent(BuildEntryPage("Stock Management", "Monitor inventory and process movements", new[] { Field("Warehouse", "Select", "WH-HK-01|WH-CN-01|WH-VN-01"), Field("Item Type", "Select", "Product|Raw Material"), Field("Movement Type", "Select", "Receipt|Issue|Transfer|Adjustment"), Field("Status", "Select", "Normal|LOW STOCK|Overstock"), Field("Quantity", "Text", "0"), Field("Reason", "Text", "") }, BuildStockTable(), "Status", "Update Stock", "Create Transfer", "Create Reorder"));
    private void ShowPurchaseOrder() => SetContent(BuildEntryPage("Purchase Orders / AP", "Request, approve and confirm supplier purchases", new[] { Field("PO No", "Text", "PO-2026-0001"), Field("Supplier", "Select", "SUP-001|SUP-002|SUP-003"), Field("Related Order", "Text", "SO-2026-0001"), Field("Approval Status", "Select", "Request|Approved|Supplier Confirmed|Received"), Field("Order Date", "Text", DateTime.Today.ToShortDateString()), Field("Expected Date", "Text", DateTime.Today.AddDays(14).ToShortDateString()) }, BuildPurchaseTable(), "Status", "Create PO", "Manager Approve", "Goods Received"));
    private void ShowComplaintManagement() => SetContent(BuildEntryPage("Complaints", "Log, assign, escalate and resolve service cases", new[] { Field("Feedback ID", "Text", "FB-2026-0001"), Field("Customer", "Text", "C0001"), Field("Issue Type", "Select", "Late delivery|Damage|Wrong item|Missing item|Other"), Field("Priority", "Select", "Low|Medium|High"), Field("Status", "Select", "Open|In Progress|Resolved|Closed"), Field("Assigned To", "Select", "CS|Logistics|Warehouse|Manager") }, BuildComplaintTable(), "Status", "Submit", "Assign", "Resolve"));
    private void ShowUserManagement() => SetContent(BuildEntryPage("Users & Roles", "Maintain users, roles and account status", new[] { Field("User ID", "Text", "U0001"), Field("Username", "Text", "admin"), Field("Role", "Select", "Admin|Manager|Sales Staff|Warehouse Clerk|Finance Staff"), Field("Status", "Select", "Active|Locked|Disabled"), Field("Email", "Text", "admin@example.com"), Field("MFA", "Select", "Enabled|Disabled") }, BuildUserTable(), "Status", "Create User", "Reset Password", "Disable User"));
    private void ShowSettings() => SetContent(BuildEntryPage("Settings", "Change language, theme, thresholds and document prefixes", new[] { Field("Language", "Select", "English|繁體中文|简体中文"), Field("Theme", "Select", "Light|Dark|Blue"), Field("Currency", "Select", "HKD|CNY|VND"), Field("Low Stock Threshold", "Text", "10"), Field("Invoice Prefix", "Text", "INV"), Field("Delivery Note Prefix", "Text", "DN") }, BuildSettingsTable(), "Setting", "Save Settings", "Audit Change", "Backup Database"));

    private void ShowSimpleCrud(string title, string subtitle, DataTable table, string filterColumn) => SetContent(BuildEntryPage(title, subtitle, new[] { Field("Code / No", "Text", "Auto"), Field("Name", "Text", ""), Field("Status", "Select", "Active|Inactive|Pending"), Field("Type", "Select", "B2B|B2C|Internal"), Field("Date", "Text", DateTime.Today.ToShortDateString()), Field("Remarks", "Text", "") }, table, filterColumn, "Add", "Update", "Search"));

    private static (string Label, string Type, string Value) Field(string label, string type, string value) => (label, type, value);

    private static Control BuildEntryPage(string title, string subtitle, (string Label, string Type, string Value)[] fields, DataTable gridData, string filterColumn, params string[] buttons)
    {
        var page = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1, BackColor = ModernTheme.Background };
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 132)); page.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        page.Controls.Add(PageTitle(title, subtitle), 0, 0);
        var card = new RoundedPanel { Dock = DockStyle.Fill, Radius = 20, BackColor = ModernTheme.Surface, Margin = new Padding(24, 0, 24, 24), Padding = new Padding(22) };
        var body = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, BackColor = ModernTheme.Surface };
        body.RowStyles.Add(new RowStyle(SizeType.Absolute, 150)); body.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); body.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); body.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
        var form = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 3, Padding = new Padding(0, 0, 0, 12), BackColor = ModernTheme.Surface };
        form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145)); form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145)); form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        for (int i = 0; i < 3; i++) form.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        for (int i = 0; i < fields.Length; i++)
        {
            int row = i / 2, col = (i % 2) * 2;
            form.Controls.Add(new Label { Text = fields[i].Label, Dock = DockStyle.Fill, ForeColor = ModernTheme.TextMuted, Font = new Font("Segoe UI", 9, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft }, col, row);
            Control input = fields[i].Type == "Select" ? ModernTheme.SelectInput(fields[i].Value.Split('|'), fields[i].Value.Split('|')[0]) : ModernTheme.TextInput(fields[i].Value);
            form.Controls.Add(input, col + 1, row);
        }
        var searchPanel = BuildSearchPanel(gridData, filterColumn, out var view, out var grid);
        var buttonPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, BackColor = ModernTheme.Surface };
        bool first = true;
        foreach (var text in buttons.Reverse())
        {
            var btn = first ? ModernTheme.PrimaryButton(text, 165) : ModernTheme.SecondaryButton(text, 165);
            first = false; btn.Click += (_, _) => MessageBox.Show($"{text} completed in prototype mode.", "Popup Window"); buttonPanel.Controls.Add(btn);
        }
        body.Controls.Add(form, 0, 0); body.Controls.Add(searchPanel, 0, 1); body.Controls.Add(grid, 0, 2); body.Controls.Add(buttonPanel, 0, 3);
        card.Controls.Add(body); page.Controls.Add(card, 0, 1); return page;
    }

    private static DataTable Table(params string[] columns) { var t = new DataTable(); foreach (var c in columns) t.Columns.Add(c); return t; }
    private static DataTable BuildOrderTable(){ var t=Table("Order No","Customer","Date","Priority","Status"); t.Rows.Add("SO-2026-0001","C0001",DateTime.Today.ToShortDateString(),"VIP","Confirmed"); t.Rows.Add("SO-2026-0002","C0002",DateTime.Today.AddDays(-1).ToShortDateString(),"Normal","Processing"); return t; }
    private static DataTable BuildOrderItemsTable(){ var t=Table("Product ID","Description","Quantity","Unit Price","Discount","Line Total"); t.Rows.Add("P001","Custom Sofa","2","4500","0","9000"); t.Rows.Add("P002","Dining Chair","6","650","100","3800"); return t; }
    private static DataTable BuildDeliveryTable(){ var t=Table("Item ID","Description","Ordered","Delivered","Outstanding"); t.Rows.Add("P001","Custom Sofa","2","2","0"); t.Rows.Add("P002","Dining Chair","6","4","2"); return t; }
    private static DataTable BuildInvoiceTable(){ var t=Table("Item ID","Description","Quantity","Unit Cost","Discount","Price"); t.Rows.Add("P001","Custom Sofa","2","4500","0","9000"); t.Rows.Add("P002","Dining Chair","6","650","100","3800"); t.Rows.Add("","","","","Subtotal","12800"); return t; }
    private static DataTable BuildStockTable(){ var t=Table("Item ID","Item Type","Warehouse","Qty On Hand","Reorder Point","Status"); t.Rows.Add("P001","Product","WH-HK-01","12","5","Normal"); t.Rows.Add("RM008","Raw Material","WH-CN-01","3","10","LOW STOCK"); return t; }
    private static DataTable BuildPurchaseTable(){ var t=Table("Material ID","Description","Qty","Unit Cost","Expected Date","Status"); t.Rows.Add("RM008","Oak wood board","100","80",DateTime.Today.AddDays(14).ToShortDateString(),"Request"); return t; }
    private static DataTable BuildComplaintTable(){ var t=Table("Feedback ID","Issue Type","Priority","Assigned To","Status","Resolution Notes"); t.Rows.Add("FB-2026-0001","Late delivery","High","Logistics","Open","Pending investigation"); return t; }
    private static DataTable BuildUserTable(){ var t=Table("User ID","Username","Full Name","Role","Status","Last Login"); t.Rows.Add("U0001","admin","System Administrator","Admin","Active",DateTime.Now.ToString("g")); t.Rows.Add("U0002","sales01","Sales Staff","Sales Staff","Active",""); return t; }
    private static DataTable BuildSalesTable(){ var t=Table("Quotation No","Customer","Product","Amount","Status","Version"); t.Rows.Add("QT-2026-0001","C0001","Custom Sofa","9000","Approved","v1"); return t; }
    private static DataTable BuildSettingsTable(){ var t=Table("Setting","Value","Description"); t.Rows.Add("Language",ModernTheme.CurrentLanguage.ToString(),"Current UI language"); t.Rows.Add("Theme",ModernTheme.CurrentTheme.ToString(),"Current visual theme"); t.Rows.Add("Currency","HKD","Default currency display"); return t; }
}
