using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1.Data
{
    internal class Settings
    {
        public static Font Font = Properties.Settings.Default.Font;
        public static int Theme = Properties.Settings.Default.Theme;
        public static BindingSource WordsShow = new BindingSource(Properties.Settings.Default.WordsShow, null);
        public static BindingSource WordsHide = new BindingSource(Properties.Settings.Default.WordsHide, null);
        public static BindingSource TagsShow = new BindingSource(Properties.Settings.Default.TagsShow, null);
        public static BindingSource TagsHide = new BindingSource(Properties.Settings.Default.TagsHide, null);
        public static BindingSource PidsShow = new BindingSource(Properties.Settings.Default.PidsShow, null);
        public static BindingSource PidsHide = new BindingSource(Properties.Settings.Default.PidsHide, null);
        public static BindingSource TidsShow = new BindingSource(Properties.Settings.Default.TidsShow, null);
        public static BindingSource TidsHide = new BindingSource(Properties.Settings.Default.TidsHide, null);
        public static BindingSource Colors1 = new BindingSource(Properties.Settings.Default.Colors1, null);
        public static BindingSource Colors2 = new BindingSource(Properties.Settings.Default.Colors2, null);
        public static BindingSource Colors3 = new BindingSource(Properties.Settings.Default.Colors3, null);
        public static BindingSource Colors4 = new BindingSource(Properties.Settings.Default.Colors4, null);
        public static BindingSource Colors5 = new BindingSource(Properties.Settings.Default.Colors5, null);
        public static BindingSource Colors6 = new BindingSource(Properties.Settings.Default.Colors6, null);

        public static string BACK_COLOR = "back_color";
        public static string TEXT_COLOR = "text_color";
        public static string LIST_VIEW_COLOR = "list_view_color";
        public static int DarkMode = 0;
        public static int LightMode = 1;

        public static Dictionary<string, Color> dicDarkTheme = new Dictionary<string, Color>()
            {
                { BACK_COLOR, Color.Gray },
                { TEXT_COLOR, Color.White },
                { LIST_VIEW_COLOR, Color.LightGray },
            };

        public static Dictionary<string, Color> dicLightTheme = new Dictionary<string, Color>()
            {
                { BACK_COLOR, Color.WhiteSmoke },
                { TEXT_COLOR, Color.Black },
                { LIST_VIEW_COLOR, Color.White },
            };

        public static Dictionary<string, Color> getCurrentTheme()
        {
            if (Theme == DarkMode)
            {
                return dicDarkTheme;
            }
            else if (Theme == LightMode)
            {
                return dicLightTheme;
            }
            else
            {
                return dicDarkTheme;
            }
        }

        public static void saveSettings()
        {
            Properties.Settings.Default.Font = Font;
            Properties.Settings.Default.Theme = Theme;

            Properties.Settings.Default.TagsShow = new StringCollection();
            foreach (string key in TagsShow)
            {
                Properties.Settings.Default.TagsShow.Add(key);
            }

            Properties.Settings.Default.TagsHide = new StringCollection();
            foreach (string key in TagsHide)
            {
                Properties.Settings.Default.TagsHide.Add(key);
            }

            Properties.Settings.Default.WordsShow = new StringCollection();
            foreach (string key in WordsShow)
            {
                Properties.Settings.Default.WordsShow.Add(key);
            }

            Properties.Settings.Default.WordsHide = new StringCollection();
            foreach (string key in WordsHide)
            {
                Properties.Settings.Default.WordsHide.Add(key);
            }

            Properties.Settings.Default.PidsShow = new StringCollection();
            foreach (string key in PidsShow)
            {
                Properties.Settings.Default.PidsShow.Add(key);
            }

            Properties.Settings.Default.PidsHide = new StringCollection();
            foreach (string key in PidsHide)
            {
                Properties.Settings.Default.PidsHide.Add(key);
            }

            Properties.Settings.Default.TidsShow = new StringCollection();
            foreach (string key in TidsShow)
            {
                Properties.Settings.Default.TidsShow.Add(key);
            }

            Properties.Settings.Default.TidsHide = new StringCollection();
            foreach (string key in TidsHide)
            {
                Properties.Settings.Default.TidsHide.Add(key);
            }

            Properties.Settings.Default.Colors1 = new StringCollection();
            foreach (string key in Colors1)
            {
                Properties.Settings.Default.Colors1.Add(key);
            }

            Properties.Settings.Default.Colors2 = new StringCollection();
            foreach (string key in Colors2)
            {
                Properties.Settings.Default.Colors2.Add(key);
            }

            Properties.Settings.Default.Colors3 = new StringCollection();
            foreach (string key in Colors3)
            {
                Properties.Settings.Default.Colors3.Add(key);
            }

            Properties.Settings.Default.Colors4 = new StringCollection();
            foreach (string key in Colors4)
            {
                Properties.Settings.Default.Colors4.Add(key);
            }

            Properties.Settings.Default.Colors5 = new StringCollection();
            foreach (string key in Colors5)
            {
                Properties.Settings.Default.Colors5.Add(key);
            }

            Properties.Settings.Default.Colors6 = new StringCollection();
            foreach (string key in Colors6)
            {
                Properties.Settings.Default.Colors6.Add(key);
            }
            Properties.Settings.Default.Save();
        }

        public static void resetSettings()
        {
            Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
            Theme = DarkMode;

            WordsShow.Clear();
            WordsHide.Clear();
            TagsShow.Clear();
            TagsHide.Clear();
            PidsShow.Clear();
            PidsHide.Clear();
            TidsShow.Clear();
            TidsHide.Clear();
            Colors1.Clear();
            Colors2.Clear();
            Colors3.Clear();
            Colors4.Clear();
            Colors5.Clear();
            Colors6.Clear();
    }
}
}
