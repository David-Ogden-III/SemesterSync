using C971_Ogden.ViewModel;

namespace C971_Ogden.Pages;

public partial class TermDetails : ContentPage
{
	public TermDetails(TermDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

	public void Cancel_Clicked(object sender, EventArgs e)
	{
		Navigation.PopModalAsync();
	}

	public async void Save_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}

	//public void NewClassSelected(object sender, EventArgs e)
	//{
	//       var picker = (Picker)sender;
	//       int selectedIndex = picker.SelectedIndex;

	//       if (selectedIndex != -1)
	//       {
	//		Class1 selectedClass = (Class1)picker.ItemsSource[selectedIndex];
	//           SelectedClasses.Add(selectedClass);
	//		Debug.WriteLine($"\nAdded {selectedClass.ClassName}\n");
	//       }
	//   }

	public void RemoveClass_Clicked(object sender, EventArgs e)
	{
        Button clickedButton = (Button)sender;
        object classToRemove = (object)clickedButton.BindingContext;
		var vm = (TermDetailsViewModel)BindingContext;
		if (vm.RemoveClassCommand.CanExecute(classToRemove))
		{
			vm.RemoveClassCommand.Execute(classToRemove);
		}
	}



}