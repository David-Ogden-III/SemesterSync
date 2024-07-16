using C971_Ogden.Database;
using C971_Ogden.ViewModel;
using CommunityToolkit.Maui.Views;

namespace C971_Ogden.Pages;

public partial class AddModifyExamPopup : Popup
{
    public AddModifyExamPopup(AddModifyExamPopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }
    private AddModifyExamPopupViewModel vm;
    public async void Cancel_Clicked(object? sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await CloseAsync(null, cts.Token);
    }

    public async void Save_Clicked(object? sender, EventArgs e)
    {
        DetailedExam? exam = vm.Save();
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await CloseAsync(exam, cts.Token);
    }
}