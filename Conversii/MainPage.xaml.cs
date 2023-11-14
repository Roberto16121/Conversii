using System.Diagnostics;

namespace Conversii
{
    public partial class MainPage : ContentPage
    {
        Color Good, Bad;
        bool isSigned = false, isDashAllowned = false;
        int numberBase = 2, toConvertBase = 10;
        string validCharacters = ".0123456789ABCDEF";

        public MainPage()
        {
            InitializeComponent();
            Signed.SelectedItem = "UnSigned";
            Base.SelectedItem = "Base-2";
            BaseToConvert.SelectedItem = "Base-10";
            Good = new Color(0, 0, 0);
            Bad = new Color(255, 0, 0);
        }

        private void OnSignedChanged(object sender, EventArgs e)
        {
            if (Signed.SelectedIndex == 0)
                isSigned = false;
            else
                isSigned = true;
            Trace.WriteLine(isSigned);
            CheckIfDashIsAllowed();
            OnTextChanged(sender, e);
        }

        void CheckIfDashIsAllowed()
        {
            if (!isSigned || numberBase is 2)
                isDashAllowned = false;
            else
                isDashAllowned = true;
        }



        private void OnBaseChanged(object sender, EventArgs e)
        {
            numberBase = Base.SelectedIndex + 2;
            CheckIfDashIsAllowed();
            OnTextChanged(sender, e);

        }
        private void OnToConvertBaseChanged(object sender, EventArgs e)
        {
            Trace.WriteLine("ToConvertBase changed");
            toConvertBase = BaseToConvert.SelectedIndex + 2;
        }


        private void OnTextChanged(object sender, EventArgs e)
        {
            if(TextEntry.Text == null)
                return;

            string text = TextEntry.Text;
            string currentValid = validCharacters.Substring(0, numberBase + 1);
            bool isValid = true;
            foreach (char c in text)
            {
                if(isDashAllowned)
                {
                    if(!currentValid.Contains(c) && c != '-')
                    {
                        isValid = false;
                        break;
                    }
                }
                else
                {
                    if(!currentValid.Contains(c))
                    {
                        isValid = false;
                        break;
                    }
                }
                
            }
            if(isValid)
            {
                TextEntry.TextColor = Good;
            }
            else
            {
                TextEntry.TextColor = Bad;
            }
        }

        String ConvertToDecimal() /// 10111 . 011 -> 
        {
            string toRet = "";

            return toRet;
        }

    }
}