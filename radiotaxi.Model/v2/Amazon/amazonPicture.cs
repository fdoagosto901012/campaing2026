using radiotaxi.Model;
using radiotaxi.Model.v2;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class amazonPicture : baseModel
    {
        public amazonPicture() { }
        public amazonPicture save() {
            try
            {
                db.amazonPictures.Add(this);
                db.SaveChanges();
                return this;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public amazonPicture update() {
            try
            {
                db.Entry(this).State = EntityState.Modified;
                db.SaveChanges();
                return this;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}