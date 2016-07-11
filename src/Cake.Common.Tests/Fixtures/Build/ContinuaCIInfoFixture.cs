// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Build.ContinuaCI.Data;
using Cake.Core;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace Cake.Common.Tests.Fixtures.Build
{
	internal sealed class ContinuaCIInfoFixture
	{
		public ICakeEnvironment Environment { get; set; }

		public ContinuaCIInfoFixture()
		{
			Environment = Substitute.For<ICakeEnvironment>();

			//ContinuaCIBuildInfo
			Environment.GetEnvironmentVariable("ContinuaCI.Build.Id").Returns("99");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.Version").Returns("v1.2.3");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.StartedBy").Returns("TestTrigger");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.IsFeatureBranchBuild").Returns("true");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.BuildNumber").Returns("999");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.Started").Returns("2015-12-15T22:53:37.847+01:00");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.UsesDefaultBranch").Returns("false");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.HasNewChanges").Returns("true");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.ChangesetCount").Returns("6");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.IssueCount").Returns("3");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.Elapsed").Returns(TimeSpan.FromMinutes(5).ToString());
			Environment.GetEnvironmentVariable("ContinuaCI.Build.TimeOnQueue").Returns("7777");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.Repositories").Returns("Repo1,Repo2,Repo3");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.RepositoryBranches").Returns("Branch1,Branch2,Branch3");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.TriggeringBranch").Returns("Branch2");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.ChangesetRevisions").Returns("6,8,65");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.ChangesetUserNames").Returns("george,bill");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.ChangesetTagNames").Returns("tag1,tag2,tag 3");

			//ContinuaCIChangesetInfo
			Environment.GetEnvironmentVariable("ContinuaCI.Build.LatestChangeset.Revision").Returns("55");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.LatestChangeset.Branch").Returns("master");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.LatestChangeset.Created").Returns("2016-01-02T12:00:16.666+11:00");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.LatestChangeset.FileCount").Returns("77");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.LatestChangeset.UserName").Returns("georgedawes");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.LatestChangeset.TagCount").Returns("2");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.LatestChangeset.IssueCount").Returns("3");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.LatestChangeset.TagNames").Returns("the tag,the other tag");
			Environment.GetEnvironmentVariable("ContinuaCI.Build.LatestChangeset.IssueNames").Returns("an important issue,another more important issue,a not so important issue");

			//ContinuaCIProjectInfo
			Environment.GetEnvironmentVariable("ContinuaCI.Project.Name").Returns("the project from hell");

			//ContinuaCIConfigurationInfo
			Environment.GetEnvironmentVariable("ContinuaCI.Configuration.Name").Returns("The configuration from the end of the universe");

			//ContinuaCIEnvironmentInfo
			Environment.GetEnvironmentVariables().Returns(new Dictionary<string, string>()
															{
																{ "ContinuaCI.Variable.TestVar1", "gorgonzola" },
																{ "ContinuaCI.Variable.TestVar2", "is" },
																{ "ContinuaCI.Variable.TestVarX", "tasty" },
																{ "This.Is.A.Dummy", "Init?" },
																{ "ContinuaCI.AgentProperty.DotNet.4.0.FrameworkPathX64", @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319" },
																{ "ContinuaCI.AgentProperty.MSBuild.4.0.PathX86", @"C:\Windows\Microsoft.NET\Framework\v4.0.30319" },
																{ "ContinuaCI.AgentProperty.ServerFileTransport.UNCAvailable", "True" }
															});


			Environment.GetEnvironmentVariable("ContinuaCI.Version").Returns("v1.6.6.6");
		}

		public ContinuaCIBuildInfo CreateBuildInfo()
		{
			return new ContinuaCIBuildInfo(Environment, "ContinuaCI.Build");
		}

		public ContinuaCIEnvironmentInfo CreateEnvironmentInfo()
		{
			return new ContinuaCIEnvironmentInfo(Environment);
		}

		public ContinuaCIProjectInfo CreateProjectInfo()
		{
			return new ContinuaCIProjectInfo(Environment, "ContinuaCI.Project");
		}

		public ContinuaCIConfigurationInfo CreateConfigurationInfo()
		{
			return new ContinuaCIConfigurationInfo(Environment, "ContinuaCI.Configuration");
		}

		public ContinuaCIChangesetInfo CreateChangesetInfo()
		{
			return new ContinuaCIChangesetInfo(Environment, "ContinuaCI.Build.LatestChangeset");
		}
	}
}
