using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DialogsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<RGDialogsClients> Get()
        {
            RGDialogsClients DialogsClass = new RGDialogsClients();
            List<RGDialogsClients> Dialogs = DialogsClass.Init();
            return Dialogs;
        }

        [HttpPost]
        public IEnumerable<Guid> Post(List<Guid> clientsGuids)
        {
            IEnumerable<Guid> Answer = new List<Guid>();

            RGDialogsClients DialogsClass = new RGDialogsClients();
            List<RGDialogsClients> Dialogs = DialogsClass.Init();

            var guidDialogsAllowed = from d in Dialogs join c in clientsGuids on d.IDClient equals c select new { guid = d.IDRGDialog };
            var guidDialogsCount = guidDialogsAllowed.GroupBy(g => g.guid).OrderByDescending(o => o.Count()).Select(s => new { guid = s.Key, count = s.Count() });
            Answer = guidDialogsCount.Where(g => g.count == clientsGuids.Count).Select(g => g.guid);

            return Answer;
        }
    }
}
