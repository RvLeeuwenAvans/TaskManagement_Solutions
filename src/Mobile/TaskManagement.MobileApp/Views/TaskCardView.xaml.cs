namespace TaskManagement.MobileApp.Views;

public partial class TaskCardView : ContentView
{
    public TaskCardView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty IconLetterProperty = BindableProperty.Create(
        nameof(IconLetter), typeof(string), typeof(TaskCardView));
    
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(TaskCardView));
    
    public static readonly BindableProperty SubtitleProperty = BindableProperty.Create(
        nameof(Subtitle), typeof(string), typeof(TaskCardView));
    
    public static readonly BindableProperty DateTextProperty = BindableProperty.Create(
        nameof(DateText), typeof(string), typeof(TaskCardView));

    public string IconLetter
    {
        get => (string)GetValue(IconLetterProperty);
        set => SetValue(IconLetterProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public string DateText
    {
        get => (string)GetValue(DateTextProperty);
        set => SetValue(DateTextProperty, value);
    }
}