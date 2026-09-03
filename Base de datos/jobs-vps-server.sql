USE [msdb]
GO

/****** Object:  Job [ActualizarCatalogoObligacionesFiscales]    Script Date: 30/06/2026 01:28:38 a. m. ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [Full-Text]    Script Date: 30/06/2026 01:28:38 a. m. ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'Full-Text' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'Full-Text'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'ActualizarCatalogoObligacionesFiscales', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Actualiza Bimestre y año de ingreso Fiscal', 
		@category_name=N'Full-Text', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Actualiza Control Obligaciones]    Script Date: 30/06/2026 01:28:40 a. m. ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Actualiza Control Obligaciones', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'BEGIN TRAN
UPDATE  f SET FECHAINGRESO = S.Fch_Alta  FROM dbo.ControlObligaciones F INNER JOIN 
(SELECT Numero, FCH_ALTA FROM dbo.Soc_DatFiscales WITH (NOLOCK) WHERE NOT FCH_ALTA  IS NULL) S ON F.NUMERO = S.NUMERO

UPDATE CO SET CO.BIMESTRE = FC.BIMESTRE
FROM ControlObligaciones CO INNER JOIN dbo.FISC_CAT_OBLIGACIONES fc 
ON CO.Id_obligacion = FC.ID_OBLIGACION WHERE CO.BIMESTRE IS NULL  

--delete FROM ControlObligaciones 
--WHERE (MONTH(FECHAINGRESO) > BIMESTRE AND YEAR(fechaIngreso) =  año) or   ( año < YEAR(fechaIngreso) ) 


COMMIT TRAN', 
		@database_name=N'SindQR', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Diario', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20210813, 
		@active_end_date=99991231, 
		@active_start_time=50000, 
		@active_end_time=235959, 
		@schedule_uid=N'fdaa86b2-bcf8-49d1-a8f8-e9aa4c669eac'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [AJR_CONVENIOSINVYBLOQUEOS]    Script Date: 30/06/2026 01:28:43 a. m. ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 30/06/2026 01:28:43 a. m. ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'AJR_CONVENIOSINVYBLOQUEOS', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [PASO_AJR_CONVENIOSINVYBLOQUEOS]    Script Date: 30/06/2026 01:28:45 a. m. ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'PASO_AJR_CONVENIOSINVYBLOQUEOS', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=4, 
		@on_success_step_id=2, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC AJR_CONVENIOSINVYBLOQUEOS', 
		@database_name=N'SindQR', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [AJR_BLOQUEO_POLIZAVENCE]    Script Date: 30/06/2026 01:28:46 a. m. ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'AJR_BLOQUEO_POLIZAVENCE', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC AJR_BLOQUEO_POLIZAVENCE', 
		@database_name=N'SindQR', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'PRG_AJR_CONVENIOSINVYBLOQUEOS', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20210813, 
		@active_end_date=99991231, 
		@active_start_time=1500, 
		@active_end_time=235959, 
		@schedule_uid=N'd0539d73-6bf1-459d-87d4-2617fc15d19c'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [AJR_MARCAROBLIGACIONESCUBIERTAS]    Script Date: 30/06/2026 01:28:47 a. m. ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 30/06/2026 01:28:48 a. m. ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'AJR_MARCAROBLIGACIONESCUBIERTAS', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Elimina del inventario las obligaciones que no se han cubierto en el dia y marca como pagadas las diferentes obligaciones pagadas por cada socio', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [BorrarObligacionesNoPagadasenelDia]    Script Date: 30/06/2026 01:28:50 a. m. ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'BorrarObligacionesNoPagadasenelDia', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'UPDATE Inventario
SET Exist = 0 where Id_familia in (''22'',''24'',''111'') and Tipo = ''PRO'' AND EXIST > 0', 
		@database_name=N'SindQR', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [AJR_MARCAROBLIGACIONESCUBIERTAS]    Script Date: 30/06/2026 01:28:50 a. m. ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'AJR_MARCAROBLIGACIONESCUBIERTAS', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'exec AJR_MARCAROBLIGACIONESCUBIERTAS', 
		@database_name=N'SindQR', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Madrugada', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20210813, 
		@active_end_date=99991231, 
		@active_start_time=2000, 
		@active_end_time=235959, 
		@schedule_uid=N'd7f7320c-afac-468c-bf2c-143f96682904'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [AJR_RECALCULO]    Script Date: 30/06/2026 01:28:52 a. m. ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 30/06/2026 01:28:52 a. m. ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'AJR_RECALCULO', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [PASO_AJR_RECALCULO]    Script Date: 30/06/2026 01:28:54 a. m. ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'PASO_AJR_RECALCULO', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC AJR_RECALCULO', 
		@database_name=N'SindQR', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [PASO_AJR_INGRESARADIOTAXI]    Script Date: 30/06/2026 01:28:55 a. m. ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'PASO_AJR_INGRESARADIOTAXI', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC AJR_INGRESARADIOTAXI', 
		@database_name=N'SindQR', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'PRG_AJR_RECALCULO', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20210813, 
		@active_end_date=99991231, 
		@active_start_time=1000, 
		@active_end_time=235959, 
		@schedule_uid=N'ea2c268d-b6f4-4ce0-bcf2-977399c6b567'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [auto-backup-compresion-pre]    Script Date: 30/06/2026 01:28:57 a. m. ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 30/06/2026 01:28:57 a. m. ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'auto-backup-compresion-pre', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [compresion-pre]    Script Date: 30/06/2026 01:28:59 a. m. ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'compresion-pre', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'/* =====================================================
   HABILITAR xp_cmdshell
===================================================== */
EXEC sp_configure ''show advanced options'', 1;
RECONFIGURE;
EXEC sp_configure ''xp_cmdshell'', 1;
RECONFIGURE;

/* =====================================================
   VARIABLES
===================================================== */
DECLARE 
    @BackupPath VARCHAR(500) = ''C:\BACKUPS\'',
    @NetworkPath VARCHAR(500) = ''\\100.76.228.11\vps'',
    @DBName SYSNAME,
    @DateSuffix VARCHAR(30) = 
        REPLACE(REPLACE(CONVERT(VARCHAR(19), GETDATE(), 120), ''-'', ''''), '':'', ''''),
    @FileName VARCHAR(500),
    @RarName VARCHAR(500),
    @SQL NVARCHAR(MAX),
    @Cmd VARCHAR(8000);


/* =====================================================
   BACKUP + COMPRESIÓN
===================================================== */
DECLARE db_cursor CURSOR LOCAL FAST_FORWARD FOR
SELECT name 
FROM sys.databases
WHERE name NOT IN (''master'',''model'',''msdb'',''tempdb'')
AND state_desc = ''ONLINE'';

OPEN db_cursor;
FETCH NEXT FROM db_cursor INTO @DBName;

WHILE @@FETCH_STATUS = 0
BEGIN
    BEGIN TRY

        SET @FileName = @BackupPath + @DBName + ''_'' + @DateSuffix + ''.bak'';
        SET @RarName  = @BackupPath + @DBName + ''_'' + @DateSuffix + ''.rar'';

        PRINT ''================================================='';
        PRINT ''Respaldando: '' + @DBName;

        /* BACKUP */
        SET @SQL = ''
        BACKUP DATABASE ['' + @DBName + ''] 
        TO DISK = N'''''' + @FileName + '''''' 
        WITH INIT, COMPRESSION, STATS = 10;'';
        EXEC sp_executesql @SQL;

        /* COMPRIMIR */
        PRINT ''Comprimiendo archivo...'';
        SET @Cmd = ''cmd /c ""C:\Program Files\WinRAR\rar.exe" a -m5 "'' 
                   + @RarName + ''" "'' + @FileName + ''""'';
        EXEC xp_cmdshell @Cmd;

        /* ELIMINAR BAK */
        PRINT ''Eliminando .bak...'';
        SET @Cmd = ''cmd /c del "'' + @FileName + ''"'';
        EXEC xp_cmdshell @Cmd;

        PRINT ''Base procesada correctamente: '' + @DBName;

    END TRY
    BEGIN CATCH
        PRINT ''ERROR EN BASE: '' + @DBName;
        PRINT ERROR_MESSAGE();
    END CATCH;

    FETCH NEXT FROM db_cursor INTO @DBName;
END;

CLOSE db_cursor;
DEALLOCATE db_cursor;


PRINT ''================================================='';
PRINT ''LIMPIEZA LOCAL DE ARCHIVOS ANTIGUOS FINALIZADO CORRECTAMENTE'';
PRINT ''================================================='';

PRINT ''Ejecutando radiotaxi.job.exe...'';
SET @Cmd = ''cmd /c "C:\job_exe\radiotaxi.job.exe"'';
EXEC xp_cmdshell @Cmd;
PRINT ''radiotaxi.job.exe ejecutado.'';

PRINT ''================================================='';
PRINT ''FINALIZADO'';
PRINT ''================================================='';', 
		@database_name=N'SindQR', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'pre', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20260618, 
		@active_end_date=99991231, 
		@active_start_time=223000, 
		@active_end_time=235959, 
		@schedule_uid=N'b50838c0-3929-44c6-be9b-6cfedb65c846'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [auto-backup-compression]    Script Date: 30/06/2026 01:29:01 a. m. ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 30/06/2026 01:29:01 a. m. ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'auto-backup-compression', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'FDOAC : Este procedimiento hace un backup de las bases de datos y las comprime de manera independiente y posteriormente las envia a un servidor FTP para su resguardo.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [save]    Script Date: 30/06/2026 01:29:03 a. m. ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'save', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'/* =====================================================
   HABILITAR xp_cmdshell
===================================================== */
EXEC sp_configure ''show advanced options'', 1;
RECONFIGURE;
EXEC sp_configure ''xp_cmdshell'', 1;
RECONFIGURE;

/* =====================================================
   VARIABLES
===================================================== */
DECLARE 
    @BackupPath VARCHAR(500) = ''C:\BACKUPS\'',
    @NetworkPath VARCHAR(500) = ''\\100.76.228.11\vps'',
    @DBName SYSNAME,
    @DateSuffix VARCHAR(30) = 
        REPLACE(REPLACE(CONVERT(VARCHAR(19), GETDATE(), 120), ''-'', ''''), '':'', ''''),
    @FileName VARCHAR(500),
    @RarName VARCHAR(500),
    @SQL NVARCHAR(MAX),
    @Cmd VARCHAR(8000);


/* =====================================================
   BACKUP + COMPRESIÓN
===================================================== */
DECLARE db_cursor CURSOR LOCAL FAST_FORWARD FOR
SELECT name 
FROM sys.databases
WHERE name NOT IN (''master'',''model'',''msdb'',''tempdb'')
AND state_desc = ''ONLINE'';

OPEN db_cursor;
FETCH NEXT FROM db_cursor INTO @DBName;

WHILE @@FETCH_STATUS = 0
BEGIN
    BEGIN TRY

        SET @FileName = @BackupPath + @DBName + ''_'' + @DateSuffix + ''.bak'';
        SET @RarName  = @BackupPath + @DBName + ''_'' + @DateSuffix + ''.rar'';

        PRINT ''================================================='';
        PRINT ''Respaldando: '' + @DBName;

        /* BACKUP */
        SET @SQL = ''
        BACKUP DATABASE ['' + @DBName + ''] 
        TO DISK = N'''''' + @FileName + '''''' 
        WITH INIT, COMPRESSION, STATS = 10;'';
        EXEC sp_executesql @SQL;

        /* COMPRIMIR */
        PRINT ''Comprimiendo archivo...'';
        SET @Cmd = ''cmd /c ""C:\Program Files\WinRAR\rar.exe" a -m5 "'' 
                   + @RarName + ''" "'' + @FileName + ''""'';
        EXEC xp_cmdshell @Cmd;

        /* ELIMINAR BAK */
        PRINT ''Eliminando .bak...'';
        SET @Cmd = ''cmd /c del "'' + @FileName + ''"'';
        EXEC xp_cmdshell @Cmd;

        PRINT ''Base procesada correctamente: '' + @DBName;

    END TRY
    BEGIN CATCH
        PRINT ''ERROR EN BASE: '' + @DBName;
        PRINT ERROR_MESSAGE();
    END CATCH;

    FETCH NEXT FROM db_cursor INTO @DBName;
END;

CLOSE db_cursor;
DEALLOCATE db_cursor;


/* =====================================================
   CONEXIÓN A RECURSO DE RED
===================================================== */

PRINT ''================================================='';
PRINT ''Conectando a recurso de red...'';

-- Eliminar conexión previa
--SET @Cmd = ''cmd /c "net use '' + @NetworkPath + '' /delete /y"'';
-- EXEC xp_cmdshell @Cmd;

-- Conectar con credenciales
SET @Cmd = ''cmd /c "net use '' + @NetworkPath + '' tc /user:tc"'';
EXEC xp_cmdshell @Cmd;

/* =====================================================
   MOVER ARCHIVOS .RAR
===================================================== */

PRINT ''Moviendo archivos .rar a red...'';

SET @Cmd = ''cmd /c "move /Y C:\BACKUPS\*.rar '' + @NetworkPath + ''"'';
EXEC xp_cmdshell @Cmd;


/* =====================================================
   DESCONECTAR RECURSO
===================================================== */

PRINT ''Desconectando recurso...'';

SET @Cmd = ''cmd /c "net use '' + @NetworkPath + '' /delete /y"'';
EXEC xp_cmdshell @Cmd;


/* =====================================================
   LIMPIEZA LOCAL DE ARCHIVOS ANTIGUOS
===================================================== */

PRINT ''Eliminando archivos .rar locales mayores a 2 días...'';

SET @Cmd = ''cmd /c forfiles /p "C:\BACKUPS" /m *.rar /d -2 /c "cmd /c del @path" 2>nul'';
EXEC xp_cmdshell @Cmd;

PRINT ''================================================='';
PRINT ''LIMPIEZA LOCAL DE ARCHIVOS ANTIGUOS FINALIZADO CORRECTAMENTE'';
PRINT ''================================================='';


PRINT ''Ejecutando radiotaxi.job.exe...'';
SET @Cmd = ''cmd /c "C:\job_exe\radiotaxi.job.exe"'';
EXEC xp_cmdshell @Cmd;
PRINT ''radiotaxi.job.exe ejecutado.'';

PRINT ''================================================='';
PRINT ''FINALIZADO'';
PRINT ''================================================='';


', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Diario', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20250603, 
		@active_end_date=99991231, 
		@active_start_time=30000, 
		@active_end_time=235959, 
		@schedule_uid=N'1758aec6-d638-4e61-8cd5-106f4799cdc5'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [fdo_cuotas_auto]    Script Date: 30/06/2026 01:29:05 a. m. ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 30/06/2026 01:29:05 a. m. ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'fdo_cuotas_auto', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [fdo_cuotas_auto]    Script Date: 30/06/2026 01:29:07 a. m. ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'fdo_cuotas_auto', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC dbo.fdo_cuotas_auto @Preview = 0;', 
		@database_name=N'SindQR', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'moth', 
		@enabled=1, 
		@freq_type=16, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=1, 
		@active_start_date=20260604, 
		@active_end_date=99991231, 
		@active_start_time=0, 
		@active_end_time=235959, 
		@schedule_uid=N'be4359ed-0a0d-4f63-891a-dad962890614'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [syspolicy_purge_history]    Script Date: 30/06/2026 01:29:09 a. m. ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 30/06/2026 01:29:09 a. m. ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'syspolicy_purge_history', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Verify that automation is enabled.]    Script Date: 30/06/2026 01:29:11 a. m. ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Verify that automation is enabled.', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=1, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'IF (msdb.dbo.fn_syspolicy_is_automation_enabled() != 1)
        BEGIN
            RAISERROR(34022, 16, 1)
        END', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Purge history.]    Script Date: 30/06/2026 01:29:12 a. m. ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Purge history.', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC msdb.dbo.sp_syspolicy_purge_history', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Erase Phantom System Health Records.]    Script Date: 30/06/2026 01:29:12 a. m. ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Erase Phantom System Health Records.', 
		@step_id=3, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'PowerShell', 
		@command=N'if (''$(ESCAPE_SQUOTE(INST))'' -eq ''MSSQLSERVER'') {$a = ''\DEFAULT''} ELSE {$a = ''''};
(Get-Item SQLSERVER:\SQLPolicy\$(ESCAPE_NONE(SRVR))$a).EraseSystemHealthPhantomRecords()', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'syspolicy_purge_history_schedule', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20080101, 
		@active_end_date=99991231, 
		@active_start_time=20000, 
		@active_end_time=235959, 
		@schedule_uid=N'79feb90b-cff6-4f4e-b27f-2991a6c6d828'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

