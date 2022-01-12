using System;
using System.Linq;
using Ardalis.Specification;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.ScheduleAggregate.Specifications
{
  public class ScheduleByIdWithAppointmentsSpec : Specification<Schedule>, ISingleResultSpecification
  {
    public ScheduleByIdWithAppointmentsSpec(Guid scheduleId)
    {
      Query
        .Where(schedule => schedule.Id == scheduleId)
        .Include(schedule => schedule.Appointments); // NOTE: Includes *all* appointments
    }

    public ScheduleByIdWithAppointmentsSpec(Guid scheduleId, DateTimeOffsetRange dateTimeOffsetRange)
    {
      var allAppointments =
      Query
        .Where(schedule => schedule.Id == scheduleId)
        .Include(schedule => schedule.Appointments
            .Where(app => app.TimeRange.Start >= dateTimeOffsetRange.Start && app.TimeRange.End <= dateTimeOffsetRange.End));

    }
  }
}
