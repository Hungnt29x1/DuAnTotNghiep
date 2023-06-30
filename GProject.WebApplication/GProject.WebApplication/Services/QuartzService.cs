using Quartz.Impl;
using Quartz;

namespace GProject.WebApplication.Services
{
    public class QuartzService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IScheduler _scheduler;

        public QuartzService()
        {
            _schedulerFactory = new StdSchedulerFactory();
            _scheduler = _schedulerFactory.GetScheduler().GetAwaiter().GetResult();
        }

        public async Task Start()
        {
            _scheduler.Start().GetAwaiter().GetResult();

            var job = JobBuilder.Create<ReturnProductJob>()
                .WithIdentity("ReturnProductJob", "Group1")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("ReturnProductTrigger", "Group1")
                .WithCronSchedule("0 */1 * * * ?")
                .StartNow()
                .Build();

            _scheduler.ScheduleJob(job, trigger).GetAwaiter().GetResult();

            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(60));
            }
        }

        public void Stop()
        {
            _scheduler.Shutdown().GetAwaiter().GetResult();
        }
    }
}
