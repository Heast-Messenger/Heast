using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Client.View.Controls;
using Client.ViewModel;

namespace Client.View.Content.Modals;

public partial class EmailVerificationModal : ModalBase
{
    private const string Charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public EmailVerificationModal(IEmailVerifiable emailVerifiable)
    {
        EmailVerifiable = emailVerifiable;
        InitializeComponent();
        KeyDownEvent.AddClassHandler<TopLevel>(OnKeyDown, handledEventsToo: true);
    }

    public bool CanSubmit => Characters.All(x => x.Character is not null);

    private IClipboard? ClipBoardService => TopLevel.GetTopLevel(this)?.Clipboard;

    private IEmailVerifiable EmailVerifiable { get; }

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

    private void Button_Submit(object? sender, RoutedEventArgs e)
    {
        if (CanSubmit)
        {
            EmailVerifiable.VerifySignupCode(VerificationCode);
        }
    }
}