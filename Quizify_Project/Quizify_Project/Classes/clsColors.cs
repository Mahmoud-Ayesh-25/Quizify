using System.Windows.Media;

namespace Quizify_Project.Classes
{
    public class clsColors
    {
        public struct stColor
        {
            public Color MainBackgroundColorUp;
            public Color MainBackgroundColorDown;

            public Color BorderColorUp;
            public Color BorderColorDown;

            public Color LessonsCountAreaBackgroundColor;

            public Color MouseEnterBorderColor;
        }

        static Dictionary<string, stColor> _buttonColors = new Dictionary<string, stColor>();

        public Dictionary<string, stColor> buttonColors { get { return _buttonColors; } }

        public clsColors()
        {
            if (!_buttonColors.ContainsKey("Brown"))
                _buttonColors.Add("Brown", New_stColor("#FF583B3C", "#FF392433", "#FF856363", "#FF65464A", "#FF392433", "#FFD9D3D3"));

            if (!_buttonColors.ContainsKey("Red"))
                _buttonColors.Add("Red", New_stColor("#FFAF2222", "#FF841313", "#FFE64141", "#FFAB3434", "#FF841313", "#FFF5B4B4"));

            if (!_buttonColors.ContainsKey("Orange"))
                _buttonColors.Add("Orange", New_stColor("#FFE46D1F", "#FFAE390F", "#FFC9953B", "#FFED6216", "#FFAE390F", "#FFEFD5CA"));

            if (!_buttonColors.ContainsKey("Yellow"))
                _buttonColors.Add("Yellow", New_stColor("#FFF0AA18", "#FFD67B08", "#FFFDDF60", "#FFF6A51C", "#FFD67B08", "#FFEFD5CA"));

            if (!_buttonColors.ContainsKey("Green"))
                _buttonColors.Add("Green", New_stColor("#FF519547", "#FF205838", "#FF68A950", "#FF31684E", "#FF205838", "#FFD2DDD7"));

            if (!_buttonColors.ContainsKey("Blue"))
                _buttonColors.Add("Blue", New_stColor("#FF2459B9", "#FF133A84", "#FF3B7AE2", "#FF2C4A90", "#FF133A84", "#FF9ABAEF"));

            if (!_buttonColors.ContainsKey("Teal"))
                _buttonColors.Add("Teal", New_stColor("#FF1E4360", "#FF112943", "#FF417095", "#FF244C68", "#FF112943", "#FFC9CFD5"));

            if (!_buttonColors.ContainsKey("Burble"))
                _buttonColors.Add("Burble", New_stColor("#FF584398", "#FF2C296C", "#FF8865B8", "#FF47367E", "#FF2C296C", "#FFA19FBD"));

            if (!_buttonColors.ContainsKey("Pink"))
                _buttonColors.Add("Pink", New_stColor("#FFC44D88", "#FF95215F", "#FFEA89BE", "#FF98307D", "#FF95215F", "#FFE7CDDB"));
        }

        stColor New_stColor(string mainBackgroundUp, string mainBackgroundDown, string borderUp,
            string borderDown, string lessonsCountArea, string borderMouseEnter)
        {
            stColor clr = new stColor();

            clr.MainBackgroundColorUp = (Color)ColorConverter.ConvertFromString(mainBackgroundUp);
            clr.MainBackgroundColorDown = (Color)ColorConverter.ConvertFromString(mainBackgroundDown);

            clr.BorderColorUp = (Color)ColorConverter.ConvertFromString(borderUp);
            clr.BorderColorDown = (Color)ColorConverter.ConvertFromString(borderDown);

            clr.LessonsCountAreaBackgroundColor = (Color)ColorConverter.ConvertFromString(lessonsCountArea);

            clr.MouseEnterBorderColor = (Color)ColorConverter.ConvertFromString(borderMouseEnter);

            return clr;
        }
    }
}
