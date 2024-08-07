using ViewModelLibrary;

namespace SemesterSync.Views;

public partial class Progress : ContentPage
{
	public Progress(ProgressViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}