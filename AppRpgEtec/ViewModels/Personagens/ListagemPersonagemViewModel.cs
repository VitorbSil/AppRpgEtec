using AppRpgEtec.Models;
using AppRpgEtec.Services.Personagens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Personagens
{
    public class ListagemPersonagemViewModel : BaseViewModel
    {
        private PersonagemService pService;
        public ObservableCollection<Personagem> Personagens { get; set; }
        public ListagemPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);
            Personagens = new ObservableCollection<Personagem>();
            //PersonagemSelecionado = new Personagem();

            _ = ObterPersonagens();
            NovoPersonagemCommand = new Command(async () => { await ExibirCadastroPersonagem(); });

            RemoverPersonagemCommand =
                new Command<Personagem>(async (Personagem p) => { await RemoverPersonagem(p); });
            ZerarRankingRestaurarVidasGeralCommand =
                new Command(async () => { await ZerarRankingRestaurarVidasGeral(); });
        }
        public ICommand NovoPersonagemCommand { get; }
        public ICommand RemoverPersonagemCommand { get; set; }
        public ICommand ZerarRankingRestaurarVidasGeralCommand { get; set; }

        //Próximos elementos da classe aqui
        public async Task ObterPersonagens()
        {
            try //Junto com o Cacth evitará que erros fechem o aplicativo
            {
                Personagens = await pService.GetPersonagensAsync();
                OnPropertyChanged(nameof(Personagens)); //Informará a View que houve carregamento                       
            }
            catch (Exception ex)
            {
                //Captará o erro para exibir em tela
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ExibirCadastroPersonagem()
        {
            try
            {
                await Shell.Current.GoToAsync("cadPersonagemView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private Personagem personagemSelecionado;//CTRL + R,E

        public Personagem PersonagemSelecionado
        {
            get { return personagemSelecionado; }
            set
            {
                if (value != null)
                {
                    personagemSelecionado = value;

                    /*Shell.Current
                        .GoToAsync($"cadPersonagemView?pId={personagemSelecionado.Id}");*/

                    _ = ExibirOpcoesAsync(personagemSelecionado);
                }
            }
        }

        public async Task RemoverPersonagem(Personagem p)
        {
            try
            {
                if (await Application.Current.MainPage
                        .DisplayAlert("Confirmação", $"Confirma a remoção de {p.Nome}?", "Sim", "Não"))
                {
                    await pService.DeletePersonagemAsync(p.Id);

                    await Application.Current.MainPage.DisplayAlert("Mensagem",
                        "Personagem removido com sucesso!", "Ok");

                    _ = ObterPersonagens();
                }
            }
            catch (Exception ex)
            {

                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ExecutarRestaurarPontosPersonagem(Personagem p)
        {
            await pService.PutRestaurarPontosAsync(p);
        }

        public async Task ExecutarZerarRankingPersonagem(Personagem p)
        {
            await pService.PutZerarRankingAsync(p);
        }

        public async Task ExecutarZerarRankingRestaurarVidasGeral()
        {
            await pService.PutZerarRankingRestaurarVidasGeralAsync();
        }

        public async void ProcessarOpcaoRespondidaAsync(Personagem personagem, string result)
        {
            if (result.Equals("Editar Personagem"))
            {
                await Shell.Current
                .GoToAsync($"cadPersonagemView?pId={personagem.Id}");
            }
            else if (result.Equals("Remover Personagem"))
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação",
                $"Deseja realmente remover o personagem {personagem.Nome.ToUpper()}?",
                "Yes", "No"))
                {
                    await RemoverPersonagem(personagem);
                    await Application.Current.MainPage.DisplayAlert("Informação",
                    "Personagem removido com sucesso!", "Ok");
                    await ObterPersonagens();
                }
            }
            else if (result.Equals("Restaurar Pontos de Vida"))
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação",
                $"Restaurar os pontos de vida de {personagem.Nome.ToUpper()}?", "Yes", "No"))
                {
                    await ExecutarRestaurarPontosPersonagem(personagem);
                    await Application.Current.MainPage.DisplayAlert("Informação",
                    "Os pontos foram restaurados com sucesso.", "Ok");
                    await ObterPersonagens();
                }
            }
            else if (result.Equals("Zerar Ranking do Personagem"))
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação",
                $"Zerar o ranking de {personagem.Nome.ToUpper()}?", "Yes", "No"))
                {
                    await ExecutarZerarRankingPersonagem(personagem);
                    await Application.Current.MainPage.DisplayAlert("Informação",
                    "O ranking foi zerado com sucesso.", "Ok");
                    await ObterPersonagens();
                }
            }
        }

        public async Task ExibirOpcoesAsync(Personagem personagem)
        {
            try
            {
                personagemSelecionado = null;
                string result = string.Empty;

                if(personagem.PontosVida > 0)
                {
                    result = await Application.Current.MainPage
                                        .DisplayActionSheet("Opções para o personagem " + personagem.Nome,
                                        "Cancelar",
                                        "Editar Personagem",
                                        "Restaurar Pontos de Vida",
                                        "Zerar Ranking do Personagem",
                                        "Remover Personagem");
                }
                else
                {
                    result = await Application.Current.MainPage
                        .DisplayActionSheet("Opções para o personagem " + personagem.Nome,
                        "Cancelar",
                        "Restaurar Pontos de Vida");

                    if (result != null)
                        ProcessarOpcaoRespondidaAsync(personagem, result);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        public async Task ZerarRankingRestaurarVidasGeral()
        {
            try
            {
                if(await Application.Current.MainPage.DisplayAlert("Confirmação",
                    "Deseja realmente zerar todo o ranking?", "Yes", "No"))
                {
                    await ExecutarZerarRankingRestaurarVidasGeral();

                    await Application.Current.MainPage
                        .DisplayAlert("Informação", "Ranking zerado com sucesso", "Ok");

                    await ObterPersonagens();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }









    }//Fim da classe
}
