using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;
using WindowsFormsApp1.Data;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private const string TAG = "FORM";
        public static Form1 Instance;
        public TextBox tbInfo;
        public FastObjectListView lvBookMark;
        public TabControl tabControl;

        private TabControlHandler tabControlHandler;

        public Form1()
        {
            InitializeComponent();
        }
        private void init()
        {
            Instance = this;
            tbInfo = tb_info;
            lvBookMark = lv_bookmark;
            tabControl = tc_log;
            tabControlHandler = new TabControlHandler();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //WindowState = FormWindowState.Maximized;
            init();
            // HMI //
            resize_by_form();
            resize_split();
            initToolTip();
            initListFilter();
            reloadSettings();
            reloadInformation();
            reload();
        }

        //=============== HMI =================//
        private void resize_split()
        {
            int widthHighLight = splitContainer8.Panel2.Width / 3 - 10;
            panel_highlight_1.Location = new Point(0, 0);
            panel_highlight_1.Width = widthHighLight;
            panel_highlight_2.Location = new Point(widthHighLight, 0);
            panel_highlight_2.Width = widthHighLight;
            panel_highlight_3.Location = new Point(widthHighLight * 2, 0);
            panel_highlight_3.Width = widthHighLight;

            int widthCbColor = (int)(widthHighLight - 60);
            cb_highlight_1.Width = widthCbColor;
            cb_highlight_2.Width = widthCbColor;
            cb_highlight_3.Width = widthCbColor;
            cb_highlight_4.Width = widthCbColor;
            cb_highlight_5.Width = widthCbColor;
            cb_highlight_6.Width = widthCbColor;

            lv_bookmark.Width = split_listview.SplitterDistance - 3;
            lv_bookmark.Height = split_listview.Height - pa_header_book_mark.Height - 3;
        }
        private void resize_by_form()
        {
            pa_form.Size = new Size(Size.Width, Size.Height);
            pa1.Width = Size.Width;
            pa2.Width = Size.Width;
            pa3.Width = Size.Width;
            pa4.Width = Size.Width;
            pa4.Height = Size.Height - menuStrip1.Height - pa1.Height - pa2.Height - pa3.Height;

            split_listview.SplitterDistance = (int)(split_listview.Width * 0.2);

            // Pa2
            splitContainer1.SplitterDistance = (int)(splitContainer1.Width * 0.15);
            splitContainer2.SplitterDistance = (int)(splitContainer1.Width * 0.15);
            splitContainer4.SplitterDistance = (int)(splitContainer1.Width * 0.15);
            splitContainer6.SplitterDistance = (int)(splitContainer1.Width * 0.08);
            //splitContainer8.SplitterDistance = (int)(splitContainer1.Width * 0.08);

        }

        private void initToolTip()
        {
            toolTip.SetToolTip(lb_word_filter, "Click to clear words filter and words hide filter");
            toolTip.SetToolTip(lb_tag_filter, "Click to clear tags filter and tags hide filter");
            toolTip.SetToolTip(lb_pid_filter, "Click to clear pids filter and pid hide filter");
            toolTip.SetToolTip(lb_tid_filter, "Click to clear tids filter and tid hide filter");

            toolTip.SetToolTip(lb_word_show, "Click to clear words filter");
            toolTip.SetToolTip(lb_word_hide, "Click to clear words filter");
            toolTip.SetToolTip(lb_tag_show, "Click to clear tags filter");
            toolTip.SetToolTip(lb_tag_hide, "Click to clear tags hide filter");

            toolTip.SetToolTip(lb_pid_show, "Click to clear pids filter");
            toolTip.SetToolTip(lb_pid_hide, "Click to clear pids filter");
            toolTip.SetToolTip(lb_tid_show, "Click to clear tids filter");
            toolTip.SetToolTip(lb_tid_hide, "Click to clear tids hide filter");

            toolTip.SetToolTip(lb_color_1, "Click to clear color");
            toolTip.SetToolTip(lb_color_2, "Click to clear color");
            toolTip.SetToolTip(lb_color_3, "Click to clear color");
            toolTip.SetToolTip(lb_color_4, "Click to clear color");
            toolTip.SetToolTip(lb_color_5, "Click to clear color");
            toolTip.SetToolTip(lb_color_6, "Click to clear color");

            toolTip.SetToolTip(bt_combine_1, "Select Or/And to set the way combinate words filter");
            toolTip.SetToolTip(bt_combine_2, "Select Or/And to set the way combinate tags filter");
            toolTip.SetToolTip(bt_combine_3, "Select Or/And to set the way combinate pids filter");
        }
        private void initListFilter()
        {
            cb_word_show.DataSource = Settings.WordsShow;
            cb_word_hide.DataSource = Settings.WordsHide;
            cb_tag_show.DataSource = Settings.TagsShow;
            cb_tag_hide.DataSource = Settings.TagsHide;
            cb_pid_show.DataSource = Settings.PidsShow;
            cb_pid_hide.DataSource = Settings.PidsHide;
            cb_tid_show.DataSource = Settings.TidsShow;
            cb_tid_hide.DataSource = Settings.TidsHide;
            cb_highlight_1.DataSource = Settings.Colors1;
            cb_highlight_2.DataSource = Settings.Colors2;
            cb_highlight_3.DataSource = Settings.Colors3;
            cb_highlight_4.DataSource = Settings.Colors4;
            cb_highlight_5.DataSource = Settings.Colors5;
            cb_highlight_6.DataSource = Settings.Colors6;

            cb_word_show.Text = string.Empty;
            cb_word_hide.Text = string.Empty;
            cb_tag_show.Text = string.Empty;
            cb_tag_hide.Text = string.Empty;
            cb_pid_show.Text = string.Empty;
            cb_pid_hide.Text = string.Empty;
            cb_tid_show.Text = string.Empty;
            cb_tid_hide.Text = string.Empty;
            cb_highlight_1.Text = string.Empty;
            cb_highlight_2.Text = string.Empty;
            cb_highlight_3.Text = string.Empty;
            cb_highlight_4.Text = string.Empty;
            cb_highlight_5.Text = string.Empty;
            cb_highlight_6.Text = string.Empty;
        }
        //============== reload ================//

        private Dictionary<int, string> getHightLight()
        {
            Dictionary<int, string> highlights = new Dictionary<int, string>
            {
                { Renderer.KEY_HIGHLIGHT_1, cb_highlight_1.Text.Trim().ToLower() },
                { Renderer.KEY_HIGHLIGHT_2, cb_highlight_2.Text.Trim().ToLower() },
                { Renderer.KEY_HIGHLIGHT_3, cb_highlight_3.Text.Trim().ToLower() },
                { Renderer.KEY_HIGHLIGHT_4, cb_highlight_4.Text.Trim().ToLower() },
                { Renderer.KEY_HIGHLIGHT_5, cb_highlight_5.Text.Trim().ToLower() },
                { Renderer.KEY_HIGHLIGHT_6, cb_highlight_6.Text.Trim().ToLower() }
            };

            return highlights;
        }
        private List<string> getLevels()
        {
            List<string> levels = new List<string>() { };
            if (cb_log_v.Checked) levels.Add(Log.LevelV);
            if (cb_log_d.Checked) levels.Add(Log.LevelD);
            if (cb_log_i.Checked) levels.Add(Log.LevelI);
            if (cb_log_w.Checked) levels.Add(Log.LevelW);
            if (cb_log_e.Checked) levels.Add(Log.LevelE);
            return levels;
        }
        private Dictionary<int, bool> getKeysCombineType()
        {
            Dictionary<int, bool> keysCombineType = new Dictionary<int, bool>()
            {
                { FilterHandler.KEY_TAG_SHOW, cb_tag_show.Text.Trim().ToLower().Contains(FilterHandler.andSig)},
                { FilterHandler.KEY_TAG_HIDE, cb_tag_show.Text.Trim().ToLower().Contains(FilterHandler.andSig)},
                { FilterHandler.KEY_WORD_SHOW, cb_tag_show.Text.Trim().ToLower().Contains(FilterHandler.andSig)},
                { FilterHandler.KEY_WORD_HIDE, cb_tag_show.Text.Trim().ToLower().Contains(FilterHandler.andSig)},
                { FilterHandler.KEY_PID_SHOW, cb_tag_show.Text.Trim().ToLower().Contains(FilterHandler.andSig)},
                { FilterHandler.KEY_PID_HIDE, cb_tag_show.Text.Trim().ToLower().Contains(FilterHandler.andSig)},
                { FilterHandler.KEY_TID_SHOW, cb_tag_show.Text.Trim().ToLower().Contains(FilterHandler.andSig)},
                { FilterHandler.KEY_TID_HIDE, cb_tag_show.Text.Trim().ToLower().Contains(FilterHandler.andSig)},
            };
            return keysCombineType;
        }
        private Dictionary<int, List<string>> getKeys()
        {
            Dictionary<int, List<string>> keys = new Dictionary<int, List<string>>
            {
                {FilterHandler.KEY_TAG_SHOW, getKeyClean(cb_tag_show)},
                {FilterHandler.KEY_TAG_HIDE, getKeyClean(cb_tag_hide)},
                {FilterHandler.KEY_WORD_SHOW, getKeyClean(cb_word_show)},
                {FilterHandler.KEY_WORD_HIDE, getKeyClean(cb_word_hide)},
                {FilterHandler.KEY_PID_SHOW, getKeyClean(cb_pid_show)},
                {FilterHandler.KEY_PID_HIDE, getKeyClean(cb_pid_hide)},
                {FilterHandler.KEY_TID_SHOW, getKeyClean(cb_tid_show)},
                {FilterHandler.KEY_TID_HIDE, getKeyClean(cb_tid_hide)},
            };
            return keys;
        }
        private List<bool> getColumns()
        {
            List<bool> columns = new List<bool>()
            {
                cb_column_line.Checked,
                cb_column_date.Checked,
                cb_column_time.Checked,
                cb_column_pid.Checked,
                cb_column_tid.Checked,
            };
            return columns;
        }
        private Dictionary<int, bool> getCombines()
        {
            Dictionary<int, bool> combines = new Dictionary<int, bool>
            {
                {FilterHandler.KEY_COMBINE_1, bt_combine_1.Text == FilterHandler.and},
                {FilterHandler.KEY_COMBINE_2, bt_combine_2.Text == FilterHandler.and},
                {FilterHandler.KEY_COMBINE_3, bt_combine_3.Text == FilterHandler.and},
            };

            return combines;
        }
        private List<string> getKeyClean(ComboBox comboBox)
        {
            string[] contents = { };
            string content = comboBox.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(content))
            {
                contents = content.Contains(FilterHandler.andSig)
                                ? content.Split(new string[] { FilterHandler.andSig }, StringSplitOptions.RemoveEmptyEntries)
                                : content.Split(new string[] { FilterHandler.orSig }, StringSplitOptions.RemoveEmptyEntries);
            }
            return contents.ToList();
        }

        //================ Event ===============//
        private void resize_split_event(object sender, SplitterEventArgs e)
        {
            resize_split();
        }
        private void resize_form_event(object sender, EventArgs e)
        {
            resize_by_form();
            resize_split();
        }

        private void create_tabpage_event(object sender, EventArgs e)
        {
            tabControlHandler.createTabPage(null);
        }

        private void change_tabpage_event(object sender, EventArgs e)
        {
            Logger.logD(TAG, "Change tabpage event");

            if (tc_log.SelectedIndex != 0)
            {
                tabControlHandler.changeTabPage(tc_log.SelectedIndex - 1);
                reloadInformation();
            } else
            {
                lv_bookmark.ClearObjects();
                tb_info.Text = string.Empty;
                tb_from.Text = string.Empty;
                tb_to.Text = string.Empty;
                cb_highlight_1.Text = string.Empty;
                cb_highlight_2.Text = string.Empty;
                cb_highlight_3.Text = string.Empty;
                cb_highlight_4.Text = string.Empty;
                cb_highlight_5.Text = string.Empty;
                cb_highlight_6.Text = string.Empty;
                cb_tag_show.Text = string.Empty;
                cb_tag_hide.Text = string.Empty;
                cb_word_show.Text = string.Empty;
                cb_word_hide.Text = string.Empty;
                cb_pid_show.Text = string.Empty;
                cb_pid_hide.Text = string.Empty;
                cb_tid_show.Text = string.Empty;
                cb_tid_hide.Text = string.Empty;
            }
        }
        private void change_shorted_event(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tc_log.TabIndex != 0)
                {
                    Logger.logD(TAG, "Change shorted event");
                    reloadShorted();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        private void change_level_event(object sender, EventArgs e)
        {
            if (tc_log.SelectedIndex != 0)
            {
                Logger.logD(TAG, "Change level event");
                tabControlHandler.setLevels(getLevels());
                tabControlHandler.reloadFilter();
            }
        }
        private void change_key_event(object sender, EventArgs e)
        {
            if (tc_log.SelectedIndex != 0)
            {
                tabControlHandler.setKeys(getKeys());
                tabControlHandler.setKeysCombineType(getKeysCombineType());

            }
        }
        private void change_key_event(object sender, KeyEventArgs e)
        {
            if (tc_log.SelectedIndex != 0)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Logger.logD(TAG, "Change key event");
                    tabControlHandler.setKeys(getKeys());
                    tabControlHandler.setKeysCombineType(getKeysCombineType());
                    tabControlHandler.reloadFilter();

                    ComboBox comboBox = (ComboBox)sender;
                    if (comboBox == cb_word_show) Settings.WordsShow.Insert(0, comboBox.Text);
                    else if (comboBox == cb_word_hide) Settings.WordsHide.Insert(0, comboBox.Text);
                    else if (comboBox == cb_tag_show) Settings.TagsShow.Insert(0, comboBox.Text);
                    else if (comboBox == cb_tag_hide) Settings.TagsHide.Insert(0, comboBox.Text);
                    else if (comboBox == cb_pid_show) Settings.PidsShow.Insert(0, comboBox.Text);
                    else if (comboBox == cb_pid_hide) Settings.PidsHide.Insert(0, comboBox.Text);
                    else if (comboBox == cb_tid_show) Settings.TidsShow.Insert(0, comboBox.Text);
                    else if (comboBox == cb_tid_hide) Settings.TidsHide.Insert(0, comboBox.Text);
                    comboBox.SelectedIndex = 0;
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
        }
        private void change_highlight_event(object sender, EventArgs e)
        {
            if (tc_log.SelectedIndex != 0)
            {
                Logger.logD(TAG, "Change highlight event");
                tabControlHandler.setHightLight(getHightLight());
                tabControlHandler.reloadHighLight();
            }
        }
        private void change_hightlight_event(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tc_log.SelectedIndex != 0)
                {
                    Logger.logD(TAG, "Change highlight change event");
                    tabControlHandler.setHightLight(getHightLight());
                    tabControlHandler.reloadHighLight();
                    ComboBox comboBox = (ComboBox)sender;
                    if (comboBox == cb_highlight_1) Settings.Colors1.Insert(0, comboBox.Text);
                    else if (comboBox == cb_highlight_2) Settings.Colors2.Insert(0, comboBox.Text);
                    else if (comboBox == cb_highlight_3) Settings.Colors3.Insert(0, comboBox.Text);
                    else if (comboBox == cb_highlight_4) Settings.Colors4.Insert(0, comboBox.Text);
                    else if (comboBox == cb_highlight_5) Settings.Colors5.Insert(0, comboBox.Text);
                    else if (comboBox == cb_highlight_6) Settings.Colors6.Insert(0, comboBox.Text);
                    comboBox.SelectedIndex = 0;
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
        }
        private void change_hightlight_event(object sender, EventArgs e)
        {
            if (tc_log.SelectedIndex != 0)
            {
                tabControlHandler.setHightLight(getHightLight());
            }
        }
        private void change_column_event(object sender, EventArgs e)
        {
            if (tc_log.SelectedIndex != 0)
            {
                Logger.logD(TAG, "Column change event");
                tabControlHandler.setColumns(getColumns());
                tabControlHandler.reloadColumn();
            }
        }
        private void change_combine_event(object sender, EventArgs e)
        {
            Logger.logD(TAG, "Change combine event");

            Button btAndOr = (Button)sender;
            if (btAndOr.Text == FilterHandler.and)
            {
                btAndOr.Text = FilterHandler.or;
                btAndOr.BackColor = Color.White;
            }
            else
            {
                btAndOr.Text = FilterHandler.and;
                btAndOr.BackColor = Color.LightGray;
            }

            if (tc_log.SelectedIndex != 0)
            {
                tabControlHandler.setCombines(getCombines());
                tabControlHandler.reloadFilter();
            }
        }

        private void clear_key_event(object sender, EventArgs e)
        {
            Label label = (Label)sender;

            if (label == lb_word_show) cb_word_show.Text = string.Empty;
            else if (label == lb_word_hide) cb_word_hide.Text = string.Empty;
            else if (label == lb_tag_show) cb_tag_show.Text = string.Empty;
            else if (label == lb_tag_hide) cb_tag_hide.Text = string.Empty;
            else if (label == lb_pid_show) cb_pid_show.Text = string.Empty;
            else if (label == lb_pid_hide) cb_pid_hide.Text = string.Empty;
            else if (label == lb_tid_show) cb_tid_show.Text = string.Empty;
            else if (label == lb_tid_hide) cb_tid_hide.Text = string.Empty;

            else if (label == lb_word_filter)
            {
                cb_word_show.Text = string.Empty;
                cb_word_hide.Text = string.Empty;
            }
            else if (label == lb_tag_filter)
            {
                cb_tag_show.Text = string.Empty;
                cb_tag_hide.Text = string.Empty;
            }
            else if (label == lb_pid_filter)
            {
                cb_pid_show.Text = string.Empty;
                cb_pid_hide.Text = string.Empty;
            }
            else if (label == lb_tid_filter)
            {
                cb_tid_show.Text = string.Empty;
                cb_tid_hide.Text = string.Empty;
            }
            if (tc_log.SelectedIndex != 0)
            {
                Logger.logD(TAG, "Clear key event");
                tabControlHandler.setKeys(getKeys());
                tabControlHandler.setKeysCombineType(getKeysCombineType());
                tabControlHandler.reloadFilter();
            }

        }
        private void clear_highlight_event(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            if (label == lb_color_1)
            {
                cb_highlight_1.Text = string.Empty;
            }
            if (label == lb_color_2)
            {
                cb_highlight_2.Text = string.Empty;
            }
            if (label == lb_color_3)
            {
                cb_highlight_3.Text = string.Empty;
            }
            if (label == lb_color_4)
            {
                cb_highlight_4.Text = string.Empty;
            }
            if (label == lb_color_5)
            {
                cb_highlight_5.Text = string.Empty;
            }
            if (label == lb_color_6)
            {
                cb_highlight_6.Text = string.Empty;
            }
            if (label == lb_hight_light)
            {
                cb_highlight_1.Text = string.Empty;
                cb_highlight_2.Text = string.Empty;
                cb_highlight_3.Text = string.Empty;
                cb_highlight_4.Text = string.Empty;
                cb_highlight_5.Text = string.Empty;
                cb_highlight_6.Text = string.Empty;
            }
            if (tc_log.SelectedIndex != 0)
            {
                Logger.logD(TAG, "Clear highlight event");
                tabControlHandler.setHightLight(getHightLight());
                tabControlHandler.reloadHighLight();
            }
        }
        private void clear_log_event(object sender, EventArgs e)
        {
            if (tc_log.SelectedIndex != 0)
            {
                tabControlHandler.clearLogs();
            }
        }
        private void clear_from_to_event(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            if (label == lb_from)
            {
                tb_from.Text = string.Empty;
            }
            if (label == lb_to)
            {
                tb_to.Text = string.Empty;
            }
            if (tc_log.SelectedIndex != 0)
            {
                reloadShorted();
            }
        }

        private void clear_all_log_event(object sender, EventArgs e)
        {
            if (tc_log.SelectedIndex != 0)
            {
                Logger.logD(TAG, "Clear all log event");
                tabControlHandler.clearBookMark();
            }
        }
        private void unmark_log_event(object sender, EventArgs e)
        {
            Logger.logD(TAG, "Unmark log event");

            if (lv_bookmark.SelectedObject is Log selectedLog)
            {
                selectedLog.Mark = false;
                lv_bookmark.RemoveObject(selectedLog);
            }
        }

        private void open_file_event(object sender, EventArgs e)
        {
            Logger.logD(TAG, "Open file event");
            tabControlHandler.openLog();
        }
        private void save_file_event(object sender, EventArgs e)
        {
            if (tc_log.SelectedIndex != 0)
            {
                Logger.logD(TAG, "Save file event");
                tabControlHandler.saveLog();
            }
        }
        private void start_log_adb_event(object sender, EventArgs e)
        {
            Logger.logD(TAG, "Start log from adb");
            tabControlHandler.startLogAdb();
        }
        private void stop_log_adb_event(object sender, EventArgs e)
        {
            Logger.logD(TAG, "Stop log from adb");
            if (tc_log.SelectedIndex != 0)
            {
                tabControlHandler.stopLogAdb();
            }
        }
        private void close_form_event(object sender, FormClosingEventArgs e)
        {
            Logger.logD(TAG, "Close form event");

            Settings.saveSettings();
        }
        private void remove_tabpage_event(object sender, EventArgs e)
        {
            Logger.logD(TAG, "Remove tabpage");
            if (tc_log.SelectedIndex != 0)
            {
                tabControlHandler.removeTabPage();
            }
        }
        private void select_bookmark_event(object sender, EventArgs e)
        {
            Logger.logD(TAG, "Select bookmark event");
            if (lv_bookmark.SelectedObject is Log selectedLog)
            {
                tabControlHandler.focusLog(selectedLog);
                tb_info.Text = selectedLog.ToString();
            }
        }

        private void select_font_event(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = lv_bookmark.Font;

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Font = fontDialog.Font;
                reloadSettings();
            }
        }
        private void select_dark_mode_event(object sender, EventArgs e)
        {
            Logger.logD(TAG, "Get select mode event: Dark mode");

            Settings.Theme = Settings.DarkMode;
            reloadSettings();
        }
        private void select_light_mode_event(object sender, EventArgs e)
        {
            Logger.logD(TAG, "Get select mode event: White mode");

            Settings.Theme = Settings.LightMode;
            reloadSettings();
        }
        private void reset_factory_event(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to reset all settings?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Logger.logD(TAG, "in");
                Settings.resetSettings();
                reload();
                reloadSettings();
            }
        }

        private void reload()
        {
            Logger.logD(TAG, "Reload filter");
            if (tc_log.TabIndex != 0)
            {
                tabControlHandler.setLevels(getLevels());
                tabControlHandler.setKeys(getKeys());
                tabControlHandler.setKeysCombineType(getKeysCombineType());
                tabControlHandler.setColumns(getColumns());
                tabControlHandler.setCombines(getCombines());
                tabControlHandler.reload();
            }
        }
        private void reloadSettings()
        {
            Logger.logD(TAG, "Reload Settings");

            tabControlHandler.reloadSettings();
            lv_bookmark.Font = Settings.Font;

            Dictionary<string, Color> currentTheme = Settings.getCurrentTheme();
            Color backColor = currentTheme[Settings.BACK_COLOR];
            Color textColor = currentTheme[Settings.TEXT_COLOR];
            Color listColor = currentTheme[Settings.LIST_VIEW_COLOR];

            lv_bookmark.BackColor = listColor;

            pa_form.BackColor = backColor;

            lb_hight_light.ForeColor = textColor;
            lb_pid_filter.ForeColor = textColor;
            lb_pid_hide.ForeColor = textColor;
            lb_pid_show.ForeColor = textColor;
            lb_tid_filter.ForeColor = textColor;
            lb_tid_hide.ForeColor = textColor;
            lb_tid_show.ForeColor = textColor;
            lb_tag_filter.ForeColor = textColor;
            lb_tag_hide.ForeColor = textColor;
            lb_tag_show.ForeColor = textColor;
            lb_word_filter.ForeColor = textColor;
            lb_word_hide.ForeColor = textColor;
            lb_word_show.ForeColor = textColor;
            lb_log.ForeColor = textColor;
            lb_column.ForeColor = textColor;
            lb_bookmark.ForeColor = textColor;
            lb_from.ForeColor = textColor;
            lb_to.ForeColor = textColor;

            cb_log_d.ForeColor = textColor;
            cb_log_e.ForeColor = textColor;
            cb_log_i.ForeColor = textColor;
            cb_log_v.ForeColor = textColor;
            cb_log_w.ForeColor = textColor;
            cb_column_line.ForeColor = textColor;
            cb_column_date.ForeColor = textColor;
            cb_column_time.ForeColor = textColor;
            cb_column_pid.ForeColor = textColor;
            cb_column_tid.ForeColor = textColor;

            bt_clear_bookmarks.BackColor = backColor;
            bt_find.BackColor = backColor;
            bt_start.BackColor = backColor;
            bt_stop.BackColor = backColor;
            bt_pause.BackColor = backColor;

            bt_find.ForeColor = textColor;
            bt_start.ForeColor = textColor;
            bt_stop.ForeColor = textColor;
            bt_pause.ForeColor = textColor;
        }
        private void reloadInformation()
        {
            Logger.logD(TAG, "Reload Information");
            TabPageHandler currentTabPageHandler = tabControlHandler.currentTabPageHandler;
            if (currentTabPageHandler == null)
            {
                return;
            }

            Dictionary<int, List<string>> keys = currentTabPageHandler.getKeys();
            Dictionary<int, bool> keysCombineType = currentTabPageHandler.getKeysCombineType();
            if (keys != null)
            {
                cb_tag_show.Text = string.Join(FilterHandler.convertKeyCombineType(keysCombineType[FilterHandler.KEY_TAG_SHOW]),
                                                keys[FilterHandler.KEY_TAG_SHOW]);

                cb_tag_hide.Text = string.Join(FilterHandler.convertKeyCombineType(keysCombineType[FilterHandler.KEY_TAG_HIDE]),
                                                keys[FilterHandler.KEY_TAG_HIDE]);

                cb_word_show.Text = string.Join(FilterHandler.convertKeyCombineType(keysCombineType[FilterHandler.KEY_WORD_SHOW]),
                                                keys[FilterHandler.KEY_WORD_SHOW]);

                cb_word_hide.Text = string.Join(FilterHandler.convertKeyCombineType(keysCombineType[FilterHandler.KEY_WORD_HIDE]),
                                                keys[FilterHandler.KEY_WORD_HIDE]);

                cb_pid_show.Text = string.Join(FilterHandler.convertKeyCombineType(keysCombineType[FilterHandler.KEY_PID_SHOW]),
                                                keys[FilterHandler.KEY_PID_SHOW]);

                cb_pid_hide.Text = string.Join(FilterHandler.convertKeyCombineType(keysCombineType[FilterHandler.KEY_PID_HIDE]),
                                                keys[FilterHandler.KEY_PID_HIDE]);

                cb_tid_show.Text = string.Join(FilterHandler.convertKeyCombineType(keysCombineType[FilterHandler.KEY_TID_SHOW]),
                                                keys[FilterHandler.KEY_TID_SHOW]);

                cb_tid_hide.Text = string.Join(FilterHandler.convertKeyCombineType(keysCombineType[FilterHandler.KEY_TID_HIDE]),
                                                keys[FilterHandler.KEY_TID_HIDE]);
            }

            Dictionary<int, string> highlights = currentTabPageHandler.getHighLights();
            if (highlights != null)
            {
                cb_highlight_1.Text = highlights[Renderer.KEY_HIGHLIGHT_1];
                cb_highlight_2.Text = highlights[Renderer.KEY_HIGHLIGHT_2];
                cb_highlight_3.Text = highlights[Renderer.KEY_HIGHLIGHT_3];
                cb_highlight_4.Text = highlights[Renderer.KEY_HIGHLIGHT_4];
                cb_highlight_5.Text = highlights[Renderer.KEY_HIGHLIGHT_5];
                cb_highlight_6.Text = highlights[Renderer.KEY_HIGHLIGHT_6];

            }

            tb_from.Text = currentTabPageHandler.from.ToString();
            tb_to.Text = currentTabPageHandler.to.ToString();

            bt_start.Enabled = currentTabPageHandler.controlButtons[0];
            bt_pause.Enabled = currentTabPageHandler.controlButtons[1];
            bt_stop.Enabled = currentTabPageHandler.controlButtons[2];

            lvBookMark.SetObjects(currentTabPageHandler.bookmarks);
        }
        private void reloadShorted()
        {
            Logger.logD(TAG, "Reload shorted");
            string from = tb_from.Text;
            string to = tb_to.Text;
            if (from == string.Empty && to == string.Empty)
            {
                tabControlHandler.removeShorted();
                return;
            }
            try
            {
                if (from != string.Empty && to != string.Empty)
                {
                    int fromLine = int.Parse(from);
                    int toLine = int.Parse(to);
                    tabControlHandler.setShorted(fromLine, toLine, TabPageHandler.SHORTED_BY_LINE);
                    return;
                }

                if (from != string.Empty && to == string.Empty)
                {
                    int fromLine = int.Parse(from);
                    tabControlHandler.setShorted(fromLine, int.MaxValue, TabPageHandler.SHORTED_BY_LINE);
                    return;
                }

                if (from == string.Empty && to != string.Empty)
                {
                    int toLine = int.Parse(to);
                    tabControlHandler.setShorted(0, toLine, TabPageHandler.SHORTED_BY_LINE);
                    return;
                }

            }
            catch (FormatException)
            {
                MessageBox.Show("Cannot convert");
            }
        }

        private void bt_find_Click(object sender, EventArgs e)
        {

        }


    }
}
