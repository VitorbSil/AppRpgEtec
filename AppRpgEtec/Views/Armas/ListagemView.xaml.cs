using AppRpgEtec.ViewModels.Armas;
using AppRpgEtec.ViewModels.Personagens;

namespace AppRpgEtec.Views.Armas;

public partial class ListagemView : ContentPage
{
    ListagemArmaViewModel viewModel;

    public ListagemView()
	{
		InitializeComponent();

        viewModel = new ListagemArmaViewModel();
        BindingContext = viewModel;
        Title = "Armas";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.ObterArmas();
    }
}