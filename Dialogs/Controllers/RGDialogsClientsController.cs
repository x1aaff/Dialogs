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
        public Guid? Get([FromQuery] Guid[] clientsIDs)
        {
            var data = new RGDialogsClients().Init();

            var query = data
                .GroupBy(d => d.IDRGDialog)
                .Where(g => g.Count() == clientsIDs.Count())
                .Where(g => g.Select(d => d.IDClient).Intersect(clientsIDs).Count() == g.Count())?
                .FirstOrDefault()?
                .FirstOrDefault()?
                .IDRGDialog;

            if (query != null)
            {
                return query;
            }

            return Guid.Empty;
        }
    }
}
