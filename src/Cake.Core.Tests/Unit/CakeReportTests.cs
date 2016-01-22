namespace Cake.Core.Tests.Unit
{
    using System;
    using System.Linq;
    using Xunit;

    public sealed class CakeReportTests
    {
        public sealed class TheAddMethod
        {
            [Fact]
            public void Should_Add_A_New_Task()
            {
                // Given
                var report = new CakeReport();
                var taskName = "task";
                var duration = TimeSpan.FromMilliseconds(100);

                // When
                report.Add("task", duration);

                // Then
                var firstTask = report.FirstOrDefault();
                
                Assert.NotNull(firstTask);
                Assert.Equal(taskName, firstTask.TaskName);
                Assert.Equal(duration, firstTask.Duration);
                Assert.False(firstTask.Skipped);
            }

            [Fact]
            public void Should_Add_To_End_Of_Sequence()
            {
                // Given
                var report = new CakeReport();
                report.Add("task 1", TimeSpan.FromMilliseconds(100));
                
                var taskName = "task";
                var duration = TimeSpan.FromMilliseconds(200);

                // When
                report.Add(taskName, duration);

                // Then
                var lastTask = report.LastOrDefault();

                Assert.NotNull(lastTask);
                Assert.Equal(taskName, lastTask.TaskName);
                Assert.Equal(duration, lastTask.Duration);
                Assert.False(lastTask.Skipped);
            }
        }

        public sealed class TheAddSkippedMethod
        {
            [Fact]
            public void Should_Add_A_New_Task()
            {
                // Given
                var report = new CakeReport();
                var taskName = "task";

                // When
                report.AddSkipped(taskName);

                // Then
                var firstTask = report.FirstOrDefault();

                Assert.NotNull(firstTask);
                Assert.Equal(taskName, firstTask.TaskName);
                Assert.Equal(TimeSpan.Zero, firstTask.Duration);
                Assert.True(firstTask.Skipped);
            }

            [Fact]
            public void Should_Add_To_End_Of_Sequence()
            {
                // Given
                var report = new CakeReport();
                report.AddSkipped("task 1");

                var taskName = "task 2";

                // When
                report.AddSkipped(taskName);

                // Then
                var lastTask = report.LastOrDefault();

                Assert.NotNull(lastTask);
                Assert.Equal(taskName, lastTask.TaskName);
                Assert.Equal(TimeSpan.Zero, lastTask.Duration);
                Assert.True(lastTask.Skipped);
            }
        }
    }
}