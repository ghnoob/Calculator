using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool flagClearScreen;
        private double operationFistTerm, operationSecondTerm;
        private char operationSign;

        public MainWindow()
        {
            InitializeComponent();
            flagClearScreen = true;
            operationFistTerm = 0;
            operationSecondTerm = 0;
            operationSign = default;
        }

        private void number_Click(object sender, RoutedEventArgs e) => AddDigit((sender as Button).Content.ToString());
        private void clearEntry_Click(object sender, RoutedEventArgs e) => ClearEntry();
        private void clear_Click(object sender, RoutedEventArgs e) => Clear();
        private void decimalDot_Click(object sender, RoutedEventArgs e) => AddDecimalDot();
        private void binaryOperation_Click(object sender, RoutedEventArgs e) => SetOperationSign(Convert.ToChar((sender as Button).Content));
        private void equals_Click(object sender, RoutedEventArgs e) => SetResultOnScreen();

        private void squareRoot_Click(object sender, RoutedEventArgs e)
        {
            double sqrt = Math.Sqrt(Convert.ToDouble(Screen.Content, CultureInfo.InvariantCulture));
            Screen.Content = sqrt;
            if (double.IsNaN(sqrt))
            {
                Screen.Content = "Math Error";
                ResetVariables();
            }
            else flagClearScreen = true;
        }

        private void changeSign_Click(object sender, RoutedEventArgs e)
        {
            double number = Convert.ToDouble(Screen.Content, CultureInfo.InvariantCulture);
            number *= -1;
            Screen.Content = number;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key keyCode when (keyCode >= Key.D0 && keyCode <= Key.D9) || (keyCode >= Key.NumPad0 && keyCode <= Key.NumPad9):
                    string key = e.Key.ToString();
                    key = key[key.Length - 1].ToString();
                    AddDigit(key);
                    break;
                case Key.Back:
                    ClearEntry();
                    break;
                case Key.Enter:
                    SetResultOnScreen();
                    break;
                case Key.Escape:
                case Key.Delete:
                    Clear();
                    break;
                case Key.Multiply:
                    SetOperationSign('*');
                    break;
                case Key.Add:
                    SetOperationSign('+');
                    break;
                case Key.Subtract:
                    SetOperationSign('-');
                    break;
                case Key.Divide:
                    SetOperationSign('/');
                    break;
                case Key.OemPeriod:
                    AddDecimalDot();
                    break;
            }
        }

        private void ClearEntry()
        {
            if (!flagClearScreen)
            {
                Screen.Content = 0;
                flagClearScreen = true;
            }
        }

        private void Clear()
        {
            Screen.Content = 0;
            ResetVariables();
        }

        private void AddDigit(string digit)
        {
            if (flagClearScreen) Screen.Content = digit;
            else Screen.Content = string.Concat(Screen.Content, digit);
            if (digit != "0") flagClearScreen = false;
        }
        
        private void AddDecimalDot()
        {
            if (flagClearScreen) Screen.Content = "0";
            if (!Screen.Content.ToString().Contains('.')) Screen.Content += ".";
            flagClearScreen = false;
        }

        private void SetOperationSign(char sign)
        {
            if (operationSign != default && !flagClearScreen) SetResultOnScreen();
            operationSign = sign;
            if (Screen.Content.ToString() != "Math Error") operationFistTerm = Convert.ToDouble(Screen.Content, CultureInfo.InvariantCulture);
            flagClearScreen = true;
        }

        private double SolveBinaryOperation(double a, double b)
        {
            switch (operationSign)
            {
                case '+':
                    return a + b;
                case '-':
                    return a - b;
                case '*':
                    return a * b;
                case '/':
                    return a / b;
                default:
                    return b;
            }
        }

        private void SetResultOnScreen()
        {
            if (!flagClearScreen) operationSecondTerm = Convert.ToDouble(Screen.Content, CultureInfo.InvariantCulture);
            double result = SolveBinaryOperation(operationFistTerm, operationSecondTerm);
            if (!double.IsInfinity(result))
            {
                operationFistTerm = SolveBinaryOperation(operationFistTerm, operationSecondTerm);
                Screen.Content = operationFistTerm;
            }
            else
            {
                Screen.Content = "Math Error";
                ResetVariables();
            }
            flagClearScreen = true;
        }
        private void ResetVariables()
        {
            flagClearScreen = true;
            operationFistTerm = 0;
            operationSecondTerm = 0;
            operationSign = default;
        }
    }
}
