USE [MAIR_DS40]
GO
INSERT [EventManager].[ActionsType] ([Id], [Name], [Description], [DateTime]) VALUES (1, N'Email', N'Invio di una Mail', CAST(N'2020-04-01T17:16:15.043' AS DateTime))
GO
INSERT [EventManager].[ActionsType] ([Id], [Name], [Description], [DateTime]) VALUES (2, N'DataBase', N'Log sul DataBase', CAST(N'2020-04-01T17:16:43.063' AS DateTime))
GO
INSERT [EventManager].[Actions] ([Id], [ActionTypeId], [ActionId], [Name], [Description], [DateTime]) VALUES (1, 2, 1, NULL, NULL, CAST(N'2020-04-01T17:17:44.343' AS DateTime))
GO
INSERT [EventManager].[Actions] ([Id], [ActionTypeId], [ActionId], [Name], [Description], [DateTime]) VALUES (2, 1, 1, NULL, NULL, CAST(N'2020-04-01T17:19:06.720' AS DateTime))
GO
INSERT [EventManager].[DbActions] ([Id], [Name], [Description], [DateTime]) VALUES (1, NULL, N'DbAction Description 1', CAST(N'2020-04-01T17:18:12.673' AS DateTime))
GO
INSERT [EventManager].[EmailActions] ([Id], [Name], [Description], [Subject], [MailToAddresses], [Message], [DateTime]) VALUES (1, NULL, N'EmailAction Description 1', N'EmailAction Subject 1', N'EmailAction MailToAddress 1', N'EmailAction Message 1', CAST(N'2020-04-01T17:20:31.147' AS DateTime))
GO
INSERT [EventManager].[EmailActions] ([Id], [Name], [Description], [Subject], [MailToAddresses], [Message], [DateTime]) VALUES (2, NULL, N'EmailAction Description 2', N'EmailAction Subject 2', N'EmailAction MailToAddress 2', N'EmailAction Message 2', CAST(N'2020-04-01T17:21:24.240' AS DateTime))
GO
INSERT [EventManager].[Conditions] ([Id], [Name], [Description], [JsonCondition], [DateTime]) VALUES (1, N'Condition Name 1', N'Condition Description 1', N'{"name":"Condizione di prova","tagid":"23","operator":">","value":"100"}', CAST(N'2020-04-01T17:15:47.133' AS DateTime))
GO
INSERT [EventManager].[Conditions] ([Id], [Name], [Description], [JsonCondition], [DateTime]) VALUES (2, N'Condition Name 2', N'Condition Description 2', N'{"name":"Condizione di prova 2","tagid":"50","operator":">","value":"200"}', CAST(N'2020-04-01T17:23:16.787' AS DateTime))
GO
INSERT [EventManager].[Events] ([Id], [Name], [Description], [ActionId], [ConditionId], [DateTime]) VALUES (1, N'Events Name 1', NULL, 1, 1, CAST(N'2020-04-01T17:22:21.893' AS DateTime))
GO
INSERT [EventManager].[Events] ([Id], [Name], [Description], [ActionId], [ConditionId], [DateTime]) VALUES (2, N'Events Name 2', NULL, 2, 2, CAST(N'2020-04-01T17:23:49.953' AS DateTime))
GO
SET IDENTITY_INSERT [EventManager].[DbActionsDetails] ON 
GO
INSERT [EventManager].[DbActionsDetails] ([Id], [DbActionsId], [Reference], [DateTime], [Name], [Description], [Type]) VALUES (2, 1, N'ns=3;s="T402_PCPLC"."ARCHIVE"."bPrinterEnable"', CAST(N'2020-04-14T14:45:38.023' AS DateTime), N'DbActionDetails Name 1', N'DbActionDetails Description 1', N'1')
GO
INSERT [EventManager].[DbActionsDetails] ([Id], [DbActionsId], [Reference], [DateTime], [Name], [Description], [Type]) VALUES (3, 1, N'ns=3;s="T402_PCPLC"."ARCHIVE"."i2"', CAST(N'2020-04-14T14:46:13.393' AS DateTime), N'DbActionDetails Name 2', N'DbActionDetails Description 2', N'1')
GO
SET IDENTITY_INSERT [EventManager].[DbActionsDetails] OFF
GO
