using _WebAppHang.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _WebAppHang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IGuidService _guidService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IServiceProvider _serviceProvider;

        public ValuesController(IGuidService guidService,
            IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
        {
            _guidService = guidService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
            _serviceProvider = serviceProvider;
        }



        [HttpGet]
        public ActionResult Get()
        {
            ProgramarTarea();

            return Ok("Empezo la ejecucion jojo");
        }
        private void ProgramarTarea()
        {
            _recurringJobManager.AddOrUpdate("Esto correra cada 1 minuto desde el controller value",
                                    () => _guidService.GetRandomIdentifier(),
                                    Cron.Minutely);
        }


        /*
        backgroundJobClient.Enqueue(() => Console.WriteLine("Hola desde Hangfire"));

        backgroundJobClient.Schedule(() => Console.WriteLine("Tarea programada"), TimeSpan.FromSeconds(30));

        recurringJobManager.AddOrUpdate("Esto correra cada 1 minuto",
                                        () => Console.WriteLine("Esto es una tarea recurrente"),
                                        Cron.Minutely);



        var myService = serviceProvider.GetRequiredService<IGuidService>();

        recurringJobManager.AddOrUpdate("Esto correra cada 1 minuto",
                            () => myService.GetRandomIdentifier(),
                            Cron.Minutely);
        */

    }
}
