using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace BusStation
{
    public class PlaceholderTextBox : TextBox
    {
        //--------------------------------------------------------//
        //                 Клас PlaceholderTextBox                //
        //                                                        //
        // Цей клас має методи для створення, обробки, налашту-   //
        // вання підказок у текстових полях (або так званих       //
        // плейсхолдерів). В проекті використано всього декілька  //
        // методів. Клас розрахований на окреме, незалежне від    //
        // проекту використання.                                  //
        //--------------------------------------------------------//

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(PlaceholderTextBox), new PropertyMetadata(""));

        public static readonly DependencyProperty PlaceholderColorProperty =
            DependencyProperty.Register("PlaceholderColor", typeof(Brush), typeof(PlaceholderTextBox), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public static readonly DependencyProperty ActiveTextColorProperty =
            DependencyProperty.Register("ActiveTextColor", typeof(Brush), typeof(PlaceholderTextBox), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public Brush PlaceholderColor
        {
            get => (Brush)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public Brush ActiveTextColor
        {
            get => (Brush)GetValue(ActiveTextColorProperty);
            set => SetValue(ActiveTextColorProperty, value);
        }

        public PlaceholderTextBox()
        {
            Loaded += (sender, e) => SetPlaceholder();

            GotFocus += (sender, e) => RemovePlaceholder();
            LostFocus += (sender, e) => SetPlaceholder();
            KeyDown += HandleEnterKey;
        }

        private void SetPlaceholder()
        {
            if (string.IsNullOrEmpty(Text) || Text == PlaceholderText)
            {
                Text = PlaceholderText;
                Foreground = PlaceholderColor;
            }
            else
            {
                Foreground = ActiveTextColor;
            }
        }

        private void RemovePlaceholder()
        {
            if (Text == PlaceholderText)
            {
                Text = "";
                Foreground = ActiveTextColor;
            }
        }

        private void HandleEnterKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (!IsFocused)
            {
                SetPlaceholder();
            }
        }

        public void SelectAllOnFocus()
        {
            GotFocus += (sender, e) => SelectAll();
        }

        public void ClearOnEscape()
        {
            PreviewKeyDown += (sender, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    Text = "";
                    e.Handled = true;
                }
            };
        }

        public void EnableCapsLockMode()
        {
            PreviewTextInput += (sender, e) =>
            {
                if (Keyboard.IsKeyToggled(Key.CapsLock))
                {
                    e.Handled = true;
                    Text += e.Text.ToUpper();
                    CaretIndex = Text.Length;
                }
            };
        }

        public void EnableNumericOnly()
        {
            PreviewTextInput += (sender, e) =>
            {
                e.Handled = !decimal.TryParse(e.Text, out _);
            };

            PreviewKeyDown += (sender, e) =>
            {
                if (e.Key == Key.Space)
                {
                    e.Handled = true;
                }
            };
        }

        public void AppendTextWithPlaceholderCheck(string additionalText)
        {
            if (Text == PlaceholderText)
                Text = "";

            Text += additionalText;
            CaretIndex = Text.Length;
        }

        public void TransformText(Func<string, string> transformation)
        {
            int caretPosition = CaretIndex;
            Text = transformation(Text);
            CaretIndex = Math.Min(caretPosition, Text.Length);
        }

        public void EnableAutoCapitalization()
        {
            LostFocus += (sender, e) =>
            {
                if (Text != PlaceholderText && !string.IsNullOrWhiteSpace(Text))
                {
                    Text = char.ToUpper(Text[0]) + Text.Substring(1);
                }
            };
        }

        public void EnableTrimmingOnFocusLost()
        {
            LostFocus += (sender, e) =>
            {
                if (Text != PlaceholderText)
                {
                    Text = Text.Trim();
                }
            };
        }

        public void SetRegexValidation(string regexPattern)
        {
            var regex = new System.Text.RegularExpressions.Regex(regexPattern);
            PreviewTextInput += (sender, e) =>
            {
                if (!regex.IsMatch(e.Text))
                {
                    e.Handled = true;
                }
            };
        }

        public void HighlightWords(string word, Brush highlightColor)
        {
            // Метод створено для RichTextBox. Так як проєкт працює тільки з TextBox,
            // код для метода не було прописано. За потреби користувач сам може додати
            // код обробки для RichTextBox.
        }
    }
}