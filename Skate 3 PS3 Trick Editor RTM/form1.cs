using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using MetroFramework.Forms;
using MetroFramework.Controls;
using PS3ManagerAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Skate3TrickModifier
{
    public partial class Form1 : MetroForm
    {
        // CORE RTM API INSTANCES
        // Setup PS3MAPI initialization
        private PS3MAPI MAPI = new PS3MAPI();
        private bool isAttached = false;
        private List<TrickItem> trickDatabase = new List<TrickItem>();
        private TrickSlot[] slots = new TrickSlot[4];


        // Handle target trick
        public class AnimItem
        {
            public string Name { get; set; }
            public object Address { get; set; }
            public string Aob { get; set; }

            public override string ToString() => Name ?? "Unknown Animation";

            public List<ulong> GetAddresses()
            {
                List<ulong> list = new List<ulong>();
                if (Address == null) return list;

                if (Address is string str)
                {
                    string clean = str.Replace("0x", "").Trim();
                    if (!string.IsNullOrEmpty(clean) && ulong.TryParse(clean, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong parsed))
                        list.Add(parsed);
                }
                else if (Address is JArray arr)
                {
                    foreach (var token in arr)
                    {
                        string clean = token.ToString().Replace("0x", "").Trim();
                        if (!string.IsNullOrEmpty(clean) && ulong.TryParse(clean, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong parsed))
                            list.Add(parsed);
                    }
                }
                return list;
            }

            public byte[] GetAobBytes()
            {
                if (string.IsNullOrEmpty(Aob)) return null;

                // Clean up any trailing comment lines like "----" from the JSON line string safely
                string cleanAob = Aob;
                int commentIndex = cleanAob.IndexOf("//");
                if (commentIndex >= 0) cleanAob = cleanAob.Substring(0, commentIndex);
                commentIndex = cleanAob.IndexOf("-");
                if (commentIndex >= 0) cleanAob = cleanAob.Substring(0, commentIndex);

                string[] hexParts = cleanAob.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                byte[] bytes = new byte[hexParts.Length];
                for (int i = 0; i < hexParts.Length; i++)
                {
                    bytes[i] = byte.Parse(hexParts[i], NumberStyles.HexNumber);
                }
                return bytes;
            }
        }

        // DATA STRUCTURES
        // Trick data element structure
        public class TrickItem
        {
            public string description { get; set; }
            public string address { get; set; }
            public string type { get; set; }

            public override string ToString()
            {
                if (string.IsNullOrEmpty(description)) return "Unknown Trick";
                
                int bracketIndex = description.IndexOf('[');
                if (bracketIndex > 0)
                {
                    return description.Substring(0, bracketIndex).Trim();
                }
                return description;
            }
        }

        // UI MANAGEMENT COUPLING
        public class TrickSlot
        {
            public ComboBox ComboBox { get; set; } 
            public TextBox TxtValue { get; set; }
            public uint CachedOriginalValue { get; set; }
            public bool HasOriginalCached { get; set; }
            public string TargetCategory { get; set; }
        }

        // CACHE MEMORY
        // Retain unmodded background trick blocks
        private Dictionary<ulong, byte[]> originalAobCache = new Dictionary<ulong, byte[]>();

        // === WINDOW CONSTRUCTOR INITS ===
        // Primary program execution engine startup
        public Form1()
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager1;
            
            this.DisplayHeader = true;
            this.ControlBox = true; 
            this.Resizable = false;
            this.MaximizeBox = false;

            // Auto-fill IP
            txtIP.Text = "192.168.254.122";

            LoadTrickDatabase();
            InitializeSlotMapping();
            LoadAnimationDatabase();
        }

        // DATABASE LOADING
        // Read JSON files
        private void LoadTrickDatabase()
        {
            try
            {
                string jsonPath = Path.Combine(Application.StartupPath, "trick values2.json");
                if (File.Exists(jsonPath))
                {
                    string rawJson = File.ReadAllText(jsonPath);
                    trickDatabase = JsonConvert.DeserializeObject<List<TrickItem>>(rawJson) ?? new List<TrickItem>();
                }
                else
                {
                    MessageBox.Show("trick values2.json not found in execution directory.", "Configuration Missing");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading trick database: " + ex.Message);
            }
        }

        // JSON ANIMATION PARSING
        // Parse flips target array and sort alphabetically
        private void LoadAnimationDatabase()
        {
            try
            {
                string jsonPath = Path.Combine(Application.StartupPath, "Animation AoBs.json");
                if (!File.Exists(jsonPath)) return;

                string jsonContent = File.ReadAllText(jsonPath);
                var root = JsonConvert.DeserializeObject<JObject>(jsonContent);
                if (root != null && root["Animations"]?["Flip"] is JArray flipArray)
                {
                    List<AnimItem> animListTarget = new List<AnimItem>();
                    List<AnimItem> animListSource = new List<AnimItem>();

                    foreach (var item in flipArray)
                    {
                        var anim = item.ToObject<AnimItem>();
                        if (anim != null)
                        {
                            if (anim.GetAddresses().Count > 0)
                            {
                                animListTarget.Add(anim);
                            }
                            if (!string.IsNullOrEmpty(anim.Aob))
                            {
                                animListSource.Add(anim);
                            }
                        }
                    }

                    // Sort lists alphabetically by Name
                    animListTarget.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase));
                    animListSource.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase));

                    cmbAnimTarget.Items.Clear();
                    cmbAnimSource.Items.Clear();

                    cmbAnimTarget.Items.AddRange(animListTarget.ToArray());
                    cmbAnimSource.Items.AddRange(animListSource.ToArray());

                    // [Null] at bottom of "with" list
                    cmbAnimSource.Items.Add(new AnimItem 
                    { 
                        Name = "[NULL]", 
                        Address = null, 
                        Aob = "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00" 
                    });

                    // Default Select: "360 Inward Heel" for Target list
                    for (int i = 0; i < cmbAnimTarget.Items.Count; i++)
                    {
                        if (cmbAnimTarget.Items[i].ToString().Equals("360 Inward Heel", StringComparison.OrdinalIgnoreCase))
                        {
                            cmbAnimTarget.SelectedIndex = i;
                            break;
                        }
                    }
                    if (cmbAnimTarget.SelectedIndex == -1 && cmbAnimTarget.Items.Count > 0) cmbAnimTarget.SelectedIndex = 0;

                    // Default Select: "360 Flip" for Source list
                    for (int i = 0; i < cmbAnimSource.Items.Count; i++)
                    {
                        if (cmbAnimSource.Items[i].ToString().Equals("360 Flip", StringComparison.OrdinalIgnoreCase))
                        {
                            cmbAnimSource.SelectedIndex = i;
                            break;
                        }
                    }
                    if (cmbAnimSource.SelectedIndex == -1 && cmbAnimSource.Items.Count > 0) cmbAnimSource.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error parsing Animation AoBs: " + ex.Message);
            }
        }

        // CONTROL BINDING LOGIC
        // Map elements categories while maintaining file order structure
        private void InitializeSlotMapping()
        {
            slots[0] = new TrickSlot { ComboBox = cmbTrick1, TxtValue = txtVal1, TargetCategory = "Flip" };
            slots[1] = new TrickSlot { ComboBox = cmbTrick2, TxtValue = txtVal2, TargetCategory = "Grab" };
            slots[2] = new TrickSlot { ComboBox = cmbTrick3, TxtValue = txtVal3, TargetCategory = "Grind" };
            slots[3] = new TrickSlot { ComboBox = cmbTrick4, TxtValue = txtVal4, TargetCategory = "Misc" };

            foreach (var slot in slots)
            {
                slot.ComboBox.Items.Clear();
                
                foreach (var item in trickDatabase)
                {
                    if (item.type != null && item.type.Equals(slot.TargetCategory, StringComparison.OrdinalIgnoreCase))
                    {
                        slot.ComboBox.Items.Add(item);
                    }
                }

                if (slot.ComboBox.Items.Count > 0)
                {
                    slot.ComboBox.SelectedIndex = 0;
                }

                slot.ComboBox.SelectedIndexChanged += (s, e) => { ReadSlotCurrentValue(slot); };
            }
        }

        // TEXT DRAW RENDERING
        // Dark Dropdown
        private void CmbTrick_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox combo = sender as ComboBox;
            if (combo == null) return;
            
            Color backColor = Color.FromArgb(25, 25, 25);
            Color textNormalColor = Color.FromArgb(240, 240, 240);
            Color highlightColor = Color.FromArgb(50, 50, 50); 

            Color currentBg = (e.State & DrawItemState.Selected) == DrawItemState.Selected ? highlightColor : backColor;

            using (SolidBrush bgBrush = new SolidBrush(currentBg))
            {
                e.Graphics.FillRectangle(bgBrush, e.Bounds);
            }

            string itemText = combo.Items[e.Index].ToString();
            
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding;
            
            Rectangle textBounds = new Rectangle(e.Bounds.X + 4, e.Bounds.Y, e.Bounds.Width - 4, e.Bounds.Height);
            
            Font renderFont = e.Font ?? combo.Font ?? new Font("Segoe UI", 9f, FontStyle.Regular);
            TextRenderer.DrawText(e.Graphics, itemText, renderFont, textBounds, textNormalColor, flags);

            e.DrawFocusRectangle();
        }

        // CONNECTION INTERFACES
        // Execute targeting hardware socket link
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (MAPI.ConnectTarget(txtIP.Text, 7887))
            {
                lblStatus.Text = "Status: Connected";
                lblStatus.ForeColor = Color.Lime;
                try { MAPI.PS3.Notify("Skate 3 Trick Editor Connected"); } catch { }
            }
            else
            {
                lblStatus.Text = "Status: Connection Failed";
                lblStatus.ForeColor = Color.Crimson;
            }
        }

        // ATTACHMENT INTERFACES
        // Hook system running runtime context
        private void btnAttach_Click(object sender, EventArgs e)
        {
            if (MAPI.AttachProcess())
            {
                isAttached = true;
                lblStatus.Text = "Status: Attached to Game";
                lblStatus.ForeColor = Color.MediumPurple;
                try { MAPI.PS3.Notify("Process Attached Successfully!"); } catch { }
                RefreshAllSlots();
            }
            else
            {
                lblStatus.Text = "Status: Attachment Failed";
                lblStatus.ForeColor = Color.Crimson;
            }
        }

        // SYNCING VALUES 
        // Update values from game memory
        private void RefreshAllSlots()
        {
            if (!isAttached) return;
            foreach (var slot in slots)
            {
                ReadSlotCurrentValue(slot);
            }
        }

        // MEMORY READS
        // Pull data and display as standard integer string
        private void ReadSlotCurrentValue(TrickSlot slot)
        {
            if (!isAttached || slot.ComboBox.SelectedItem == null) return;
            try
            {
                TrickItem item = (TrickItem)slot.ComboBox.SelectedItem;
                ulong address = ulong.Parse(item.address, NumberStyles.HexNumber);

                byte[] buffer = new byte[4];
                MAPI.Process.Memory.Get(MAPI.Process.Process_Pid, address, buffer);

                Array.Reverse(buffer);
                uint runtimeValue = BitConverter.ToUInt32(buffer, 0);

                if (!slot.HasOriginalCached)
                {
                    slot.CachedOriginalValue = runtimeValue;
                    slot.HasOriginalCached = true;
                }

                // Show basic numbers instead of hex string format
                slot.TxtValue.Text = runtimeValue.ToString();
            }
            catch { }
        }

        // HARDWARE MEMORY WRITES
        // Push big endian changes
        private void CommitMemoryWrite(TrickSlot slot, uint value)
        {
            if (!isAttached || slot.ComboBox.SelectedItem == null) return;

            try
            {
                TrickItem item = (TrickItem)slot.ComboBox.SelectedItem;
                ulong address = ulong.Parse(item.address, NumberStyles.HexNumber);

                byte[] outputBytes = BitConverter.GetBytes(value);
                Array.Reverse(outputBytes);

                MAPI.Process.Memory.Set(MAPI.Process.Process_Pid, address, outputBytes);

                // Notification: {trick} value set to: {value}
                string trickName = slot.ComboBox.SelectedItem?.ToString() ?? "Unknown Trick";
                try { MAPI.PS3.Notify($"{trickName} value set to: {value}"); } catch { }

                ReadSlotCurrentValue(slot);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed modifying memory address: " + ex.Message);
            }
        }

        // Map injection click layout actions
        private void btnWrite1_Click(object sender, EventArgs e) => ParseAndWriteRow(slots[0]);
        private void btnWrite2_Click(object sender, EventArgs e) => ParseAndWriteRow(slots[1]);
        private void btnWrite3_Click(object sender, EventArgs e) => ParseAndWriteRow(slots[2]);
        private void btnWrite4_Click(object sender, EventArgs e) => ParseAndWriteRow(slots[3]);

        // VALUE CONVERSION CHECKS
        // Handles standard decimal entry input, with support for 0x hex input
        private void ParseAndWriteRow(TrickSlot slot)
        {
            string input = slot.TxtValue.Text.Trim();

            if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                if (uint.TryParse(input.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint targetHexVal))
                {
                    CommitMemoryWrite(slot, targetHexVal);
                }
            }
            else if (uint.TryParse(input, out uint targetVal))
            {
                CommitMemoryWrite(slot, targetVal);
            }
        }


        // Map default click rollback actions
        private void btnReset1_Click(object sender, EventArgs e) => RevertToDefault(slots[0]);
        private void btnReset2_Click(object sender, EventArgs e) => RevertToDefault(slots[1]);
        private void btnReset3_Click(object sender, EventArgs e) => RevertToDefault(slots[2]);
        private void btnReset4_Click(object sender, EventArgs e) => RevertToDefault(slots[3]);

        // RESTORE STOCK VALUES
        // Reinject stock tracked cache values
        private void RevertToDefault(TrickSlot slot)
        {
            if (!slot.HasOriginalCached) return;

            // Notification: {trick} value reset
            string trickName = slot.ComboBox.SelectedItem?.ToString() ?? "Unknown Trick";
            try { MAPI.PS3.Notify($"{trickName} value reset"); } catch { }

            CommitMemoryWrite(slot, slot.CachedOriginalValue);
        }

        // INJECT ANIM LOGIC
        // Write swapped array data sequences
        private void btnAnimInject_Click(object sender, EventArgs e)
        {
            if (!isAttached)
            {
                MessageBox.Show("Please connect and attach to process first.", "Not Attached");
                return;
            }

            if (cmbAnimTarget.SelectedItem is AnimItem target && cmbAnimSource.SelectedItem is AnimItem source)
            {
                byte[] sourceBytes = source.GetAobBytes();
                if (sourceBytes == null) return;

                try
                {
                    foreach (ulong addr in target.GetAddresses())
                    {
                        if (!originalAobCache.ContainsKey(addr))
                        {
                            byte[] originalBuffer = new byte[sourceBytes.Length];
                            MAPI.Process.Memory.Get(MAPI.Process.Process_Pid, addr, originalBuffer);
                            originalAobCache[addr] = originalBuffer;
                        }

                        MAPI.Process.Memory.Set(MAPI.Process.Process_Pid, addr, sourceBytes);
                    }

                    // Notification: {replaced trick} animation replaced with {new trick}
                    string targetName = cmbAnimTarget.SelectedItem?.ToString() ?? "Unknown Trick";
                    string sourceName = cmbAnimSource.SelectedItem?.ToString() ?? "Unknown Animation";
                    try { MAPI.PS3.Notify($"{targetName} animation replaced with {sourceName}"); } catch { }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing animation: " + ex.Message);
                }
            }
        }

        // RESET ANIMIMAATIONS
        // Restore original animation bytes from JSON
        private void btnAnimReset_Click(object sender, EventArgs e)
        {
            if (!isAttached) return;

            if (cmbAnimTarget.SelectedItem is AnimItem target)
            {
                byte[] originalJsonBytes = target.GetAobBytes();
                if (originalJsonBytes == null)
                {
                    MessageBox.Show("Could not read original AoB bytes from JSON file for restoration.", "Restoration Error");
                    return;
                }

                try
                {
                    foreach (ulong addr in target.GetAddresses())
                    {
                        // Write from JSON file
                        MAPI.Process.Memory.Set(MAPI.Process.Process_Pid, addr, originalJsonBytes);
                    }

                    // Notification: {replaced trick} animation reset
                    string targetName = cmbAnimTarget.SelectedItem?.ToString() ?? "Unknown Trick";
                    try { MAPI.PS3.Notify($"{targetName} animation reset"); } catch { }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error restoring animation from file layout: " + ex.Message);
                }
            }
        }
    }
}