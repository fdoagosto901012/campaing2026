using Newtonsoft.Json;
using radiotaxi.Model.v2;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class userlog : baseModel
    {
        public bool alreadyExisted { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        private userlog User { get; set; }
        public bool isLoggedIn { get; set; }
        public bool isError { get; set; }
        public string ErrorMessage { get; set; }
        public bool isPasswordValid { get; set; }
        public string Role { get; set; }
        public int TotalRows { get; set; }
        public String RoleName { get; set; }
        public String partnerReference { get; set; }
        public int RoleID { get; set; }
        public bool removePassword { get; set; } 
        public string id_secretaria { get; set; }

        public userlog log()
        {
            try
            {
                password = this.Hash(this.username + "_" + this.password);
                this.User = this.db.userlogs
                    .Where(x => x.userpassword == password && x.isActive == true)
                    .Include(x => x.userCompanies.Select(c => c.company))
                    .Include(x => x.userRoles.Select(r => r.role))
                    .FirstOrDefault();

                if (this.User is null)
                {
                    return null;
                }
                
                // Actualizamos la ultima vez que realizo el login.
                this.User.lastLoginDt = DateTime.Now;
                db.Entry(this.User).State = EntityState.Modified;
                db.SaveChanges();
                return this.User;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool log(string username, string password)
        {
            try
            {
                password = this.Hash(username + "_" + password);
                this.User = this.db.userlogs
                    .Where(x => x.userpassword == password && x.isActive == true)
                    .Include(x => x.userCompanies.Select(c => c.company))
                    .Include(x => x.userRoles.Select(r => r.role))
                    //.Include(x => x.userDepartments.Select(d => d.de))
                    .FirstOrDefault();
                if (this.User is null)
                {
                    return isLoggedIn = false;
                }
                this.userId = this.User.userId;
                this.firstName = this.User.firstName;
                this.lastName = this.User.lastName;
                this.email = this.User.email;
                this.Role = this.User.userRoles.ToList().FirstOrDefault().role.name;
                this.id_secretaria = this.User.userRoles.ToList().FirstOrDefault().role.id_secretaria;
                this.authyId = this.User.authyId;
                this.padronID = this.User.padronID;
                this.phone = this.User.phone;
                this.IP = this.User.IP; // Se asignala ip a session
                // Obtenemos los ultimos datos de session.
                return isLoggedIn = true;
            }
            catch (Exception ex)
            {
                return isLoggedIn = false;
            }
        }
        public bool register()
        {
            try
            {
                string AuthyID = this.RandomString(9);

             
                this.userCompanies.Add(new userCompany()
                {
                    companyId = 1
                });
                this.userRoles.Add(new userRole()
                {
                    roleId = 50
                });
                this.userDepartments.Add(new userDepartment()
                {
                    departmentId = 2
                });
                db.SaveChanges();
                this.StatusAction = "create";
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
        public bool save()
        {
            try
            {
                userlog userDB = db.userlogs.Where(x => x.email == this.email).FirstOrDefault();
                string AuthyID = this.RandomString(9);
                if (userDB == null)
                {
                    this.email = this.email.Replace(" ", "");
                    this.password = this.password.Replace(" ", "");
                    this.password = this.Hash(this.email + "_" + this.password);
                    this.userpassword = this.password;
                    this.authyId = AuthyID;
                    this.userCompanies.Add(new userCompany()
                    {
                        companyId = 1
                    });
                    this.userRoles.Add(new userRole()
                    {
                        roleId = this.RoleID
                    });
                    this.userDepartments.Add(new userDepartment()
                    {
                        departmentId = 1
                    });
                    db.userlogs.Add(this);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
        public UserLogPagination get(int page = 1, int pagesize = 100, string gafet = null, string Name = null, string lastName1 = null)
        {
            gafet = gafet == "" || gafet is null ? "null" : "'" + gafet + "'";
            Name = Name == "" || Name is null ? "null" : "'" + Name + "'";
            lastName1 = lastName1 == "" || lastName1 is null ? "null" : "'" + lastName1 + "'";
            string query = "fdo_get_users " + page + ", " + pagesize + ", " + gafet + ", " + Name + ", " + lastName1;
            List<userlogDTO> Users = this.db.Database.SqlQuery<userlogDTO>(query).ToList();
            int NumberOfClients = 0;
            if (Users.Count > 0) NumberOfClients = (int)Users.FirstOrDefault().TotalRows;
            int TotalPages = (int)Math.Ceiling((double)NumberOfClients / pagesize);
            UserLogPagination userLogPagination = new UserLogPagination(NumberOfClients, Users, page, pagesize);
            return userLogPagination;
        }
        public List<userlog> get()
        {
            try
            {
                return this.db.userlogs.ToList(); // Obtenemos todos los tarjetones.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public List<userlog> getAutorize() { 
            return null;
        }

        public userlog get(int id) {
            try
            {
                userlog User = db.userlogs.Where(x => x.userId == id).Include(x => x.userRoles).FirstOrDefault();
                return User;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public bool update() {
            try
            {
                if (this.removePassword) // Si dice que tiene que actualizar el password lo actualiza.
                {
                    this.email = this.email.Replace(" ", ""); // Limpiamos email
                    this.password = this.password.Replace(" ", ""); // Limpiamos contraseña
                    this.userpassword = this.Hash(this.email + "_" + this.password);
                }
                // ACTIALIZAR EL ROLE
                role _role = new role().get(this.RoleID);
                userRole userRole = new userRole()
                {
                    roleId = this.RoleID,
                    userId = this.userId
                };
                db.userlogs.Attach(this);
                foreach (userRole item in db.userRoles.Where(x => x.userId == this.userId).ToList())
                {
                    db.userRoles.Remove(item);
                }
                db.userRoles.Add(userRole);
                db.Entry(this).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);
                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex_) {
                Console.WriteLine(ex_.Message);
                return false;
            }
        }
    
    }
}