using System.Drawing;

namespace WindowsFormsApp1.Data
{
    internal class Log
    {
        public static string LevelV = "V";
        public static string LevelD = "D";
        public static string LevelI = "I";
        public static string LevelW = "W";
        public static string LevelE = "E";
        
        public static Color colorV = Color.Black;
        public static Color colorD = Color.DarkBlue;
        public static Color colorI = Color.DarkGreen;
        public static Color colorW = Color.DarkRed;
        public static Color colorE = Color.Red;
        public static Color colorDefault = Color.Gray;

        public int Line { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Pid { get; set; }
        public string Tid { get; set; }
        public string Level { get; set; }
        public string Tag { get; set; }
        public string Message { get; set; }
        public bool Mark { get; set; }
        public Color Color
        {
            get
            {
                switch (Level)
                {
                    case "V": return colorV;
                    case "D": return colorD;
                    case "I": return colorI;
                    case "W": return colorW;
                    case "E": return colorE;
                }
                return colorDefault;
            } 
        }

        public Log(int line, string date, string time, string pid, string tid, string level, string tag, string message)
        {
            Line = line;
            Date = date;
            Time = time;
            Pid = pid;
            Tid = tid;
            Level = level;
            Tag = tag;
            Message = message;
        }

        public override string ToString()
        {
            return Date + " " + Time + " " + Pid + " " + Tid + " " + Level + " " + Tag + " " + Message;
        }
    }
}
