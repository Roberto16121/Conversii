using System.Diagnostics;
using System.Text;

namespace Conversii
{
    public partial class MainPage : ContentPage
    {
        Color Good, Bad;
        bool isSigned = false, isDashAllowned = false;
        int numberBase = 2, toConvertBase = 10;
        string validCharacters = ".0123456789ABCDEF";
        int textLenght = 16;

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
            OnTextChanged(sender, e);
        }


        private void OnTextChanged(object sender, EventArgs e)
        {
            TextBox.Text = "";
            if(TextEntry.Text == null || TextEntry.Text.Length == 0 || TextEntry.Text.Length > textLenght)
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
            if (text[text.Length - 1] == '.')
                isValid = false;
            if(isValid)
            { 
                TextEntry.TextColor = Good;
                DoConversion(text);
            }
            else
            {
                TextBox.Text = "";
                TextEntry.TextColor = Bad;
            }
        }

        void DoConversion(string text)
        {
            bool negativeNumber = false;
            if(isSigned)
            {
                string firstBit = text.Substring(0, 1);
                if (numberBase == 2)
                {
                    if(firstBit == "1")
                    {
                        negativeNumber = true;
                        text = text.Substring(1);
                    }
                }
                else
                {
                    if(firstBit == "-")
                    {
                        negativeNumber = true;
                        text = text.Substring(1);
                    }
                }
            }
            
            string DecimalText = ConvertToDecimal(text);
            string Whole, Fraction = "0";
            bool hasDecimal = text.IndexOf('.') != -1;
            if(hasDecimal)
            {
                Whole = DecimalText.Substring(0, DecimalText.IndexOf('.'));
                Fraction = DecimalText.Substring(DecimalText.IndexOf('.') + 1);
                Fraction = ConvertFractionToBase(Fraction, toConvertBase);
            }
            else
                Whole = DecimalText;
            Whole = ConvertWholeToBase(Whole, toConvertBase);
            
            if (Whole.Length == 0)
                Whole = "0";
            bool startsWithZero = Whole[0] == '0';
            TextBox.Text = hasDecimal ? (Whole + "." + Fraction) : (Whole);
            if (negativeNumber)
            {
                if (toConvertBase != 2)
                    TextBox.Text = "-" + TextBox.Text;
                else if(startsWithZero)
                {
                    StringBuilder stringBuilder = new StringBuilder(TextBox.Text);
                    stringBuilder[0] = '1';
                    TextBox.Text = stringBuilder.ToString();
                }
                else
                    TextBox.Text = "1" + TextBox.Text;
            }

        }

        string ConvertWholeToBase(string whole, int toBase)
        {
            long number = long.Parse(whole);    
            string toRet = "";
            while(number != 0)
            {
                long rest = number % toBase;
                number /= toBase;
                toRet = validCharacters[(int)rest+1] + toRet;
            }
            return toRet;
        }

        string ConvertFractionToBase(string fraction, int toBase) //.125
        {
            decimal number = decimal.Parse(fraction);
            number = number / (decimal)Math.Pow(10, fraction.Length);
            string toRet = "";
            List<decimal> decimals = new List<decimal>();
            bool isRepeating = false;
            int decimalPoints = 0, limit = 8;
            while(!isRepeating && decimalPoints < limit)
            {
                number *= toBase;
                int nr = (int)number;
                number -= nr;
                if(decimals.Contains(number))
                {
                    isRepeating = true;
                    break;
                }
                decimals.Add(number);
                toRet +=  validCharacters[nr+1];
                if(number == 0)
                    break;
                decimalPoints++;
            }
            if(isRepeating)
            {
                decimals.IndexOf(number);
                toRet = toRet.Substring(0, decimals.IndexOf(number)) + "(" + toRet.Substring(decimals.IndexOf(number)) + ")";
            }

            return toRet;   
        }

        string ConvertToDecimal(string text) /// 10111.011  101.11101 
        {
            int exp = 0;
            if (text.IndexOf('.') != -1)
                exp = text.IndexOf('.') - 1;
            else exp = text.Length - 1;
            string toRet = "";
            decimal number = 0;
            foreach(char c in text)
            {
                if(c == '.')
                    continue;
                
                int nr  = validCharacters.IndexOf(c) - 1;
                number += (decimal)nr*(decimal)Math.Pow(numberBase, exp);
                exp--;
            }
            toRet = number.ToString();
            return toRet;
        }



    }
}