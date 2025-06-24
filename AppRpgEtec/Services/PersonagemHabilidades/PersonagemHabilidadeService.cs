using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppRpgEtec.Models;

namespace AppRpgEtec.Services.PersonagemHabilidades
{
    public class PersonagemHabilidadeService : Request
    {
        private readonly Request _request = null;
        
        private const string _apiUrlBase = "http://luizsouza.somee.com/RpgApi/PersonagemHabilidades/";
        private string _token;
        public PersonagemHabilidadeService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<ObservableCollection<PersonagemHabilidade>> GetPersonagemHabilidadesAsync(int personagemId)
        {
            string urlComplementar = string.Format("{0}", personagemId);
            ObservableCollection<Models.PersonagemHabilidade> listaPH = await
            _request.GetAsync<ObservableCollection<Models.PersonagemHabilidade>>(_apiUrlBase + urlComplementar, _token);
            return listaPH;
        }
        public async Task<ObservableCollection<Habilidade>> GetHabilidadesAsync()
        {
            string urlComplementar = string.Format("{0}", "GetHabilidades");
            ObservableCollection<Models.Habilidade> listaHabilidades = await
            _request.GetAsync<ObservableCollection<Models.Habilidade>>(_apiUrlBase + urlComplementar, _token); return listaHabilidades;
        }








    }
}
