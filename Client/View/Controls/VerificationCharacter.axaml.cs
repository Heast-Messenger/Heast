using Avalonia;
using Avalonia.Controls.Primitives;

namespace Client.View.Controls;

public class VerificationCharacter : TemplatedControl
{
    public static readonly DirectProperty<VerificationCharacter, char?> CharacterProperty =
        AvaloniaProperty.RegisterDirect<VerificationCharacter, char?>(nameof(Character),
            o => o.Character,
            (o, v) => o.Character = v);

    private char? _char;

    public char? Character
    {
        get => _char;
        set => SetAndRaise(CharacterProperty, ref _char, value);
    }
}