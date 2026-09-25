// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using Cake.Common.Build.GoCD;
using Cake.Common.Build.GoCD.Data;

namespace Cake.Common.Tests.Unit.Build.GoCD.Data;

public sealed class GoCDHistoryInfoTests
{
    public sealed class TheDeserializeMethod
    {
        [Fact]
        public void Should_Deserialize_Legacy_Pipeline_History_Payload()
        {
            // Given
            const string json =
                """
                {
                  "pipelines": [
                    {
                      "build_cause": {
                        "approver": "anonymous",
                        "material_revisions": [
                          {
                            "modifications": [
                              {
                                "email_address": null,
                                "id": 1,
                                "modified_time": 1434957613000,
                                "user_name": "Pick E Reader <pick.e.reader@example.com>",
                                "comment": "my hola mundo changes",
                                "revision": "c194b49db102b705ebc13e604e490ae13ac92d96"
                              }
                            ],
                            "material": {
                              "description": "URL: https://github.com/gocd/gocd, Branch: master",
                              "fingerprint": "f6e7a3899c55e1682ffb00383bdf8f882bcee2141e79a8728254190a1fddcf4f",
                              "type": "Git",
                              "id": 1
                            },
                            "changed": false
                          }
                        ],
                        "trigger_forced": true,
                        "trigger_message": "Forced by anonymous"
                      },
                      "name": "pipeline1",
                      "natural_order": 11,
                      "can_run": true,
                      "comment": null,
                      "stages": [
                        {
                          "name": "stage1",
                          "approved_by": "admin",
                          "jobs": [
                            {
                              "name": "job1",
                              "result": "Failed",
                              "state": "Completed",
                              "id": 13,
                              "scheduled_date": 1436172201081
                            }
                          ],
                          "can_run": true,
                          "result": "Failed",
                          "approval_type": "success",
                          "counter": "1",
                          "id": 13,
                          "operate_permission": true,
                          "rerun_of_counter": null,
                          "scheduled": true
                        }
                      ],
                      "counter": 11,
                      "id": 13,
                      "preparing_to_schedule": false,
                      "label": "11"
                    }
                  ],
                  "pagination": {
                    "offset": 0,
                    "total": 2,
                    "page_size": 10
                  }
                }
                """;

            // When
            var result = JsonSerializer.Deserialize(json, GoCDJsonContext.Default.GoCDHistoryInfo);

            // Then
            Assert.NotNull(result);
            var pipeline = Assert.Single(result.Pipelines);
            Assert.Equal("pipeline1", pipeline.Name);
            Assert.Null(pipeline.Comment);
            Assert.Equal("11", pipeline.NaturalOrder);
            Assert.Equal("anonymous", pipeline.BuildCause.Approver);
            Assert.True(pipeline.BuildCause.TriggerForced);
            Assert.Equal("Forced by anonymous", pipeline.BuildCause.TriggerMessage);

            var revision = Assert.Single(pipeline.BuildCause.MaterialRevisions);
            Assert.False(revision.Changed);
            var modification = Assert.Single(revision.Modifications);
            Assert.Null(modification.EmailAddress);
            Assert.Equal(1, modification.Id);
            Assert.Equal(1434957613000, modification.ModifiedTimeUnixMilliseconds);
            Assert.Equal(new DateTime(2015, 6, 22, 7, 20, 13, DateTimeKind.Utc), modification.ModifiedTime);
            Assert.Equal(DateTimeKind.Utc, modification.ModifiedTime.Kind);
            Assert.Equal("Pick E Reader <pick.e.reader@example.com>", modification.Username);
            Assert.Equal("my hola mundo changes", modification.Comment);
            Assert.Equal("c194b49db102b705ebc13e604e490ae13ac92d96", modification.Revision);
        }

        [Fact]
        public void Should_Deserialize_Current_V1_Pipeline_History_Payload()
        {
            // Given
            const string json =
                """
                {
                  "_links": {
                    "next": {
                      "href": "http://ci.example.com/go/api/pipelines/pipeline1/history?after=35"
                    },
                    "previous": {
                      "href": "http://ci.example.com/go/api/pipelines/pipeline1/history?before=9"
                    }
                  },
                  "pipelines": [
                    {
                      "name": "pipeline1",
                      "counter": 13,
                      "label": "13",
                      "natural_order": 13.0,
                      "can_run": true,
                      "preparing_to_schedule": false,
                      "comment": null,
                      "scheduled_date": 1436519914578,
                      "build_cause": {
                        "trigger_message": "modified by user <user@users.noreply.github.com>",
                        "trigger_forced": false,
                        "approver": "",
                        "material_revisions": [
                          {
                            "changed": false,
                            "material": {
                              "name": "https://github.com/gocd/gocd",
                              "fingerprint": "de08b34d116a1cfc1fb988b6683bf21",
                              "type": "Git",
                              "description": "URL: https://github.com/gocd/gocd, Branch: master"
                            },
                            "modifications": [
                              {
                                "revision": "40f0a7ba43df37794cfc78b158483b",
                                "modified_time": 1436519914378,
                                "user_name": "user <user@users.noreply.github.com>",
                                "comment": "some commit message.",
                                "email_address": null
                              }
                            ]
                          }
                        ]
                      },
                      "stages": [
                        {
                          "result": "Failed",
                          "status": "Failed",
                          "rerun_of_counter": null,
                          "name": "pipeline1_stage",
                          "counter": "1",
                          "scheduled": true,
                          "approval_type": "success",
                          "approved_by": "changes",
                          "operate_permission": true,
                          "can_run": true,
                          "jobs": [
                            {
                              "name": "pipeline1_job",
                              "scheduled_date": 1436582744378,
                              "state": "Completed",
                              "result": "Failed"
                            }
                          ]
                        }
                      ]
                    }
                  ]
                }
                """;

            // When
            var result = JsonSerializer.Deserialize(json, GoCDJsonContext.Default.GoCDHistoryInfo);

            // Then
            Assert.NotNull(result);
            var pipeline = Assert.Single(result.Pipelines);
            Assert.Equal("pipeline1", pipeline.Name);
            Assert.Null(pipeline.Comment);
            Assert.Equal("13", pipeline.NaturalOrder);
            Assert.Equal(string.Empty, pipeline.BuildCause.Approver);
            Assert.False(pipeline.BuildCause.TriggerForced);
            Assert.Equal("modified by user <user@users.noreply.github.com>", pipeline.BuildCause.TriggerMessage);

            var revision = Assert.Single(pipeline.BuildCause.MaterialRevisions);
            Assert.False(revision.Changed);
            var modification = Assert.Single(revision.Modifications);
            Assert.Null(modification.EmailAddress);
            Assert.Equal(0, modification.Id);
            Assert.Equal(1436519914378, modification.ModifiedTimeUnixMilliseconds);
            Assert.Equal("user <user@users.noreply.github.com>", modification.Username);
            Assert.Equal("some commit message.", modification.Comment);
            Assert.Equal("40f0a7ba43df37794cfc78b158483b", modification.Revision);
        }

        [Fact]
        public void Should_Deserialize_Natural_Order_When_Json_String()
        {
            // Given
            const string json =
                """
                {
                  "pipelines": [
                    {
                      "name": "pipeline1",
                      "natural_order": "12",
                      "build_cause": {
                        "approver": "changes",
                        "material_revisions": [],
                        "trigger_forced": false,
                        "trigger_message": ""
                      }
                    }
                  ]
                }
                """;

            // When
            var result = JsonSerializer.Deserialize(json, GoCDJsonContext.Default.GoCDHistoryInfo);

            // Then
            Assert.Equal("12", Assert.Single(result.Pipelines).NaturalOrder);
        }
    }

    public sealed class TheSerializeMethod
    {
        [Fact]
        public void Should_Emit_Snake_Case_Names_Without_ModifiedTime()
        {
            // Given
            var history = new GoCDHistoryInfo
            {
                Pipelines = new[]
                {
                    new GoCDPipelineHistoryInfo
                    {
                        Name = "pipeline1",
                        Comment = "ship it",
                        NaturalOrder = "13",
                        BuildCause = new GoCDBuildCauseInfo
                        {
                            Approver = "changes",
                            TriggerForced = false,
                            TriggerMessage = "modified by user",
                            MaterialRevisions = new[]
                            {
                                new GoCDMaterialRevisionsInfo
                                {
                                    Changed = true,
                                    Modifications = new[]
                                    {
                                        new GoCDModificationInfo
                                        {
                                            EmailAddress = "dev@example.com",
                                            Id = 2,
                                            ModifiedTimeUnixMilliseconds = 1434957613000,
                                            Username = "developer",
                                            Comment = "Fix bug",
                                            Revision = "abc123"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // When
            var json = JsonSerializer.Serialize(history, GoCDJsonContext.Default.GoCDHistoryInfo);
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;
            var pipeline = root.GetProperty("pipelines")[0];
            var buildCause = pipeline.GetProperty("build_cause");
            var modification = buildCause.GetProperty("material_revisions")[0].GetProperty("modifications")[0];

            // Then
            Assert.False(root.TryGetProperty("Pipelines", out _));
            Assert.Equal(JsonValueKind.String, pipeline.GetProperty("natural_order").ValueKind);
            Assert.Equal("13", pipeline.GetProperty("natural_order").GetString());
            Assert.Equal("pipeline1", pipeline.GetProperty("name").GetString());
            Assert.Equal("ship it", pipeline.GetProperty("comment").GetString());
            Assert.False(pipeline.TryGetProperty("BuildCause", out _));
            Assert.Equal("changes", buildCause.GetProperty("approver").GetString());
            Assert.False(buildCause.GetProperty("trigger_forced").GetBoolean());
            Assert.Equal("modified by user", buildCause.GetProperty("trigger_message").GetString());
            Assert.True(buildCause.GetProperty("material_revisions")[0].GetProperty("changed").GetBoolean());
            Assert.Equal("dev@example.com", modification.GetProperty("email_address").GetString());
            Assert.Equal(2, modification.GetProperty("id").GetInt32());
            Assert.Equal(1434957613000, modification.GetProperty("modified_time").GetInt64());
            Assert.Equal("developer", modification.GetProperty("user_name").GetString());
            Assert.Equal("Fix bug", modification.GetProperty("comment").GetString());
            Assert.Equal("abc123", modification.GetProperty("revision").GetString());
            Assert.False(modification.TryGetProperty("ModifiedTime", out _));
            Assert.False(modification.TryGetProperty("modifiedTime", out _));
        }
    }
}
