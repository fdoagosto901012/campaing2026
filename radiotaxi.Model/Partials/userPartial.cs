using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class UserPartial : baseModel
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

        public userlog log()
        {
            try
            {
                password = this.Hash(this.username + "_" + this.password);
                this.User = this.db.userlogs
                    .Where(x => x.userpassword == password && x.isActive == true)
                    .Include(x => x.userCompanies.Select(c => c.company))
                    .Include(x => x.userRoles.Select(r => r.role))
                    //.Include(x => x.userDepartments.Select(d => d))
                    .FirstOrDefault();

                if (this.User is null)
                {
                    return null;
                }
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
                this.authyId = this.User.authyId;
                this.padronID = this.User.padronID;
                this.phone = this.User.phone;
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
                userlog userDB = new userlog()
                {
                    firstName = "Developer",
                    lastName = "User",
                    email = this.username,
                    createdDt = DateTime.UtcNow,
                    lastLoginDt = DateTime.UtcNow,
                    isActive = true,
                    phone = "0000000000",
                    authyId = AuthyID,
                    userpassword = this.Hash(this.username + "_" + this.password),
                    alreadyExisted = false,
                    padronID = 0
                };
                userDB.userCompanies.Add(new userCompany()
                {
                    companyId = 1
                });
                userDB.userRoles.Add(new userRole()
                {
                    roleId = 50
                });
                userDB.userDepartments.Add(new userDepartment()
                {
                    departmentId = 2
                });
                db.userlogs.Add(userDB);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
    }
}