using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace radiotaxi.API.Helpers
{
    public class Functions
    {
        /*
        private static AdmonEntities AdmonEntities
        {
            get
            {
                return new AdmonEntities();
            }
        }

        private const string _salt = "PVI04893WCd98237Fqwh";
        private static Random random = new Random();

        //Get the encrypted password. Used to compare the provided password with the one storaged in the bd.
        public static string Hash(string valueToHash)
        {
            var computedHash = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(valueToHash + _salt));
            return Convert.ToBase64String(computedHash);
        }

        //Get the user information
        public static user GetUser(string email, string password)
        {
            try
            {

                
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public static user GetAuthyUser(string authy)
        {
            return AdmonEntities.users.Where(x => x.authyId == authy).FirstOrDefault();
        }
        //Get the info to get access to the resource server
        public static audience GetAudience(string clientId)
        {

            try
            {
                var audience = AdmonEntities.audiences.AsQueryable();
                audience = audience.Where(a => a.clientid == clientId);
                return audience.FirstOrDefault();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        public static string GetCompany(string email)
        {
            AdmonEntities db = new AdmonEntities();
            db.Configuration.LazyLoadingEnabled = true;
            db.Configuration.ProxyCreationEnabled = false;

            roleDTO info = (from g in db.users
                            join m in db.userRoles on g.userId equals m.userId
                            join l in db.roles on m.roleId equals l.id
                            join uc in db.userCompanies on m.userId equals uc.userId
                            join c in db.companies on uc.companyId equals c.id
                            where g.email == email
                            select new roleDTO
                            {
                                id = l.id,
                                role = l.name,
                                companyid = c.id,
                                company = c.name
                            }).FirstOrDefault();

            if (info != null)
            {
                return info.company;
            }
            else
            {
                return "Ninguna";
            }

        }

        public static string GetDepartment(string email)
        {
            AdmonEntities db = new AdmonEntities();
            db.Configuration.LazyLoadingEnabled = true;
            db.Configuration.ProxyCreationEnabled = false;

            roleDTO info = (from g in db.users
                            join ud in db.userDepartments on g.userId equals ud.userId
                            join d in db.departments on ud.departmentId equals d.id
                            where g.email == email
                            select new roleDTO
                            {
                                departmentid = d.id,
                                department = d.name
                            }).FirstOrDefault();

            if (info != null)
            {
                return info.department;
            }
            else
            {
                return "Ninguno";
            }

        }

        public static int GetDepartmentId(string email)
        {
            AdmonEntities db = new AdmonEntities();
            db.Configuration.LazyLoadingEnabled = true;
            db.Configuration.ProxyCreationEnabled = false;

            roleDTO info = (from g in db.users
                            join ud in db.userDepartments on g.userId equals ud.userId
                            join d in db.departments on ud.departmentId equals d.id
                            where g.email == email
                            select new roleDTO
                            {
                                departmentid = d.id,
                                department = d.name
                            }).FirstOrDefault();

            if (info != null)
            {
                return info.departmentid;
            }
            else
            {
                return 0;
            }

        }

        public static string GetRole(string email)
        {
            AdmonEntities db = new AdmonEntities();
            db.Configuration.LazyLoadingEnabled = true;
            db.Configuration.ProxyCreationEnabled = false;

            roleDTO info = (from g in db.users
                            join m in db.userRoles on g.userId equals m.userId
                            join l in db.roles on m.roleId equals l.id
                            join uc in db.userCompanies on m.userId equals uc.userId
                            join c in db.companies on uc.companyId equals c.id
                            where g.email == email
                            select new roleDTO
                            {
                                id = l.id,
                                role = l.name,
                                companyid = c.id,
                                company = c.name
                            }).FirstOrDefault();

            if (info != null)
            {
                return info.role;
            }
            else
            {
                return "Ninguno";
            }

        }

        public static string GetName(string email)
        {

            try
            {
                string info = AdmonEntities.users.Where(x => x.email == email).FirstOrDefault().firstName;

                if (!String.IsNullOrEmpty(info))
                {
                    return info;
                }
                else
                {
                    return "N/A";
                }
            }
            catch (Exception)
            {

                return "N/A";
            }

        }

        public static string GetLastName(string email)
        {

            try
            {
                string info = AdmonEntities.users.Where(x => x.email == email).FirstOrDefault().lastName;

                if (!String.IsNullOrEmpty(info))
                {
                    return info;
                }
                else
                {
                    return "N/A";
                }
            }
            catch (Exception)
            {

                return "N/A";
            }

        }

        public static async void SaveLastLogin(string email)
        {
            AdmonEntities db = new AdmonEntities();
            db.Configuration.LazyLoadingEnabled = true;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                user info = db.users.Where(x => x.email == email).FirstOrDefault();
                if (info != null)
                {
                    info.lastLoginDt = DateTime.Now.ToLocalTime();
                    db.Entry(info).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {

            }

        }

        public static user GetUserByEmail(string email, string phone)
        {
            return AdmonEntities.users.Where<user>(x => x.email == email && x.phone == phone).FirstOrDefault<user>();
        }

        public static user GetUserByPhone(string phone)
        {
            return AdmonEntities.users.Where<user>(x => x.phone == phone).FirstOrDefault<user>();
        }

        public static string RandomString(int length)
        {
            return new string(Enumerable.Repeat<string>("1234567890", length).Select<string, char>((Func<string, char>)(s => s[Functions.random.Next(s.Length)])).ToArray<char>());
        }

        */
    }
}