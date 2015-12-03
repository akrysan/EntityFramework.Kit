DELETE FROM [dbo].Tasks
GO


SET IDENTITY_INSERT [dbo].Tasks ON 
INSERT [dbo].Tasks (Id, Name,userId) VALUES (1, N'Task 1-1',1)
INSERT [dbo].Tasks (Id, Name,userId) VALUES (2, N'Task 1-2',1)

INSERT [dbo].Tasks (Id, Name,userId) VALUES (3, N'Task 2-1',2)
INSERT [dbo].Tasks (Id, Name,userId) VALUES (4, N'Task 2-2',2)

INSERT [dbo].Tasks (Id, Name,userId) VALUES (5, N'Task 3-1',3)
INSERT [dbo].Tasks (Id, Name,userId) VALUES (6, N'Task 3-2',3)
SET IDENTITY_INSERT [dbo].Tasks OFF
