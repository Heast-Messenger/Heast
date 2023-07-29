using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Client.View.Controls;
using Client.ViewModel;

namespace Client.View.Content.Login;

public partial class EmailVerificationPanel : LoginBase
{
    private const string Charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public EmailVerificationPanel()
    {
        InitializeComponent();
        KeyDownEvent.AddClassHandler<TopLevel>(OnKeyDown, handledEventsToo: true);
    }

    private IClipboard? ClipBoardService => TopLevel.GetTopLevel(this)?.Clipboard;

    public required LoginBase Origin { get; init; }

    public override LoginBase Back => Origin;

    public override Size? WindowSize => new(width: 400.0, height: 500.0);

    private LoginWindowViewModel LoginWindowViewModel => (DataContext as LoginWindowViewModel)!;

    private int CursorIndex { get; set; }

    private VerificationCharacter[] Characters => VerificationContainer.GetLogicalChildren().Cast<VerificationCharacter>().ToArray();

    private string VerificationCode => string.Join("", Characters.Select(c => c.Character));

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Right)
        {
            if (CursorIndex < Characters.Length)
            {
                CursorIndex++;
            }
        }

        else if (e.Key == Key.Left)
        {
            if (CursorIndex > 0)
            {
                CursorIndex--;
            }
        }

        else if (e.Key is Key.Back or Key.Delete && CursorIndex > 0)
        {
            Characters[--CursorIndex].Character = ' ';
        }

        else if (e.Key is Key.V && e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            if (ClipBoardService is null)
            {
                return;
            }

            ClipBoardService.GetTextAsync().ContinueWith(task =>
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    var text = task.Result?.Trim();
                    if (text is not null && text.Length == 5)
                    {
                        for (var i = 0; i < Characters.Length; i++)
                        {
                            Characters[i].Character = text[i];
                        }
                    }
                });
            });
        }

        else
        {
            var keycode = e.Key.ToString();
            if (!e.KeyModifiers.HasFlag(KeyModifiers.Shift))
            {
                keycode = keycode.ToLowerInvariant();
            }

            if (Charset.Contains(keycode) && CursorIndex < Characters.Length)
            {
                Characters[CursorIndex++].Character = keycode[0];
            }
        }

        for (var i = 0; i < Characters.Length; i++)
        {
            Characters[i].Classes.Remove("Selected");
            if (i == CursorIndex)
            {
                Characters[i].Classes.Add("Selected");
            }
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        LoginWindowViewModel.VerifySignupCode(VerificationCode);
    }
}