using System.Diagnostics;
using System.Text;
using System;

namespace Conversii
{
    public partial class MainPage : ContentPage
    {
        readonly Color Good, Bad;
        int numberBase = 2, toConvertBase = 10;
        string validCharacters = ".0123456789ABCDEF";
        int textLenght = 16;

        public MainPage()
        {
            InitializeComponent();
            Base.SelectedItem = "Base-2";
            BaseToConvert.SelectedItem = "Base-10";
            Good = new Color(0, 0, 0);
            Bad = new Color(255, 0, 0);
        }


        private void OnBaseChanged(object sender, EventArgs e)
        {
            numberBase = Base.SelectedIndex + 2;
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
            //We reset the output text
            TextBox.Text = "";
            if(TextEntry.Text == null || TextEntry.Text.Length == 0 || TextEntry.Text.Length > textLenght)
                return;

            string text = TextEntry.Text;
            //currentValid holds the valid characters for the current base
            string currentValid = validCharacters.Substring(0, numberBase + 1);
            bool isValid = true;
            //going through the text and checking if it contains only valid characters
            foreach (char c in text)
            {
                
                if(!currentValid.Contains(c))
                {
                    isValid = false;
                    break;
                }
                 
            }
            //if the input has this format x. we don't allow it
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
            
            //We convert the number to decimal based on if it has decimal points or not
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
            //We add the whole part and the fraction part to the output text
            TextBox.Text = hasDecimal ? (Whole + "." + Fraction) : (Whole);
            

        }

        //Converts the whole part of the number from decimal to any base
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

        //converts the fraction part of the number from decimal to any base
        string ConvertFractionToBase(string fraction, int toBase)
        {
            decimal number = decimal.Parse(fraction);
            number /= (decimal)Math.Pow(10, fraction.Length);
            string toRet = "";
            List<decimal> decimals = new (); // it stores the decimals that we already encountered
            bool isRepeating = false;
            int decimalPoints = 0, limit = 8; //limit is the maximum number of decimal points we allow
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
            //if a number is repeating we add the brackets
            if(isRepeating)
            {
                decimals.IndexOf(number);
                toRet = string.Concat(toRet.AsSpan(0, decimals.IndexOf(number)), "(", toRet.AsSpan(decimals.IndexOf(number)), ")");
            }

            return toRet;   
        }

        //Converts a number from any base to decimal
        string ConvertToDecimal(string text)
        {
            int exp;
            if (text.IndexOf('.') != -1)
                exp = text.IndexOf('.') - 1;
            else exp = text.Length - 1;
            string toRet;
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