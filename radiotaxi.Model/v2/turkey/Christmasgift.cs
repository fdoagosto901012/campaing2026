using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class Christmasgift : baseModel
    {

        public void save()
        {
            try
            {
                this.db.Christmasgifts.Add(this);
                this.db.SaveChanges();
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
                Message = e.Message;
                StatusAction = error;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Christmasgift get(int userid) {
            try
            {
                Christmasgift christmasgift = this.db.Christmasgifts.Where(x=> x.userid == userid).Include(x => x.ChristmasgiftPrinteds).FirstOrDefault();
                return christmasgift;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void update()
        {
            try
            {
                db.Entry(this).State = EntityState.Modified;
                foreach (var printed in this.ChristmasgiftPrinteds)
                {
                    if (printed.id != 0)
                    {
                        db.Entry(printed).State = EntityState.Modified;
                    }
                    else { 
                        db.ChristmasgiftPrinteds.Add(printed);
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
        }

        // Estadisticas... 
        public List<Pavday> getByDayDeliver() {
            try
            {
                string query = @"
                    select CONVERT(DATE, c.DeliveryDate) as [date], count(-1) as amount from ChristmasgiftPrinted as cp
                    left join Christmasgift as c on c.id = cp.Christmasgiftid
                    left join [user] as u on c.userid = u.id
                    where c.DeliveryDate is not null and UPPER(u.partnerReference) not like 'OP-%'
                    group by CONVERT(DATE, c.DeliveryDate) 
                    order by [date] asc; 
                ";
                List<Pavday> response = this.db.Database.SqlQuery<Pavday>(query).ToList();
                return response;
            }
            catch (Exception ex)
            {
                return new List<Pavday>();
            }
        }

        public List<Pavday> getByDayDeliverOP()
        {
            try
            {
                string query = @"
                    select CONVERT(DATE, c.DeliveryDate) as [date], count(-1) as amount from ChristmasgiftPrinted as cp
                    left join Christmasgift as c on c.id = cp.Christmasgiftid
                    left join [user] as u on c.userid = u.id
                    where c.DeliveryDate is not null and UPPER(u.partnerReference) like 'OP-%'
                    group by CONVERT(DATE, c.DeliveryDate) 
                    order by [date] asc; 
                ";
                List<Pavday> response = this.db.Database.SqlQuery<Pavday>(query).ToList();
                return response;
            }
            catch (Exception ex)
            {
                return new List<Pavday>();
            }
        }

        public List<List<Pavtype>> getByDayDeliverVsTIckets()
        {
            try
            {
                string query = @"
                    select 'Pavos pendientes' as [type], COUNT(*) as amount from ChristmasgiftPrinted as cp
                    left join Christmasgift as c on c.id = cp.Christmasgiftid
                    left join [user] as u on c.userid = u.id
                    where c.DeliveryDate is null and UPPER(u.partnerReference) not like 'OP-%';
                ";
                List<Pavtype> response = this.db.Database.SqlQuery<Pavtype>(query).ToList();

                query = @"
                    select 'Pavos entregados' as [type], COUNT(*) as amount from ChristmasgiftPrinted as cp
                    left join Christmasgift as c on c.id = cp.Christmasgiftid
                    left join [user] as u on c.userid = u.id
                    where c.DeliveryDate is not null and UPPER(u.partnerReference) not like 'OP-%';
                ";
                List<Pavtype> response2 = this.db.Database.SqlQuery<Pavtype>(query).ToList();
                List<List<Pavtype>> a = new List<List<Pavtype>>();
                a.Add(response);
                a.Add(response2);
                return a;
            }
            catch (Exception ex)
            {
                return new List<List<Pavtype>>();
            }
        }

        public List<List<Pavtype>> getByDayDeliverVsTIcketsop()
        {
            try
            {
                string query = @"
                    select 'Pavos pendientes' as [type], COUNT(*) as amount from ChristmasgiftPrinted as cp
                    left join Christmasgift as c on c.id = cp.Christmasgiftid
                    left join [user] as u on c.userid = u.id
                    where c.DeliveryDate is null and UPPER(u.partnerReference) like 'OP-%';
                ";
                List<Pavtype> response = this.db.Database.SqlQuery<Pavtype>(query).ToList();

                query = @"
                    select 'Pavos entregados' as [type], COUNT(*) as amount from ChristmasgiftPrinted as cp
                    left join Christmasgift as c on c.id = cp.Christmasgiftid
                    left join [user] as u on c.userid = u.id
                    where c.DeliveryDate is not null and UPPER(u.partnerReference) like 'OP-%';
                ";
                List<Pavtype> response2 = this.db.Database.SqlQuery<Pavtype>(query).ToList();
                List<List<Pavtype>> a = new List<List<Pavtype>>();
                a.Add(response);
                a.Add(response2);
                return a;
            }
            catch (Exception ex)
            {
                return new List<List<Pavtype>>();
            }
        }


        public List<Pavtcashier> getCachies()
        {
            try
            {
                string query = @"
                    select upper(cp.PrintedBy) as nombre , count(-1) as impresiones from ChristmasgiftPrinted as cp
                    left join Christmasgift as c on c.id = cp.Christmasgiftid
                    left join [user] as u on c.userid = u.id
                    group by cp.PrintedBy
                    order by impresiones asc;
                ";
                List<Pavtcashier> response = this.db.Database.SqlQuery<Pavtcashier>(query).ToList();
                return response;
            }
            catch (Exception ex)
            {
                return new List<Pavtcashier>();
            }
        }

        public List<Pavtotal> gettotal()
        {
            try
            {
                string query = @"
                    select count(*) as total from ChristmasgiftPrinted as cp
                    left join Christmasgift as c on c.id = cp.Christmasgiftid
                    left join[user] as u on c.userid = u.id
                    where c.DeliveryDate is not null;
                ";
                List<Pavtotal> response = this.db.Database.SqlQuery<Pavtotal>(query).ToList();
                return response;
            }
            catch (Exception ex)
            {
                return new List<Pavtotal>();
            }
        }

    }

    public partial class Pavday {
        public int amount {get; set;}
        public DateTime date { get; set; }
    }

    public partial class Pavtype
    {
        public int amount { get; set; }
        public string type { get; set; }
    }

    public partial class Pavtcashier
    {
        public string nombre { get; set; }
        public int impresiones { get; set; }
    }

    public partial class Pavtotal
    {
        public int total { get; set; }
    }
}
