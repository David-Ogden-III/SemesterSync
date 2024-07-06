using C971_Ogden.Database;
namespace C971_Ogden.Pages;

public partial class EditTerm : ContentPage
{
	public EditTerm(Term selectedTerm)
	{
		InitializeComponent();
		SelectedTerm = selectedTerm;
	}

	public Term SelectedTerm { get; set; }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopAsync();
    }
}