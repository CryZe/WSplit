namespace WSplitTimer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using Properties;
    using System.Text;
    using System.Globalization;
    using LiveSplit.Model;
    using LiveSplit.Model.Comparisons;

    public class RunEditorDialog : Form
    {
        public TextBox attemptsBox;
        private DataGridViewTextBoxColumn best;
        private DataGridViewTextBoxColumn bseg;
        private int cellHeight;
        private Button discardButton;
        private Control eCtl;
        public List<ISegment> editList = new List<ISegment>();
        private DataGridViewImageColumn icon;
        private DataGridViewTextBoxColumn iconPath;
        private Button insertButton;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button offsetButton;
        private DataGridViewTextBoxColumn old;
        private TextBox oldOffset;
        private OpenFileDialog openIconDialog;
        private Button resetButton;
        private DataGridView runView;
        private Button saveButton;
        private DataGridViewTextBoxColumn segment;
        public TextBox titleBox;

        private Button buttonAutoFillBests;

        private OpenFileDialog openFileDialog;

        private int windowHeight;

        public RunEditorDialog(IRun splits)
        {
            this.InitializeComponent();
            this.cellHeight = this.runView.RowTemplate.Height;
            this.windowHeight = (base.Height - (this.runView.Height - this.cellHeight)) - 2;
            this.MaximumSize = new Size(500, (15 * this.cellHeight) + this.windowHeight);

            foreach (var segment in splits)
                this.editList.Add(segment);

            this.populateList(this.editList);
            this.runView.EditingControlShowing += this.runView_EditingControlShowing;
        }

        private void attemptsBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void attemptsBox_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(this.attemptsBox.Text, "[^0-9]"))
                this.attemptsBox.Text = Regex.Replace(this.attemptsBox.Text, "[^0-9]", "");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.runView.CurrentCell.ColumnIndex == 0)
            {
                if (e.KeyChar == ',')
                    e.Handled = true;
            }
            else if (((!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) && ((e.KeyChar != ':') && (e.KeyChar != '.'))) && (e.KeyChar != ','))
                e.Handled = true;
        }

        private void eCtl_TextChanged(object sender, EventArgs e)
        {
            if (this.runView.CurrentCell.ColumnIndex == 0)
            {
                if (Regex.IsMatch(this.eCtl.Text, ","))
                    this.eCtl.Text = Regex.Replace(this.eCtl.Text, ",", "");
            }
            else if (Regex.IsMatch(this.eCtl.Text, "[^0-9:.,]"))
                this.eCtl.Text = Regex.Replace(this.eCtl.Text, "[^0-9:.,]", "");
        }

        private void InitializeComponent()
        {
            this.runView = new DataGridView();
            this.segment = new DataGridViewTextBoxColumn();
            this.old = new DataGridViewTextBoxColumn();
            this.best = new DataGridViewTextBoxColumn();
            this.bseg = new DataGridViewTextBoxColumn();
            this.iconPath = new DataGridViewTextBoxColumn();
            this.icon = new DataGridViewImageColumn();
            this.saveButton = new Button();
            this.discardButton = new Button();
            this.resetButton = new Button();
            this.oldOffset = new TextBox();
            this.label1 = new Label();
            this.offsetButton = new Button();
            this.titleBox = new TextBox();
            this.label2 = new Label();
            this.insertButton = new Button();
            this.openIconDialog = new OpenFileDialog();
            this.label3 = new Label();
            this.attemptsBox = new TextBox();
            this.buttonAutoFillBests = new Button();
            this.openFileDialog = new OpenFileDialog();

            ((ISupportInitialize)this.runView).BeginInit();
            base.SuspendLayout();

            this.runView.AllowUserToResizeRows = false;
            this.runView.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.runView.BackgroundColor = SystemColors.Window;
            this.runView.BorderStyle = BorderStyle.Fixed3D;
            this.runView.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
            this.runView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.runView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.runView.Columns.AddRange(new DataGridViewColumn[] { this.segment, this.old, this.best, this.bseg, this.iconPath, this.icon });
            this.runView.Location = new Point(12, 0x3a);
            this.runView.Name = "runView";
            this.runView.RowHeadersVisible = false;
            this.runView.Size = new Size(0x167, 0x2a);
            this.runView.TabIndex = 0;
            this.runView.CellDoubleClick += new DataGridViewCellEventHandler(this.runView_CellDoubleClick);
            this.runView.UserAddedRow += new DataGridViewRowEventHandler(this.runView_UserAddedRow);
            this.runView.UserDeletedRow += new DataGridViewRowEventHandler(this.runView_UserDeletedRow);
            this.runView.KeyDown += new KeyEventHandler(this.runView_KeyDown);

            this.segment.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.segment.HeaderText = "Segment";
            this.segment.Name = "segment";
            this.segment.SortMode = DataGridViewColumnSortMode.NotSortable;

            this.old.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.old.HeaderText = "Old Time";
            this.old.Name = "old";
            this.old.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.old.Width = 0x37;

            this.best.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.best.HeaderText = "Best Time";
            this.best.Name = "best";
            this.best.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.best.Width = 60;

            this.bseg.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.bseg.HeaderText = "Best Seg.";
            this.bseg.Name = "bseg";
            this.bseg.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.bseg.Width = 0x3b;

            this.iconPath.HeaderText = "Icon Path";
            this.iconPath.Name = "iconPath";
            this.iconPath.Visible = false;

            this.icon.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.icon.HeaderText = "Icon";
            this.icon.ImageLayout = DataGridViewImageCellLayout.Zoom;
            this.icon.MinimumWidth = 40;
            this.icon.Name = "icon";
            this.icon.ReadOnly = true;
            this.icon.Resizable = DataGridViewTriState.False;
            this.icon.Width = 40;

            this.saveButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.saveButton.DialogResult = DialogResult.OK;
            this.saveButton.Location = new Point(266, 136);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new Size(50, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new EventHandler(this.saveButton_Click);

            this.discardButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.discardButton.DialogResult = DialogResult.Cancel;
            this.discardButton.Location = new Point(322, 136);
            this.discardButton.Name = "discardButton";
            this.discardButton.Size = new Size(50, 23);
            this.discardButton.TabIndex = 2;
            this.discardButton.Text = "Cancel";
            this.discardButton.UseVisualStyleBackColor = true;

            this.resetButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.resetButton.Location = new Point(266, 106);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new Size(106, 23);
            this.resetButton.TabIndex = 3;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new EventHandler(this.resetButton_Click);

            this.buttonAutoFillBests.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonAutoFillBests.Location = new Point(88, 106);
            this.buttonAutoFillBests.Name = "buttonAutoFillBests";
            this.buttonAutoFillBests.Size = new Size(120, 23);
            this.buttonAutoFillBests.TabIndex = 4;
            this.buttonAutoFillBests.Text = "Auto-fill best segments";
            this.buttonAutoFillBests.UseVisualStyleBackColor = true;
            this.buttonAutoFillBests.Click += this.buttonAutoFillBests_Click;

            this.oldOffset.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.oldOffset.Location = new Point(230, 0x1f);
            this.oldOffset.Name = "oldOffset";
            this.oldOffset.Size = new Size(0x56, 20);
            this.oldOffset.TabIndex = 5;
            this.oldOffset.TextChanged += new EventHandler(this.oldOffset_TextChanged);
            this.oldOffset.KeyDown += new KeyEventHandler(this.oldOffset_KeyDown);
            this.oldOffset.KeyPress += new KeyPressEventHandler(this.oldOffset_KeyPress);

            this.label1.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(230, 15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4d, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Old time offset:";

            this.offsetButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.offsetButton.Location = new Point(0x142, 0x1d);
            this.offsetButton.Name = "offsetButton";
            this.offsetButton.Size = new Size(50, 0x17);
            this.offsetButton.TabIndex = 7;
            this.offsetButton.Text = "Apply";
            this.offsetButton.UseVisualStyleBackColor = true;
            this.offsetButton.Click += new EventHandler(this.offsetButton_Click);

            this.titleBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.titleBox.Location = new Point(12, 0x1f);
            this.titleBox.Name = "titleBox";
            this.titleBox.Size = new Size(0xd4, 20);
            this.titleBox.TabIndex = 8;

            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x31, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Run title:";

            this.insertButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.insertButton.Location = new Point(12, 136);
            this.insertButton.Name = "insertButton";
            this.insertButton.Size = new Size(50, 0x17);
            this.insertButton.TabIndex = 10;
            this.insertButton.Text = "Insert";
            this.insertButton.UseVisualStyleBackColor = true;
            this.insertButton.Click += new EventHandler(this.insertButton_Click);

            this.openIconDialog.Filter = "Image files (*.bmp; *.gif; *.jpg; *.png; *.tiff)|*.bmp;*.gif;*.jpg;*.png;*.tiff";

            this.label3.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x44, 141);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x33, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Attempts:";

            this.attemptsBox.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.attemptsBox.Location = new Point(0x7d, 138);
            this.attemptsBox.Name = "attemptsBox";
            this.attemptsBox.Size = new Size(40, 20);
            this.attemptsBox.TabIndex = 13;
            this.attemptsBox.Text = "0";
            this.attemptsBox.TextAlign = HorizontalAlignment.Right;
            this.attemptsBox.TextChanged += new EventHandler(this.attemptsBox_TextChanged);
            this.attemptsBox.KeyPress += new KeyPressEventHandler(this.attemptsBox_KeyPress);


            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(384, 171);
            base.Controls.Add(this.attemptsBox);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.insertButton);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.titleBox);
            base.Controls.Add(this.offsetButton);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.oldOffset);
            base.Controls.Add(this.resetButton);
            base.Controls.Add(this.discardButton);
            base.Controls.Add(this.saveButton);
            base.Controls.Add(this.runView);
            base.Controls.Add(this.buttonAutoFillBests);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            this.MinimumSize = new Size(390, 120);
            base.Name = "RunEditorDialog";
            this.Text = "Run Editor";
            base.Shown += new EventHandler(this.RunEditor_Shown);
            ((ISupportInitialize)this.runView).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private byte[] toConverterEndianness(byte[] array, int offset, int length)
        {
            if (BitConverter.IsLittleEndian)
            {
                byte[] newArray = (byte[])array.Clone();
                Array.Reverse(newArray, offset, length);
                return newArray;
            }

            return array;
        }

        private void buttonAutoFillBests_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Show(this, "Are you sure?", "Auto-fill best segments", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                List<ISegment> splitList = new List<ISegment>();
                this.FillSplitList(ref splitList);

                for (int i = 0; i < splitList.Count; ++i)
                {
                    double segmentTime = 0.0;

                    if (i == 0)
                        segmentTime = splitList[i].BestTime;
                    else if (splitList[i].BestTime != 0.0 && splitList[i - 1].BestTime != 0.0)
                        segmentTime = splitList[i].BestTime - splitList[i - 1].BestTime;

                    if (splitList[i].BestSegment == 0.0 || (segmentTime != 0.0 && segmentTime < splitList[i].BestSegment))
                        splitList[i].BestSegment = segmentTime;
                }

                this.populateList(splitList);
            }
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            if (this.runView.SelectedCells.Count > 0)
            {
                new DataGridViewRow();
                this.runView.Rows.Insert(this.runView.SelectedCells[0].RowIndex, new object[0]);
                base.Height = (Math.Min(15, this.runView.Rows.Count) * this.cellHeight) + this.windowHeight;
                this.runView.CurrentCell = this.runView.Rows[this.runView.SelectedCells[0].RowIndex - 1].Cells[0];
            }
        }

        private void offsetButton_Click(object sender, EventArgs e)
        {
            if (this.oldOffset.Text.Length != 0)
            {
                foreach (DataGridViewRow row in (IEnumerable)this.runView.Rows)
                {
                    if (row.Cells[1].Value != null)
                        row.Cells[1].Value = this.timeFormat(Math.Max((double)0.0, (double)(this.timeParse(row.Cells[1].Value.ToString()) - this.timeParse(this.oldOffset.Text))));
                }
                this.oldOffset.Text = "";
            }
        }

        private void oldOffset_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (this.oldOffset.Text.Length != 0))
            {
                foreach (DataGridViewRow row in (IEnumerable)this.runView.Rows)
                {
                    if (row.Cells[1].Value != null)
                        row.Cells[1].Value = this.timeFormat(Math.Max((double)0.0, (double)(this.timeParse(row.Cells[1].Value.ToString()) - this.timeParse(this.oldOffset.Text))));
                }
                this.oldOffset.Text = "";
                e.SuppressKeyPress = true;
            }
        }

        private void oldOffset_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) && ((e.KeyChar != ':') && (e.KeyChar != '.'))) && (e.KeyChar != ','))
                e.Handled = true;
        }

        private void oldOffset_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(this.oldOffset.Text, "[^0-9:.,]"))
                this.oldOffset.Text = Regex.Replace(this.oldOffset.Text, "[^0-9:.,]", "");
        }

        private void populateList(List<ISegment> splitList)
        {
            this.runView.Rows.Clear();
            this.runView.Rows[0].Cells[5].Value = Resources.MissingIcon;
            foreach (var segment in splitList)
            {
                if (segment.Name != null)
                {
                    string name = segment.Name;
                    string str2 = "";
                    string str3 = "";
                    string str4 = "";
                    string iconPath = "";
                    Image missingIcon = Resources.MissingIcon;

                    if (segment.Comparisons["Old"].RealTime.HasValue)
                        str2 = this.timeFormat(segment.Comparisons["Old"].RealTime.Value.TotalSeconds);

                    if (segment.Comparisons[Run.PersonalBestComparisonName].RealTime.HasValue)
                        str3 = this.timeFormat(segment.Comparisons[Run.PersonalBestComparisonName].RealTime.Value.TotalSeconds);

                    if (segment.Comparisons[BestSegmentsComparisonGenerator.ComparisonName].RealTime.HasValue)
                        str4 = this.timeFormat(segment.Comparisons[BestSegmentsComparisonGenerator.ComparisonName].RealTime.Value.TotalSeconds);

                    this.runView.Rows.Add(new object[] { name, str2, str3, str4, "", segment.Icon ?? Resources.MissingIcon });
                }
            }

            base.Height = (Math.Min(15, this.runView.Rows.Count) * this.cellHeight) + this.windowHeight;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            this.populateList(this.editList);
        }

        private void RunEditor_Shown(object sender, EventArgs e)
        {
            base.BringToFront();
        }

        private void runView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 5) && (e.RowIndex >= 0))
            {
                if (this.runView.Rows[e.RowIndex].Cells[0].Value != null)
                    this.openIconDialog.Title = "Set Icon for " + this.runView.Rows[e.RowIndex].Cells[0].Value.ToString() + "...";
                else
                    this.openIconDialog.Title = "Set Icon...";

                if (this.openIconDialog.ShowDialog() == DialogResult.OK)
                {
                    this.runView.Rows[e.RowIndex].Cells[4].Value = this.openIconDialog.FileName;
                    Image missingIcon = Resources.MissingIcon;
                    try
                    {
                        missingIcon = Image.FromFile(this.openIconDialog.FileName);
                    }
                    catch
                    { }

                    this.runView.Rows[e.RowIndex].Cells[5].Value = missingIcon;
                }
            }
        }

        private void runView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            this.eCtl = e.Control;
            this.eCtl.TextChanged -= new EventHandler(this.eCtl_TextChanged);
            this.eCtl.KeyPress -= new KeyPressEventHandler(this.eCtl_KeyPress);
            this.eCtl.TextChanged += new EventHandler(this.eCtl_TextChanged);
            this.eCtl.KeyPress += new KeyPressEventHandler(this.eCtl_KeyPress);
        }

        private void runView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                foreach (DataGridViewCell cell in this.runView.SelectedCells)
                {
                    if (((cell.RowIndex >= 0) && (cell.ColumnIndex >= 0)) && !this.runView.Rows[cell.RowIndex].IsNewRow)
                    {
                        if (cell.ColumnIndex == 0)
                            this.runView.Rows.RemoveAt(cell.RowIndex);
                        else if (cell.ColumnIndex == 5)
                        {
                            cell.Value = Resources.MissingIcon;
                            this.runView.Rows[cell.RowIndex].Cells[4].Value = "";
                        }
                        else
                            cell.Value = null;
                    }
                }
                base.Height = (Math.Min(15, this.runView.Rows.Count) * this.cellHeight) + this.windowHeight;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                for (int i = this.runView.SelectedCells.Count - 1; i >= 0; i--)
                {
                    DataGridViewCell cell2 = this.runView.SelectedCells[i];
                    if ((cell2.RowIndex >= 0) && (cell2.ColumnIndex == 5))
                    {
                        if (this.runView.Rows[cell2.RowIndex].Cells[0].Value != null)
                            this.openIconDialog.Title = "Set Icon for " + this.runView.Rows[cell2.RowIndex].Cells[0].Value.ToString() + "...";
                        else
                            this.openIconDialog.Title = "Set Icon...";

                        if (this.openIconDialog.ShowDialog() != DialogResult.OK)
                            break;

                        this.runView.Rows[cell2.RowIndex].Cells[4].Value = this.openIconDialog.FileName;
                        Image missingIcon = Resources.MissingIcon;

                        try
                        {
                            missingIcon = Image.FromFile(this.openIconDialog.FileName);
                        }
                        catch
                        { }
                        cell2.Value = missingIcon;
                    }
                }
            }
        }

        private void runView_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            base.Height = (Math.Min(15, this.runView.Rows.Count) * this.cellHeight) + this.windowHeight;
            e.Row.Cells[5].Value = Resources.MissingIcon;
        }

        private void runView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            base.Height = (Math.Min(15, this.runView.Rows.Count) * this.cellHeight) + this.windowHeight;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            this.editList.Clear();
            FillSplitList(ref this.editList);
        }

        private void FillSplitList(ref List<ISegment> splitList)
        {
            foreach (DataGridViewRow row in (IEnumerable)this.runView.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    Bitmap missingIcon = Resources.MissingIcon;
                    Segment item = new Segment(row.Cells[0].Value.ToString());
                    if (row.Cells[1].Value != null)
                        item.OldTime = this.timeParse(row.Cells[1].Value.ToString());

                    if (row.Cells[2].Value != null)
                        item.BestTime = this.timeParse(row.Cells[2].Value.ToString());

                    if (row.Cells[3].Value != null)
                        item.BestSegment = this.timeParse(row.Cells[3].Value.ToString());

                    if (row.Cells[4].Value != null)
                    {
                        item.IconPath = row.Cells[4].Value.ToString();
                        try
                        {
                            item.Icon = Image.FromFile(item.IconPath);
                        }
                        catch
                        {
                            item.Icon = Resources.MissingIcon;
                        }
                    }
                    else
                        item.Icon = Resources.MissingIcon;

                    splitList.Add(item);
                }
            }
        }

        private string timeFormat(double secs)
        {
            TimeSpan span = TimeSpan.FromSeconds(Math.Truncate(secs * 100) / 100);
            //TimeSpan span = TimeSpan.FromSeconds(Math.Round(secs, 2));
            if (span.TotalHours >= 1.0)
                return string.Format("{0}:{1:00}:{2:00.00}", Math.Floor(span.TotalHours), span.Minutes, span.Seconds + (((double)span.Milliseconds) / 1000.0));

            if (span.TotalMinutes >= 1.0)
                return string.Format("{0}:{1:00.00}", Math.Floor(span.TotalMinutes), span.Seconds + (((double)span.Milliseconds) / 1000.0));

            return string.Format("{0:0.00}", span.TotalSeconds);
        }

        private double timeParse(string timeString)
        {
            double num = 0.0;
            foreach (string str in timeString.Split(new char[] { ':' }))
            {
                double num2;
                if (double.TryParse(str, out num2))
                    num = (num * 60.0) + num2;
            }
            return num;
        }
    }
}

