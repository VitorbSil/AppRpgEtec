using AppRpgEtec.ViewModels.Disputas;

namespace AppRpgEtec.Views.Disputas;

public partial class ListagemView : ContentPage
{
	DisputasViewModel viewModel;
	public ListagemView()
	{
		InitializeComponent();

		viewModel = new DisputasViewModel();
		BindingContext = viewModel;
	}
}