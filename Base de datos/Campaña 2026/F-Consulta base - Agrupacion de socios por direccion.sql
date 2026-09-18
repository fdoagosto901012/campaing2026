;WITH Base AS
(
    SELECT
        U.id AS UserId,
        LTRIM(RTRIM(U.partnerReference)) AS NumeroSocio,

        U.firstName,
        U.lastNameF,
        U.lastNameM,

        U.birthDate,

        CASE
            WHEN U.sex = 1 THEN 'Hombre'
            WHEN U.sex = 2 THEN 'Mujer'
            ELSE 'No especificado'
        END AS Sexo,

        CASE
            WHEN U.birthDate IS NULL THEN NULL
            ELSE
                DATEDIFF(YEAR, U.birthDate, GETDATE())
                -
                CASE
                    WHEN DATEADD(
                        YEAR,
                        DATEDIFF(YEAR, U.birthDate, GETDATE()),
                        U.birthDate
                    ) > GETDATE()
                    THEN 1
                    ELSE 0
                END
        END AS Edad,

        U.curp,
        U.rfc,
        U.ine,
        U.active AS UserActivo,

        SDP.Telefono AS TelefonoOriginal,
        SDP.email,
        SDP.Ocupacion,
        SDP.Escolaridad,
        SDP.fechaingreso,

        /* ================================
           DIRECCION
           ================================ */

        UA.supermanzana,
        UA.manzana,
        UA.lote,
        UA.street,
        UA.colony,
        UA.postalCode,
        UA.cityId,
        UA.stateId,
        UA.zoneId,
        UA.latitude,
        UA.longitude

    FROM dbo.[user] U

    LEFT JOIN dbo.Soc_DatPersonales SDP
        ON CONVERT(VARCHAR(50), SDP.Numero) =
           LTRIM(RTRIM(U.partnerReference))

    LEFT JOIN dbo.userAddress UA
        ON U.id = UA.userId

    WHERE
        U.active = 1

        AND U.partnerReference IS NOT NULL

        AND LTRIM(RTRIM(U.partnerReference)) <> ''

        /* Solamente NumeroSocio numérico */
        AND LTRIM(RTRIM(U.partnerReference)) NOT LIKE '%[^0-9]%'

        /* Supermanzanas */
        AND UA.supermanzana IN ('024', '027')
),

/* ============================================================
   SOCIO DUPLICADO

   Se considera duplicado cuando coinciden:

       firstName
       lastNameF
       lastNameM

   No se elimina ningún registro.
   ============================================================ */

Duplicados AS
(
    SELECT
        B.*,

        CASE
            WHEN COUNT(*) OVER
            (
                PARTITION BY
                    B.firstName,
                    B.lastNameF,
                    B.lastNameM
            ) > 1
            THEN 'TRUE'
            ELSE 'FALSE'
        END AS SocioDuplicado

    FROM Base B
),

/* ============================================================
   CLAVE DE DIRECCION

   Ejemplo:

       Supermanzana = 027
       Manzana      = 15
       Lote          = 08

       027-15-08
   ============================================================ */

Direcciones AS
(
    SELECT
        D.*,

        LTRIM(RTRIM(ISNULL(D.supermanzana, '')))
        + '-'
        + LTRIM(RTRIM(ISNULL(D.manzana, '')))
        + '-'
        + LTRIM(RTRIM(ISNULL(D.lote, '')))
        AS DireccionClave

    FROM Duplicados D
),

/* ============================================================
   CANTIDAD DE REGISTROS POR DIRECCION
   ============================================================ */

DireccionesAnalizadas AS
(
    SELECT
        D.*,

        COUNT(*) OVER
        (
            PARTITION BY
                D.DireccionClave
        ) AS CantidadRegistrosDireccion

    FROM Direcciones D
),

/* ============================================================
   SOCIOS QUE COMPARTEN DIRECCION

   Incluye al propio socio.

   Orden descendente:

       945, 103, 10

   ============================================================ */

DireccionesConSocios AS
(
    SELECT
        D.*,

        STUFF
        (
            (
                SELECT
                    ', ' + D2.NumeroSocio

                FROM
                (
                    SELECT DISTINCT
                        D3.DireccionClave,
                        D3.NumeroSocio
                    FROM DireccionesAnalizadas D3
                    WHERE
                        D3.NumeroSocio IS NOT NULL
                ) D2

                WHERE
                    D2.DireccionClave = D.DireccionClave

                ORDER BY
                    LEN(D2.NumeroSocio) DESC,
                    D2.NumeroSocio DESC

                FOR XML PATH(''), TYPE
            ).value('.', 'VARCHAR(MAX)'),

            1,
            2,
            ''
        ) AS SociosQueCompartenDireccion

    FROM DireccionesAnalizadas D
),

/* ============================================================
   PREPARAR TELEFONOS
   ============================================================ */

Preparado AS
(
    SELECT
        D.*,

        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
            UPPER(ISNULL(D.TelefonoOriginal, '')),
            'TEL:', '|'),
            'TEL-', '|'),
            'TEL', '|'),
            'CEL:', '|'),
            'CEL-', '|'),
            'CEL', '|'),
            '/', '|'),
            ',', '|'),
            ';', '|'),
            ' Y ', '|')
        AS TextoSeparado

    FROM DireccionesConSocios D
),

/* ============================================================
   SEPARAR TELEFONOS
   ============================================================ */

Partes AS
(
    SELECT
        P.*,

        LTRIM(
            RTRIM(
                X.N.value('.', 'varchar(200)')
            )
        ) AS Parte

    FROM Preparado P

    CROSS APPLY
    (
        SELECT CAST(
            '<r><x>' +
            REPLACE(
                P.TextoSeparado,
                '|',
                '</x><x>'
            )
            + '</x></r>' AS XML
        ) AS XMLData
    ) XMLD

    CROSS APPLY
        XMLD.XMLData.nodes('/r/x') AS X(N)

    WHERE
        LTRIM(
            RTRIM(
                X.N.value('.', 'varchar(200)')
            )
        ) <> ''
),

/* ============================================================
   LIMPIAR TELEFONOS
   ============================================================ */

Limpios AS
(
    SELECT
        P.*,

        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
            P.Parte,
            ' ', ''),
            '-', ''),
            '(', ''),
            ')', ''),
            '.', ''),
            ':', '')
        AS NumeroLimpio

    FROM Partes P
),

/* ============================================================
   VALIDAR Y FORMATEAR TELEFONOS
   ============================================================ */

Validos AS
(
    SELECT
        L.*,

        CASE

            /* 7 DIGITOS */

            WHEN L.NumeroLimpio NOT LIKE '%[^0-9]%'
                 AND LEN(L.NumeroLimpio) = 7

            THEN
                LEFT(L.NumeroLimpio,3)
                + ' '
                + RIGHT(L.NumeroLimpio,4)


            /* 10 DIGITOS */

            WHEN L.NumeroLimpio NOT LIKE '%[^0-9]%'
                 AND LEN(L.NumeroLimpio) = 10

            THEN
                '('
                + LEFT(L.NumeroLimpio,3)
                + ') '
                + SUBSTRING(L.NumeroLimpio,4,3)
                + ' '
                + RIGHT(L.NumeroLimpio,4)

            ELSE NULL

        END AS TelefonoFormateado,

        CASE

            WHEN L.NumeroLimpio NOT LIKE '%[^0-9]%'
                 AND LEN(L.NumeroLimpio) = 7

            THEN '7 DIGITOS'


            WHEN L.NumeroLimpio NOT LIKE '%[^0-9]%'
                 AND LEN(L.NumeroLimpio) = 10

            THEN '10 DIGITOS'


            WHEN L.NumeroLimpio IN
            (
                '0',
                '000',
                '0000000',
                '00000000',
                '0000000000'
            )

            THEN 'SIN TELEFONO'


            ELSE 'REVISAR'

        END AS EstadoTelefono

    FROM Limpios L
),

/* ============================================================
   ELIMINAR TELEFONOS REPETIDOS

   Ejemplo:

       884-0370
       8840370
       884 0370

   Se consideran el mismo teléfono.
   ============================================================ */

TelefonosUnicos AS
(
    SELECT
        V.*,

        ROW_NUMBER() OVER
        (
            PARTITION BY
                V.UserId,
                V.TelefonoFormateado

            ORDER BY
                V.Parte

        ) AS Repeticion

    FROM Validos V

    WHERE
        V.TelefonoFormateado IS NOT NULL
),

/* ============================================================
   ORDENAR TELEFONOS
   ============================================================ */

Ordenados AS
(
    SELECT
        T.*,

        ROW_NUMBER() OVER
        (
            PARTITION BY
                T.UserId

            ORDER BY
                T.Parte

        ) AS NumeroOrden

    FROM TelefonosUnicos T

    WHERE
        T.Repeticion = 1
)

/* ============================================================
   RESULTADO FINAL
   ============================================================ */

SELECT

    /* ========================================================
       IDENTIFICACION
       ======================================================== */

    B.UserId,
    B.NumeroSocio,

    B.firstName,
    B.lastNameF,
    B.lastNameM,

    B.SocioDuplicado,


    /* ========================================================
       DATOS PERSONALES
       ======================================================== */

    B.birthDate,
    B.Sexo,
    B.Edad,

    B.curp,
    B.rfc,
    B.ine,

    B.UserActivo,


    /* ========================================================
       TELEFONO ORIGINAL
       ======================================================== */

    B.TelefonoOriginal,


    /* ========================================================
       TELEFONO 1
       ======================================================== */

    MAX(
        CASE
            WHEN O.NumeroOrden = 1
            THEN O.TelefonoFormateado
        END
    ) AS Telefono1,


    /* ========================================================
       TELEFONO 2
       ======================================================== */

    MAX(
        CASE
            WHEN O.NumeroOrden = 2
            THEN O.TelefonoFormateado
        END
    ) AS Telefono2,


    /* ========================================================
       TELEFONO 3
       ======================================================== */

    MAX(
        CASE
            WHEN O.NumeroOrden = 3
            THEN O.TelefonoFormateado
        END
    ) AS Telefono3,


    /* ========================================================
       TELEFONO 4
       ======================================================== */

    MAX(
        CASE
            WHEN O.NumeroOrden = 4
            THEN O.TelefonoFormateado
        END
    ) AS Telefono4,


    /* ========================================================
       TODOS LOS TELEFONOS
       ======================================================== */

    CASE

        WHEN MAX(CASE
            WHEN O.NumeroOrden = 1
            THEN O.TelefonoFormateado
        END) IS NOT NULL

        AND MAX(CASE
            WHEN O.NumeroOrden = 2
            THEN O.TelefonoFormateado
        END) IS NOT NULL

        AND MAX(CASE
            WHEN O.NumeroOrden = 3
            THEN O.TelefonoFormateado
        END) IS NOT NULL

        AND MAX(CASE
            WHEN O.NumeroOrden = 4
            THEN O.TelefonoFormateado
        END) IS NOT NULL

        THEN
            MAX(CASE
                WHEN O.NumeroOrden = 1
                THEN O.TelefonoFormateado
            END)
            + ' / ' +
            MAX(CASE
                WHEN O.NumeroOrden = 2
                THEN O.TelefonoFormateado
            END)
            + ' / ' +
            MAX(CASE
                WHEN O.NumeroOrden = 3
                THEN O.TelefonoFormateado
            END)
            + ' / ' +
            MAX(CASE
                WHEN O.NumeroOrden = 4
                THEN O.TelefonoFormateado
            END)


        WHEN MAX(CASE
            WHEN O.NumeroOrden = 1
            THEN O.TelefonoFormateado
        END) IS NOT NULL

        AND MAX(CASE
            WHEN O.NumeroOrden = 2
            THEN O.TelefonoFormateado
        END) IS NOT NULL

        AND MAX(CASE
            WHEN O.NumeroOrden = 3
            THEN O.TelefonoFormateado
        END) IS NOT NULL

        THEN
            MAX(CASE
                WHEN O.NumeroOrden = 1
                THEN O.TelefonoFormateado
            END)
            + ' / ' +
            MAX(CASE
                WHEN O.NumeroOrden = 2
                THEN O.TelefonoFormateado
            END)
            + ' / ' +
            MAX(CASE
                WHEN O.NumeroOrden = 3
                THEN O.TelefonoFormateado
            END)


        WHEN MAX(CASE
            WHEN O.NumeroOrden = 1
            THEN O.TelefonoFormateado
        END) IS NOT NULL

        AND MAX(CASE
            WHEN O.NumeroOrden = 2
            THEN O.TelefonoFormateado
        END) IS NOT NULL

        THEN
            MAX(CASE
                WHEN O.NumeroOrden = 1
                THEN O.TelefonoFormateado
            END)
            + ' / ' +
            MAX(CASE
                WHEN O.NumeroOrden = 2
                THEN O.TelefonoFormateado
            END)


        WHEN MAX(CASE
            WHEN O.NumeroOrden = 1
            THEN O.TelefonoFormateado
        END) IS NOT NULL

        THEN
            MAX(CASE
                WHEN O.NumeroOrden = 1
                THEN O.TelefonoFormateado
            END)

        ELSE NULL

    END AS TelefonosContacto,


    /* ========================================================
       TIENE TELEFONO
       ======================================================== */

    CASE
        WHEN EXISTS
        (
            SELECT 1
            FROM Ordenados O2
            WHERE O2.UserId = B.UserId
        )
        THEN 'Sí'
        ELSE 'No'
    END AS TieneTelefono,


    /* ========================================================
       DATOS PERSONALES ADICIONALES
       ======================================================== */

    B.email,
    B.Ocupacion,
    B.Escolaridad,
    B.fechaingreso,


    /* ========================================================
       DIRECCION
       ======================================================== */

    B.supermanzana,
    B.manzana,
    B.lote,

    B.street,
    B.colony,
    B.postalCode,

    B.cityId,
    B.stateId,
    B.zoneId,

    B.latitude,
    B.longitude,


    /* ========================================================
       ANALISIS DE DIRECCION
       ======================================================== */

    B.DireccionClave,

    B.CantidadRegistrosDireccion,

    CASE
        WHEN B.CantidadRegistrosDireccion > 1
        THEN 'TRUE'
        ELSE 'FALSE'
    END AS DireccionCompartida,


    /* ========================================================
       SOCIOS QUE COMPARTEN DIRECCION

       INCLUYE AL PROPIO SOCIO.

       EJEMPLO:

           945, 103, 10
       ======================================================== */

    B.SociosQueCompartenDireccion


FROM DireccionesConSocios B

LEFT JOIN Ordenados O
    ON B.UserId = O.UserId


GROUP BY

    B.UserId,
    B.NumeroSocio,

    B.firstName,
    B.lastNameF,
    B.lastNameM,

    B.SocioDuplicado,

    B.birthDate,
    B.Sexo,
    B.Edad,

    B.curp,
    B.rfc,
    B.ine,

    B.UserActivo,

    B.TelefonoOriginal,
	  
    B.email,
    B.Ocupacion,
    B.Escolaridad,
    B.fechaingreso,

    B.supermanzana,
    B.manzana,
    B.lote,

    B.street,
    B.colony,
    B.postalCode,

    B.cityId,
    B.stateId,
    B.zoneId,

    B.latitude,
    B.longitude,

    B.DireccionClave,

    B.CantidadRegistrosDireccion,

    B.SociosQueCompartenDireccion


ORDER BY

    B.supermanzana,
    B.manzana,
    B.lote,

    B.lastNameF,
    B.lastNameM,
    B.firstName;