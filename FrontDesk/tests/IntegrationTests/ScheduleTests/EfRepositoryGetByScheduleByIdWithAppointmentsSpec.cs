using System;
using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Core.ScheduleAggregate;
using FrontDesk.Core.ScheduleAggregate.Specifications;
using FrontDesk.Infrastructure.Data;
using PluralsightDdd.SharedKernel;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ScheduleTests
{
  public class EfRepositoryGetByScheduleByIdWithAppointmentsSpec : IClassFixture<SharedDatabaseFixture>
  {
    public SharedDatabaseFixture Fixture { get; }
    public EfRepositoryGetByScheduleByIdWithAppointmentsSpec(SharedDatabaseFixture fixture) => Fixture = fixture;

    [Fact]
    public async Task ReturnScheduleWithAllAppointments()
    {
      using (var transaction = await Fixture.Connection.BeginTransactionAsync())
      {
        var id = Guid.NewGuid();
        var newSchedule = new ScheduleBuilder().WithDefaultValues().WithId(id).Build();

        newSchedule.AddNewAppointment(new AppointmentBuilder().WithDefaultValues().Build());
        newSchedule.AddNewAppointment(new AppointmentBuilder().WithDefaultValues().Build());
        newSchedule.AddNewAppointment(new AppointmentBuilder().WithDefaultValues().Build());

        var repo1 = new EfRepository<Schedule>(Fixture.CreateContext(transaction));
        await repo1.AddAsync(newSchedule);

        var spec = new ScheduleByIdWithAppointmentsSpec(id);
        var repo2 = new EfRepository<Schedule>(Fixture.CreateContext(transaction));
        var scheduleFromRepo = await repo2.GetBySpecAsync(spec);

        Assert.Equal(newSchedule.Id, scheduleFromRepo.Id);
        Assert.True(scheduleFromRepo.Id == id);
        Assert.Equal(3, scheduleFromRepo.Appointments.Count());
      }
    }

    [Fact]
    public async Task ReturnScheduleWithAppointmentsOfTheDay()
    {
      using (var transaction = await Fixture.Connection.BeginTransactionAsync())
      {
        var id = Guid.NewGuid();
        var newSchedule = new ScheduleBuilder().WithDefaultValues().WithId(id).Build();

        var app1Time = new DateTimeOffsetRange(DateTimeOffset.Now.Date.AddHours(9), TimeSpan.FromHours(1));
        var app2Time = new DateTimeOffsetRange(DateTimeOffset.Now.Date.AddHours(10), TimeSpan.FromHours(2));
        var app3Time = new DateTimeOffsetRange(DateTimeOffset.Now.Date.AddDays(1).AddHours(9), TimeSpan.FromHours(1));

        newSchedule.AddNewAppointment(new AppointmentBuilder().WithDefaultValues().WithDateTimeOffsetRange(app1Time).Build());
        newSchedule.AddNewAppointment(new AppointmentBuilder().WithDefaultValues().WithDateTimeOffsetRange(app2Time).Build());
        newSchedule.AddNewAppointment(new AppointmentBuilder().WithDefaultValues().WithDateTimeOffsetRange(app3Time).Build());

        var repo1 = new EfRepository<Schedule>(Fixture.CreateContext(transaction));
        await repo1.AddAsync(newSchedule);

        var today = DateTimeOffsetRange.CreateOneDayRange(DateTime.Today);
        var spec = new ScheduleByIdWithAppointmentsSpec(id, today);
        var repo2 = new EfRepository<Schedule>(Fixture.CreateContext(transaction));
        var scheduleFromRepo = await repo2.GetBySpecAsync(spec);

        Assert.Equal(newSchedule.Id, scheduleFromRepo.Id);
        Assert.True(scheduleFromRepo.Id == id);
        Assert.Equal(2, scheduleFromRepo.Appointments.Count());
      }
    }
  }
}
