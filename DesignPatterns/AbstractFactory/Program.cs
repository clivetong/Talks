
// https://refactoring.guru/design-patterns/abstract-factory

// UI framework example
//   I need the buttons and dialogs to be from the same platform
//    ie I need CreateButton and createDialog to be of the same type


IGuiFactory factory = new WindowsFactory(); // Choose at runtime

var button = factory.CreateButton();
var textbox = factory.CreateTextBox();

button.Press();
textbox.SetText("Hello world!");

interface IButton 
{
    void Press();
}

interface ITextBox
{
    void SetText(string text);
}


class WindowsTextBox : ITextBox
{
    public void SetText(string text)
    {
        Console.WriteLine($"Set windows textbox text to {text}");
    }
}

class WindowsButton : IButton
{
    public void Press()
    {
        Console.WriteLine($"Set windows button pressed");
    }
}

interface IGuiFactory
{
    IButton CreateButton();
    ITextBox CreateTextBox();
}

class WindowsFactory : IGuiFactory
{
    public IButton CreateButton() => new WindowsButton();
    public ITextBox CreateTextBox() => new WindowsTextBox();
}