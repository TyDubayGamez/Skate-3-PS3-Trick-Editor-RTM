using System.Windows.Forms;
using MetroFramework.Controls;

namespace Skate3TrickModifier
{
    partial class Form1
    {
        // CORE STRUCT COMPONENTS
        // Form designer components container
        private System.ComponentModel.IContainer components = null;

        // DISPOSAL LAYOUTS
        // Clean up used resources
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // WINDOW CONFIGURATION SETUP
        // Initialize controls and settings
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.tabControl = new MetroFramework.Controls.MetroTabControl();
            this.tabConnection = new MetroFramework.Controls.MetroTabPage();
            this.tabTricks = new MetroFramework.Controls.MetroTabPage();
            this.tabAnimReplacer = new MetroFramework.Controls.MetroTabPage();
            
            this.lblIP = new MetroFramework.Controls.MetroLabel();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnAttach = new System.Windows.Forms.Button();
            this.lblStatus = new MetroFramework.Controls.MetroLabel();

            this.cmbTrick1 = new System.Windows.Forms.ComboBox();
            this.txtVal1 = new System.Windows.Forms.TextBox();
            this.btnWrite1 = new System.Windows.Forms.Button();
            this.btnReset1 = new System.Windows.Forms.Button();

            this.cmbTrick2 = new System.Windows.Forms.ComboBox();
            this.txtVal2 = new System.Windows.Forms.TextBox();
            this.btnWrite2 = new System.Windows.Forms.Button();
            this.btnReset2 = new System.Windows.Forms.Button();

            this.cmbTrick3 = new System.Windows.Forms.ComboBox();
            this.txtVal3 = new System.Windows.Forms.TextBox();
            this.btnWrite3 = new System.Windows.Forms.Button();
            this.btnReset3 = new System.Windows.Forms.Button();

            this.cmbTrick4 = new System.Windows.Forms.ComboBox();
            this.txtVal4 = new System.Windows.Forms.TextBox();
            this.btnWrite4 = new System.Windows.Forms.Button();
            this.btnReset4 = new System.Windows.Forms.Button();

            this.lblReplaceHeader = new Label();
            this.lblWithHeader = new Label();
            this.cmbAnimTarget = new System.Windows.Forms.ComboBox();
            this.cmbAnimSource = new System.Windows.Forms.ComboBox();
            this.btnAnimInject = new System.Windows.Forms.Button();
            this.btnAnimReset = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabConnection.SuspendLayout();
            this.tabTricks.SuspendLayout();
            this.tabAnimReplacer.SuspendLayout();
            this.SuspendLayout();

            this.metroStyleManager1.Owner = this;
            this.metroStyleManager1.Style = MetroFramework.MetroColorStyle.Magenta;
            this.metroStyleManager1.Theme = MetroFramework.MetroThemeStyle.Dark;

            this.tabControl.Controls.Add(this.tabConnection);
            this.tabControl.Controls.Add(this.tabTricks);
            this.tabControl.Controls.Add(this.tabAnimReplacer);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(20, 60);
            this.tabControl.Size = new System.Drawing.Size(660, 280);
            this.tabControl.Theme = MetroFramework.MetroThemeStyle.Dark;

            // CONNECTION PANEL CONTROLS
            // Add connection tab elements
            this.tabConnection.Controls.Add(this.lblIP);
            this.tabConnection.Controls.Add(this.txtIP);
            this.tabConnection.Controls.Add(this.btnConnect);
            this.tabConnection.Controls.Add(this.btnAttach);
            this.tabConnection.Controls.Add(this.lblStatus);
            this.tabConnection.HorizontalScrollbarBarColor = true;
            this.tabConnection.Text = "Connection";
            this.tabConnection.Theme = MetroFramework.MetroThemeStyle.Dark;

            this.lblIP.Location = new System.Drawing.Point(23, 15);
            this.lblIP.Size = new System.Drawing.Size(150, 23);
            this.lblIP.Text = "PS3 IP ADDRESS";
            this.lblIP.Theme = MetroFramework.MetroThemeStyle.Dark;
            
            this.txtIP.Location = new System.Drawing.Point(23, 40);
            this.txtIP.Size = new System.Drawing.Size(150, 23);
            this.txtIP.BackColor = System.Drawing.Color.FromArgb(25, 25, 25);
            this.txtIP.ForeColor = System.Drawing.Color.White;
            this.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIP.Text = "10.0.0.4";

            StyleDarkButton(this.btnConnect, "CONNECT", 23, 75, 150, 40);
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            StyleDarkButton(this.btnAttach, "ATTACH", 23, 125, 150, 40);
            this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);

            this.lblStatus.Location = new System.Drawing.Point(23, 180);
            this.lblStatus.Size = new System.Drawing.Size(150, 23);
            this.lblStatus.Text = "Status: Idle";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatus.Theme = MetroFramework.MetroThemeStyle.Dark;

            // TRICK PANEL PAGES
            // Setup trick data categories
            this.tabTricks.HorizontalScrollbarBarColor = true;
            this.tabTricks.Text = "Trick Values";
            this.tabTricks.Theme = MetroFramework.MetroThemeStyle.Dark;

            int startY = 32;
            int rowSpacing = 54; 

            BuildRowLayout(this.tabTricks, "Flip Tricks", cmbTrick1, txtVal1, btnWrite1, btnReset1, startY + (rowSpacing * 0));
            this.btnWrite1.Click += new System.EventHandler(this.btnWrite1_Click);
            this.btnReset1.Click += new System.EventHandler(this.btnReset1_Click);

            BuildRowLayout(this.tabTricks, "Grab Tricks", cmbTrick2, txtVal2, btnWrite2, btnReset2, startY + (rowSpacing * 1));
            this.btnWrite2.Click += new System.EventHandler(this.btnWrite2_Click);
            this.btnReset2.Click += new System.EventHandler(this.btnReset2_Click);

            BuildRowLayout(this.tabTricks, "Grind Tricks", cmbTrick3, txtVal3, btnWrite3, btnReset3, startY + (rowSpacing * 2));
            this.btnWrite3.Click += new System.EventHandler(this.btnWrite3_Click);
            this.btnReset3.Click += new System.EventHandler(this.btnReset3_Click);

            BuildRowLayout(this.tabTricks, "Misc Tricks", cmbTrick4, txtVal4, btnWrite4, btnReset4, startY + (rowSpacing * 3));
            this.btnWrite4.Click += new System.EventHandler(this.btnWrite4_Click);
            this.btnReset4.Click += new System.EventHandler(this.btnReset4_Click);

            // ANIMATION SWAP DESIGN CONTEXTS
            // Build animation selector layout
            this.tabAnimReplacer.HorizontalScrollbarBarColor = true;
            this.tabAnimReplacer.Text = "Animation Replacer";
            this.tabAnimReplacer.Theme = MetroFramework.MetroThemeStyle.Dark;

            this.lblReplaceHeader.Text = "REPLACE";
            this.lblReplaceHeader.Location = new System.Drawing.Point(15, 25);
            this.lblReplaceHeader.Size = new System.Drawing.Size(250, 15);
            this.lblReplaceHeader.ForeColor = System.Drawing.Color.FromArgb(170, 170, 170);
            this.lblReplaceHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F);
            this.lblReplaceHeader.BackColor = System.Drawing.Color.Transparent;

            this.lblWithHeader.Text = "WITH";
            this.lblWithHeader.Location = new System.Drawing.Point(285, 25);
            this.lblWithHeader.Size = new System.Drawing.Size(250, 15);
            this.lblWithHeader.ForeColor = System.Drawing.Color.FromArgb(170, 170, 170);
            this.lblWithHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F);
            this.lblWithHeader.BackColor = System.Drawing.Color.Transparent;

            this.cmbAnimTarget.Location = new System.Drawing.Point(15, 43);
            this.cmbAnimTarget.Size = new System.Drawing.Size(250, 25);
            this.cmbAnimTarget.FlatStyle = FlatStyle.Flat;
            this.cmbAnimTarget.BackColor = System.Drawing.Color.FromArgb(25, 25, 25);
            this.cmbAnimTarget.ForeColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.cmbAnimTarget.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbAnimTarget.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAnimTarget.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CmbTrick_DrawItem);

            this.cmbAnimSource.Location = new System.Drawing.Point(285, 43);
            this.cmbAnimSource.Size = new System.Drawing.Size(250, 25);
            this.cmbAnimSource.FlatStyle = FlatStyle.Flat;
            this.cmbAnimSource.BackColor = System.Drawing.Color.FromArgb(25, 25, 25);
            this.cmbAnimSource.ForeColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.cmbAnimSource.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbAnimSource.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAnimSource.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CmbTrick_DrawItem);

            StyleDarkButton(this.btnAnimInject, "Inject", 15, 85, 110, 30);
            this.btnAnimInject.Click += new System.EventHandler(this.btnAnimInject_Click);

            StyleDarkButton(this.btnAnimReset, "Default", 140, 85, 110, 30);
            this.btnAnimReset.Click += new System.EventHandler(this.btnAnimReset_Click);

            this.tabAnimReplacer.Controls.Add(this.lblReplaceHeader);
            this.tabAnimReplacer.Controls.Add(this.lblWithHeader);
            this.tabAnimReplacer.Controls.Add(this.cmbAnimTarget);
            this.tabAnimReplacer.Controls.Add(this.cmbAnimSource);
            this.tabAnimReplacer.Controls.Add(this.btnAnimInject);
            this.tabAnimReplacer.Controls.Add(this.btnAnimReset);

            // MAIN CONTAINER STYLES
            // Set main window dimensions
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 360);
            this.Controls.Add(this.tabControl);
            this.Name = "Form1";
            this.Text = "Skate 3 Trick Editor";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabConnection.ResumeLayout(false);
            this.tabTricks.ResumeLayout(false);
            this.tabAnimReplacer.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        // ROW DESIGN FACTORIES
        // Generate UI row options
        private void BuildRowLayout(MetroTabPage page, string categoryLabel, ComboBox cmb, TextBox txt, System.Windows.Forms.Button btnW, System.Windows.Forms.Button btnR, int yPos)
        {
            Label lblCatHeader = new Label();
            lblCatHeader.Text = categoryLabel;
            lblCatHeader.Location = new System.Drawing.Point(15, yPos - 18);
            lblCatHeader.Size = new System.Drawing.Size(200, 15);
            lblCatHeader.ForeColor = System.Drawing.Color.FromArgb(170, 170, 170);
            lblCatHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F);
            lblCatHeader.BackColor = System.Drawing.Color.Transparent;

            Label lblValHeader = new Label();
            lblValHeader.Text = "Value";
            lblValHeader.Location = new System.Drawing.Point(330, yPos - 18);
            lblValHeader.Size = new System.Drawing.Size(80, 15);
            lblValHeader.ForeColor = System.Drawing.Color.FromArgb(170, 170, 170);
            lblValHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F);
            lblValHeader.BackColor = System.Drawing.Color.Transparent;

            cmb.Location = new System.Drawing.Point(15, yPos); 
            cmb.Size = new System.Drawing.Size(300, 25);
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.BackColor = System.Drawing.Color.FromArgb(25, 25, 25);
            cmb.ForeColor = System.Drawing.Color.FromArgb(240, 240, 240);
            cmb.Font = new System.Drawing.Font("Segoe UI", 9F);
            cmb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            cmb.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CmbTrick_DrawItem);
            
            txt.Location = new System.Drawing.Point(330, yPos); txt.Size = new System.Drawing.Size(110, 23);
            txt.BackColor = System.Drawing.Color.FromArgb(25, 25, 25);
            txt.ForeColor = System.Drawing.Color.FromArgb(240, 240, 240);
            txt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            StyleDarkButton(btnW, "Inject", 455, yPos, 85, 25);
            StyleDarkButton(btnR, "Default", 550, yPos, 85, 25);

            page.Controls.Add(lblCatHeader);
            page.Controls.Add(lblValHeader);
            page.Controls.Add(cmb);
            page.Controls.Add(txt);
            page.Controls.Add(btnW);
            page.Controls.Add(btnR);
        }

        // CONTROL RENDERERS
        // Apply custom dark buttons
        private void StyleDarkButton(System.Windows.Forms.Button btn, string labelText, int x, int y, int width, int height)
        {
            btn.Text = labelText;
            btn.Location = new System.Drawing.Point(x, y);
            btn.Size = new System.Drawing.Size(width, height);
            btn.BackColor = System.Drawing.Color.FromArgb(34, 34, 34);
            btn.ForeColor = System.Drawing.Color.FromArgb(240, 240, 240); 
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(65, 65, 65);
            btn.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
        }

        #endregion

        // OBJECT STRUCT REFERENCES
        // Define UI control elements
        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private MetroFramework.Controls.MetroTabControl tabControl;
        private MetroFramework.Controls.MetroTabPage tabConnection;
        private MetroFramework.Controls.MetroTabPage tabTricks;
        private MetroFramework.Controls.MetroTabPage tabAnimReplacer;
        
        private MetroFramework.Controls.MetroLabel lblIP, lblStatus;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Button btnConnect, btnAttach;

        private System.Windows.Forms.ComboBox cmbTrick1, cmbTrick2, cmbTrick3, cmbTrick4;
        private System.Windows.Forms.TextBox txtVal1, txtVal2, txtVal3, txtVal4;
        private System.Windows.Forms.Button btnWrite1, btnWrite2, btnWrite3, btnWrite4;
        private System.Windows.Forms.Button btnReset1, btnReset2, btnReset3, btnReset4;

        private System.Windows.Forms.Label lblReplaceHeader, lblWithHeader;
        private System.Windows.Forms.ComboBox cmbAnimTarget, cmbAnimSource;
        private System.Windows.Forms.Button btnAnimInject, btnAnimReset;
    }
}