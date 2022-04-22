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

        public ValuesController(IGuidService guidService,
            IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _guidService = guidService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }



        [HttpGet]
        public ActionResult Get()
        {
            ProgramarTareaRecurrente();
            ProgramarEjecucionDeTareaUnaSolaVez();
            MandarTareaALaColaParaEjecutarYa();
            return Ok("Empezo la ejecucion jojo");
        }
        private void ProgramarTareaRecurrente()
        {
            _recurringJobManager.AddOrUpdate("Esto correra cada 1 minuto desde el controller value",
                                    () => _guidService.GetRandomIdentifier(),
                                    Cron.Minutely);
        }

        private void ProgramarEjecucionDeTareaUnaSolaVez()
        {
            _backgroundJobClient.Schedule(() => _guidService.GetRandomIdentifier(),
                                            TimeSpan.FromSeconds(45));
        }

        private void MandarTareaALaColaParaEjecutarYa()
        {
            _backgroundJobClient.Enqueue(() => _guidService.GetRandomIdentifier());
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
