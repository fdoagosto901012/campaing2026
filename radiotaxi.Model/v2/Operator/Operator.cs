using Newtonsoft.Json;
using radiotaxi.Model.v2.finance;
using radiotaxi.Model.v2.General;
using radiotaxi.Model.v2.Operator;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class Operator : Chof_Detalle
    {
        public string urlImage { get; set; }
        public string bucket { get; set; }
        public string key { get; set; }
        public string curp { get; set; }
        public string ine { get; set; }
        public bool? organDonor { get; set; }
        public string bloodType { get; set; }
        public string bloodTypeId { get; set; }
        public string typeoperator { get; set; }
        public string relationship { get; set; }
        public string relationShipStatus { get; set; }
        public string allergies { get; set; }
        public string realtionship { get; set; }

        public string recommendedBy { get; set; }

        public string descripalergies { get; set; }
        public ICollection<userAddress> userAddresses { get; set; }
        public ICollection<email> emails { get; set; }
        public ICollection<userPhone> userPhones { get; set; }

        public ICollection<userHealthInformation> userHealthInformation { get; set; }
        public ICollection<amazonPicture> amazonPictures { get; set; }
        public int userId { get; set; }
        public string partnerReference { get; set; }
        public string firstName { get; set; }
        public string lastNameF { get; set; }
        public string lastNameM { get; set; }
        public DateTime? createdDt { get; set; }
        public int partnerTypeId { get; set; }
        public string birthPlace { get; set; }
        public int sex { get; set; }

        public DateTime? birthDate { get; set; }
        public int userTypeId { get; set; }
        public DateTime? dtLastUpdate { get; set; }
        public int relationShipStatusId { get; set; }
        public int? sectionId { get; set; }
        public bool? cafecude { get; set; }
        public bool? at { get; set; }

        public bool active { get; set; }
        public string reason { get; set; }
        public string createdBy { get; set; }
        public string editBy { get; set; }
        public int? meeting { get; set; }
        public bool death { get; set; }
        public bool enemy { get; set; }
        public bool? payroll { get; set; }

        public bool? ttesoc { get; set; }
        public bool? noVote { get; set; }
        public int? candidateId { get; set; }
        public int? candidateHardvoteId { get; set; }
        public int? candidateQuizID { get; set; }
        public int? gps_id { get; set; }
        public string googleplus { get; set; }
        public string facebook { get; set; }
        public string instagram { get; set; }

        public string twitter { get; set; }
        public string comments { get; set; }
        public string urfc { get; set; }

        public string typeentrance { get; set; }
        public string openpayClientID { get; set; }

        public Operator() {

        }

        public OperatorPagination get(int page = 1, int pagesize = 100, string gafet = null, string Name = null, string lastName1 = null, string lastName2 = null)
        {
            gafet = gafet == "" || gafet is null ? "null" : "'" + gafet + "'";
            Name = Name == "" || Name is null ? "null" : "'" + Name + "'";
            lastName1 = lastName1 == "" || lastName1 is null ? "null" : "'" + lastName1 + "'";
            lastName2 = lastName2 == "" || lastName2 is null ? "null" : "'" + lastName2 + "'";
            string query = "fdo_get_operators " + page + ", " + pagesize + ", " + gafet + ", " + Name + ", " + lastName1 + ", " + lastName2;
            List<Operator> operators = this.db.Database.SqlQuery<Operator>(query).ToList();
            int NumberOfClients = 0;
            if (operators.Count > 0) NumberOfClients = (int)operators.FirstOrDefault().TotalRows;
            int TotalPages = (int)Math.Ceiling((double)NumberOfClients / pagesize);
            OperatorPagination clientPagination = new OperatorPagination(NumberOfClients, operators, page, pagesize);
            return clientPagination;
        }

        public List<fdi_result> fdi(DateTime? start, DateTime? end)
        {
            try
            {
                if (start == null)
                {
                    start = DateTime.Parse(("09/09/2021"));
                }
                if (end == null)
                {
                    end = DateTime.Now.AddDays(1);
                }
                string query = "fdo_fdi_reporte_chof '" + start?.ToString("yyyy-MM-dd") + "', '" + end?.ToString("yyyy-MM-dd") + "'";
                this.db.Database.CommandTimeout = 350;
                List<fdi_result> response = this.db.Database.SqlQuery<fdi_result>(query).ToList();
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public Operator get(string gafet)
        {
            var chofer = gafet.Replace(" ","").Replace("#","").ToUpper();
            if (chofer.Contains("OP-"))
            {
                gafet = gafet.ToUpper() == "" || gafet is null ? "null" : "'" + gafet + "'";
                string query = "fdo_get_operator " + gafet;
                try
                {
                    // Comprobar si ese elemento existe en la tabla users (Gente pendeja que no captura bien)
                    user User = db.users.Where(x => x.partnerReference == chofer && x.active == true).FirstOrDefault();
                    // Comprovar si existe el usuario en chof detalle.
                    Chof_Detalle Chof = db.Chof_Detalle.Where(x => x.CHOFER == chofer).FirstOrDefault();

                    if (Chof != null && User != null)
                    {

                    }
                    else if (Chof != null && User == null)
                    {
                        // Aqui creamos al operador con los datos de chofDatPersonal
                        User = new user();
                        // Rellenamos el objeto user para el funcionamiento extendido de practicontrol 
                        User.active = true;
                        User.at = false;
                        User.birthDate = Chof.FECHANAC;
                        User.birthPlace = Chof.LUGARNAC;
                        User.cafecude = false;
                        User.candidateHardvoteId = null;
                        User.candidateId = null;
                        User.candidateQuizID = 0;
                        User.comments = "";
                        User.createdBy = "Sistema"; // Quien Crea el objeto
                        User.createdDt = DateTime.Now; // Fecha de creacion
                        User.curp = "";
                        User.death = false;
                        User.dtLastUpdate = DateTime.Now;
                        User.editBy = "Sistema";
                        User.rfc = "";
                        // Emails pendientes 
                        //User.emails;
                        User.facebook = "";
                        User.firstName = Chof.NOMBRE.ToUpper();
                        User.googleplus = "";
                        User.ine = "";
                        User.instagram = "";
                        User.lastNameF = Chof.APELLIDOS.ToUpper();
                        User.lastNameM = Chof.APELLIDOS.ToUpper();
                        User.Message = "";
                        User.partnerReference = Chof.CHOFER;
                        User.partnerTypeId = 2; // Definimos que es un operador (Evitar errores de capa 8 con archivo)
                        User.payroll = false;
                        User.reason = "";
                        User.relationShipStatusId = 1;
                        User.sex = Chof.SEXO == "M" ? 1 : 0;
                        User.statusId = 1;
                        User.userTypeId = 1;
                        User.rt = false;
                        User.death = false;
                        User.enemy = false;
                        User.payroll = false;
                        User.ttesoc = false;
                        this.db.users.Add(User);
                        this.db.SaveChanges();
                    }
                    
                    Operator _operator = this.db.Database.SqlQuery<Operator>(query).ToList().FirstOrDefault();
                    if (_operator == null) return null;
                    _operator.organDonor = _operator.organDonor == null ? false : true;
                    _operator.userAddresses = db.userAddresses.Where(x => x.userId == _operator.userId && x.active == true)
                        .Include(x => x.state)
                        .Include(x => x.city)
                        .ToList();
                    _operator.emails = db.emails.Where(x => x.userId == _operator.userId && x.active == true).ToList();
                    _operator.userPhones = db.userPhones.Where(x => x.userId == _operator.userId && x.active == true).ToList();
                    _operator.userHealthInformation = db.userHealthInformations.Where(x => x.userId == _operator.userId).ToList();
                    _operator.allergies = _operator.userHealthInformation != null && _operator.userHealthInformation.Count() > 0 ? _operator.userHealthInformation.FirstOrDefault().allergies : "";
                    _operator.amazonPictures = db.amazonPictures.Where(x => x.userId == _operator.userId).ToList();
                    _operator.partnerReference.ToUpper();
                    _operator.firstName.ToUpper();
                    _operator.lastNameF.ToUpper();
                    _operator.lastNameM.ToUpper();
                    return _operator;
                }
                catch (EntityException ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
            else
            {
                gafet = gafet == "" || gafet is null ? "null" : "'" + gafet + "'";
                string query = "fdo_get_partner " + gafet;
                try
                {
                    Operator _operator = this.db.Database.SqlQuery<Operator>(query).ToList().FirstOrDefault();
                    _operator.organDonor = _operator.organDonor == null ? false : true;
                    _operator.userAddresses = db.userAddresses.Where(x => x.userId == _operator.userId && x.active == true)
                        .Include(x => x.state)
                        .Include(x => x.city)
                        .ToList();
                    _operator.emails = db.emails.Where(x => x.userId == _operator.userId && x.active == true).ToList();
                    _operator.userPhones = db.userPhones.Where(x => x.userId == _operator.userId && x.active == true).ToList();
                    _operator.userHealthInformation = db.userHealthInformations.Where(x => x.userId == _operator.userId).ToList();
                    _operator.allergies = _operator.userHealthInformation.FirstOrDefault().allergies;
                    _operator.amazonPictures = db.amazonPictures.Where(x => x.userId == _operator.userId).ToList();
                    return _operator;
                }
                catch (EntityException ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public Operator save() { // OPERATOR ES UN OBJETO COMPUESTO POR CHOF_DETALLE Y USER
            try
            {
                bool isExpress = false;
                String query = "";
                String sexo = this.sex == 1 ? "M" : "F";
                String date = DateTime.Now.ToString();
                String dateFortmat = DateTime.Now.ToString("yyyyMMdd");
                DocumentCounter counter = new DocumentCounter();
                string _relatationShip = "S";
                if (this.relationShipStatusId == 2)
                {
                    _relatationShip = "C";
                }
                if (this.relationShipStatusId == 3)
                {
                    _relatationShip = "V";
                }
                else if (this.relationShipStatusId == 5)
                {
                    _relatationShip = "U";
                }

                if (this.partnerReference == null)
                {
                    // Obtenemos el numero de operador automaticamente.
                    if (this.typeoperator == "tte")
                    {
                        query = "SELECT DOCUMENTO, VALOR FROM CONSECUTIVOS WHERE DOCUMENTO='TTE'";
                    }
                    else
                    {
                        query = "SELECT DOCUMENTO, VALOR FROM CONSECUTIVOS WHERE DOCUMENTO='OPERADORES'";
                    }
                    counter = this.db.Database.SqlQuery<DocumentCounter>(query).FirstOrDefault<DocumentCounter>();
                    if (counter != null)
                    {
                        this.partnerReference = "OP-" + ((int)counter.VALOR + 1).ToString();
                    }
                }
                else {
                    counter = new DocumentCounter()
                    {
                        DOCUMENTO = "",
                        VALOR = 0,
                    };
                }

                query = "SELECT * FROM chof_detalle WHERE STATUS='A' AND CHOFER = '" + this.partnerReference + "'";
                // Verificamos que el operador no exista....
                Chof_Detalle chof_Detalle = this.db.Database.SqlQuery<Chof_Detalle>(query).FirstOrDefault<Chof_Detalle>();
                if (chof_Detalle != null) // Defube que ya existe el proceso.
                {
                    this.MessageAction = "Operador existente";
                    this.StatusAction = error;
                    return null;
                }

                // Guardar en base de datos practicontrol.
                //if ((this.typeentrance == "normal" && this.typeoperator == "tte") || (this.typeentrance == "normal" && this.typeoperator != "tte"))
                if ((this.typeentrance == "normal" && this.typeoperator == "tte") || (this.typeentrance == "normal" && this.typeoperator != "tte"))
                {
                    query = @"
                        BEGIN TRAN;
                            exec sp_executesql N'INSERT INTO ""SINDQR""..""chof_detalle"" (""CHOFER"",""NOMBRE"",""APELLIDOS"",""SOBRENOMB"",""SEXO"",""FECHANAC"",""LUGARNAC"",""EDOCIVIL"",""CREDELECTOR"",""SECCION"",""OCUPACION"",""TIPOSANGRE"",""CARTILLA"",""TELEFONO"",""FONDDEF"",""STATUS"",""FECHAING"",""DOMICACT"",""CIUDADACT"",""DOMICANT"",""CIUDADANT"",""LICENCIA"",""LICVENCE"",""RFC"",""COMEN1"",""FECHAOP"",""ID_USUARIO"",""impacto"") VALUES (@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9,@P10,@P11,@P12,@P13,@P14,@P15,@P16,@P17,@P18,@P19,@P20,@P21,@P22,@P23,@P24,@P25,@P26,@P27,@P28)',N'@P1 varchar(8000),@P2 varchar(8000),@P3 varchar(8000),@P4 varchar(8000),@P5 varchar(8000),@P6 datetime2(7),@P7 varchar(8000),@P8 varchar(8000),@P9 varchar(8000),@P10 varchar(8000),@P11 varchar(8000),@P12 varchar(8000),@P13 varchar(8000),@P14 varchar(8000),@P15 bit,@P16 varchar(8000),@P17 datetime2(7),@P18 varchar(8000),@P19 varchar(8000),@P20 varchar(8000),@P21 varchar(8000),@P22 varchar(8000),@P23 datetime2(7),@P24 varchar(8000),@P25 varchar(8000),@P26 datetime2(7),@P27 varchar(8000),@P28 bit',
                            '" + this.partnerReference.ToUpper() + @"',
                            '" + this.firstName.ToUpper() + @"',
                            '" + this.lastNameF.ToUpper() + " " + this.lastNameM.ToUpper() + @"',
                            '',
                            '" + sexo + @"',
                            '" + dateFortmat + @"',
                            '" + this.birthPlace + @"',
                            '" + _relatationShip + @"',
                            '',
                            '1',
                            '',
                            '" + this.TIPOSANGRE + @"',
                            '',
                            '" + TELEFONO + @"',
                            0,
                            'A',
                            '" + DateTime.Now.ToString("yyyyMMdd") + @"',
                            '',
                            'CANCUN',
                            'N/A',
                            'N/A',
                            '" + dateFortmat + @"',
                            '" + dateFortmat + @"',
                            '',
                            ' ',
                            '" + dateFortmat + @"',
                            '10-1',
                            0;

                            exec sp_executesql N'INSERT INTO ""SINDQR""..""chof_head"" (""CHOFER"") VALUES (@P1)',N'@P1 varchar(8000)',
                            '" + this.partnerReference.ToUpper() + @"';

                            EXECUTE AJR_REGISTRAENFONDO '" + this.partnerReference.ToUpper() + @"'
                            EXECUTE AJR_REGISTRAENPROVEEDORES '" + this.partnerReference.ToUpper() + @"','" + this.firstName.ToUpper() + " " + this.lastNameF.ToUpper() + " " + this.lastNameM.ToUpper() + @"'
                            EXECUTE AJR_NUEVOREGISTRAFALTASYASISEHISTORICO '" + this.partnerReference.ToUpper() + @"',0,'" + dateFortmat + @"','10-1','OP'
                            EXECUTE AJR_NUEVOINSCRIPCIONFDI '" + this.partnerReference.ToUpper() + @"','" + dateFortmat + @"','10-1','OP'
                            exec sp_executesql N'UPDATE ""SINDQR""..""CONSECUTIVOS"" SET ""VALOR""=@P1 WHERE ""DOCUMENTO""=@P2 AND ""VALOR""=@P3',N'@P1 money,@P2 varchar(8000),@P3 money',$" + ((int)(counter.VALOR + 1)).ToString() + @",'" + counter.DOCUMENTO + "',$" + ((int)counter.VALOR).ToString() + @"

                            COMMIT TRAN
                            --ROLLBACK TRAN
                    ";
                    var a = this.db.Database.ExecuteSqlCommand(query);
                    // Modificamos los valores del reporte diario.
                    query = @"update Inventario set Precio = 20, PrecioConIva = 20 where Id_Familia = 0 and Id_Proveedor = '" + this.partnerReference.ToUpper() + @"' or Id_Familia = 36 and Id_Proveedor = '" + this.partnerReference.ToUpper() + @"';";
                    a = this.db.Database.ExecuteSqlCommand(query);
                }
                else if ((this.typeentrance == "flash" && this.typeoperator != "tte")) // ESTO DEFINE QUE ES UNA ENTRADA EXPRESS.
                {
                    query = @"
                    BEGIN TRAN;
                        exec sp_executesql N'INSERT INTO ""SINDQR""..""chof_detalle"" (""CHOFER"",""NOMBRE"",""APELLIDOS"",""SOBRENOMB"",""SEXO"",""FECHANAC"",""LUGARNAC"",""EDOCIVIL"",""CREDELECTOR"",""SECCION"",""OCUPACION"",""TIPOSANGRE"",""CARTILLA"",""TELEFONO"",""FONDDEF"",""STATUS"",""FECHAING"",""DOMICACT"",""CIUDADACT"",""DOMICANT"",""CIUDADANT"",""LICENCIA"",""LICVENCE"",""RFC"",""COMEN1"",""FECHAOP"",""ID_USUARIO"",""impacto"",""express"") VALUES (@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9,@P10,@P11,@P12,@P13,@P14,@P15,@P16,@P17,@P18,@P19,@P20,@P21,@P22,@P23,@P24,@P25,@P26,@P27,@P28,@P29)',N'@P1 varchar(8000),@P2 varchar(8000),@P3 varchar(8000),@P4 varchar(8000),@P5 varchar(8000),@P6 datetime2(7),@P7 varchar(8000),@P8 varchar(8000),@P9 varchar(8000),@P10 varchar(8000),@P11 varchar(8000),@P12 varchar(8000),@P13 varchar(8000),@P14 varchar(8000),@P15 bit,@P16 varchar(8000),@P17 datetime2(7),@P18 varchar(8000),@P19 varchar(8000),@P20 varchar(8000),@P21 varchar(8000),@P22 varchar(8000),@P23 datetime2(7),@P24 varchar(8000),@P25 varchar(8000),@P26 datetime2(7),@P27 varchar(8000),@P28 bit,@P29 bit',
                        '" + this.partnerReference.ToUpper() + @"',
                        '" + this.firstName.ToUpper() + @"',
                        '" + this.lastNameF.ToUpper() + " " + this.lastNameM.ToUpper() + @"',
                        '',
                        '" + sexo + @"',
                        '" + dateFortmat + @"',
                        '" + this.birthPlace + @"',
                        '" + _relatationShip + @"',
                        '',
                        '1',
                        '',
                        '" + this.TIPOSANGRE + @"',
                        '',
                        '" + TELEFONO + @"',
                        0,
                        'A',
                        '" + DateTime.Now.ToString("yyyyMMdd") + @"',
                        '',
                        'CANCUN',
                        'N/A',
                        'N/A',
                        '" + dateFortmat + @"',
                        '" + dateFortmat + @"',
                        '',
                        ' ',
                        '" + dateFortmat + @"',
                        '10-1',
                        0,
                        1;

                        exec sp_executesql N'INSERT INTO ""SINDQR""..""chof_head"" (""CHOFER"") VALUES (@P1)',N'@P1 varchar(8000)',
                        '" + this.partnerReference.ToUpper() + @"';

                        EXECUTE AJR_REGISTRAENPROVEEDORES '" + this.partnerReference.ToUpper() + @"','" + this.firstName.ToUpper() + " " + this.lastNameF.ToUpper() + " " + this.lastNameM.ToUpper() + @"'
                        EXECUTE AJR_NUEVOREGISTRAFALTASYASISEHISTORICO '" + this.partnerReference.ToUpper() + @"',0,'" + dateFortmat + @"','10-1','OP'
                        exec sp_executesql N'UPDATE ""SINDQR""..""CONSECUTIVOS"" SET ""VALOR""=@P1 WHERE ""DOCUMENTO""=@P2 AND ""VALOR""=@P3',N'@P1 money,@P2 varchar(8000),@P3 money',$" + ((int)(counter.VALOR + 1)).ToString() + @",'" + counter.DOCUMENTO + "',$" + ((int)counter.VALOR).ToString() + @"

                        COMMIT TRAN
                        --ROLLBACK TRAN
                        ";
                    isExpress = true;
                    var a = this.db.Database.ExecuteSqlCommand(query);

                    // EXPRESS
                }
                else {
                    return null;
                }

                //this.db.Database.ExecuteSqlCommand(query);
                // El objeto Operator esta formadio por dos subobjetos chof_detalle y user;
                user User = new user();
                // Rellenamos el objeto user para el funcionamiento extendido de practicontrol 
                User.active = true;
                User.at = this.at;
                User.birthDate = this.birthDate;
                User.birthPlace = this.birthPlace;
                User.cafecude = this.cafecude;
                User.candidateHardvoteId = null;
                User.candidateId = null;
                User.candidateQuizID = 0;
                User.comments = this.comments;
                User.createdBy = this.createdBy; // Quien Crea el objeto
                User.createdDt = DateTime.Now; // Fecha de creacion
                User.curp = this.curp;
                User.death = this.death;
                User.dtLastUpdate = this.dtLastUpdate;
                User.editBy = this.editBy;
                User.rfc = this.RFC;
                // Emails pendientes 
                //User.emails;
                User.facebook = this.facebook;
                User.firstName = this.firstName.ToUpper();
                User.googleplus = this.googleplus;
                User.ine = this.ine;
                User.instagram = this.instagram;
                User.lastNameF = this.lastNameF.ToUpper();
                User.lastNameM = this.lastNameM.ToUpper();
                User.Message = this.Message;
                User.partnerReference = this.partnerReference;
                User.partnerTypeId = 2; // Definimos que es un operador (Evitar errores de capa 8 con archivo)
                User.payroll = this.payroll;
                User.reason = this.reason;
                User.relationShipStatusId = this.relationShipStatusId;
                User.sex = this.sex;
                User.statusId = 1;
                User.twitter = this.twitter;
                User.userTypeId = 1;
                User.rt = false;
                User.twitter = this.twitter;
                User.facebook = this.facebook;
                User.userAddresses = this.userAddresses;
                User.openpayClientID = "";
                
                User.noVote = this.noVote;
                User.meeting = 0;
                User.death = false;
                User.enemy = false;
                User.payroll = false;
                User.ttesoc = false;

                // Informacion de salud
                User.userHealthInformation = new userHealthInformation();
                User.userHealthInformation.organDonor = this.organDonor != null ? true : false;
                User.userHealthInformation.allergies = this.allergies;
                User.userHealthInformation.descripalergies = this.descripalergies;

                // Agregamos la nueva corre.
                email email = new email();
                email.userId = this.userId;
                email.email1 = this.emails.FirstOrDefault().email1;
                email.emailPriorityTypeId = 1;
                email.active = true;
                //db.emails.Add(email);
                User.emails = new List<email>();
                User.emails.Add(email);

                // Agregamos la nueva direccion.
                userAddress address = new userAddress();
                address.userId = this.userId;
                address.address = this.userAddresses.FirstOrDefault().address;
                address.address2 = this.userAddresses.FirstOrDefault().address2;
                address.colony = this.userAddresses.FirstOrDefault().colony;
                address.cityId = this.userAddresses.FirstOrDefault().cityId;
                address.active = true;
                address.isValid = true;
                address.interiorNumber = this.userAddresses.FirstOrDefault().interiorNumber;
                address.supermanzana = this.userAddresses.FirstOrDefault().supermanzana;
                address.manzana = this.userAddresses.FirstOrDefault().manzana;
                address.lote = this.userAddresses.FirstOrDefault().lote;
                address.street = this.userAddresses.FirstOrDefault().street;
                address.postalCode = this.userAddresses.FirstOrDefault().postalCode;

                address.createdDate = DateTime.Now;
                address.createdBy = "";
                address.editBy = "";
                //db.userAddresses.Add(address);
                User.userAddresses = new List<userAddress>();
                User.userAddresses.Add(address);

                // Agregamos el referido
                User.recommendedBy = this.recommendedBy;

                // Agregamos la nueva direccion.
                userPhone phone = new userPhone();
                phone.userId = this.userId;
                phone.phone = this.userPhones.FirstOrDefault().phone;
                phone.phoneTypeId = 1;
                phone.active = true;
                phone.isValid = true;
                //db.userPhones.Add(phone);
                User.userPhones = new List<userPhone>();
                User.userPhones.Add(phone);
                this.db.users.Add(User);
                this.db.SaveChanges();
                
                // regresamos el userId 
                this.userId = User.id;
                // AJUSTAMOS EL ID_USER AL PERMISO SI EXISTE
                if (isExpress)
                {
                    // CREAMOS EL PERMISO EXPRESS PERO TENEMOS QUE ACTUALZAR AL FINAL PARA OBTENER EL ID_USER PARE RELACIONAR LOS PERMISOS CON EL OBJETO USER
                    C_permissions obj = new C_permissions()
                    {
                        active = true,
                        descripcion = "PERMISO EXPRESS",
                        days = 30,
                        gafet = this.partnerReference.ToUpper(),
                        CreatedBy = this.createdBy,
                        dateCreated = DateTime.Now,
                        id_user = User.id,
                        start = DateTime.Now,
                        end = DateTime.Now.AddDays(30)
                    };
                    obj.save(); // Aqui adentro hacemos todo procesos.
                    // Reportalo 30 dias que compete a los 900.
                    HistAsistencia histAsistencia = new HistAsistencia();
                    histAsistencia.reportar(User.partnerReference, "OP", "000","P",DateTime.Now, 1, "P"+obj.id.ToString(), DateTime.Now, 1,DateTime.Now );
                }
                return this;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Operator update()
        {
            try
            {
                string query = "fdo_get_operator '" + this.partnerReference + "'";
                Operator _operator = this.db.Database.SqlQuery<Operator>(query).ToList().FirstOrDefault();
                _operator.organDonor = _operator.organDonor == null ? false : true;
                _operator.userAddresses = db.userAddresses.Where(x => x.userId == _operator.userId).ToList();
                string sex = _operator.sex == 1 ? "M" : "F";
                // Sacar tipo de relacion 
                string relation = "";
                switch (this.relationShipStatusId)
                {
                    case 1:
                        relation = "S";
                        break;
                    case 2:
                        relation = "C";
                        break;
                    case 3:
                        relation = "D";
                        break;
                    default:
                        relation = "U";
                        break;
                }

                String dateFortmat = DateTime.Now.ToString("yyyyMMdd");
                // CONSULTA BASE PARA MODIFICAR.

                // ACTUALIZAMOS PADRON
                user User = this.db.users
                    .Include(x => x.userAddresses)
                    .Include(x => x.userPhones)
                    .Include(x => x.emails)
                    .Include(x => x.amazonPictures)
                    .Where(x => x.id == _operator.userId)
                    .FirstOrDefault();
                // GUARDAMSO EL OBJETO EN EL HISTORIAL
                userHistory UserH = new userHistory();
                //UserH.jsonObj = JsonConvert.SerializeObject(User);
                UserH.LastupdatedDate = DateTime.Now;


                // Rellenamos el objeto user para el funcionamiento extendido de practicontrol 
                User.active = true;
                User.at = this.at;
                User.birthDate = this.birthDate;
                User.birthPlace = this.birthPlace;
                User.cafecude = this.cafecude;
                User.candidateHardvoteId = null;
                User.candidateId = null;
                User.candidateQuizID = 0;
                User.comments = this.comments;
                User.createdBy = this.createdBy;
                User.createdDt = ((DateTime)this.createdDt);
                User.curp = this.curp;
                User.death = this.death;
                User.dtLastUpdate = this.dtLastUpdate;
                User.editBy = this.editBy;
                User.rfc = this.RFC;

                // Emails pendientes 
                //User.emails;
                
                User.facebook = this.facebook;
                User.firstName = this.firstName.ToUpper();
                User.googleplus = this.googleplus;
                User.ine = this.ine;
                User.instagram = this.instagram;
                User.lastNameF = this.lastNameF.ToUpper();
                User.lastNameM = this.lastNameM.ToUpper();
                User.Message = this.Message;
                User.partnerReference = this.partnerReference;
                User.partnerTypeId = 2; // Definimos que es un operador (Evitar errores de capa 8 con archivo)
                User.payroll = this.payroll;
                User.reason = this.reason;
                User.relationShipStatusId = this.relationShipStatusId;
                User.sex = this.sex;
                User.statusId = 1;
                User.twitter = this.twitter;
                User.userTypeId = 1;
                User.rt = false;
                User.twitter = this.twitter;
                User.facebook = this.facebook;

                User.noVote = this.noVote;
                User.meeting = 0;
                User.death = false;
                User.enemy = false;
                User.payroll = false;
                User.ttesoc = false;

                // Informacion de salud
                // Obtenems el objeto de saludo si existe 

                userHealthInformation healthInformation = db.userHealthInformations.Where(x => x.userId == User.id).FirstOrDefault();
                if (healthInformation == null)
                {
                    healthInformation = new userHealthInformation();
                }
                healthInformation.organDonor = this.organDonor != null ? true : false;
                healthInformation.allergies = this.allergies;
                healthInformation.descripalergies = this.descripalergies;
                User.userHealthInformation = healthInformation;
                // Actualizar referido
                User.recommendedBy = this.recommendedBy;

                db.Entry(User).State = EntityState.Modified;
                this.db.SaveChanges();

                // Actualizar subobjetos.
                // Telefono
                List<userPhone> phones = db.userPhones.Where(x => x.userId == this.userId).ToList();
                string _phone = "";
                foreach (var phone in phones)
                {
                    foreach (var nphones in this.userPhones)
                    {
                        if (nphones.id == phone.id)
                        {
                            _phone = phone.phone = nphones.phone;
                            db.Entry(phone).State = EntityState.Modified;
                        }
                        else
                        {
                            db.userPhones.Remove(phone);
                        }
                    }
                }
                if (phones.Count() == 0)
                {
                    // Agregamos la nueva direccion.
                    userPhone phone = new userPhone();
                    phone.userId = this.userId;
                    _phone = phone.phone = this.userPhones.FirstOrDefault()?.phone;
                    phone.phoneTypeId = 1;
                    phone.active = true;
                    phone.isValid = true;
                    db.userPhones.Add(phone);
                }

                // Direcciones 
                List<userAddress> useraddress = db.userAddresses.Where(x => x.userId == this.userId).ToList();
                foreach (var address in useraddress)
                {
                    foreach (var naddress in this.userAddresses)
                    {
                        if (naddress.id == address.id)
                        {
                            address.address = naddress.address;
                            address.address2 = naddress.address2;
                            address.colony = naddress.colony;
                            address.cityId = naddress.cityId;
                            address.active = true;
                            address.isValid = true;
                            address.interiorNumber = naddress.interiorNumber;
                            address.supermanzana = naddress.supermanzana;
                            address.manzana = naddress.manzana;
                            address.lote = naddress.lote;
                            address.street = naddress.street;
                            address.postalCode = naddress.postalCode;
                            db.Entry(address).State = EntityState.Modified;
                        }
                        else
                        {
                            db.userAddresses.Remove(address);
                        }
                    }
                }
                if (useraddress.Count() == 0)
                {
                    // Agregamos la nueva direccion.
                    userAddress address = new userAddress();
                    address.userId = this.userId;
                    address.address = this.userAddresses.FirstOrDefault().address;
                    address.address2 = this.userAddresses.FirstOrDefault().address2;
                    address.colony = this.userAddresses.FirstOrDefault().colony;
                    address.cityId = this.userAddresses.FirstOrDefault().cityId;
                    address.active = true;
                    address.isValid = true;
                    address.interiorNumber = this.userAddresses.FirstOrDefault().interiorNumber;
                    address.supermanzana = this.userAddresses.FirstOrDefault().supermanzana;
                    address.manzana = this.userAddresses.FirstOrDefault().manzana;
                    address.lote = this.userAddresses.FirstOrDefault().lote;
                    address.street = this.userAddresses.FirstOrDefault().street;
                    address.postalCode = this.userAddresses.FirstOrDefault().postalCode;

                    address.createdDate = DateTime.Now;
                    address.createdBy = "";
                    address.editBy = "";
                    db.userAddresses.Add(address);
                }

                // Email
                List<email> emails = db.emails.Where(x => x.userId == this.userId).ToList();
                foreach (var email in emails)
                {
                    foreach (var nemail in this.emails)
                    {
                        if (nemail.id == email.id)
                        {
                            email.email1 = nemail.email1;
                            db.Entry(email).State = EntityState.Modified;
                        }
                        else
                        {
                            db.emails.Remove(email);
                        }
                    }
                }
                if (emails.Count() == 0)
                {
                    // Agregamos la nueva direccion.
                    email email = new email();
                    email.userId = this.userId;
                    email.email1 = this.emails.FirstOrDefault().email1;
                    email.emailPriorityTypeId = 1;
                    email.active = true;
                    db.emails.Add(email);
                }
                db.SaveChanges();

                // ACTUALIZAMOS PRACTICONTROL.
                query = @"
                BEGIN TRAN;
                exec sp_executesql N' 
	                UPDATE [dbo].Chof_Detalle 
	                SET
	                [NOMBRE] =  @NOMBRE,
	                [APELLIDOS] =  @APELLIDOS,
	                [SEXO] =  @SEXO,
	                [FECHANAC] =  @FECHANAC,
	                [LUGARNAC] =  @LUGARNAC,
	                [EDOCIVIL] =  @EDOCIVIL,
	                [TIPOSANGRE] =  @TIPOSANGRE,
	                [TELEFONO] =  @TELEFONO,
	                [FECHAING] =  @FECHAING
	                WHERE [CHOFER] = @CHOFER;',
	
	                N'
	                @NOMBRE varchar(255), 
	                @APELLIDOS varchar(255),
	                @SEXO varchar(255),
	                @FECHANAC varchar(255),
	                @LUGARNAC varchar(255),
	                @EDOCIVIL varchar(255),
	                @TIPOSANGRE varchar(255),
	                @TELEFONO varchar(255),
	                @FECHAING varchar(255),
	                @CHOFER varchar(255)
	                ',
	                '" + this.firstName.ToUpper() + @"',
	                '" + this.lastNameF.ToUpper() + " " + this.lastNameM.ToUpper() + @"',
                    '" + sex + @"',
	                '" + ((DateTime)this.birthDate).ToString("yyyyMMdd") + @"',
	                '" + this.birthPlace + @"',
	                '" + relation + @"',
	                 '" + this.TIPOSANGRE + @"',
	                '" + _phone + @"',
	                '" + ((DateTime)this.createdDt).ToString("yyyyMMdd") + @"',
	                '" + this.partnerReference.ToUpper() + @"'; 
                COMMIT TRAN;
                ";
                var a = this.db.Database.ExecuteSqlCommand(query);

                // Cambiar nombre en otras tablas
                query = @"
                
                DECLARE @gafete varchar(12)
                DECLARE @nombre varchar(200)
                DECLARE @apellidos varchar(200)
                SET @gafete = '" + _operator.partnerReference + @"'
                SET @nombre = '" + this.firstName.ToUpper() + @"'
                SET @apellidos = '" + this.lastNameF.ToUpper() + " " + this.lastNameM.ToUpper() + @"'
                BEGIN TRANSACTION  
                UPDATE Chof_Detalle SET Chof_Detalle.NOMBRE = @nombre , 
                                        Chof_Detalle.APELLIDOS = @apellidos
                WHERE Chof_Detalle.CHOFER = @gafete
                UPDATE Proveedores SET Proveedores.Proveedor = @nombre + ' ' + @apellidos
                WHERE Proveedores.Id_Proveedor = @gafete
                COMMIT TRANSACTION";
                a = this.db.Database.ExecuteSqlCommand(query);

                return _operator;
            }
            catch (EntityException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // Obtiene los ultimos 365 dias de reportes.
        public List<HistAsistencia> reportes() {
            try
            {
                using (var db = new radiotaxiEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    string query = @"
                        DECLARE @gafet AS VARCHAR(100);
                        SET @gafet = '" + this.partnerReference + @"';                
                        with reportes as ( -- Reportes contabilizando si son dobles o simples
                            SELECT top 365 Gafete, Taxi, FechaReporte, FechaOp, Ticket, count(-1) as r
                            FROM HistAsistencias
                            WHERE (Gafete = @gafet)
                            group by Gafete, Taxi, FechaReporte, Ticket, FechaOp
                            ORDER BY FechaReporte DESC
                        ), reportes_u as ( -- Reportes simples si son vespertinos o matutinos
                            SELECT top 365 Gafete, Taxi, FechaReporte, FechaOp, Ticket, count(-1) as r
                            FROM HistAsistencias
                            WHERE (Gafete = @gafet)
                            group by Gafete, Taxi, FechaReporte, FechaOp, Ticket
                            having count (-1) < 2
                            ORDER BY FechaReporte DESC
                        ), reportes_a as (
                            SELECT top 1000 Gafete, Tipo, Taxi, Turno, FechaReporte, Concepto, Ticket, FechaOp
                            FROM HistAsistencias
                            WHERE (Gafete = @gafet) and Turno in ('V','F','M')
                            ORDER BY FechaReporte DESC
                        )
                        select top 365 
                        r.Gafete, r.Taxi, r.FechaReporte, r.FechaOp, r.Ticket, 'na' as Tipo,'na' as Concepto, null as id_permission,
                        CASE 
                            WHEN Turno is null THEN 'Completo'
                            WHEN Turno = 'F' THEN 'Falta'
                            WHEN Turno = 'M' THEN 'Matutino'
                            WHEN Turno = 'V' THEN 'Vespertino'
                            ELSE 'NA'
                        END as Turno
                        from reportes as r 
                        left join reportes_a as a on r.FechaReporte = a.FechaReporte and r.r = 1
                        order by r.FechaReporte desc;
                        ";
                    List<HistAsistencia> _Asistencias = db.Database.SqlQuery<HistAsistencia>(query).ToList();
                    return _Asistencias;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<HistAsistencia>();
            }
        }

        public financesOperator financesOperator()
        {
            try
            {
                using (var db = new radiotaxiEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    string query = @"
                            DECLARE @gafet AS VARCHAR(100);
                            SET @gafet = '" + this.partnerReference + @"';
                
                            SELECT SUM(CASE
                                    WHEN ABS(Id_Familia) IN(0, 113)
                                    THEN Exist
                                    ELSE 0
                                END) AS faltas,
                            SUM(CASE
                                    WHEN ABS(Id_Familia) IN(28, 105)
                                    THEN Exist * PrecioConIva
                                    ELSE 0
                                END) AS cuotas_fdi,
                            SUM(CASE
                                    WHEN ABS(Id_Familia) NOT IN(0, 113, 28, 105, 103)
                                    THEN Exist * PrecioConIva
                                    ELSE 0
                                END) AS otros_cargos
                                FROM Inventario
                            WHERE Id_Proveedor = @gafet 
                            ";
                    financesOperator _finance = db.Database.SqlQuery<financesOperator>(query).ToList().FirstOrDefault();
                    return _finance;
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                financesOperator _finance = new financesOperator();
                _finance.faltas = 100000;
                _finance.cuotas_fdi = 0;
                _finance.otros_cargos = 0;
                return _finance;
            }
        }

        Operator delete()
        {
            return null;
        }

        // Verificar tarjetones pagados
        public List<pay> payCards() {
            try
            {
                using (var db = new radiotaxiEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    string query = @"SELECT VD.Id_Op, VD.Id_Proveedor, V.Id_Cliente,VD.Id_Producto, VD.Importe as IMPORTE_MES, F.Familia, v.FechaOp
                            FROM [dbo].[VentasDetalle] VD
                            left join [dbo].[Ventas] as V on VD.Id_Op = V.ID_OP
                            left join [dbo].Familia as  f on f.Id_Familia = VD.Id_Familia
                            --left join [dbo].Chof_Detalle as cd on cd.CHOFER = v.id_pro
                            WHERE V.FechaOp  > '2024-01-15'
                            and VD.Id_Familia = 138 and Id_Vendedor = '" + this.partnerReference + @"'
                            order by V.FechaOp desc;";
                    List<pay> _pays = db.Database.SqlQuery<pay>(query).ToList();
                    return _pays;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<Convenio> agreements()
        {

            using (var db = new radiotaxiEntities())
            {
                try
                {
                    string query = @"
                        select * from Convenios as c
                        where c.[status] = 'Vigente' and AfavordeNum = '" + this.partnerReference + @"' or
                        c.[status] = 'Vigente' and CargoANum = '" + this.partnerReference + @"'; ";
                    List<Convenio> _agreements = db.Database.SqlQuery<Convenio>(query).ToList();
                    return _agreements;
                }
                catch (Exception ex)
                {

                    return null;
                }

            }
        }

        public List<Venta> sales() {
            try
            {
                return this.db.Ventas.Where(x => x.Id_Vendedor == this.partnerReference || x.Id_Cliente == this.partnerReference).OrderByDescending(x => x.FechaOp).Take(100).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<p_debt> debs()
        {
            try
            {
                string query = @"
                    
                    DECLARE @gafete AS VARCHAR(15);
                    SET @gafete='" + this.partnerReference + @"';

                    SELECT I.ID_FAMILIA AS CLAVE,F.FAMILIA AS CARGO,S.SUBFAMILIA AS A, SUM(I.EXIST)AS DEBE, ROUND(SUM(1*I.PrecioConIva),2)  AS PRECIO_IVA ,ROUND(SUM(I.Exist*I.PrecioConIva),2) AS IMPORTE_DEBE 
                    FROM INVENTARIO I 
                    INNER JOIN FAMILIA F ON F.ID_FAMILIA=I.ID_FAMILIA 
                    INNER JOIN SUBFAMILIA S ON S.ID_SUBFAMILIA=I.ID_SUBFAMILIA 
                    WHERE (I.ID_FAMILIA<>'36') 
                    AND (I.ID_PROVEEDOR=@gafete AND I.EXIST >0)  
                    GROUP BY I.ID_FAMILIA,F.FAMILIA,I.ID_SUBFAMILIA,S.SUBFAMILIA ORDER BY I.ID_SUBFAMILIA,I.ID_FAMILIA";
                List<p_debt> debs = db.Database.SqlQuery<p_debt>(query).ToList();
                return debs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Operadores_Baja_Datos getIdentity() {
            try
            {
                string query = @"exec sp_executesql N'SELECT 
                    [Extent1].[control] AS [control], 
                    [Extent1].[fecha_insert] AS [fecha_insert], 
                    [Extent1].[baja_por_faltas] AS [baja_por_faltas], 
                    [Extent1].[datos_regresados] AS [datos_regresados], 
                    [Extent1].[fecha_regreso] AS [fecha_regreso], 
                    [Extent1].[id_usuario_regreso] AS [id_usuario_regreso], 
                    [Extent1].[CHOFER] AS [CHOFER], 
                    [Extent1].[NOMBRE] AS [NOMBRE], 
                    [Extent1].[APELLIDOS] AS [APELLIDOS], 
                    [Extent1].[SOBRENOMB] AS [SOBRENOMB], 
                    [Extent1].[SEXO] AS [SEXO], 
                    [Extent1].[FECHANAC] AS [FECHANAC], 
                    [Extent1].[LUGARNAC] AS [LUGARNAC], 
                    [Extent1].[EDOCIVIL] AS [EDOCIVIL], 
                    [Extent1].[CREDELECTOR] AS [CREDELECTOR], 
                    [Extent1].[SECCION] AS [SECCION], 
                    [Extent1].[OCUPACION] AS [OCUPACION], 
                    [Extent1].[TIPOSANGRE] AS [TIPOSANGRE], 
                    [Extent1].[CARTILLA] AS [CARTILLA], 
                    [Extent1].[ALERGIAS] AS [ALERGIAS], 
                    [Extent1].[ESCOLARIDAD] AS [ESCOLARIDAD], 
                    [Extent1].[CONYUGE] AS [CONYUGE], 
                    [Extent1].[TELEFONO] AS [TELEFONO], 
                    [Extent1].[FONDDEF] AS [FONDDEF], 
                    [Extent1].[STATUS] AS [STATUS], 
                    [Extent1].[SOC_ALTA] AS [SOC_ALTA], 
                    [Extent1].[FECHAING] AS [FECHAING], 
                    [Extent1].[PATRON] AS [PATRON], 
                    [Extent1].[DOMICACT] AS [DOMICACT], 
                    [Extent1].[CIUDADACT] AS [CIUDADACT], 
                    [Extent1].[DOMICANT] AS [DOMICANT], 
                    [Extent1].[CIUDADANT] AS [CIUDADANT], 
                    [Extent1].[LICENCIA] AS [LICENCIA], 
                    [Extent1].[LICVENCE] AS [LICVENCE], 
                    [Extent1].[RFC] AS [RFC], 
                    [Extent1].[FECHABAJ] AS [FECHABAJ], 
                    [Extent1].[OBS] AS [OBS], 
                    [Extent1].[COMEN1] AS [COMEN1], 
                    [Extent1].[FECHAOP] AS [FECHAOP], 
                    [Extent1].[ID_USUARIO] AS [ID_USUARIO], 
                    [Extent1].[FECHAEDIT] AS [FECHAEDIT], 
                    [Extent1].[ID_USUARIOEDIT] AS [ID_USUARIOEDIT], 
                    [Extent1].[FECHAREACTIVACION] AS [FECHAREACTIVACION], 
                    [Extent1].[FIANZA] AS [FIANZA], 
                    [Extent1].[Mayacaribe] AS [Mayacaribe], 
                    [Extent1].[UltTaxi] AS [UltTaxi], 
                    [Extent1].[ULTTURNO] AS [ULTTURNO], 
                    [Extent1].[SECRETARIA] AS [SECRETARIA], 
                    [Extent1].[PUESTO] AS [PUESTO], 
                    [Extent1].[PosicionPadron] AS [PosicionPadron], 
                    [Extent1].[postemp] AS [postemp], 
                    [Extent1].[strenta] AS [strenta], 
                    [Extent1].[impacto] AS [impacto]
                    FROM [dbo].[Operadores_Baja_Datos] AS [Extent1]
                    WHERE [Extent1].[CHOFER] = @p__linq__0',N'@p__linq__0 varchar(8000)',@p__linq__0='" + this.partnerReference + @"';";
                List<Operadores_Baja_Datos> _Operadores_Baja_Datos = db.Database.SqlQuery<Operadores_Baja_Datos>(query).ToList();

                // Obtenemos el numero de control 
                decimal control = _Operadores_Baja_Datos.FirstOrDefault().control;

                query = @"
                exec sp_executesql N'SELECT TOP (1) 
                [Extent1].[control] AS [control], 
                [Extent1].[fecha_insert] AS [fecha_insert], 
                [Extent1].[baja_por_faltas] AS [baja_por_faltas], 
                [Extent1].[datos_regresados] AS [datos_regresados], 
                [Extent1].[fecha_regreso] AS [fecha_regreso], 
                [Extent1].[id_usuario_regreso] AS [id_usuario_regreso], 
                [Extent1].[CHOFER] AS [CHOFER], 
                [Extent1].[NOMBRE] AS [NOMBRE], 
                [Extent1].[APELLIDOS] AS [APELLIDOS], 
                [Extent1].[SOBRENOMB] AS [SOBRENOMB], 
                [Extent1].[SEXO] AS [SEXO], 
                [Extent1].[FECHANAC] AS [FECHANAC], 
                [Extent1].[LUGARNAC] AS [LUGARNAC], 
                [Extent1].[EDOCIVIL] AS [EDOCIVIL], 
                [Extent1].[CREDELECTOR] AS [CREDELECTOR], 
                [Extent1].[SECCION] AS [SECCION], 
                [Extent1].[OCUPACION] AS [OCUPACION], 
                [Extent1].[TIPOSANGRE] AS [TIPOSANGRE], 
                [Extent1].[CARTILLA] AS [CARTILLA], 
                [Extent1].[ALERGIAS] AS [ALERGIAS], 
                [Extent1].[ESCOLARIDAD] AS [ESCOLARIDAD], 
                [Extent1].[CONYUGE] AS [CONYUGE], 
                [Extent1].[TELEFONO] AS [TELEFONO], 
                [Extent1].[FONDDEF] AS [FONDDEF], 
                [Extent1].[STATUS] AS [STATUS], 
                [Extent1].[SOC_ALTA] AS [SOC_ALTA], 
                [Extent1].[FECHAING] AS [FECHAING], 
                [Extent1].[PATRON] AS [PATRON], 
                [Extent1].[DOMICACT] AS [DOMICACT], 
                [Extent1].[CIUDADACT] AS [CIUDADACT], 
                [Extent1].[DOMICANT] AS [DOMICANT], 
                [Extent1].[CIUDADANT] AS [CIUDADANT], 
                [Extent1].[LICENCIA] AS [LICENCIA], 
                [Extent1].[LICVENCE] AS [LICVENCE], 
                [Extent1].[RFC] AS [RFC], 
                [Extent1].[FECHABAJ] AS [FECHABAJ], 
                [Extent1].[OBS] AS [OBS], 
                [Extent1].[COMEN1] AS [COMEN1], 
                [Extent1].[FECHAOP] AS [FECHAOP], 
                [Extent1].[ID_USUARIO] AS [ID_USUARIO], 
                [Extent1].[FECHAEDIT] AS [FECHAEDIT], 
                [Extent1].[ID_USUARIOEDIT] AS [ID_USUARIOEDIT], 
                [Extent1].[FECHAREACTIVACION] AS [FECHAREACTIVACION], 
                [Extent1].[FIANZA] AS [FIANZA], 
                [Extent1].[Mayacaribe] AS [Mayacaribe], 
                [Extent1].[UltTaxi] AS [UltTaxi], 
                [Extent1].[ULTTURNO] AS [ULTTURNO], 
                [Extent1].[SECRETARIA] AS [SECRETARIA], 
                [Extent1].[PUESTO] AS [PUESTO], 
                [Extent1].[PosicionPadron] AS [PosicionPadron], 
                [Extent1].[postemp] AS [postemp], 
                [Extent1].[strenta] AS [strenta], 
                [Extent1].[impacto] AS [impacto]
                FROM [dbo].[Operadores_Baja_Datos] AS [Extent1]
                WHERE ([Extent1].[CHOFER] = @p__linq__0) AND ([Extent1].[control] = @p__linq__1)',N'@p__linq__0 varchar(8000),@p__linq__1 bigint',@p__linq__0='" + this.partnerReference + @"',@p__linq__1=" + control + @";";
                Operadores_Baja_Datos __Operadores_Baja_Datos = db.Database.SqlQuery<Operadores_Baja_Datos>(query).ToList().FirstOrDefault();
                return __Operadores_Baja_Datos;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public void identityReturn() {
            try
            {

                string query = @"
                    
                    exec sp_executesql N'SELECT 
                    [Extent1].[CHOFER] AS [CHOFER], 
                    [Extent1].[NOMBRE] AS [NOMBRE], 
                    [Extent1].[APELLIDOS] AS [APELLIDOS], 
                    [Extent1].[SOBRENOMB] AS [SOBRENOMB], 
                    [Extent1].[SEXO] AS [SEXO], 
                    [Extent1].[FECHANAC] AS [FECHANAC], 
                    [Extent1].[LUGARNAC] AS [LUGARNAC], 
                    [Extent1].[EDOCIVIL] AS [EDOCIVIL], 
                    [Extent1].[CREDELECTOR] AS [CREDELECTOR], 
                    [Extent1].[SECCION] AS [SECCION], 
                    [Extent1].[OCUPACION] AS [OCUPACION], 
                    [Extent1].[TIPOSANGRE] AS [TIPOSANGRE], 
                    [Extent1].[CARTILLA] AS [CARTILLA], 
                    [Extent1].[ALERGIAS] AS [ALERGIAS], 
                    [Extent1].[ESCOLARIDAD] AS [ESCOLARIDAD], 
                    [Extent1].[CONYUGE] AS [CONYUGE], 
                    [Extent1].[TELEFONO] AS [TELEFONO], 
                    [Extent1].[FONDDEF] AS [FONDDEF], 
                    [Extent1].[STATUS] AS [STATUS], 
                    [Extent1].[SOC_ALTA] AS [SOC_ALTA], 
                    [Extent1].[FECHAING] AS [FECHAING], 
                    [Extent1].[PATRON] AS [PATRON], 
                    [Extent1].[DOMICACT] AS [DOMICACT], 
                    [Extent1].[CIUDADACT] AS [CIUDADACT], 
                    [Extent1].[DOMICANT] AS [DOMICANT], 
                    [Extent1].[CIUDADANT] AS [CIUDADANT], 
                    [Extent1].[LICENCIA] AS [LICENCIA], 
                    [Extent1].[LICVENCE] AS [LICVENCE], 
                    [Extent1].[RFC] AS [RFC], 
                    [Extent1].[FECHABAJ] AS [FECHABAJ], 
                    [Extent1].[OBS] AS [OBS], 
                    [Extent1].[COMEN1] AS [COMEN1], 
                    [Extent1].[FECHAOP] AS [FECHAOP], 
                    [Extent1].[ID_USUARIO] AS [ID_USUARIO], 
                    [Extent1].[FECHAEDIT] AS [FECHAEDIT], 
                    [Extent1].[ID_USUARIOEDIT] AS [ID_USUARIOEDIT], 
                    [Extent1].[FECHAREACTIVACION] AS [FECHAREACTIVACION], 
                    [Extent1].[FIANZA] AS [FIANZA], 
                    [Extent1].[Mayacaribe] AS [Mayacaribe], 
                    [Extent1].[UltTaxi] AS [UltTaxi], 
                    [Extent1].[ULTTURNO] AS [ULTTURNO], 
                    [Extent1].[SECRETARIA] AS [SECRETARIA], 
                    [Extent1].[PUESTO] AS [PUESTO], 
                    [Extent1].[PosicionPadron] AS [PosicionPadron], 
                    [Extent1].[postemp] AS [postemp], 
                    [Extent1].[strenta] AS [strenta], 
                    [Extent1].[impacto] AS [impacto], 
                    [Extent1].[imprimirenpadron] AS [imprimirenpadron], 
                    [Extent1].[edicioncontrolada] AS [edicioncontrolada],
                    [Extent1].[express] AS [express]
                    FROM [dbo].[Chof_Detalle] AS [Extent1]
                    WHERE [Extent1].[CHOFER] = @p__linq__0',N'@p__linq__0 varchar(8000)',@p__linq__0='" + this.partnerReference + @"'";
                List<Chof_Detalle> _Chof_Detalle = db.Database.SqlQuery<Chof_Detalle>(query).ToList();

                query = @"exec sp_executesql N'SELECT 
                    [Extent1].[control] AS [control], 
                    [Extent1].[fecha_insert] AS [fecha_insert], 
                    [Extent1].[baja_por_faltas] AS [baja_por_faltas], 
                    [Extent1].[datos_regresados] AS [datos_regresados], 
                    [Extent1].[fecha_regreso] AS [fecha_regreso], 
                    [Extent1].[id_usuario_regreso] AS [id_usuario_regreso], 
                    [Extent1].[CHOFER] AS [CHOFER], 
                    [Extent1].[NOMBRE] AS [NOMBRE], 
                    [Extent1].[APELLIDOS] AS [APELLIDOS], 
                    [Extent1].[SOBRENOMB] AS [SOBRENOMB], 
                    [Extent1].[SEXO] AS [SEXO], 
                    [Extent1].[FECHANAC] AS [FECHANAC], 
                    [Extent1].[LUGARNAC] AS [LUGARNAC], 
                    [Extent1].[EDOCIVIL] AS [EDOCIVIL], 
                    [Extent1].[CREDELECTOR] AS [CREDELECTOR], 
                    [Extent1].[SECCION] AS [SECCION], 
                    [Extent1].[OCUPACION] AS [OCUPACION], 
                    [Extent1].[TIPOSANGRE] AS [TIPOSANGRE], 
                    [Extent1].[CARTILLA] AS [CARTILLA], 
                    [Extent1].[ALERGIAS] AS [ALERGIAS], 
                    [Extent1].[ESCOLARIDAD] AS [ESCOLARIDAD], 
                    [Extent1].[CONYUGE] AS [CONYUGE], 
                    [Extent1].[TELEFONO] AS [TELEFONO], 
                    [Extent1].[FONDDEF] AS [FONDDEF], 
                    [Extent1].[STATUS] AS [STATUS], 
                    [Extent1].[SOC_ALTA] AS [SOC_ALTA], 
                    [Extent1].[FECHAING] AS [FECHAING], 
                    [Extent1].[PATRON] AS [PATRON], 
                    [Extent1].[DOMICACT] AS [DOMICACT], 
                    [Extent1].[CIUDADACT] AS [CIUDADACT], 
                    [Extent1].[DOMICANT] AS [DOMICANT], 
                    [Extent1].[CIUDADANT] AS [CIUDADANT], 
                    [Extent1].[LICENCIA] AS [LICENCIA], 
                    [Extent1].[LICVENCE] AS [LICVENCE], 
                    [Extent1].[RFC] AS [RFC], 
                    [Extent1].[FECHABAJ] AS [FECHABAJ], 
                    [Extent1].[OBS] AS [OBS], 
                    [Extent1].[COMEN1] AS [COMEN1], 
                    [Extent1].[FECHAOP] AS [FECHAOP], 
                    [Extent1].[ID_USUARIO] AS [ID_USUARIO], 
                    [Extent1].[FECHAEDIT] AS [FECHAEDIT], 
                    [Extent1].[ID_USUARIOEDIT] AS [ID_USUARIOEDIT], 
                    [Extent1].[FECHAREACTIVACION] AS [FECHAREACTIVACION], 
                    [Extent1].[FIANZA] AS [FIANZA], 
                    [Extent1].[Mayacaribe] AS [Mayacaribe], 
                    [Extent1].[UltTaxi] AS [UltTaxi], 
                    [Extent1].[ULTTURNO] AS [ULTTURNO], 
                    [Extent1].[SECRETARIA] AS [SECRETARIA], 
                    [Extent1].[PUESTO] AS [PUESTO], 
                    [Extent1].[PosicionPadron] AS [PosicionPadron], 
                    [Extent1].[postemp] AS [postemp], 
                    [Extent1].[strenta] AS [strenta], 
                    [Extent1].[impacto] AS [impacto]
                    FROM [dbo].[Operadores_Baja_Datos] AS [Extent1]
                    WHERE [Extent1].[CHOFER] = @p__linq__0',N'@p__linq__0 varchar(8000)',@p__linq__0='" + this.partnerReference + @"';";
                List<Operadores_Baja_Datos> _Operadores_Baja_Datos = db.Database.SqlQuery<Operadores_Baja_Datos>(query).ToList();

                Operadores_Baja_Datos Op = _Operadores_Baja_Datos.Where(x => x.datos_regresados == false).FirstOrDefault();
                if (Op != null)
                {
                    // Obtenemos el numero de control 
                    decimal control = Op.control;
                    query = @"exec sp_executesql N'SELECT TOP (1) 
                    [Extent1].[Id_Proveedor] AS [Id_Proveedor], 
                    [Extent1].[Proveedor] AS [Proveedor], 
                    [Extent1].[Direccion] AS [Direccion], 
                    [Extent1].[RFC] AS [RFC], 
                    [Extent1].[Ciudad] AS [Ciudad], 
                    [Extent1].[RefCobranza] AS [RefCobranza], 
                    [Extent1].[RefCompras] AS [RefCompras], 
                    [Extent1].[Telefono] AS [Telefono], 
                    [Extent1].[Obs] AS [Obs], 
                    [Extent1].[Credito] AS [Credito], 
                    [Extent1].[Saldo] AS [Saldo], 
                    [Extent1].[DiasCred] AS [DiasCred], 
                    [Extent1].[FechaPago] AS [FechaPago], 
                    [Extent1].[UltimaComp] AS [UltimaComp], 
                    [Extent1].[CompAnuales] AS [CompAnuales], 
                    [Extent1].[Activo] AS [Activo], 
                    [Extent1].[FechaOp] AS [FechaOp], 
                    [Extent1].[FechaEdit] AS [FechaEdit], 
                    [Extent1].[Id_Usuario] AS [Id_Usuario], 
                    [Extent1].[Id_UsuarioEdit] AS [Id_UsuarioEdit], 
                    [Extent1].[Periocidad] AS [Periocidad], 
                    [Extent1].[Operador] AS [Operador], 
                    [Extent1].[borrar] AS [borrar]
                    FROM [dbo].[Proveedores] AS [Extent1]
                    WHERE [Extent1].[Id_Proveedor] = @p__linq__0',N'@p__linq__0 varchar(8000)',@p__linq__0='" + this.partnerReference + @"'";
                    List<Proveedore> _Proveedores = db.Database.SqlQuery<Proveedore>(query).ToList();
                    query = @"
                exec sp_executesql N'SELECT 
                [GroupBy1].[A1] AS[C1]
                FROM(SELECT
                    COUNT(1) AS[A1]
                    FROM[dbo].[Operadores_Baja_Datos] AS[Extent1]
                    WHERE[Extent1].[CHOFER] = @p__linq__0
                )  AS[GroupBy1]',N'@p__linq__0 varchar(8000)',@p__linq__0='" + this.partnerReference + @"'";
                    List<int> _C1 = db.Database.SqlQuery<int>(query).ToList();

                    query = @"
                exec sp_executesql N'SELECT TOP (1) 
                [Extent1].[control] AS [control], 
                [Extent1].[fecha_insert] AS [fecha_insert], 
                [Extent1].[baja_por_faltas] AS [baja_por_faltas], 
                [Extent1].[datos_regresados] AS [datos_regresados], 
                [Extent1].[fecha_regreso] AS [fecha_regreso], 
                [Extent1].[id_usuario_regreso] AS [id_usuario_regreso], 
                [Extent1].[CHOFER] AS [CHOFER], 
                [Extent1].[NOMBRE] AS [NOMBRE], 
                [Extent1].[APELLIDOS] AS [APELLIDOS], 
                [Extent1].[SOBRENOMB] AS [SOBRENOMB], 
                [Extent1].[SEXO] AS [SEXO], 
                [Extent1].[FECHANAC] AS [FECHANAC], 
                [Extent1].[LUGARNAC] AS [LUGARNAC], 
                [Extent1].[EDOCIVIL] AS [EDOCIVIL], 
                [Extent1].[CREDELECTOR] AS [CREDELECTOR], 
                [Extent1].[SECCION] AS [SECCION], 
                [Extent1].[OCUPACION] AS [OCUPACION], 
                [Extent1].[TIPOSANGRE] AS [TIPOSANGRE], 
                [Extent1].[CARTILLA] AS [CARTILLA], 
                [Extent1].[ALERGIAS] AS [ALERGIAS], 
                [Extent1].[ESCOLARIDAD] AS [ESCOLARIDAD], 
                [Extent1].[CONYUGE] AS [CONYUGE], 
                [Extent1].[TELEFONO] AS [TELEFONO], 
                [Extent1].[FONDDEF] AS [FONDDEF], 
                [Extent1].[STATUS] AS [STATUS], 
                [Extent1].[SOC_ALTA] AS [SOC_ALTA], 
                [Extent1].[FECHAING] AS [FECHAING], 
                [Extent1].[PATRON] AS [PATRON], 
                [Extent1].[DOMICACT] AS [DOMICACT], 
                [Extent1].[CIUDADACT] AS [CIUDADACT], 
                [Extent1].[DOMICANT] AS [DOMICANT], 
                [Extent1].[CIUDADANT] AS [CIUDADANT], 
                [Extent1].[LICENCIA] AS [LICENCIA], 
                [Extent1].[LICVENCE] AS [LICVENCE], 
                [Extent1].[RFC] AS [RFC], 
                [Extent1].[FECHABAJ] AS [FECHABAJ], 
                [Extent1].[OBS] AS [OBS], 
                [Extent1].[COMEN1] AS [COMEN1], 
                [Extent1].[FECHAOP] AS [FECHAOP], 
                [Extent1].[ID_USUARIO] AS [ID_USUARIO], 
                [Extent1].[FECHAEDIT] AS [FECHAEDIT], 
                [Extent1].[ID_USUARIOEDIT] AS [ID_USUARIOEDIT], 
                [Extent1].[FECHAREACTIVACION] AS [FECHAREACTIVACION], 
                [Extent1].[FIANZA] AS [FIANZA], 
                [Extent1].[Mayacaribe] AS [Mayacaribe], 
                [Extent1].[UltTaxi] AS [UltTaxi], 
                [Extent1].[ULTTURNO] AS [ULTTURNO], 
                [Extent1].[SECRETARIA] AS [SECRETARIA], 
                [Extent1].[PUESTO] AS [PUESTO], 
                [Extent1].[PosicionPadron] AS [PosicionPadron], 
                [Extent1].[postemp] AS [postemp], 
                [Extent1].[strenta] AS [strenta], 
                [Extent1].[impacto] AS [impacto]

                FROM [dbo].[Operadores_Baja_Datos] AS [Extent1]
                WHERE ([Extent1].[CHOFER] = @p__linq__0) AND ([Extent1].[control] = @p__linq__1)',N'@p__linq__0 varchar(8000),@p__linq__1 bigint',@p__linq__0='" + this.partnerReference + @"',@p__linq__1=" + control + @";";
                    List<Operadores_Baja_Datos> __Operadores_Baja_Datos = db.Database.SqlQuery<Operadores_Baja_Datos>(query).ToList();
                    try
                    {
                        // Actualizar la tabla de detalles.
                        Operadores_Baja_Datos Operador = __Operadores_Baja_Datos.FirstOrDefault();
                        query = @"
                    exec sp_executesql N'UPDATE [dbo].[Chof_Detalle]
                    SET [NOMBRE] = @0, [APELLIDOS] = @1, [SEXO] = @2, [FECHANAC] = @3, [LUGARNAC] = @4, [TIPOSANGRE] = @5, [TELEFONO] = @6, [PATRON] = @7, [DOMICACT] = @8, [CIUDADACT] = @9, [LICENCIA] = @10, [LICVENCE] = @11
                    WHERE ([CHOFER] = @12)
                    ',N'@0 varchar(90),@1 varchar(255),@2 varchar(2),@3 datetime2(7),@4 varchar(50),@5 varchar(30),@6 varchar(20),@7 varchar(90),@8 varchar(255),@9 varchar(50),@10 varchar(50),@11 datetime2(7),@12 varchar(10)',
                    @0='" + Op.NOMBRE + @"',
                    @1='" + Op.APELLIDOS + @"',
                    @2='" + Op.SEXO + @"',
                    @3='" + Op.FECHANAC?.ToString("yyyy-MM-dd H:mm:ss") + @"',
                    @4='" + Op.LUGARNAC + @"',
                    @5='" + Op.TIPOSANGRE + @"',
                    @6='" + Op.TELEFONO + @"',
                    @7='" + Op.PATRON + @"',
                    @8='" + Op.DOMICACT + @"',
                    @9='" + Op.CIUDADACT + @"',
                    @10='" + Op.LICENCIA + @"',
                    @11='" + Op.LICVENCE?.ToString("yyyy-MM-dd H:mm:ss") + @"',
                    @12='" + this.partnerReference + @"'
                ";
                        // Ejecutar comando.
                        var command = this.db.Database.ExecuteSqlCommand(query);

                        // Actualizar datos de cuando se regresaron los datos de identidad.
                        query = @"
                    exec sp_executesql N'UPDATE [dbo].[Operadores_Baja_Datos]
                    SET [datos_regresados] = @0, [fecha_regreso] = @1, [id_usuario_regreso] = @2
                    WHERE ([control] = @3)
                    ',N'@0 bit,@1 datetime2(7),@2 varchar(6),@3 bigint',
                    @0=1,
                    @1='" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + @"',
                    @2='10-1',
                    @3=" + control + @"";
                        // Ejecutar comando.
                        command = this.db.Database.ExecuteSqlCommand(query);

                        // Actualizar la tabla de proveedores.
                        query = @"exec sp_executesql N'UPDATE [dbo].[Proveedores]
                        SET [Proveedor] = @0
                        WHERE ([Id_Proveedor] = @1)
                        ',N'@0 varchar(90),@1 varchar(10)',
                        @0='" + Op.NOMBRE + " " + Op.APELLIDOS + @"',
                        @1='" + this.partnerReference + @"'";
                        // Ejecutar comando.
                        command = this.db.Database.ExecuteSqlCommand(query);

                        // Actualizamos y activamos al operador en la base de datos.
                        Operator op = this.get(this.partnerReference);
                        op.active = true;
                        op.update();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public OperatorPagination getIfexist(int page = 1, int pagesize = 100, string gafet = null, string Name = null, string lastName1 = null, string lastName2 = null) {
            try
            {
                gafet = gafet == "" || gafet is null ? "null" : "'" + gafet + "'";
                Name = Name == "" || Name is null ? "null" : "'" + Name + "'";
                lastName1 = lastName1 == "" || lastName1 is null ? "null" : "'" + lastName1 + "'";
                lastName2 = lastName2 == "" || lastName2 is null ? "null" : "'" + lastName2 + "'";
                string query = "fdo_get_operators_baja " + page + ", " + pagesize + ", " + gafet + ", " + Name + ", " + lastName1 + ", " + lastName2;
                List<Operator> operators = this.db.Database.SqlQuery<Operator>(query).ToList();
                int NumberOfClients = 0;
                if (operators.Count > 0) NumberOfClients = (int)operators.FirstOrDefault().TotalRows;
                int TotalPages = (int)Math.Ceiling((double)NumberOfClients / pagesize);
                OperatorPagination clientPagination = new OperatorPagination(NumberOfClients, operators, page, pagesize);
                return clientPagination;

            }
            catch (Exception)
            {
                return null;
            }
        }

        // Generar contraseña para usuario
        public RegisterViewModel generatePassword(RegisterViewModel register)
        {
            try
            {
                userlog userDB = db.userlogs.Where(x => x.padronID == register.IdPadron).FirstOrDefault();// Obtenemos el registro del login.
                userlog userE = db.userlogs.Where(x => x.email == register.Email && x.isActive == true).FirstOrDefault();// Obtenemos el registro que tiene el correo registrado.

                if (userE != null && register.Email == userE.email && userE.padronID != register.IdPadron)
                {
                    register.success = false;
                    register.error = "Existe un usuario que hace uso del E-mail, intente con otro correo nuevamente.";
                    return register;
                }
                else if (userDB == null)
                {
                    register.Email = register.Email.Replace(" ", "");
                    register.Password = register.Password.Replace(" ", "");
                    userDB = new userlog()
                    {
                        firstName = register.Name,
                        lastName = register.Lastname,
                        email = register.Email,
                        createdDt = DateTime.UtcNow,
                        lastLoginDt = DateTime.UtcNow,
                        isActive = true,
                        phone = register.PhoneNumber,
                        authyId = this.RandomString(9),
                        userpassword = this.Hash(register.Email + "_" + register.Password),
                        padronID = register.IdPadron
                    };
                    db.userCompanies.Add(new userCompany()
                    {
                        companyId = 1
                    });
                    db.userRoles.Add(new userRole()
                    {
                        roleId = register.roleID
                    });
                    db.userDepartments.Add(new userDepartment()
                    {
                        departmentId = 2
                    });
                    db.userlogs.Add(userDB);
                    db.SaveChanges();
                    register.success = true;
                    register.error = null;
                    return register;
                }
                else if (userDB != null)
                {
                    userDB.email = register.Email;
                    userDB.phone = register.PhoneNumber;
                    userDB.userpassword = this.Hash(register.Email + "_" + register.Password);
                    db.Entry(userDB).State = EntityState.Modified;
                    db.SaveChanges();
                    register.success = true;
                    register.error = null;
                    return register;
                }
                else
                {
                    register.success = false;
                    register.error = "Error desconocido.";
                    return register;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                register.success = false;
                register.error = e.Message;
                return register;
            }
        }

        public userlog userlog()
        {
            try
            {
                // Obtenemos el userlog por medio del padron ID registrado.
                return db.userlogs.Where(x => x.padronID == this.userId).Include(x => x.userRoles.Select(z => x.Role)).FirstOrDefault();// Obtenemos el registro del login. 
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool userlogUpdate(RegisterViewModel user)
        {
            try
            {
                userlog _user = db.userlogs.Where(x => x.userId == user.userId).FirstOrDefault();// Obtenemos el registro del login. 
                _user.email = user.Email;
                _user.phone = user.PhoneNumber;
                _user.password = this.Hash(user.Email + "_" + user.Password);
                db.Entry(_user).State = EntityState.Modified;

                // Buscamos la relacion para actualizar el role.
                userRole urole = db.userRoles.Where(x => x.userId == _user.userId).FirstOrDefault();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public List<Inventario> getinventario(int family_id)
        {
            try
            {
                // Obtener todas las couotas sindicales 
                string query = @"SELECT  * FROM  inventario WHERE Id_Familia = '" + family_id + "' AND id_proveedor = '" + this.partnerReference + "' AND exist > 0 --CUOTAS SINDICALES";
                List<Inventario> _inventarios = db.Database.SqlQuery<Inventario>(query).ToList();
                return _inventarios;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public bool canclearfaltas(int cantidad, string movimiento, string autoriza = "SIS", string EditBy = "SIS")
        {
            try
            {
                db.Database.ExecuteSqlCommand(
                "EXEC dbo.sp_CobrarFaltas @p0, @p1, @p2, @p3",
                this.partnerReference,
                cantidad,
                movimiento,
                autoriza);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}