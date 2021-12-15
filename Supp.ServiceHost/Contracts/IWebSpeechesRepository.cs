using Supp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface IWebSpeechesRepository
    {
        Task<WebSpeechResult> GetAllWebSpeeches();

        Task<WebSpeechResult> GetWebSpeechesById(long id);

        Task<WebSpeechResult> UpdateWebSpeech(WebSpeechDto dto);

        Task<WebSpeechResult> AddWebSpeech(WebSpeechDto dto);

        Task<WebSpeechResult> DeleteWebSpeechById(long id);
    }
}
