using radiotaxi.Model.v2.General;
using radiotaxi.Model.v2.Operator;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class user : baseModel
    {
        public user getwithpicture(string partnerreference)
        {
            try
            {
                return db.users
                    .Where(x => x.partnerReference == partnerreference && x.active == true)
                    .Include(x => x.amazonPictures)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public user get(int id)
        {
            try
            {
                return db.users
                    .Where(x => x.id == id && x.active == true)
                    .Include(x => x.amazonPictures)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool OpenPayUpdateClientToken(string token)
        {
            try
            {
                this.openpayClientID = token;
                db.Entry(this).State = EntityState.Modified;
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
