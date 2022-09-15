using Microsoft.AspNetCore.Mvc;
using Dialogs.Models;

namespace Dialogs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RGDialogsClientsController : ControllerBase
    {
        private readonly ILogger<RGDialogsClientsController> _logger;

        public RGDialogsClientsController(ILogger<RGDialogsClientsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetClientsDialog")]
        public Guid Get([FromQuery]Guid[] clientsIDs)
        {
            var data = new RGDialogsClients();
            var dataList = data.Init();

            //инициализация переменной, которая будет содержать результаты поиска
            HashSet<Guid> clientDialogs = new();

            foreach (var dialog in dataList.Select(d => d.IDRGDialog).ToHashSet())
            {
                //чаты с таким же количеством участников, что и заявлено в параметрах метода
                if (dataList.Where(d => d.IDRGDialog == dialog).Select(d => d.IDRGDialog).Count() == clientsIDs.Length)
                {
                    clientDialogs.Add(dialog);
                }
            }

            foreach (var clientID in clientsIDs)
            {
                //чаты, где состоит рассматриваемый клиент
                clientDialogs.IntersectWith(dataList.Where(d => d.IDClient == clientID).Select(d => d.IDRGDialog));
            }

            if (clientDialogs.Any())
            {
                return clientDialogs.First();
            }

            return Guid.Empty;
        }
    }
}
