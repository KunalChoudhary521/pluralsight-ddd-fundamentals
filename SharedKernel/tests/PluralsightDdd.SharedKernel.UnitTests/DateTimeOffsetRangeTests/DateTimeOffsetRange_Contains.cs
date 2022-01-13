using System;
using FluentAssertions;
using Xunit;

namespace PluralsightDdd.SharedKernel.UnitTests.DateTimeRangeOffsetTests
{
  public class DateTimeOffsetRange_Contains
  {
    [Fact]
    public void ReturnsTrueGivenSameDateTimeOffsetRange()
    {
      var dtor = new DateTimeOffsetRange(DateTimeOffsets.TestDateTime, TimeSpan.FromHours(1));

      var result = dtor.Contains(dtor);

      result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrueGivenRangeWithinRange()
    {
      var dtor = new DateTimeOffsetRange(DateTimeOffsets.TestDateTime, TimeSpan.FromHours(2));

      var within = new DateTimeOffsetRange(DateTimeOffsets.TestDateTime.AddSeconds(1), TimeSpan.FromHours(2).Subtract(TimeSpan.FromSeconds(2)));

      var result = dtor.Contains(within);

      result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalseGivenEarlierStartDateTimeOffset()
    {
      var dtor = new DateTimeOffsetRange(DateTimeOffsets.TestDateTime, TimeSpan.FromHours(2));

      var earlierStart = new DateTimeOffsetRange(DateTimeOffsets.TestDateTime.AddSeconds(-1), TimeSpan.FromHours(1));

      var result = dtor.Contains(earlierStart);

      result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsFalseGivenLaterEndDateTimeOffset()
    {
      var dtor = new DateTimeOffsetRange(DateTimeOffsets.TestDateTime, TimeSpan.FromHours(2));

      var laterEnd = new DateTimeOffsetRange(DateTimeOffsets.TestDateTime, TimeSpan.FromHours(2).Add(TimeSpan.FromSeconds(1)));

      var result = dtor.Contains(laterEnd);

      result.Should().BeFalse();
    }

  }
}