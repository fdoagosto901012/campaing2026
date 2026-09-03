using Microsoft.Win32;
using radiotaxi.Model.v2;
using radiotaxi.Model.v2.finance;
using radiotaxi.Model.v2.General;
using radiotaxi.Model.v2.Partner;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace radiotaxi.Model
{
    public partial class Partner : Soc_DatPersonales
    {
        public int userId { get; set; }
        public string urlImage { get; set; }
        public string bucket { get; set; }
        public string key { get; set; }
        public string curp { get; set; }
        public string ine { get; set; }
        public bool? organDonor { get; set; }
        public string bloodType { get; set; }
        public string bloodTypeId { get; set; }
        public string partnerReference { get; set; }
        public string firstName { get; set; }
        public string lastNameF { get; set; }
        public string lastNameM { get; set; }
        public DateTime createdDt { get; set; }
        public int? partnerTypeId { get; set; }
        public string birthPlace { get; set; }
        public int? sex { get; set; }
        public DateTime birthDate { get; set; }
        public int? userTypeId { get; set; }
        public DateTime? dtLastUpdate { get; set; }
        public int? relationShipStatusId { get; set; }
        public int? sectionId { get; set; }
        public bool? cafecude { get; set; }
        public bool? at { get; set; }
        public bool? active { get; set; }
        public string reason { get; set; }
        public string createdBy { get; set; }
        public string editBy { get; set; }
        public int? meeting { get; set; }
        public bool? death { get; set; }
        public bool? enemy { get; set; }
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
        public string relationship { get; set; }
        public string relationShipStatus { get; set; }
        public string allergies { get; set; }
        public string realtionship { get; set; }
        public string openpayClientID { get; set; }

        public ICollection<userAddress> userAddresses { get; set; }
        public ICollection<email> emails { get; set; }
        public ICollection<userPhone> userPhones { get; set; }
        public ICollection<userHealthInformation> userHealthInformation { get; set; }
        public ICollection<amazonPicture> amazonPictures { get; set; }

        public Partner() { 
        
        }

        public Partner get(string gafet)
        {
            try
            {
               int Numero = Int32.Parse(gafet);
                // Comprobar si ese elemento existe en la tabla users (Gente pendeja que no captura bien)
                user User = db.users.Where(x => x.partnerReference == gafet && x.active == true).FirstOrDefault();
                // Comprovar si existe el usuario en chof detalle.
                Soc_DatPersonales Soc = db.Soc_DatPersonales.Where(x => x.Numero == Numero).FirstOrDefault();
                if (Soc != null && User != null)
                {

                }
                else if (Soc != null && User == null)
                {
                    // Aqui creamos al operador con los datos de chofDatPersonal
                    User = new user();
                    // Rellenamos el objeto user para el funcionamiento extendido de practicontrol 
                    User.active = true;
                    User.at = false;
                    User.birthDate = Soc.fechanacimiento;
                    User.birthPlace = Soc.LugarNac;
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
                    User.firstName = Soc.Nombre.ToUpper();
                    User.googleplus = "";
                    User.ine = "";
                    User.instagram = "";
                    User.lastNameF = Soc.Materno.ToUpper();
                    User.lastNameM = Soc.Paterno.ToUpper();
                    User.Message = "";
                    User.partnerReference = Soc.Numero.ToString();
                    User.partnerTypeId = 1; // Definimos que es un operador (Evitar errores de capa 8 con archivo)
                    User.payroll = false;
                    User.reason = "";
                    User.relationShipStatusId = 1;
                    User.sex = Soc.Sexo == "M" ? 1 : 0;
                    User.statusId = 1;
                    User.userTypeId = 1;
                    User.rt = false;
                    User.death = false;
                    User.enemy = false;
                    User.payroll = false;
                    User.ttesoc = false;
                    User.openpayClientID = "";
                    this.db.users.Add(User);
                    this.db.SaveChanges();
                }

                string query = "fdo_get_Partner " + gafet.ToString();
                Partner _Partner = this.db.Database.SqlQuery<Partner>(query).ToList().FirstOrDefault();
                if (_Partner == null) return null;
                _Partner.organDonor = _Partner.organDonor == null ? false : true;
                _Partner.userAddresses = db.userAddresses.Where(x => x.userId == _Partner.userId && x.active == true)
                    .Include(x => x.state)
                    .Include(x => x.city)
                    .ToList();
                _Partner.emails = db.emails.Where(x => x.userId == _Partner.userId && x.active == true).ToList();
                _Partner.userPhones = db.userPhones.Where(x => x.userId == _Partner.userId && x.active == true).ToList();
                _Partner.userHealthInformation = db.userHealthInformations.Where(x => x.userId == _Partner.userId).ToList();
                _Partner.amazonPictures = db.amazonPictures.Where(x => x.userId == _Partner.userId).ToList();
                _Partner.allergies = _Partner.userHealthInformation != null && _Partner.userHealthInformation.Count() > 0 ? _Partner.userHealthInformation.FirstOrDefault().allergies : "";
                return _Partner;
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

        public PartnerPagination get(int page = 1, int pagesize = 100, string gafet = null, string Name = null, string lastName1 = null, string lastName2 = null)
        {
            try
            {
                gafet = gafet == "" || gafet is null ? "null" : "'" + gafet + "'";
                Name = Name == "" || Name is null ? "null" : "'" + Name + "'";
                lastName1 = lastName1 == "" || lastName1 is null ? "null" : "'" + lastName1 + "'";
                lastName2 = lastName2 == "" || lastName2 is null ? "null" : "'" + lastName2 + "'";
                string query = "fdo_get_Partners " + page + ", " + pagesize + ", " + gafet + ", " + Name + ", " + lastName1 + ", " + lastName2;
                List<Partner> Partners = this.db.Database.SqlQuery<Partner>(query).ToList();
                int NumberOfClients = 0;
                if (Partners.Count > 0) NumberOfClients = (int)Partners.FirstOrDefault().TotalRows;
                int TotalPages = (int)Math.Ceiling((double)NumberOfClients / pagesize);
                PartnerPagination clientPagination = new PartnerPagination(NumberOfClients, Partners, page, pagesize);
                return clientPagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public PartnerPagination getempla(int page = 1, int pagesize = 100, string gafet = null, string Name = null, string lastName1 = null, string lastName2 = null, string serie = null, string placas = null, string EconoResp = null, string Responsable = null)
        {
            try
            {
                gafet = gafet == "" || gafet is null ? "null" : "'" + gafet + "'";
                Name = Name == "" || Name is null ? "null" : "'" + Name + "'";
                lastName1 = lastName1 == "" || lastName1 is null ? "null" : "'" + lastName1 + "'";
                lastName2 = lastName2 == "" || lastName2 is null ? "null" : "'" + lastName2 + "'";
                serie = serie == "" || serie is null ? "null" : "'" + serie + "'";
                placas = placas == "" || placas is null ? "null" : "'" + placas + "'";
                EconoResp = EconoResp == "" || EconoResp is null ? "null" : "'" + EconoResp + "'";
                Responsable = Responsable == "" || Responsable is null ? "null" : "'" + Responsable + "'";
                string query = "fdo_get_PartnersEmpla " + page + ", " + pagesize + ", " + gafet + ", " + Name + ", " + lastName1 + ", " + lastName2 + ", " + serie + ", " + placas + ", " + EconoResp + ", " + Responsable; ;
                List<Partner> Partners = this.db.Database.SqlQuery<Partner>(query).ToList();
                int NumberOfClients = 0;
                if (Partners.Count > 0) NumberOfClients = (int)Partners.FirstOrDefault().TotalRows;
                int TotalPages = (int)Math.Ceiling((double)NumberOfClients / pagesize);
                PartnerPagination clientPagination = new PartnerPagination(NumberOfClients, Partners, page, pagesize);
                return clientPagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
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
                string query = "fdo_fdi_reporte_soc '" + start?.ToString("yyyy-MM-dd") + "', '" + end?.ToString("yyyy-MM-dd") + "'";
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

        public Partner save(){ // Partner ES UN OBJETO COMPUESTO POR CHOF_DETALLE Y USER
            try
            {
                String query = "";
                String sexo = this.sex == 1 ? "M" : "F";
                String date = DateTime.Now.ToString();
                string _relatationShip = "S";
                String dateFortmat = DateTime.Now.ToString("yyyyMMdd");
                string _phone = "";
                string city_name = "";
                string state_name = "";
                // Buscamos al operador anterior si existe para deshabilitarlo
                List<user> Users = db.users.Where(x => x.partnerReference == this.partnerReference && x.active == true).ToList<user>();
                foreach (var item in Users)
                {
                    item.active = false;
                    item.openpayClientID = "";
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    List<userlog> logs = db.userlogs.Where(x => x.padronID == item.id).ToList();
                    foreach (var log in logs)
                    {
                        log.isActive = false;
                        db.Entry(log).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                // DAR DE ALTA EN PRADON.
                switch (this.relationShipStatusId)
                {
                    case 1:
                        _relatationShip = "S";
                        break;
                    case 2:
                        _relatationShip = "C";
                        break;
                    case 3:
                        _relatationShip = "D";
                        break;
                    default:
                        _relatationShip = "U";
                        break;
                }

                // DAR DE ALTA EN PADRON
                // Rellenamos el objeto user para el funcionamiento extendido de practicontrol 
                
                user User = new user();
                User.active = true;
                User.at = this.at;
                User.birthDate = this.birthDate;
                User.birthPlace = this.birthPlace.ToUpper();
                User.cafecude = this.cafecude;
                User.candidateHardvoteId = null;
                User.candidateId = null;
                User.candidateQuizID = 0;
                User.comments = this.comments;
                User.createdBy = this.createdBy;
                User.createdDt = this.createdDt;
                User.curp = this.curp.ToUpper();
                User.death = this.death;
                User.dtLastUpdate = this.dtLastUpdate;
                User.editBy = this.editBy;
                User.rfc = this.urfc.ToUpper();
                User.facebook = this.facebook;
                User.firstName = this.firstName.ToUpper();
                User.googleplus = this.googleplus;
                User.ine = this.ine.ToUpper();
                User.instagram = this.instagram;
                User.lastNameF = this.lastNameF.ToUpper();
                User.lastNameM = this.lastNameM.ToUpper();
                User.Message = this.Message;
                User.partnerReference = this.partnerReference.ToUpper();
                User.partnerTypeId = 1; // Definimos que es un operador (Evitar errores de capa 8 con archivo)
                User.payroll = this.payroll;
                User.reason = this.reason;
                User.relationShipStatusId = (int)this.relationShipStatusId;
                User.sex = (int)this.sex;
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
                User.openpayClientID = "";

                // Agregamos la nueva telefonos.
                userPhone phone = new userPhone();
                phone.userId = this.userId;
                phone.phone = this.userPhones.FirstOrDefault().phone;
                phone.phoneTypeId = 1;
                phone.active = true;
                phone.isValid = true;
                User.userPhones.Add(phone);
                _phone = phone.phone;

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
                User.userAddresses.Add(address);

                // Agregamos la nueva direccion.
                email email = new email();
                email.userId = this.userId;
                email.email1 = this.emails.FirstOrDefault().email1;
                email.emailPriorityTypeId = 1;
                email.active = true;
                User.emails.Add(email);

                // Informacion de salud
                // Obtenemos todos los tipos de sangre.
                userBloodType BloodType = db.userBloodTypes.Where(x=> x.name == this.TipoSangre).FirstOrDefault();
                User.userHealthInformation = new userHealthInformation();
                User.userHealthInformation.organDonor = this.organDonor != null ? true : false;
                User.userHealthInformation.allergies = this.allergies;
                if (BloodType != null)
                {
                    User.userHealthInformation.bloodTypeID = BloodType.id;
                }
                var city = db.cities.Where(x => x.id == address.cityId).FirstOrDefault();
                city_name = city.name;
                state_name = db.states.Where(x => x.id == city.stateId).FirstOrDefault().name;
                String birthDate = ((DateTime)User.birthDate).ToString("yyyyMMdd");
                
                try
                {
                    // Verificamos si existe en la tabla de socDatPersonal.
                    // Dar de alta socio
                    int ECO = 0;
                    int rowAffected = 0;
                    Int32.TryParse(this.partnerReference, out ECO);
                    Soc_DatPersonales soc_DatPersonales = db.Soc_DatPersonales.Where(x => x.Numero == ECO).FirstOrDefault();
                    if (soc_DatPersonales != null) // Existe el usuario y hay que actualizarlo.
                    {
                        query = @"EXECUTE AJR_ACTDATSOCIO " + User.partnerReference + ",'" + User.firstName + "','" + User.lastNameF + "','" + User.lastNameM + "','" + address.address + "','" + address.supermanzana + "','" + address.manzana + "','" + address.lote + "','" + address.street + "','" + address.interiorNumber + "','" + city_name + "','N/A','" + state_name + "','" + _phone + "','" + _phone + "','" + email.email1 + "','" + birthDate + "','" + dateFortmat + "','" + sexo + "'";
                    }
                    else {
                        query = @"EXECUTE AJR_ALTASOCIOJR " + User.partnerReference + ",'" + User.firstName + "','" + User.lastNameF + "','" + User.lastNameM + "','" + address.address + "','" + address.supermanzana + "','" + address.manzana + "','" + address.lote + "','" + address.street + "','" + address.interiorNumber + "','" + city_name + "','N/A','" + state_name + "','" + _phone + "','" + _phone + "','" + email.email1 + "','" + birthDate + "','" + dateFortmat + "','" + sexo + "'";
                    }
                    rowAffected = this.db.Database.ExecuteSqlCommand(query);
                    //Actualizamos la base de datos.
                    db.users.Add(User);
                    db.SaveChanges();
                    Console.WriteLine("");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                // Actualizamos datos a retornar.
                this.userId = User.id;
                // Preparamos el objeto para regresarlo al controlador 
                this.amazonPictures = User.amazonPictures;
                this.userAddresses = User.userAddresses;
                this.emails = User.emails;
                this.userPhones = User.userPhones;
                return this;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return null;
        }

        public Partner update()
        {
            try
            {
                string query = "fdo_get_Partner " + this.partnerReference;
                Partner partner = this.db.Database.SqlQuery<Partner>(query).ToList().FirstOrDefault();
                string _phone = "";
                
                // ACTUALIZAMOS PADRON
                user User = this.db.users
                    .Include(x => x.userAddresses)
                    .Include(x => x.userPhones)
                    .Include(x => x.emails)
                    .Include(x => x.amazonPictures)
                    .Include(x => x.userHealthInformation)
                    .Where(x => x.id == partner.userId)
                    .FirstOrDefault();

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
                User.createdDt = this.createdDt;
                User.curp = this.curp;
                User.death = this.death;
                User.dtLastUpdate = this.dtLastUpdate;
                User.editBy = this.editBy;
                User.rfc = this.urfc;
                User.partnerTypeId = 1;
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
                User.payroll = this.payroll;
                User.reason = this.reason;
                User.relationShipStatusId = (int)this.relationShipStatusId;
                User.sex = (int)this.sex;
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

                // Telefono
                List<userPhone> phones = db.userPhones.Where(x => x.userId == this.userId).ToList();
                foreach (var phone in phones)
                {
                    foreach (var nphones in this.userPhones)
                    {
                        if (nphones.id == phone.id)
                        {
                            _phone = nphones.phone;
                            phone.phone = nphones.phone;
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
                    // Agregamos la nueva telefonos.
                    userPhone phone = new userPhone();
                    phone.userId = this.userId;
                    phone.phone = this.userPhones.FirstOrDefault().phone;
                    phone.phoneTypeId = 1;
                    phone.active = true;
                    phone.isValid = true;
                    db.userPhones.Add(phone);
                    _phone = phone.phone;
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

                // actualizamos la informacion de salud.
                userBloodType BloodType = db.userBloodTypes.Where(x => x.name == this.TipoSangre).FirstOrDefault();
                if (User.userHealthInformation == null)
                {
                    User.userHealthInformation = new userHealthInformation();   
                }
                User.userHealthInformation.organDonor = this.organDonor != null ? true : false;
                User.userHealthInformation.allergies = this.allergies;
                if (BloodType != null)
                {
                    User.userHealthInformation.bloodTypeID = BloodType.id;
                }
                db.Entry(User).State = EntityState.Modified;
                this.db.SaveChanges();

                // ACTUALIZACION DE PRACTICONTROL
                string sex = this.sex == 1 ? "M" : "F";
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

                query = @"
                BEGIN TRAN;
                exec sp_executesql N' 
	                UPDATE [dbo].Soc_DatPersonales 
	                SET
	                [NOMBRE] =  @NOMBRE,
	                [Paterno] =  @Paterno,
	                [Materno] =  @Materno,
	                [SEXO] =  @SEXO,
	                [fechanacimiento] =  @FECHANAC,
	                [LUGARNAC] =  @LUGARNAC,
	                [EDOCIVIL] =  @EDOCIVIL,
	                [TIPOSANGRE] =  @TIPOSANGRE,
	                [TELEFONO] =  @TELEFONO,
	                [fechaingreso] =  @FECHAING
	                WHERE [Numero] = @CHOFER;',
	                N'
	                @NOMBRE varchar(255), 
	                @Paterno varchar(255),
	                @Materno varchar(255),
	                @SEXO varchar(255),
	                @FECHANAC varchar(255),
	                @LUGARNAC varchar(255),
	                @EDOCIVIL varchar(255),
	                @TIPOSANGRE varchar(255),
	                @TELEFONO varchar(255),
	                @FECHAING varchar(255),
	                @CHOFER varchar(255)
	                ',
	                '" + User.firstName + @"',
	                '" + User.lastNameF + @"',
	                '" + User.lastNameM + @"',
                    '" + sex + @"',
	                '" + this.birthDate.ToString("yyyyMMdd") + @"',
	                '" + this.birthPlace + @"',
	                '" + relation + @"',
	                 '" + this.TipoSangre + @"',
	                '"+ _phone + @"',
	                '" + this.createdDt.ToString("yyyyMMdd") + @"',
	                " + this.partnerReference + @"; 
                COMMIT TRAN;
                ";
                Console.WriteLine(query);
                var a = this.db.Database.ExecuteSqlCommand(query);

                // Cambiar nombre en otras tablas
                query = @"
                
                DECLARE @gafete varchar(12)
                DECLARE @nombre varchar(200)
                DECLARE @apellidos varchar(200)
                SET @gafete = '" + this.partnerReference + @"'
                SET @nombre = '" + this.firstName + @"'
                SET @apellidos = '" + this.lastNameF + " " + this.lastNameM + @"'
                BEGIN TRANSACTION
                UPDATE Proveedores SET Proveedores.Proveedor = @nombre + ' ' + @apellidos
                WHERE Proveedores.Id_Proveedor = @gafete
                COMMIT TRANSACTION";
                a = this.db.Database.ExecuteSqlCommand(query);

                // Preparamos el objeto para regresarlo al controlador
                this.amazonPictures = User.amazonPictures;
                this.userAddresses = User.userAddresses;
                this.emails = User.emails;
                this.userPhones = User.userPhones;
                return this;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // quita las faltas de socios 
        public bool RemoveReports(string eco, string _user){
            try
            {
                string query = "EXECUTE AJR_UTILSOCIOAL100 '"+ eco +"','" + DateTime.Now.ToString("yyyyMMdd") + "','" + _user + "'";
                int a = this.db.Database.ExecuteSqlCommand(query);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        Partner delete()
        {
            return null;
        }

        // Obtiene los ultimos 365 dias de reportes.
        public List<HistAsistencia> reportes()
        {
            try
            {
                using (var db = new radiotaxiEntities())
                {
                    string query = @"
                    DECLARE @gafet AS VARCHAR(100);
                    SET @gafet = '" + this.partnerReference + @"';                
                    with reportes as ( -- Reportes contabilizando si son dobles o simples
	                    SELECT top 365 Gafete, Taxi, FechaReporte, FechaOp, count(-1) as r
	                    FROM HistAsistencias
	                    WHERE (Gafete = @gafet)
	                    group by Gafete, Taxi, FechaReporte, FechaOp
	                    ORDER BY FechaReporte DESC
                    ), reportes_u as ( -- Reportes simples si son vespertinos o maturinos
	                    SELECT top 365 Gafete, Taxi, FechaReporte, FechaOp, count(-1) as r
	                    FROM HistAsistencias
	                    WHERE (Gafete = @gafet)
	                    group by Gafete, Taxi, FechaReporte, FechaOp
	                    having count (-1) < 2
	                    ORDER BY FechaReporte DESC
                    ), reportes_a as (
	                    SELECT top 1000 Gafete, Tipo, Taxi, Turno, FechaReporte, Concepto, Ticket, FechaOp
	                    FROM HistAsistencias
	                    WHERE (Gafete = @gafet)
	                    ORDER BY FechaReporte DESC
                    ) select r.Gafete, r.Taxi, r.FechaReporte, r.FechaOp, a.Ticket, 'na' as Tipo,'na' as Concepto,
                    CASE 
                        WHEN Turno is null THEN 'Completo'
                        WHEN Turno = 'F' THEN 'Falta'
                        WHEN Turno = 'M' THEN 'Matutino'
	                    WHEN Turno = 'V' THEN 'Vespertino'
                        ELSE 'NA'
                    END as Turno
                    from reportes as r 
                    left join reportes_a as a on a.FechaReporte = r.FechaReporte and r.r = 1
                    ORDER BY r.FechaReporte DESC;
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

        public List<pay> payCards()
        {
            try
            {
                using (var db = new radiotaxiEntities()) {

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
                return null;
            }
        }

            public List<Convenio> agreements(){
            
                using (var db = new radiotaxiEntities()) {
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

        public List<Inventario> removeinventario(int count)
        {
            try
            {
                // Obtener todas las couotas sindicales 

                string query = @"SELECT  * FROM  inventario WHERE Id_Familia = '12' AND id_proveedor = '" + this.partnerReference + "' AND exist =1 --CUOTAS SINDICALES";
                List<Inventario> _inventarios = db.Database.SqlQuery<Inventario>(query).ToList();
                //List<Inventario> _inventario = db.Database.SqlQuery<Inventario>(query).ToList();
                return _inventarios;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public List<Inventario> getinventario(int family_id)
        {
            try
            {
                // Obtener todas las couotas sindicales 
                string query = @"SELECT  * FROM  inventario WHERE Id_Familia = '" + family_id + "' AND id_proveedor = '" + this.partnerReference + "' AND exist =1 --CUOTAS SINDICALES";
                List<Inventario> _inventarios = db.Database.SqlQuery<Inventario>(query).ToList();
                return _inventarios;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<Inventario> cancelarcuotas(int cantidad,string movimiento, string autoriza = "SIS", string EditBy = "SIS")
        {
            try
            {
                List<Inventario> _inventarios = this.db.Inventarios.Where(x => x.Id_Familia == "12" && x.Id_Proveedor == this.partnerReference && x.Exist == 1).OrderBy(x => x.FechaOp).Take(cantidad).ToList();
                foreach (var inventario in _inventarios)
                {
                    inventario.Exist = 0; 
                    inventario.F_CubiertoHasta = DateTime.Now;
                    inventario.Id_UsuarioEdit = autoriza;
                    inventario.Color = movimiento;
                    inventario.EditBy = EditBy;
                    // Actualizamos en la base de datos.
                    db.Entry(inventario).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return _inventarios;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public financesOperator financesPartner()
        {
            try
            {
                using (var db = new radiotaxiEntities()) {
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
                    WHERE Id_Proveedor = @gafet AND Exist > 0
                    ";
                    financesOperator _finance = this.db.Database.SqlQuery<financesOperator>(query).ToList().FirstOrDefault();
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

        public List<Venta> sales()
        {
            try
            {
                using (var db = new radiotaxiEntities()) { 
                    return db.Ventas.Where(x => x.Id_Vendedor == this.partnerReference || x.Id_Cliente == this.partnerReference).OrderByDescending(x => x.FechaOp).Take(100).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<MENSAJE> message()
        {

            string query = "select * from mensajes where taxi='4662' or (chofer='4662' and economico='E') ORDER BY ECONOMICO,DETENER DESC -- Mensajes ";
            return null;
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

        public List<p_agreements> convenios()
        {
            try
            {
                string query = @"
                    
                    DECLARE @gafete AS VARCHAR(15);
                    SET @gafete='" + this.partnerReference + @"';

                    SELECT c.Id_Op, c.Concepto,c.CargoANum,c.CargoANombre,cd.Status,COUNT(1) AS partidas,SUM(cd.Importe) AS importe FROM dbo.Convenios c
                    INNER JOIN dbo.ConveniosDetalle cd ON cd.Id_Op = c.Id_Op
                    WHERE c.CargoANum=@gafete
                    AND cd.Status IN ('COBRADO','VIGENTE','PAGADO')
                    GROUP BY c.Id_Op,c.Id_Concepto,c.Concepto,c.CargoANum,c.CargoANombre,cd.Status
                    ORDER BY c.Id_Op,cd.Status";
                List<p_agreements> conv = db.Database.SqlQuery<p_agreements>(query).ToList();
                return conv;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Generar contraseña para usuario
        public RegisterViewModel generatePassword(RegisterViewModel register) {
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
                else {
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

        public userlog userlog() {
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

        public async Task<bool> cien(string autoriza = "SIS", string EditBy = "SIS")
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Database.ExecuteSqlCommand(
                        "EXECUTE AJR_UTILSOCIOAL100 @Socio, @Fecha, @Usuario",
                        new SqlParameter("@Socio", this.partnerReference),
                        new SqlParameter("@Fecha", DateTime.Now.AddDays(-1).ToString("yyyyMMdd")),
                        new SqlParameter("@Usuario", autoriza)
                    );
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

    }
}
