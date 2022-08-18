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
        //Запрос получения списка всех диалогов
        public IEnumerable<RGDialogsClients> Get()
        {
            RGDialogsClients DialogsClass = new RGDialogsClients();
            List<RGDialogsClients> Dialogs = DialogsClass.Init();
            return Dialogs;
        }

        [HttpPost]
        //Запрос поиска диалогов по пользователям
        public IEnumerable<Guid> Post(List<Guid> clientsGuids) //Задаем список GUID пользователей
        {
            //создаем список с GUID будующих подходящих диалогов
            IEnumerable<Guid> Answer = new List<Guid>();

            //Инициализируем список диалогов
            RGDialogsClients DialogsClass = new RGDialogsClients();
            List<RGDialogsClients> Dialogs = DialogsClass.Init();

            //Получаем GUID диалогов, содержащих нужных нам пользлвателей
            var guidDialogsAllowed = from d in Dialogs join c in clientsGuids on d.IDClient equals c select new { guid = d.IDRGDialog };
            //Считаем количество дублирующих GUID 
            var guidDialogsCount = guidDialogsAllowed.GroupBy(g => g.guid).OrderByDescending(o => o.Count()).Select(s => new { guid = s.Key, count = s.Count() });
            //Если количество GUID диалогов совпадает с количеством GUID клиентов полученных на входе => это наш ответ
            Answer = guidDialogsCount.Where(g => g.count == clientsGuids.Count).Select(g => g.guid);

            return Answer;
        }
    }
}
