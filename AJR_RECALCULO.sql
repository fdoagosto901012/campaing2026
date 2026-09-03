USE [SindQR]
GO
/****** Object:  StoredProcedure [dbo].[AJR_RECALCULO]    Script Date: 27/02/2025 01:04:32 a. m. ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[AJR_RECALCULO]

AS

  DECLARE @ERROR INT,@FINICIALES MONEY,@FACARGAR MONEY,@FFINAL MONEY,@STATUS VARCHAR(10)
  
  BEGIN TRANSACTION 

	-- Obtenemos las faltas globales iniciales.
	set  @FINICIALES=ISNULL((select sum(exist) from inventario where id_familia='0'),0)
	-- Obtenemos las faltas globales que se cargaran.
	set @FACARGAR=ISNULL((select COUNT(ID_PRODUCTO) from inventario where id_proveedor not in (select gafete from histasistencias where fechareporte>=CONVERT(char(11), getdate()-1,0)) and id_familia='0'),0)
 	

	-- Esta linea actualiza el inventario en un cuando la fecha de reporte es menor a la fecha actual agrega la falta.
	-- Obtiene el conjunto de operadores cuya asistencia caduco y les aumenta el inventario +1 
	update inventario set exist=exist+1 where id_proveedor not in (select gafete from histasistencias where fechareporte>=CONVERT(char(11), getdate()-1,0)) and id_familia='0'

	-- Se obtienen la cantidad de faltas finales para verificar lo actualizado
	set  @FFINAL=ISNULL((select sum(exist) from inventario where id_familia='0'),0)
	

	-- Se verifica que concuerde la carga de las faltas iniciales mas las cargadas, como resultado deberia de dar las faltas globales totales.
	IF (@FINICIALES+@FACARGAR)=@FFINAL
		BEGIN
			SET @STATUS='OK'
		END
	ELSE
		BEGIN
			SET @STATUS='ERROR'
		END
	

	-- Insertamos en historico asistencias bajo la siguiente condicion: 
	INSERT INTO histasistencias(GAFETE,TIPO,TAXI,TURNO,FECHAREPORTE,CONCEPTO,TICKET,FECHAOP)

	-- Se agregan a inventario LOS AT .. controlado por el campo maya caribe.
	SELECT CHOFER,'OP' as TIPO, isnull(convert(varchar(8),ulttaxi),0) AS ULTTAXI, ISNULL(ULTTURNO,'V') AS TURNO,
	CONVERT(char(11), getdate(),0), '3', 'AT', CONVERT(char(11), getdate(),0)
	FROM CHOF_DETALLE
	WHERE MAYACARIBE<>'' -- se sacan los de mayacaribe
	AND CHOFER NOT IN (
	-- Verifica todos los operadores que ya vencieron sus reportes.
	select GAFETE from histasistencias where fechareporte>CONVERT(char(11), getdate()-1,0) AND GAFETE IN (SELECT CHOFER FROM CHOF_DETALLE WHERE MAYACARIBE<>'')) ORDER BY CHOFER

/*
	SELECT ID_PROVEEDOR,ECONOMICO as TIPO,convert(varchar(8),ulttaxi) AS ULTTAXI,ISNULL(ULTTURNO,'V') AS TURNO,
	CONVERT(char(11), getdate(),0),'3','AT',CONVERT(char(11), getdate(),0)
	FROM FALTASYASIS
	WHERE MAYACARIBE<>'' AND ID_PROVEEDOR NOT IN (select GAFETE from histasistencias where fechareporte>CONVERT(char(11), getdate()-1,0) AND GAFETE IN (SELECT ID_PROVEEDOR FROM FALTASYASIS WHERE MAYACARIBE<>'')) ORDER BY ULTASIS,ID_PROVEEDOR
*/

	 --- LO MISMO PERO AHORA CON LOS SOCIOS.
	INSERT INTO histasistencias(GAFETE,TIPO,TAXI,TURNO,FECHAREPORTE,CONCEPTO,TICKET,FECHAOP)

	SELECT NUMERO,'E' as TIPO,isnull(convert(varchar(8),ulttaxi),0) AS ULTTAXI,ISNULL(ULTTURNO,'V') AS TURNO,
	CONVERT(char(11), getdate(),0),'3','AT',CONVERT(char(11), getdate(),0)
	FROM SOC_DATPERSONALES
	WHERE MAYACARIBE<>''
	AND NUMERO NOT IN (
	select GAFETE from histasistencias where fechareporte>CONVERT(char(11), getdate()-1,0) AND SUBSTRING(GAFETE,1,2)<>'OP' AND GAFETE IN (SELECT NUMERO FROM SOC_DATPERSONALES WHERE MAYACARIBE<>'')) ORDER BY NUMERO


/*
	SELECT ID_PROVEEDOR,ECONOMICO as TIPO,convert(varchar(8),ulttaxi) AS ULTTAXI,ISNULL(ULTTURNO,'V') AS TURNO,
	CONVERT(char(11), getdate(),0),'3','AT',CONVERT(char(11), getdate(),0)
	FROM FALTASYASIS
	WHERE MAYACARIBE<>'' AND ID_PROVEEDOR NOT IN (select GAFETE from histasistencias where fechareporte>CONVERT(char(11), getdate()-1,0) AND GAFETE IN (SELECT ID_PROVEEDOR FROM FALTASYASIS WHERE MAYACARIBE<>'')) ORDER BY ULTASIS,ID_PROVEEDOR
*/


	-- Actualiza asistencia a turno matutino cuando la fecha de reporte es menor a la fecha actual cuando es FALTA y AT por que no se aun
	UPDATE histasistencias SET TURNO='M' WHERE fechareporte>=CONVERT(char(11), getdate()-1,0) AND TURNO='F' AND TICKET='AT'

	-- Inserta en la tabla recalculo los datos del recaulculo.
	INSERT INTO RECALCULO(FALTASINICIALES, FALTASAINGRESAR, FALTASFINAL, FECHACALCULO, STATUS)
	VALUES(@FINICIALES,@FACARGAR,@FFINAL,getdate()-1,@STATUS)

	--- Actualizamo en ventas la fecha del recalculo.
	UPDATE VENTA01.DBO.CONTROL SET FECHASYS=CONVERT(char(11), getdate(),0)
	

  SET @ERROR = @@ERROR
  
  IF @ERROR <> 0 
  BEGIN
    ROLLBACK TRANSACTION
    IF @ERROR=1205 AND @@NESTLEVEL < 3 BEGIN
      EXECUTE AJR_RECALCULO
      RETURN
    END ELSE BEGIN
      RAISERROR('ERROR AL GENERAR EL RECALCULO. (AJR_RECALCULO)', 16 ,1)    
      RETURN
    END
  END 

  COMMIT TRANSACTION