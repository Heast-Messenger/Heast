using System;
using Client.ViewModel;

namespace Client;

public static partial class Hooks
{
	public static Func<LoginWindowViewModel> UseLoginViewModel()
	{
		var window = UseCurrentWindow();
		return () => (LoginWindowViewModel)window().DataContext!;
	}
}